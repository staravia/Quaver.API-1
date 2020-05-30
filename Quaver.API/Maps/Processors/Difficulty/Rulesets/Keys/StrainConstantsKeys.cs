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
        ///     How tolerant vibro will be detected with given delta.
        /// Non-Dyanmic Constant. Do not use for optimization.
        /// </summary>
        public float VibroDeltaToleranceMs { get; } = 30;

        /// <summary>
        ///     When seperate notes are under this threshold, it will count as a chord.
        ///     Non-Dyanmic Constant. Do not use for optimization.
        /// </summary>
        public float ChordClumpToleranceMs { get; } = 8;

        /// <summary>
        ///     Size of each graph partition in miliseconds.
        ///     Non-Dyanmic Constant. Do not use for optimization.
        /// </summary>
        public int GraphIntervalSizeMs { get; } = 500;

        /// <summary>
        ///     Offset between each graph partition in miliseconds.
        ///     Non-Dyanmic Constant. Do not use for optimization.
        /// </summary>
        public int GraphIntervalOffsetMs { get; } = 100;

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

        // LongJack Manipulation
        public float VibroActionDurationMs { get; }
        public float VibroActionToleranceMs { get; }
        public float VibroMultiplier { get; }
        public float VibroLengthMultiplier { get; }
        public float VibroMaxLength { get; }

        // Roll Manipulation
        public float RollRatioToleranceMs { get; }
        public float RollRatioMultiplier { get; }
        public float RollLengthMultiplier { get; }
        public float RollMaxLength { get; }

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

                // Overall Difficulty
                new ConstantVariable("StrainWeightOffset", 4.52f),
                new ConstantVariable("StrainWeightExponent", 12.34f),

                // Density Multiplier
                new ConstantVariable("MaxDensityBonus", 3.80f),
                new ConstantVariable("DensityBonusDuration", 226.43f),

                // Stamina
                new ConstantVariable("StaminaIncreaseValue", 0.043f),
                new ConstantVariable("StaminaDecreaseVelocity", 0.14f),
                new ConstantVariable("StaminaStrainMultiplier", 0.8f),

                // Chords
                new ConstantVariable("BothHandChordedMultiplier", 0.88f),

                // Simple Jack
                new ConstantVariable("SJackUpperBoundaryMs", 301.57f),
                new ConstantVariable("SJackMaxStrainValue", 68.40f),
                new ConstantVariable("SJackCurveExponential", 1.57f),

                // Tech Jack
                new ConstantVariable("TJackUpperBoundaryMs", 332.99f),
                new ConstantVariable("TJackMaxStrainValue", 75.14f),
                new ConstantVariable("TJackCurveExponential", 1.94f),

                // Roll/Trill
                new ConstantVariable("RollUpperBoundaryMs", 298.83f),
                new ConstantVariable("RollMaxStrainValue", 56.75f),
                new ConstantVariable("RollCurveExponential", 2.51f),

                // Bracket
                new ConstantVariable("BracketUpperBoundaryMs", 308.3f),
                new ConstantVariable("BracketMaxStrainValue", 79.47f),
                new ConstantVariable("BracketCurveExponential", 1.90f),

                // LN
                new ConstantVariable("LnBaseValue", 5.78f),
                new ConstantVariable("LnBaseMultiplier", 8.29f),
                new ConstantVariable("LnDifficultSizeThresholdMs", 281.88f),
                new ConstantVariable("LnReleaseAfterMultiplier", 1.26f),
                new ConstantVariable("LnReleaseBeforeMultiplier", 1.57f),
                new ConstantVariable("LnTapMultiplier", 1.69f),

                // LongJack Manipulation
                new ConstantVariable("VibroActionDurationMs", 156.96f),
                new ConstantVariable("VibroActionToleranceMs", 42.05f),
                new ConstantVariable("VibroMultiplier", 1.35f),
                new ConstantVariable("VibroLengthMultiplier", 0.80f),
                new ConstantVariable("VibroMaxLength", 15.06f),

                // Roll Manipulation
                new ConstantVariable("RollRatioToleranceMs", 4.66f),
                new ConstantVariable("RollRatioMultiplier", 0.54f),
                new ConstantVariable("RollLengthMultiplier", 1.20f),
                new ConstantVariable("RollMaxLength", 25.88f)
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

            // Chords
            BothHandChordedMultiplier = NewConstant(defaultConstants[7].Name, input[7]);

            // Simple Jacks
            SJackUpperBoundaryMs = NewConstant(defaultConstants[8].Name, input[8]);
            SJackMaxStrainValue = NewConstant(defaultConstants[9].Name, input[9]);
            SJackCurveExponential = NewConstant(defaultConstants[10].Name, input[10]);

            // Tech Jacks
            TJackUpperBoundaryMs = NewConstant(defaultConstants[11].Name, input[11]);
            TJackMaxStrainValue = NewConstant(defaultConstants[12].Name, input[12]);
            TJackCurveExponential = NewConstant(defaultConstants[13].Name, input[13]);

            // Rolls
            RollUpperBoundaryMs = NewConstant(defaultConstants[14].Name, input[14]);
            RollMaxStrainValue = NewConstant(defaultConstants[15].Name, input[15]);
            RollCurveExponential = NewConstant(defaultConstants[16].Name, input[16]);

            // Brackets
            BracketUpperBoundaryMs = NewConstant(defaultConstants[17].Name, input[17]);
            BracketMaxStrainValue = NewConstant(defaultConstants[18].Name, input[18]);
            BracketCurveExponential = NewConstant(defaultConstants[19].Name, input[19]);

            // LN
            LnBaseMultiplier = NewConstant(defaultConstants[20].Name, input[20]);
            LnBaseValue = NewConstant(defaultConstants[21].Name, input[21]);
            LnDifficultSizeThresholdMs = NewConstant(defaultConstants[22].Name, input[22]);
            LnReleaseAfterMultiplier = NewConstant(defaultConstants[23].Name, input[23]);
            LnReleaseBeforeMultiplier = NewConstant(defaultConstants[24].Name, input[24]);
            LnTapMultiplier = NewConstant(defaultConstants[25].Name, input[25]);

            // LongJack Manipulation
            VibroActionDurationMs = NewConstant(defaultConstants[26].Name, input[26]);
            VibroActionToleranceMs = NewConstant(defaultConstants[27].Name, input[27]);
            VibroMultiplier = NewConstant(defaultConstants[28].Name, input[28]);
            VibroLengthMultiplier = NewConstant(defaultConstants[29].Name, input[29]);
            VibroMaxLength = NewConstant(defaultConstants[30].Name, input[30]);

            // Roll Manipulation
            RollRatioToleranceMs = NewConstant(defaultConstants[31].Name, input[31]);
            RollRatioMultiplier = NewConstant(defaultConstants[32].Name, input[32]);
            RollLengthMultiplier = NewConstant(defaultConstants[33].Name, input[33]);
            RollMaxLength = NewConstant(defaultConstants[34].Name, input[34]);
        }
    }
}
