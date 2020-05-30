/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2017-2018 Swan & The Quaver Team <support@quavergame.com>.
*/

using Quaver.API.Maps.Processors.Difficulty.Optimization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaver.API.Maps.Processors.Difficulty.Rulesets.Keys
{
    public class StrainConstantsKeys : StrainConstants
    {
        /// <summary>
        ///     When Long Notes start/end after this threshold, it will be considered for a specific multiplier.
        ///     Non-Dyanmic Constant. Do not use for optimization.
        /// </summary>
        public float LnEndThresholdMs { get; } = 42;

        /// <summary>
        ///     When seperate notes are under this threshold, it will count as a chord.
        ///     Non-Dyanmic Constant. Do not use for optimization.
        /// </summary>
        public float ChordClumpToleranceMs { get; } = 8;

        // Overall Difficulty
        public float StrainWeightOffset { get; } = 6f;
        public float StrainWeightExponent { get; } = 4f;

        // Density Multiplier
        public float MaxDensityBonus { get; } = 3.7f;
        public float DensityBonusDuration { get; } = 200;

        // Stamina
        public float StaminaIncreaseValue { get; }
        public float StaminaDecreaseVelocity { get; }

        public float StaminaStrainMultiplier { get; }

        public float StaminaDifficultyValue { get; }

        // Chords
        public float BothHandChordedMultiplier { get; }

        // Simple Jacks
        public float SJackUpperBoundaryMs { get; }
        public float SJackMaxStrainValue { get; }
        public float SJackCurveExponential { get; }

        // Tech Jacks
        public float TJackUpperBoundaryMs { get; }
        public float TJackMaxStrainValue { get; }
        public float TJackCurveExponential { get; }

        // Rolls
        public float RollUpperBoundaryMs { get; }
        public float RollMaxStrainValue { get; }
        public float RollCurveExponential { get; }

        // Brackets
        public float BracketUpperBoundaryMs { get; }
        public float BracketMaxStrainValue { get; }
        public float BracketCurveExponential { get; }

        // LN
        public float LnBaseMultiplier { get; }
        public float LnBaseValue { get; }
        public float LnDifficultSizeThresholdMs { get; }
        public float LnReleaseAfterMultiplier { get; }
        public float LnReleaseBeforeMultiplier { get; }
        public float LnTapMultiplier { get; }

        // Wrist Manipulatin
        public float WristManipulationOffset { get; }
        public float WristManipulationMultiplier { get; }

        /// <summary>
        ///     Constructor. Create default strain constant values.
        /// </summary>
        public StrainConstantsKeys(float[] input = null)
        {
            // Defaults
            var defaultConstants = new List<ConstantVariable>()
            {
                /* OLD
                // Overall Difficulty
                new ConstantVariable("StrainWeightOffset", 2.8f),
                new ConstantVariable("StrainWeightExponent", 4f),

                // Density Multiplier
                new ConstantVariable("MaxDensityBonus", 3.7f),
                new ConstantVariable("DensityBonusDuration", 200),

                // Stamina
                new ConstantVariable("StaminaIncreaseVelocity", 2.8f),
                new ConstantVariable("StaminaDecreaseVelocity", 4.12f),
                new ConstantVariable("StaminaReliefThreshold", 10f),

                // Chords
                new ConstantVariable("BothHandChordedMultiplier", 0.88f),

                // Simple Jack
                new ConstantVariable("SJackUpperBoundaryMs", 310),
                new ConstantVariable("SJackMaxStrainValue", 72.5f),
                new ConstantVariable("SJackCurveExponential", 1.51f),

                // Tech Jack
                new ConstantVariable("TJackUpperBoundaryMs", 330),
                new ConstantVariable("TJackMaxStrainValue", 76.5f),
                new ConstantVariable("TJackCurveExponential", 1.59f),

                // Roll/Trill
                new ConstantVariable("RollUpperBoundaryMs", 250),
                new ConstantVariable("RollMaxStrainValue", 66.5f),
                new ConstantVariable("RollCurveExponential", 2.09f),

                // Bracket
                new ConstantVariable("BracketUpperBoundaryMs", 230),
                new ConstantVariable("BracketMaxStrainValue", 60),
                new ConstantVariable("BracketCurveExponential", 1.43f),

                // LN
                new ConstantVariable("LnBaseValue", 5.7f),
                new ConstantVariable("LnBaseMultiplier", 5.25f),
                new ConstantVariable("LnDifficultSizeThresholdMs", 250f),
                new ConstantVariable("LnReleaseAfterMultiplier", 1.5f),
                new ConstantVariable("LnReleaseBeforeMultiplier", 1.15f),
                new ConstantVariable("LnTapMultiplier", 1.05f),

                // LongJack Manipulation
                new ConstantVariable("VibroActionDurationMs", 88.2f),
                new ConstantVariable("VibroActionToleranceMs", 22f),
                new ConstantVariable("VibroMultiplier", 0.48f),
                new ConstantVariable("VibroLengthMultiplier", 0.3f),
                new ConstantVariable("VibroMaxLength", 6),

                // Roll Manipulation
                new ConstantVariable("RollRatioToleranceMs", 2),
                new ConstantVariable("RollRatioMultiplier", 0.25f),
                new ConstantVariable("RollLengthMultiplier", 0.6f),
                new ConstantVariable("RollMaxLength", 14)
                */

                new ConstantVariable("StrainWeightOffset", 9.160004f),
                new ConstantVariable("StrainWeightExponent", 9.295717f),
                new ConstantVariable("MaxDensityBonus", 2.557415f),
                new ConstantVariable("DensityBonusDuration", 250.335f),
                new ConstantVariable("StaminaIncreaseValue", 0.1390676f),
                new ConstantVariable("StaminaDecreaseVelocity", 0.1534792f),
                new ConstantVariable("StaminaStrainMultiplier", 1.455667f),
                new ConstantVariable("StaminaDifficultyValue", 16.25651f),
                new ConstantVariable("BothHandChordedMultiplier", 0.9484335f),
                new ConstantVariable("SJackUpperBoundaryMs", 372.5333f),
                new ConstantVariable("SJackMaxStrainValue", 81.46977f),
                new ConstantVariable("SJackCurveExponential", 3.288497f),
                new ConstantVariable("TJackUpperBoundaryMs", 498.9047f),
                new ConstantVariable("TJackMaxStrainValue", 48.48959f),
                new ConstantVariable("TJackCurveExponential", 2.681958f),
                new ConstantVariable("RollUpperBoundaryMs", 585.527f),
                new ConstantVariable("RollMaxStrainValue", 18.40234f),
                new ConstantVariable("RollCurveExponential", 2.694864f),
                new ConstantVariable("BracketUpperBoundaryMs", 443.5485f),
                new ConstantVariable("BracketMaxStrainValue", 65.67741f),
                new ConstantVariable("BracketCurveExponential", 2.873993f),
                new ConstantVariable("LnBaseValue", 4.645733f),
                new ConstantVariable("LnBaseMultiplier", 0.2272576f),
                new ConstantVariable("LnDifficultSizeThresholdMs", 362.3458f),
                new ConstantVariable("LnReleaseAfterMultiplier", 3.215357f),
                new ConstantVariable("LnReleaseBeforeMultiplier", 3.460205f),
                new ConstantVariable("LnTapMultiplier", 1.990233f),
                new ConstantVariable("WristManipulationOffset", 7.288202f),
                new ConstantVariable("WristManipulationMultiplier", 0.5026569f)
            };

            // If there's no input, use default constants
            if (input == null)
            {
                input = new float[defaultConstants.Count];

                for (var i = 0; i < defaultConstants.Count; i++)
                {
                    input[i] = defaultConstants[i].Value;
                }
            }

            // Set constant variables
            // Overall Difficulty
            StrainWeightOffset = NewConstant(defaultConstants[0].Name, input[0]);
            StrainWeightExponent = NewConstant(defaultConstants[1].Name, input[1]);

            // Density Multiplier
            MaxDensityBonus = NewConstant(defaultConstants[2].Name, input[2]);
            DensityBonusDuration = NewConstant(defaultConstants[3].Name, input[3]);

            // Stamina
            StaminaIncreaseValue = NewConstant(defaultConstants[4].Name, input[4]);
            StaminaDecreaseVelocity = NewConstant(defaultConstants[5].Name, input[5]);
            StaminaStrainMultiplier = NewConstant(defaultConstants[6].Name, input[6]);
            StaminaDifficultyValue = NewConstant(defaultConstants[7].Name, input[7]);

            // Chords
            BothHandChordedMultiplier = NewConstant(defaultConstants[8].Name, input[8]);

            // Simple Jacks
            SJackUpperBoundaryMs = NewConstant(defaultConstants[9].Name, input[9]);
            SJackMaxStrainValue = NewConstant(defaultConstants[10].Name, input[10]);
            SJackCurveExponential = NewConstant(defaultConstants[11].Name, input[11]);

            // Tech Jacks
            TJackUpperBoundaryMs = NewConstant(defaultConstants[12].Name, input[12]);
            TJackMaxStrainValue = NewConstant(defaultConstants[13].Name, input[13]);
            TJackCurveExponential = NewConstant(defaultConstants[14].Name, input[14]);

            // Rolls
            RollUpperBoundaryMs = NewConstant(defaultConstants[15].Name, input[15]);
            RollMaxStrainValue = NewConstant(defaultConstants[16].Name, input[16]);
            RollCurveExponential = NewConstant(defaultConstants[17].Name, input[17]);

            // Brackets
            BracketUpperBoundaryMs = NewConstant(defaultConstants[18].Name, input[18]);
            BracketMaxStrainValue = NewConstant(defaultConstants[19].Name, input[19]);
            BracketCurveExponential = NewConstant(defaultConstants[20].Name, input[20]);

            // LN
            LnBaseMultiplier = NewConstant(defaultConstants[21].Name, input[21]);
            LnBaseValue = NewConstant(defaultConstants[22].Name, input[22]);
            LnDifficultSizeThresholdMs = NewConstant(defaultConstants[23].Name, input[23]);
            LnReleaseAfterMultiplier = NewConstant(defaultConstants[24].Name, input[24]);
            LnReleaseBeforeMultiplier = NewConstant(defaultConstants[25].Name, input[25]);
            LnTapMultiplier = NewConstant(defaultConstants[26].Name, input[26]);

            // Wrist Manipulation
            WristManipulationOffset = NewConstant(defaultConstants[27].Name, input[27]);
            WristManipulationMultiplier = NewConstant(defaultConstants[28].Name, input[28]);
        }
    }
}
