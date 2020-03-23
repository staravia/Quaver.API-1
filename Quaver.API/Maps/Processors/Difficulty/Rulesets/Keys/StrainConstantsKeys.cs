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
        public float StrainWeightOffset { get; }
        public float StrainWeightExponent { get; }

        // Density Multiplier
        public float MaxDensityBonus { get; }
        public float DensityBonusDuration { get; }

        // Stamina
        public float StaminaIncreaseVelocity { get; }
        public float StaminaDecreaseVelocity { get; }

        public float StaminaReliefThreshold { get; }

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
        public StrainConstantsKeys()
        {
            // Overall Difficulty
            StrainWeightOffset = NewConstant("StrainWeightOffset", 6f);
            StrainWeightExponent = NewConstant("StrainWeightExponent", 4f);

            // Density Multiplier
            MaxDensityBonus = NewConstant("MaxDensityBonus", 3.7f);
            DensityBonusDuration = NewConstant("DensityBonusDuration", 200);

            // Stamina
            StaminaIncreaseVelocity = NewConstant("StaminaIncreaseVelocity", 2.8f);
            StaminaDecreaseVelocity = NewConstant("StaminaDecreaseVelocity", 4.12f);
            StaminaReliefThreshold = NewConstant("StaminaReliefThreshold", 10f);

            // Chords
            BothHandChordedMultiplier = NewConstant("BothHandChordedMultiplier", 0.88f);

            // Simple Jack
            SJackUpperBoundaryMs = NewConstant("SJackUpperBoundaryMs", 310);
            SJackMaxStrainValue = NewConstant("SJackMaxStrainValue", 69.5f);
            SJackCurveExponential = NewConstant("SJackCurveExponential", 1.51f);

            // Tech Jack
            TJackUpperBoundaryMs = NewConstant("TJackUpperBoundaryMs", 330);
            TJackMaxStrainValue = NewConstant("TJackMaxStrainValue", 72.5f);
            TJackCurveExponential = NewConstant("TJackCurveExponential", 1.59f);

            // Roll/Trill
            RollUpperBoundaryMs = NewConstant("RollUpperBoundaryMs", 250);
            RollMaxStrainValue = NewConstant("RollMaxStrainValue", 66.5f);
            RollCurveExponential = NewConstant("RollCurveExponential", 2.09f);

            // Bracket
            BracketUpperBoundaryMs = NewConstant("BracketUpperBoundaryMs", 230);
            BracketMaxStrainValue = NewConstant("BracketMaxStrainValue", 60);
            BracketCurveExponential = NewConstant("BracketCurveExponential", 1.43f);

            // LN
            LnBaseValue = NewConstant("LnBaseValue", 2.7f);
            LnBaseMultiplier = NewConstant("LnBaseMultiplier", 4.65f);
            LnDifficultSizeThresholdMs = NewConstant("LnDifficultSizeThresholdMs", 250f);
            LnReleaseAfterMultiplier = NewConstant("LnReleaseAfterMultiplier", 1.5f);
            LnReleaseBeforeMultiplier = NewConstant("LnReleaseBeforeMultiplier", 1.15f);
            LnTapMultiplier = NewConstant("LnTapMultiplier", 1.05f);

            // LongJack Manipulation
            VibroActionDurationMs = NewConstant("VibroActionDurationMs", 88.2f);
            VibroActionToleranceMs = NewConstant("VibroActionToleranceMs", 22f);
            VibroMultiplier = NewConstant("VibroMultiplier", 0.48f);
            VibroLengthMultiplier = NewConstant("VibroLengthMultiplier", 0.3f);
            VibroMaxLength = NewConstant("VibroMaxLength", 6);

            // Roll Manipulation
            RollRatioToleranceMs = NewConstant("RollRatioToleranceMs", 2);
            RollRatioMultiplier = NewConstant("RollRatioMultiplier", 0.25f);
            RollLengthMultiplier = NewConstant("RollLengthMultiplier", 0.6f);
            RollMaxLength = NewConstant("RollMaxLength", 14);
        }
    }
}
