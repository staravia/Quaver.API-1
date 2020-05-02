/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2017-2018 Swan & The Quaver Team <support@quavergame.com>.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Quaver.API.Maps.Processors.Difficulty.Optimization
{
    /// <summary>
    ///     Constant Variables for any specific Gamemode that the Strain Solver can use to solve.
    /// </summary>
    public class StrainConstants
    {
        /// <summary>
        ///     List of Constant Variables for the current Solver.
        /// </summary>
        public List<ConstantVariable> ConstantVariables { get; } = new List<ConstantVariable>();

        /// <summary>
        ///     Create a new constant variable for difficulty calculation and optimization.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public float NewConstant(string name, float value)
        {
            ConstantVariables.Add(new ConstantVariable(name, value));
            return value;
        }
    }
}
