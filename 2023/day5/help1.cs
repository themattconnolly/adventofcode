using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Web;
using System.Xml;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;
using System.Reflection.PortableExecutable;
using System.Threading.Channels;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace _2023;
internal class Help1 {
    public static void Run() {
        string[] lines = readLines(2
            );
        string title = "";
        string listString;
        Dictionary<string, List<List<long>>> Maps = new Dictionary<string, List<List<long>>>();
        List<List<long>> mapLists = new List<List<long>>();
        List<long> mapList = new List<long>();
        List<long> seedDestination = new List<long>();
        List<long> minDestinationSeeds = new List<long>();
        List<long> seedsPairRanges = new List<long>();
        string seedsString;
        Dictionary<long, List<long>> SeedDictionary = new Dictionary<long, List<long>>(); // sF, sC, mapLevel
        Dictionary<long, List<long>> tempSeedDictionary = new Dictionary<long, List<long>>();
        int SeedDictionaryCount = 0;
        long lowestSetSeed = 0;


        //	┏(-_ -)┛┗(-_ - )┓┗(-_ -)┛┏(-_ -)┓ ┏(-_ -)┛┗(-_ - )┓┗(-_ -)┛┏(-_ -)┓ ┏(-_ -)┛┗(-_ - )┓┗(-_ -)┛┏(-_ -)┓


        // creates dictionary for the maps
        mapLists = new List<List<long>>();
        for (int line = 1; line < lines.Length; line++) {
            if (lines[line] == "") {
                line++;
                title = lines[line];
                Regex pattern = new Regex("[-: ]");
                title = pattern.Replace(title, string.Empty);
                mapLists = new List<List<long>>();
                Maps.Add(title, mapLists);
                line++;

            }
            listString = lines[line];
            mapList = listString.Trim().Split(' ').ToList().Where(x => x != "").Select(y => long.Parse(y)).ToList();
            Maps[title].Add(mapList);
        }

        // Extracts the seed pair ranges
        seedsString = lines[0].Substring(lines[0].IndexOf(':') + 1);
        seedsPairRanges = seedsString.Trim().Split(' ').ToList().Where(x => x != "").Select(y => long.Parse(y)).ToList();

        // traverse maps with seed ranges
        for (int seedPairRange = 0; seedPairRange < seedsPairRanges.Count; seedPairRange += 2) {
            long seedFloor = seedsPairRanges[seedPairRange];
            long seedCeiling = seedsPairRanges[seedPairRange] + seedsPairRanges[seedPairRange + 1] - 1;
            var seed_Ceiling_Level = new List<long> { seedFloor, seedCeiling, 0 };
            SeedDictionary.Clear();
            SeedDictionary.Add(0, seed_Ceiling_Level);


            for (int level = 0; level < 7; level++) {
                SeedDictionaryCount = SeedDictionary.Count;
                for (int seedAt = 0; seedAt < SeedDictionaryCount; seedAt++) {
                    tempSeedDictionary = tranverseMap(SeedDictionary.ElementAt(seedAt).Key, SeedDictionary, Maps, level);
                    foreach (var item2 in tempSeedDictionary) {
                        if (item2.Key < SeedDictionary.Count) {
                            SeedDictionary[item2.Key] = tempSeedDictionary[item2.Key];
                        }

                        if (item2.Key >= SeedDictionary.Count) {
                            SeedDictionary.Add(item2.Key, item2.Value);
                        }
                    }
                }
                Console.WriteLine("\n========\nItems in SeedDictionary: ");
                foreach (var item in SeedDictionary) {
                    Console.Write($" Seed: {item.Key} sF: {item.Value[0]} sC:{item.Value[1]} l:{item.Value[2]}   \n");
                }
            }

            lowestSetSeed = SeedDictionary.ElementAt(0).Value[0];
            for (int seedAt = 0; seedAt < SeedDictionary.Count; seedAt++) {
                if (SeedDictionary.ElementAt(seedAt).Value[0] < lowestSetSeed) {
                    lowestSetSeed = SeedDictionary.ElementAt(seedAt).Value[0];
                }
            }
            minDestinationSeeds.Add(lowestSetSeed);
            Console.WriteLine($"Lowest Seed in this round is: {lowestSetSeed}");
        }
        Console.WriteLine($"The lowest seed overall is {minDestinationSeeds.Min()}");
    }

    private static string[] readLines(int fileNumber, bool print = false) {
        Dictionary<int, string> files = new Dictionary<int, string> {					// import txt file
            {1, @"day5/input.txt"},
            {2, @"day5/input.txt"}
        };
        string[] lines = System.IO.File.ReadAllLines(files[fileNumber]);
        if (print == true) { foreach (string l in lines) { Console.WriteLine(l); } }
        return lines;
    }

