/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2017-2018 Swan & The Quaver Team <support@quavergame.com>.
*/

using Quaver.API.Enums;
using Quaver.API.Helpers;
using Quaver.API.Maps;
using Quaver.API.Maps.Processors.Difficulty.Optimization;
using Quaver.API.Maps.Processors.Difficulty.Rulesets.Keys.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaver.API.Maps.Processors.Difficulty.Rulesets.Keys
{
    /// <summary>
    ///     Will be used to solve Strain Rating.
    /// </summary>
    public class DifficultyProcessorKeys : DifficultyProcessor
    {
        /// <summary>
        ///     The version of the processor.
        /// </summary>
        public static string Version { get; } = "0.2.0";

        /// <summary>
        ///     Constants used for solving
        /// </summary>
        public StrainConstantsKeys StrainConstants { get; private set; }

        /// <summary>
        ///     Average note density of the map
        /// </summary>
        public float AverageNoteDensity { get; private set; } = 0;

        /// <summary>
        ///     Hit objects in the map used for solving difficulty
        /// </summary>
        public List<StrainSolverData> StrainSolverData { get; private set; } = new List<StrainSolverData>();

        /// <summary>
        ///     Assumes that the assigned hand will be the one to press that key
        /// </summary>
        private Dictionary<int, Hand> LaneToHand4K { get; set; } = new Dictionary<int, Hand>()
        {
            { 1, Hand.Left },
            { 2, Hand.Left },
            { 3, Hand.Right },
            { 4, Hand.Right }
        };

        /// <summary>
        ///     Assumes that the assigned hand will be the one to press that key
        /// </summary>
        private Dictionary<int, Hand> LaneToHand7K { get; set; } = new Dictionary<int, Hand>()
        {
            { 1, Hand.Left },
            { 2, Hand.Left },
            { 3, Hand.Left },
            { 4, Hand.Ambiguous },
            { 5, Hand.Right },
            { 6, Hand.Right },
            { 7, Hand.Right }
        };

        /// <summary>
        ///     Assumes that the assigned finger will be the one to press that key.
        /// </summary>
        private Dictionary<int, FingerState> LaneToFinger4K { get; set; } = new Dictionary<int, FingerState>()
        {
            { 1, FingerState.Middle },
            { 2, FingerState.Index },
            { 3, FingerState.Index },
            { 4, FingerState.Middle }
        };

        /// <summary>
        ///     Assumes that the assigned finger will be the one to press that key.
        /// </summary>
        private Dictionary<int, FingerState> LaneToFinger7K { get; set; } = new Dictionary<int, FingerState>()
        {
            { 1, FingerState.Ring },
            { 2, FingerState.Middle },
            { 3, FingerState.Index },
            { 4, FingerState.Thumb },
            { 5, FingerState.Index },
            { 6, FingerState.Middle },
            { 7, FingerState.Ring }
        };

        /// <summary>
        ///     Value of confidence that there's vibro manipulation in the calculated map.
        /// </summary>
        private float VibroInaccuracyConfidence { get; set; }

        /// <summary>
        ///     Value of confidence that there's roll manipulation in the calculated map.
        /// </summary>
        private float RollInaccuracyConfidence { get; set; }

        /// <summary>
        ///     Solves the difficulty of a .qua file
        /// </summary>
        /// <param name="map"></param>
        /// <param name="constants"></param>
        /// <param name="mods"></param>
        /// <param name="detailedSolve"></param>
        public DifficultyProcessorKeys(Qua map, StrainConstants constants, ModIdentifier mods = ModIdentifier.None, bool detailedSolve = false) : base(map, constants, mods)
        {
            // Cast the current Strain Constants Property to the correct type.
            StrainConstants = (StrainConstantsKeys)constants;

            // Don't bother calculating map difficulty if there's less than 2 hit objects
            if (map.HitObjects.Count < 2)
                return;

            // Solve for difficulty
            CalculateDifficulty(mods);

            // If detailed solving is enabled, expand calculation
            if (detailedSolve)
            {
                // ComputeNoteDensityData();
                ComputeForPatternFlags();
            }
        }

        /// <summary>
        ///     Calculate difficulty of a map with given rate
        /// </summary>
        /// <param name="rate"></param>
        public void CalculateDifficulty(ModIdentifier mods)
        {
            // If map does not exist, ignore calculation.
            if (Map == null) return;

            // Get song rate from selected mods
            var rate = ModHelper.GetRateFromMods(mods);

            // Compute for overall difficulty
            switch (Map.Mode)
            {
                case (GameMode.Keys4):
                    OverallDifficulty = ComputeForOverallDifficulty(rate);
                    break;
                case (GameMode.Keys7):
                    OverallDifficulty = (ComputeForOverallDifficulty(rate, Hand.Left) + ComputeForOverallDifficulty(rate, Hand.Right))/2;
                    break;
            }
        }

        /// <summary>
        ///     Calculate overall difficulty of a map. "AssumeHand" is used for odd-numbered keymodes.
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="assumeHand"></param>
        /// <returns></returns>
        private float ComputeForOverallDifficulty(float rate, Hand assumeHand = Hand.Right)
        {
            ComputeBaseStrainStates(rate, assumeHand);
            ComputeForChords();
            ComputeForFingerActions();
            ComputeForWristManipulation();
            ComputeForLnMultiplier();
            ComputeForStamina();
            return CalculateOverallDifficulty();
        }

        /// <summary>
        ///     Get Note Data, and compute the base strain weights
        ///     The base strain weights are affected by LN layering
        /// </summary>
        /// <param name="qssData"></param>
        /// <param name="qua"></param>
        /// <param name="assumeHand"></param>
        private void ComputeBaseStrainStates(float rate, Hand assumeHand)
        {
            // Add hit objects from qua map to qssData
            for (var i = 0; i < Map.HitObjects.Count; i++)
            {
                if (Map.HasScratchKey && Map.HitObjects[i].Lane == Map.GetKeyCount())
                    continue;

                var curHitOb = new StrainSolverHitObject(Map.HitObjects[i]);
                var curStrainData = new StrainSolverData(curHitOb, rate);

                // Assign Finger and Hand States
                switch (Map.Mode)
                {
                    case GameMode.Keys4:
                        curHitOb.FingerState = LaneToFinger4K[Map.HitObjects[i].Lane];
                        curStrainData.Hand = LaneToHand4K[Map.HitObjects[i].Lane];
                        break;
                    case GameMode.Keys7:
                        curHitOb.FingerState = LaneToFinger7K[Map.HitObjects[i].Lane];
                        curStrainData.Hand = LaneToHand7K[Map.HitObjects[i].Lane] == Hand.Ambiguous ? assumeHand : LaneToHand7K[Map.HitObjects[i].Lane];
                        break;
                }

                // Add Strain Solver Data to list
                StrainSolverData.Add(curStrainData);
            }
        }

        /// <summary>
        ///     Iterate through the HitObject list and merges the chords together into one data point
        /// </summary>
        private void ComputeForChords()
        {
            // Search through whole hit object list and find chords
            for (var i = 0; i < StrainSolverData.Count - 1; i++)
            {
                for (var j = i + 1; j < StrainSolverData.Count; j++)
                {
                    // Check if next hit object is way past the tolerance
                    var msDiff = StrainSolverData[j].StartTime - StrainSolverData[i].StartTime;
                    if (msDiff > StrainConstants.ChordClumpToleranceMs)
                        break;

                    // Check if the next and current hit objects are chord-able
                    if ( Math.Abs(msDiff) > StrainConstants.ChordClumpToleranceMs )
                        continue;

                    if (StrainSolverData[i].Hand == StrainSolverData[j].Hand)
                    {
                        // Search through every hit object for chords
                        foreach (var k in StrainSolverData[j].HitObjects)
                        {
                            // Check if the current data point will have duplicate finger state to prevent stacked notes
                            var sameStateFound = false;
                            foreach (var l in StrainSolverData[i].HitObjects)
                            {
                                if (l.FingerState != k.FingerState)
                                    continue;

                                    sameStateFound = true;
                                    break;
                            }

                            // Add hit object to chord list if its not stacked
                            if (!sameStateFound)
                                StrainSolverData[i].HitObjects.Add(k);
                        }

                        // Remove un-needed data point because it has been merged with the current point
                        StrainSolverData.RemoveAt(j);
                        j--;
                    }

                    // Apply a chord multiplier if the other hand is chorded the current hand
                    else
                    {
                        StrainSolverData[i].ChordMultiplier = StrainConstants.BothHandChordedMultiplier;
                        StrainSolverData[j].ChordMultiplier = StrainConstants.BothHandChordedMultiplier;
                    }
                }
            }

            // Solve finger state of every object once chords have been found and applied.
            foreach (var data in StrainSolverData)
            {
                data.SolveFingerState();
            }
        }

        /// <summary>
        ///     Scans every finger state, and determines its action (JACK/TRILL/TECH, ect).
        ///     Action-Strain multiplier is applied in computation.
        /// </summary>
        /// <param name="qssData"></param>
        private void ComputeForFingerActions()
        {
            // Solve for Finger Action
            for (var i = 0; i < StrainSolverData.Count - 1; i++)
            {
                var curHitOb = StrainSolverData[i];

                // Find the next Hit Object in the current Hit Object's Hand
                for (var j = i + 1; j < StrainSolverData.Count; j++)
                {
                    var nextHitOb = StrainSolverData[j];
                    if (curHitOb.Hand == nextHitOb.Hand && nextHitOb.StartTime > curHitOb.StartTime)
                    {
                        // Determined by if there's a minijack within 2 set of chords/single notes
                        var actionJackFound = ((int)nextHitOb.FingerState & (1 << (int)curHitOb.FingerState - 1)) != 0;

                        // Determined by if a chord is found in either finger state
                        var actionChordFound = curHitOb.HandChord || nextHitOb.HandChord;

                        // Determined by if both fingerstates are exactly the same
                        var actionSameState = curHitOb.FingerState == nextHitOb.FingerState;

                        // Determined by how long the current finger action is
                        var actionDuration = nextHitOb.StartTime - curHitOb.StartTime;

                        // Apply the "NextStrainSolverDataOnCurrentHand" value on the current hit object and also apply action duration.
                        curHitOb.NextStrainSolverDataOnCurrentHand = nextHitOb;
                        curHitOb.FingerActionDurationMs = actionDuration;

                        // Trill/Roll
                        if (!actionChordFound && !actionSameState)
                        {
                            curHitOb.FingerAction = FingerAction.Trill;
                            curHitOb.ActionStrainCoefficient = GetCoefficientValue(actionDuration,
                                StrainConstants.RollUpperBoundaryMs,
                                StrainConstants.RollMaxStrainValue,
                                StrainConstants.RollCurveExponential);
                        }

                        // Simple Jack
                        else if (actionSameState)
                        {
                            curHitOb.FingerAction = FingerAction.SimpleJack;
                            curHitOb.ActionStrainCoefficient = GetCoefficientValue(actionDuration,
                                StrainConstants.SJackUpperBoundaryMs,
                                StrainConstants.SJackMaxStrainValue,
                                StrainConstants.SJackCurveExponential);
                        }

                        // Tech Jack
                        else if (actionJackFound)
                        {
                            curHitOb.FingerAction = FingerAction.TechnicalJack;
                            curHitOb.ActionStrainCoefficient = GetCoefficientValue(actionDuration,
                                StrainConstants.TJackUpperBoundaryMs,
                                StrainConstants.TJackMaxStrainValue,
                                StrainConstants.TJackCurveExponential);
                        }

                        // Bracket
                        else
                        {
                            curHitOb.FingerAction = FingerAction.Bracket;
                            curHitOb.ActionStrainCoefficient = GetCoefficientValue(actionDuration,
                                StrainConstants.BracketUpperBoundaryMs,
                                StrainConstants.BracketMaxStrainValue,
                                StrainConstants.BracketCurveExponential);
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ComputeForWristManipulation()
        {
            var curWristState = new Dictionary<Hand, FingerState>()
            {
                {Hand.Left, FingerState.None},
                {Hand.Right, FingerState.None}
            };

            var count = new Dictionary<Hand, int>()
            {
                {Hand.Left, 0},
                {Hand.Right, 0}
            };

            // Compute for wrist state
            foreach (var data in StrainSolverData)
            {
                if (data.NextStrainSolverDataOnCurrentHand == null)
                    continue;

                if (data.WristState == FingerState.None)
                {
                    var next = SolveForWristState(data, data.FingerState, 0, false);
                    data.WristState = data.FingerState;
                    data.NextStrainSolverDataAfterWristUp = next == data || next == null ? data.NextStrainSolverDataOnCurrentHand : next.NextStrainSolverDataAfterWristUp;
                }
            }

            // Compute for wrist manipulation
            foreach (var data in StrainSolverData)
            {
                if (data.NextStrainSolverDataOnCurrentHand == null)
                    continue;

                if (data.WristState == curWristState[data.Hand])
                {
                    count[data.Hand]++;
                    data.WristManipulationMultiplier = (1 - StrainConstants.WristManipulationMultiplier) + StrainConstants.WristManipulationMultiplier * StrainConstants.WristManipulationOffset / ( count[data.Hand] + StrainConstants.WristManipulationOffset );
                    continue;
                }

                count[data.Hand] = 0;
                curWristState[data.Hand] = data.WristState;
            }
        }

        /// <summary>
        ///     todo: this only works for 4k right now. It doesn't detect roll direction.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="state"></param>
        /// <param name="duration"></param>
        /// <param name="inwards"></param>
        /// <returns></returns>
        private StrainSolverData SolveForWristState(StrainSolverData data, FingerState state, float duration, bool inwards)
        {
            const float rollRatioTolerance = 1.9f;

            // Check to see if there is another strain solver data point in the current hand
            if (data.NextStrainSolverDataOnCurrentHand == null)
            {
                data.WristState = state;
                return data;
            }

            // Check to see if the next data point is wrist-down
            if (( state & data.NextStrainSolverDataOnCurrentHand.FingerState ) != 0)
            {
                // If the current duration is too low, or if the chain is only a single note long, apply the wrist state to a single noe even when there's none.
                if (duration < StrainConstants.ChordClumpToleranceMs || (state & (state - 1)) != 0 )
                {
                    data.WristState = state;
                    data.NextStrainSolverDataAfterWristUp = data.NextStrainSolverDataOnCurrentHand;
                    return null;
                }

                // Sort out which action is longer/shorter
                var min = duration;
                var max = data.FingerActionDurationMs;
                if (min > max)
                {
                    var temp = max;
                    max = min;
                    min = temp;
                }

                // Check to see if this wrist state is actually a roll
                if (max / min > rollRatioTolerance)
                {
                    data.WristState = state;
                    data.NextStrainSolverDataAfterWristUp = data.NextStrainSolverDataOnCurrentHand;
                    return data;
                }

                return null;
            }

            // If the wrist state has not been solved yet, check the next data point and use the same wrist state as the next one.
            var next = SolveForWristState(data.NextStrainSolverDataOnCurrentHand, state | data.FingerState, data.FingerActionDurationMs, false);

            if (next != null)
            {
                data.WristState = next.WristState;
                data.NextStrainSolverDataAfterWristUp = next.NextStrainSolverDataAfterWristUp;
            }

            return next;
        }

        # region OLD

        /*
        /// <summary>
        ///     Scans for roll manipulation. "Roll Manipulation" is definced as notes in sequence "A -> B -> A" with one action at least twice as long as the other.
        [ObsoleteAttribute]
        private void ComputeForRollManipulation()
        {
            var manipulationCount = new Dictionary<Hand, int>
            {
                {Hand.Left, 0},
                {Hand.Right, 0}
            };

            var manipulationFound = new Dictionary<Hand, bool>
            {
                {Hand.Left, false},
                {Hand.Right, false}
            };

            // todo: refactor this so that it works for 7k rolls
            foreach (var data in StrainSolverData)
            {
                // subtract manipulation index if manipulation was not found
                if (!manipulationFound[data.Hand])
                    manipulationCount[data.Hand] = 0;

                // Reset manipulation detection for current hand
                manipulationFound[data.Hand] = false;

                // Check to see if the current data point has two other following points
                if (data.NextStrainSolverDataOnCurrentHand == null || data.NextStrainSolverDataOnCurrentHand.NextStrainSolverDataOnCurrentHand == null)
                    continue;

                var middle = data.NextStrainSolverDataOnCurrentHand;
                var last = data.NextStrainSolverDataOnCurrentHand.NextStrainSolverDataOnCurrentHand;

                // Check to see if both data and middle points are rolls
                if (data.FingerAction != FingerAction.Trill || middle.FingerAction == FingerAction.Trill)
                    continue;

                // Make sure the first and last finger states are identical
                if (data.FingerState != last.FingerState)
                    continue;

                // Get action duration ratio from both actions
                var durationRatio = Math.Max(data.FingerActionDurationMs / middle.FingerActionDurationMs, middle.FingerActionDurationMs / data.FingerActionDurationMs);

                // If the ratio is above this threshold, count it as a roll manipulation
                if (durationRatio >= StrainConstants.RollRatioToleranceMs)
                {
                    // Apply multiplier
                    // todo: catch possible arithmetic error (division by 0)
                    var durationMultiplier = 1 / (1 + ((durationRatio - 1) * StrainConstants.RollRatioMultiplier));
                    var manipulationFoundRatio = 1 - ((manipulationCount[data.Hand] / StrainConstants.RollMaxLength) * (1 - StrainConstants.RollLengthMultiplier));
                    data.RollManipulationStrainMultiplier = durationMultiplier * manipulationFoundRatio;

                    // Count manipulation
                    manipulationFound[data.Hand] = true;
                    RollInaccuracyConfidence++;
                    if (manipulationCount[data.Hand] < StrainConstants.RollMaxLength)
                        manipulationCount[data.Hand]++;
                }
            }
        }*/

        /*
        /// <summary>
        ///     Scans for jack manipulation. "Jack Manipulation" is defined as a succession of simple jacks. ("A -> A -> A")
        /// </summary>
        [ObsoleteAttribute]
        private void ComputeForJackManipulation()
        {
            var prevActionDuration = 0f;
            var manipulationFound = new Dictionary<Hand, bool>
            {
                {Hand.Left, false},
                {Hand.Right, false}
            };

            var manipulationCount = new Dictionary<Hand, int>
            {
                {Hand.Left, 0},
                {Hand.Right, 0}
            };

            foreach (var data in StrainSolverData)
            {
                // Reset manipulation detection for current hand
                manipulationFound[data.Hand] = false;

                // Check to see if the current data point has a following data point
                if (data.NextStrainSolverDataOnCurrentHand != null )
                {
                    var next = data.NextStrainSolverDataOnCurrentHand;
                    if (data.FingerAction == FingerAction.SimpleJack && next.FingerAction == FingerAction.SimpleJack && data.FingerActionDurationMs < StrainConstants.VibroDeltaToleranceMs + prevActionDuration)
                    {
                        // Apply multiplier
                        // todo: catch possible arithmetic error (division by 0)
                        // note:    83.3ms = 180bpm 1/4 vibro
                        //          88.2ms = 170bpm 1/4 vibro
                        //          93.7ms = 160bpm 1/4 vibro

                        // 35f = 35ms tolerance before hitting vibro point (88.2ms, 170bpm vibro)
                        var durationValue = ((StrainConstants.VibroActionDurationMs + StrainConstants.VibroActionToleranceMs - data.FingerActionDurationMs) / StrainConstants.VibroActionToleranceMs).Clamp(0, 1);
                        var durationMultiplier = 1 - (durationValue * (1 - StrainConstants.VibroMultiplier));
                        var manipulationFoundRatio = 1 - ((manipulationCount[data.Hand] / StrainConstants.VibroMaxLength) * (1 - StrainConstants.VibroLengthMultiplier));
                        data.RollManipulationStrainMultiplier = durationMultiplier * manipulationFoundRatio;

                        // Count manipulation
                        manipulationFound[data.Hand] = true;
                        VibroInaccuracyConfidence++;
                        if (manipulationCount[data.Hand] < StrainConstants.VibroMaxLength)
                            manipulationCount[data.Hand]++;
                    }

                    prevActionDuration = data.FingerActionDurationMs;
                }

                // Reset manipulation count if manipulation was not found
                if (!manipulationFound[data.Hand])
                    manipulationCount[data.Hand] = 0;
            }
        }*/

        #endregion

        /// <summary>
        ///     Scans for LN layering and applies a multiplier
        /// </summary>
        private void ComputeForLnMultiplier()
        {
            foreach (var data in StrainSolverData)
            {
                // Check if data is LN
                if (data.EndTime <= data.StartTime)
                    continue;

                var durationValue = (( data.EndTime - data.StartTime ) / StrainConstants.LnDifficultSizeThresholdMs).Clamp(0, 1);
                var baseDifficulty = StrainConstants.LnBaseValue + durationValue * StrainConstants.LnBaseMultiplier;

                // Loop through all strain solver data on the current hand until the LN is finished
                var next = data;

                while (next.NextStrainSolverDataOnCurrentHand != null)
                {
                    next = next.NextStrainSolverDataOnCurrentHand;

                    if (next.StartTime >= data.EndTime - StrainConstants.LnEndThresholdMs)
                        break;

                    if (next.StartTime < data.StartTime)
                        continue;

                    // Target hitobject's LN ends after current hitobject's LN end.
                    if (next.EndTime > data.EndTime + StrainConstants.LnEndThresholdMs)
                    {
                        foreach (var k in next.HitObjects)
                        {
                            k.LnLayerType = LnLayerType.OutsideRelease;
                            k.LnStrainDifficulty = baseDifficulty * StrainConstants.LnReleaseAfterMultiplier;
                        }
                    }

                    // Target hitobject's LN ends before current hitobject's LN end
                    else if (next.EndTime > 0)
                    {
                        foreach (var k in next.HitObjects)
                        {
                            k.LnLayerType = LnLayerType.InsideRelease;
                            k.LnStrainDifficulty = baseDifficulty * StrainConstants.LnReleaseBeforeMultiplier;
                        }
                    }

                    // Target hitobject is not an LN
                    else
                    {
                        foreach (var k in next.HitObjects)
                        {
                            k.LnLayerType = LnLayerType.InsideTap;
                            k.LnStrainDifficulty = baseDifficulty * StrainConstants.LnTapMultiplier;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Checks to see if the map rating is inacurrate due to vibro/rolls
        /// </summary>
        private void ComputeForPatternFlags()
        {
            // If 10% or more of the map has longjack manip, flag it as vibro map
            if (VibroInaccuracyConfidence / StrainSolverData.Count > 0.10)
                QssPatternFlags |= QssPatternFlags.SimpleVibro;

            // If 15% or more of the map has roll manip, flag it as roll map
            if (RollInaccuracyConfidence / StrainSolverData.Count > 0.15)
                QssPatternFlags |= QssPatternFlags.Rolls;
        }

        /// <summary>
        ///     This will Handle stamina-related calculations
        /// </summary>
        private void ComputeForStamina()
        {
            // Cached diff
            float curMultiplier = 0;
            float prevTime = 0;

            // Sort Data
            SortDataByStartTime(true);

            // Compute for stamina values
            foreach (var data in StrainSolverData)
            {
                if (data.NextStrainSolverDataOnCurrentHand == null)
                    continue;

                // Solve for difficulty without stamina multiplier
                data.CalculateStrainValue();

                // Solve for delta
                var delta = (data.StartTime - prevTime) / 1000f;
                prevTime = data.StartTime;

                // Solve for stamina multiplier
                curMultiplier += data.TotalStrainValue > StrainConstants.StaminaDifficultyValue
                    ? StrainConstants.StaminaIncreaseValue
                    : -StrainConstants.StaminaDecreaseVelocity * delta;

                curMultiplier = curMultiplier.Clamp(0, 1);
                data.StaminaMultiplier = curMultiplier * StrainConstants.StaminaStrainMultiplier + (1 - StrainConstants.StaminaStrainMultiplier);

                // Solve for data difficulty with stamina multiplier
                data.CalculateStrainValue();
            }
        }

        /// <summary>
        ///     Calculate the overall difficulty of the given map
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        private float CalculateOverallDifficulty()
        {
            float weightedDiff = 0;
            float weight = 0;

            // Solve strain value of every data point
            foreach (var data in StrainSolverData)
            {
                var delta = (float)Math.Pow(data.StaminaMultiplier + StrainConstants.StrainWeightOffset, StrainConstants.StrainWeightExponent);
                weightedDiff += data.TotalStrainValue * delta;
                weight += delta;
            }

            // Calculate the overall difficulty with given weights and values
            weightedDiff /= weight;
            return weightedDiff;
        }

        /// <summary>
        ///     Compute and generate Note Density Data.
        /// </summary>
        /// <param name="qssData"></param>
        /// <param name="qua"></param>
        private void ComputeNoteDensityData(float rate)
        {
            //MapLength = Qua.Length;
            AverageNoteDensity = SECONDS_TO_MILLISECONDS * Map.HitObjects.Count / (Map.Length * rate);

            //todo: solve note density graph
            // put stuff here
        }

        /// <summary>
        ///     Used to calculate Coefficient for Strain Difficulty
        /// </summary>
        private float GetCoefficientValue(float duration, float xMax, float strainMax, float exp)
        {
            const float lowestDifficulty = 1;
            const float xMin = 20f;

            // calculate ratio between min and max value
            var densityBonus = Math.Min(StrainConstants.MaxDensityBonus, StrainConstants.DensityBonusDuration / duration);
            var ratio = Math.Max(0, 1 - (duration - xMin) / (xMax - xMin));

            // compute for difficulty
            return lowestDifficulty + densityBonus + (strainMax - lowestDifficulty) * (float)Math.Pow(ratio, exp);
        }

        /// <summary>
        ///     Will sort all the hit objects either in ascending or descending order
        /// </summary>
        /// <param name="ascending"></param>
        private void SortDataByStartTime(bool ascending)
        {
            if (ascending)
            {
                StrainSolverData.Sort((a,b) => SortByStartTime(a.StartTime, b.StartTime));
                return;
            }

            StrainSolverData.Sort((a,b) => SortByStartTime(b.StartTime, a.StartTime));
        }

        private int SortByStartTime(float a, float b)
        {
            if (a == b)
                return 0;

            if (a > b)
                return 1;

            return -1;
        }
    }
}
