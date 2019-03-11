/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2017-2019 Swan & The Quaver Team <support@quavergame.com>.
*/

using System;
using System.Collections.Generic;

namespace Quaver.API.Maps.Structures
{
    /// <summary>
    ///     SliderVelocities section of the .qua
    /// </summary>
    [Serializable]
    public class SliderVelocityInfo
    {
        /// <summary>
        ///     The time in milliseconds when the new SliderVelocity section begins
        /// </summary>
        public float StartTime { get; set; }

        /// <summary>
        ///     The velocity multiplier relative to the current timing section's BPM
        /// </summary>
        public float Multiplier { get; set; }

        /// <summary>
        ///     By-value comparer, auto-generated by Rider.
        /// </summary>
        private sealed class ByValueEqualityComparer : IEqualityComparer<SliderVelocityInfo>
        {
            public bool Equals(SliderVelocityInfo x, SliderVelocityInfo y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.StartTime.Equals(y.StartTime) && x.Multiplier.Equals(y.Multiplier);
            }

            public int GetHashCode(SliderVelocityInfo obj)
            {
                unchecked
                {
                    return (obj.StartTime.GetHashCode() * 397) ^ obj.Multiplier.GetHashCode();
                }
            }
        }

        public static IEqualityComparer<SliderVelocityInfo> ByValueComparer { get; } = new ByValueEqualityComparer();
    }
}
