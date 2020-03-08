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
    ///     An expanded version of hit object that is used for QSS implementations in API/editor/calculation
    /// </summary>
    public class StrainSolverHitObject
    {
        /// <summary>
        ///     The HitObject this class is referencing
        /// </summary>
        public HitObjectInfo HitObject { get; set; }

        /// <summary>
        ///     Current Finger State of the hit object
        /// </summary>
        public FingerState FingerState { get; set; } = FingerState.None;

        /// <summary>
        ///     Current type of layering relating to LN
        /// </summary>
        public LnLayerType LnLayerType { get; set; } = LnLayerType.None;

        /// <summary>
        ///     Strain Multiplier calculated by LN difficulty
        /// </summary>
        public float LnStrainDifficulty { get; set; } = 1;

        /// <summary>
        ///     Current Strain Value for this hit object.
        /// </summary>
        public float StrainValue { get; set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="hitOb"></param>
        public StrainSolverHitObject(HitObjectInfo hitOb) => HitObject = hitOb;
    }
}
