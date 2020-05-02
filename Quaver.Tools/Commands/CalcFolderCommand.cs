/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2017-2018 Swan & The Quaver Team <support@quavergame.com>.
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Quaver.API.Enums;
using Quaver.API.Helpers;
using Quaver.API.Maps;
using Quaver.API.Maps.Parsers;
using Quaver.Tools.Helpers;

namespace Quaver.Tools.Commands
{
    internal class CalcFolderCommand : Command
    {
        /// <summary>
        ///     The folder in which contains all the .qua or .osu files.
        /// </summary>
        public string BaseFolder { get; }

        /// <summary>
        ///
        /// </summary>
        public ModIdentifier Mods { get; }

        /// <summary>
        ///
        /// </summary>
        private string FileName;

        private float Rate;

        public string Output;

        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        public CalcFolderCommand(string[] args) : base(args)
        {
            BaseFolder = args[1];
            Mods = (ModIdentifier)Enum.Parse(typeof(ModIdentifier), args[2]);
            Rate = ModHelper.GetRateFromMods(Mods);
            FileName = args[3];

            string[] rates =
            {
                //"None",
                "Speed11X",
                "Speed12X",
                "Speed13X",
                "Speed14X",
                "Speed15X",
                "Speed09X",
                "Speed08X",
                "Speed07X",
                "Speed06X",
                "Speed05X"
            };

            if (Rate != 1)
            {
                foreach (var rate in rates)
                {
                    Mods = (ModIdentifier)Enum.Parse(typeof(ModIdentifier), rate);
                    Rate = ModHelper.GetRateFromMods(Mods);
                    Console.WriteLine($"RATE: {Rate}");
                    Execute();
                }

                File.WriteAllText($"./diff-calc-rates.txt", Output);
            }
        }

        /// <summary>
        /// </summary>
        public override void Execute()
        {
            var files = Directory.GetFiles(BaseFolder, "*.qua", SearchOption.AllDirectories).ToList();
            files.AddRange(Directory.GetFiles(BaseFolder, "*.osu", SearchOption.AllDirectories));

            var calculatedMaps = new List<Tuple<int, string, string>>();
            var startTime = DateTime.Now;

            var output = "";

            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];

                try
                {
                    var diff = -1f;
                    Qua map = null;

                    if (file.EndsWith(".qua"))
                        map = Qua.Parse(file);
                    else if (file.EndsWith(".osu"))
                    {
                        var osu = new OsuBeatmap(file);
                        map = osu.ToQua();
                        diff = osu.HitObjects.Count / Math.Max(1, (osu.HitObjects[osu.HitObjects.Count - 1].StartTime - osu.HitObjects[0].StartTime) / 1000f);
                    }

                    if (map == null)
                        continue;

                    if (map.GetKeyCount() != 4)
                        continue;

                    var diffCalc = map.SolveDifficulty(Mods);

                    Console.WriteLine($"{files.Count - i}");
                    //Console.WriteLine($"[{i}] | {map} | {diffCalc.OverallDifficulty}");
                    output += $"({Rate:#.#}x) {map}\t{Rate:#.#}\t{map.MapId}\t{diffCalc.OverallDifficulty}";
                    if (diff >= 0) output += $"\t{diff:##.##}";
                    output += "\n";

                    //Console.WriteLine(output);
                    calculatedMaps.Add(Tuple.Create(i, map.ToString(), diffCalc.OverallDifficulty.ToString(CultureInfo.InvariantCulture)));
                }
                catch (Exception e)
                {
                    continue;
                }
            }

            var table = calculatedMaps.ToStringTable(new[] {"Id", "Map", "Difficulty"}, a => a.Item1, a => a.Item2, a => a.Item3);
            Output += output;
            Console.WriteLine($"Time Elasped: { (DateTime.Now - startTime).TotalMilliseconds}ms\nOutput Directory: {Directory.GetCurrentDirectory()}");

            File.WriteAllText($"./{FileName}.txt", output);
        }
    }
}