    public static Dictionary<long, List<long>> tranverseMap(long Seedset, Dictionary<long, List<long>> seeds,
                                                                    Dictionary<string, List<List<long>>> mapDictionary, int mapLevelIndex, bool print = true) {

        Dictionary<long, List<long>> processedSeeds = new Dictionary<long, List<long>>();

        long SeedToProcessFloor = seeds[Seedset][0];
        long SeedToProcessCeiling = seeds[Seedset][1];
        long currentLevel = seeds[Seedset][2];
        long workingOnSeedSet = Seedset;
        bool splitCheck = false;

        List<List<long>> mapLevel = mapDictionary.ElementAt(mapLevelIndex).Value;

        Console.Write($"\nsF {SeedToProcessFloor} sc {SeedToProcessCeiling} \ncurrentLevel {mapDictionary.ElementAt(mapLevelIndex).Key} {currentLevel} ");
        var sorted = mapLevel.OrderBy(list => list[1]);

        foreach (List<long> line in sorted) {
            long mapFloor = line[1];
            long mapCeiling = mapFloor + line[2] - 1;
            long transformation = line[0] - line[1];
            Console.Write($"\nmF {mapFloor} mC {mapCeiling} trans {transformation} currentLine ");

            if (mapFloor <= SeedToProcessFloor & SeedToProcessCeiling <= mapCeiling) {
                Console.Write($"   total + {transformation}");
                processedSeeds.Add(workingOnSeedSet, new List<long> { SeedToProcessFloor + transformation, SeedToProcessCeiling + transformation, mapLevelIndex + 1 });
                goto end;
            }

            if (mapCeiling < SeedToProcessFloor) {
                Console.Write("   Missed under");
                continue;
            }

            if (SeedToProcessCeiling < mapFloor) {
                Console.Write("   Missed over");
                processedSeeds.Add(workingOnSeedSet, new List<long> { SeedToProcessFloor, SeedToProcessCeiling, mapLevelIndex + 1 });
                goto end;
            }

            if (mapFloor <= SeedToProcessFloor & SeedToProcessFloor <= mapCeiling & mapCeiling <= SeedToProcessCeiling) {
                Console.WriteLine($"   Lower Split @ {mapCeiling} {transformation}");
                processedSeeds.Add(workingOnSeedSet, new List<long> { SeedToProcessFloor + transformation, mapCeiling + transformation, mapLevelIndex + 1 });
                workingOnSeedSet = seeds.Count - 1 + processedSeeds.Count;
                SeedToProcessFloor = mapCeiling + 1;
                Console.Write($"sF {SeedToProcessFloor} sc {SeedToProcessCeiling} remaining");
                splitCheck = true;
                continue;
            }

            if (SeedToProcessFloor <= mapFloor & mapCeiling <= SeedToProcessCeiling) {
                Console.WriteLine($"   Middle Split btwn {mapFloor} &  {mapCeiling} {transformation}");
                processedSeeds.Add(workingOnSeedSet, new List<long> { SeedToProcessFloor, mapFloor - 1, mapLevelIndex + 1 });
                workingOnSeedSet = seeds.Count - 1 + processedSeeds.Count;
                processedSeeds.Add(workingOnSeedSet, new List<long> { mapFloor + transformation, mapCeiling + transformation, mapLevelIndex + 1 });
                SeedToProcessFloor = mapCeiling + 1;
                workingOnSeedSet = seeds.Count - 1 + processedSeeds.Count;
                Console.Write($"sF {SeedToProcessFloor} sc {SeedToProcessCeiling} remaining");
                splitCheck = true;
                continue;
            }

            if (SeedToProcessFloor <= mapFloor & mapFloor <= SeedToProcessCeiling & SeedToProcessCeiling <= mapCeiling) {
                Console.WriteLine($"   Higher Split @ {mapFloor} {transformation}");
                processedSeeds.Add(workingOnSeedSet, new List<long> { SeedToProcessFloor, mapFloor - 1, mapLevelIndex + 1 });
                workingOnSeedSet = seeds.Count - 1 + processedSeeds.Count;
                processedSeeds.Add(workingOnSeedSet, new List<long> { mapFloor + transformation, SeedToProcessCeiling + transformation, mapLevelIndex + 1 });
                goto end;
            }
        }

        if (processedSeeds.Count == 0) {
            processedSeeds.Add(workingOnSeedSet, new List<long> { SeedToProcessFloor, SeedToProcessCeiling, mapLevelIndex + 1 });
        }

        if (splitCheck == true) {
            processedSeeds.Add(workingOnSeedSet, new List<long> { SeedToProcessFloor, SeedToProcessCeiling, mapLevelIndex + 1 });
        }

    end:

        Console.Write("\n          Check: orig, sF, sC, newlevel");
        foreach (KeyValuePair<long, List<long>> item in processedSeeds) {
            Console.Write($"\n                 ");
            Console.Write($"{item.Key}, ");
            foreach (long l in item.Value) {
                Console.Write($"  {l},");
            }
        }
        return processedSeeds;
    }
}