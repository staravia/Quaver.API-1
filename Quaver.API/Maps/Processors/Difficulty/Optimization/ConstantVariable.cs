/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2017-2020 Swan & The Quaver Team <support@quavergame.com>.
*/

namespace Quaver.API.Maps.Processors.Difficulty.Optimization
{
    /// <summary>
    /// This is used as a constant for the difficulty processors. Having it as a struct allows new constants if it's used for optimization.
    /// </summary>
    public struct ConstantVariable
    {
        /// <summary>
        /// Value of the const variable
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Name of the const variable
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Constructor. Initialize a new constant variable with parameters.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ConstantVariable(string name, float value)
        {
            Name = name;
            Value = value;
        }
    }
}