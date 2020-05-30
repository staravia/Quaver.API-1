/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2017-2018 Swan & The Quaver Team <support@quavergame.com>.
*/

using Quaver.API.Maps.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaver.API.Maps.Processors.Difficulty.Rulesets.Keys.Structures
{
    /// <summary>
    ///     Data point that is represented by a group of hitobjects.
    ///     Used for calculating strain value at a given time for a given hand.
    /// </summary>
    public class StrainSolverData
    {
        /// <summary>
        ///     Chorded Hit Objects at the current start time
        /// </summary>
        public List<StrainSolverHitObject> HitObjects { get; set; } = new List<StrainSolverHitObject>();

        /// <summary>
        ///     Is determined by the following StrainSolverData object on the current hand.
        ///     It should be defined externally from this class.
        /// </summary>
        public StrainSolverData NextStrainSolverDataOnCurrentHand { get; set; }

        public StrainSolverData NextStrainSolverDataAfterWristUp { get; set; }

        /// <summary>
        ///     When the current action/pattern starts
        /// </summary>
        public float StartTime { get; set; }

        /// <summary>
        ///     When the longest LN in self.HitObjects ends
        /// </summary>
        public float EndTime { get; set; }

        /// <summary>
        ///     If this hand is chorded with another hand, a certain multiplier will be applied
        /// </summary>
        public float ChordMultiplier { get; set; } = 1;

        /// <summary>
        ///     Base strain value calculated by its action
        /// </summary>
        public float ActionStrainCoefficient { get; set; } = 1;

        /// <summary>
        ///     Strain multiplier determined by pattern difficulty
        /// </summary>
        public float PatternStrainMultiplier { get; set; } = 1;

        /// <summary>
        ///     Multiplier that gets added to any pattern that could be manipulated via rolls.
        /// </summary>
        [Obsolete]
        public float RollManipulationStrainMultiplier { get; set; } = 1;

        /// <summary>
        ///     Multiplier that gets applied to any pattern that could be manipulated via long jacks.
        /// </summary>
        [Obsolete]
        public float JackManipulationStrainMultiplier { get; set; } = 1;

        /// <summary>
        ///     Multiplier that gets applied to any pattern that could be manipulated via wrist.
        /// </summary>
        public float WristManipulationMultiplier { get; set; } = 1;

        /// <summary>
        ///     Total difficulty of this data point applied after stamina
        /// </summary>
        public float StaminaMultiplier { get; set; } = 1;

        /// <summary>
        ///     Total strain value for this data point
        /// </summary>
        public float TotalStrainValue { get; private set; }

        /// <summary>
        ///     Hand that this data point represents
        /// </summary>
        public Hand Hand { get; set; }

        /// <summary>
        ///     Finger Action that this data point represents
        /// </summary>
        public FingerAction FingerAction { get; set; } = FingerAction.None;

        /// <summary>
        ///     Duration of FingerAaction in ms
        /// </summary>
        public float FingerActionDurationMs { get; set; }

        /// <summary>
        ///     Is determined by if this data point has more than one hit object (per hand)
        /// </summary>
        public bool HandChord => HitObjects.Count > 1;

        /// <summary>
        ///     Is determined by if the wrist manipulation for this object was already solved
        /// </summary>
        public FingerState WristState { get; set; } = FingerState.None;

        /// <summary>
        ///     Is an index value of this hand's finger state. (Determined by every finger's state)
        /// </summary>
        public FingerState FingerState { get; private set; } = FingerState.None;

        /// <summary>
        ///
        /// </summary>
        private float EasyMultiplier { get; set; } = 1;

        /// <summary>
        ///     Data used to represent a point in time and other variables that influence difficulty.
        /// </summary>
        /// <param name="hitOb"></param>
        /// <param name="rate"></param>
        public StrainSolverData(StrainSolverHitObject hitOb, float rate = 1)
        {
            StartTime = hitOb.HitObject.StartTime / rate;
            EndTime = hitOb.HitObject.EndTime / rate;
            HitObjects.Add(hitOb);
        }

        /// <summary>
        ///     Calculate the strain value of this current point.
        /// </summary>
        public void CalculateStrainValue()
        {
            // Wrist Manipulation and Chord multipliers should approach 1 as the action gets easier
            //var easyMultiplier = Math.Min(1, FingerActionDurationMs);
            //WristManipulationMultiplier = SolveForEasyMultiplier(WristManipulationMultiplier, easyMultiplier);
            //ChordMultiplier = SolveForEasyMultiplier(ChordMultiplier, easyMultiplier);

            // Calculate the strain value of each individual object and add to total
            foreach (var hitOb in HitObjects)
            {
                hitOb.LnStrainDifficulty = EasyMultiplier * hitOb.LnStrainDifficulty;
                hitOb.StrainValue = ActionStrainCoefficient * WristManipulationMultiplier * ChordMultiplier * PatternStrainMultiplier * StaminaMultiplier + hitOb.LnStrainDifficulty;
                TotalStrainValue += hitOb.StrainValue;
            }

            // Average the strain value between the two objects
            TotalStrainValue /= HitObjects.Count;
        }

        public void SolveFingerState()
        {
            foreach (var hitOb in HitObjects)
                FingerState |= hitOb.FingerState;
        }

        private void SolveForEasyMultiplier(float constantDuration) => EasyMultiplier = Math.Min(1, constantDuration / (1 - (1 - FingerActionDurationMs)));
    }
}
