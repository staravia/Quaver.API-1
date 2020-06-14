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

        public float EasyActionLength { get; } = 250;

        // Density Multiplier
        public float MaxDensityBonus { get; } = 3.7f;
        public float DensityBonusDuration { get; } = 200;

        // Stamina
        public float StaminaCountValue { get; }

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

                new ConstantVariable("StrainWeightOffset", 4.925417f),
                new ConstantVariable("StrainWeightExponent", 8.854067f),
                new ConstantVariable("MaxDensityBonus", 0.5011847f),
                new ConstantVariable("DensityBonusDuration", 378.8592f),
                new ConstantVariable("StaminaCountValue", 20f),
                new ConstantVariable("BothHandChordedMultiplier", 1.021279f),
                new ConstantVariable("SJackUpperBoundaryMs", 376.5979f),
                new ConstantVariable("SJackMaxStrainValue", 89.91472f),
                new ConstantVariable("SJackCurveExponential", 4.709279f),
                new ConstantVariable("TJackUpperBoundaryMs", 490.5499f),
                new ConstantVariable("TJackMaxStrainValue", 60.53613f),
                new ConstantVariable("TJackCurveExponential", 3.023011f),
                new ConstantVariable("RollUpperBoundaryMs", 461.5932f),
                new ConstantVariable("RollMaxStrainValue", 38f),
                new ConstantVariable("RollCurveExponential", 4.046207f),
                new ConstantVariable("BracketUpperBoundaryMs", 468.5985f),
                new ConstantVariable("BracketMaxStrainValue", 38f),
                new ConstantVariable("BracketCurveExponential", 2.980352f),
                new ConstantVariable("LnBaseValue", 4.86851f),
                new ConstantVariable("LnBaseMultiplier", 0.2456147f),
                new ConstantVariable("LnDifficultSizeThresholdMs", 296.2996f),
                new ConstantVariable("LnReleaseAfterMultiplier", 3.595366f),
                new ConstantVariable("LnReleaseBeforeMultiplier", 4.426898f),
                new ConstantVariable("LnTapMultiplier", 2.064136f),
                new ConstantVariable("WristManipulationOffset", 5.752428f),
                new ConstantVariable("WristManipulationMultiplier", 0.8534786f)
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
            StaminaCountValue = NewConstant(defaultConstants[4].Name, input[4]);

            // Chords
            BothHandChordedMultiplier = NewConstant(defaultConstants[5].Name, input[5]);

            // Simple Jacks
            SJackUpperBoundaryMs = NewConstant(defaultConstants[6].Name, input[6]);
            SJackMaxStrainValue = NewConstant(defaultConstants[7].Name, input[7]);
            SJackCurveExponential = NewConstant(defaultConstants[8].Name, input[8]);

            // Tech Jacks
            TJackUpperBoundaryMs = NewConstant(defaultConstants[9].Name, input[9]);
            TJackMaxStrainValue = NewConstant(defaultConstants[10].Name, input[10]);
            TJackCurveExponential = NewConstant(defaultConstants[11].Name, input[11]);

            // Rolls
            RollUpperBoundaryMs = NewConstant(defaultConstants[12].Name, input[12]);
            RollMaxStrainValue = NewConstant(defaultConstants[13].Name, input[13]);
            RollCurveExponential = NewConstant(defaultConstants[14].Name, input[14]);

            // Brackets
            BracketUpperBoundaryMs = NewConstant(defaultConstants[15].Name, input[15]);
            BracketMaxStrainValue = NewConstant(defaultConstants[16].Name, input[16]);
            BracketCurveExponential = NewConstant(defaultConstants[17].Name, input[17]);

            // LN
            LnBaseMultiplier = NewConstant(defaultConstants[18].Name, input[18]);
            LnBaseValue = NewConstant(defaultConstants[19].Name, input[19]);
            LnDifficultSizeThresholdMs = NewConstant(defaultConstants[20].Name, input[20]);
            LnReleaseAfterMultiplier = NewConstant(defaultConstants[21].Name, input[21]);
            LnReleaseBeforeMultiplier = NewConstant(defaultConstants[22].Name, input[22]);
            LnTapMultiplier = NewConstant(defaultConstants[23].Name, input[23]);

            // Wrist Manipulation
            WristManipulationOffset = NewConstant(defaultConstants[24].Name, input[24]);
            WristManipulationMultiplier = NewConstant(defaultConstants[25].Name, input[25]);
        }
    }
}
