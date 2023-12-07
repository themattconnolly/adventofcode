using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day5
{   
    private static string filename = "day5/input.txt";
    
    // class with a source range start int, destination range start int, and range length int
    public class Range
    {
        public long sourceStart;
        public long destinationStart;
        public long length;
    }

    // class with a name and a list of Range
    public class FarmMap
    {
        public string name;
        public List<Range> ranges = new List<Range>();
    }

    public class Seed
    {
        public long seedStart;
        public long seedEnd;}

    public static void RunPart1()
    {
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        long part1sum = 0;

        List<FarmMap> farmMaps = new List<FarmMap>();
        // parse the first line into the n-digit seeds: seeds: 79 14 55 13
        line = file.ReadLine();

        List<long> seeds = new List<long>();
        List<Seed> seedSets = new List<Seed>();
        
        MatchCollection seedMatches = Regex.Matches(line, @"(\d+) (\d+)");
        foreach(Match seedMatch in seedMatches)
        {
            long seed = long.Parse(seedMatch.Groups[1].Value);
            long range = long.Parse(seedMatch.Groups[2].Value);

            Seed newSeed = new Seed();
            newSeed.seedStart = seed;
            newSeed.seedEnd = seed + range;
            seedSets.Add(newSeed);

            for(long i = seed; i < seed + range; i++)
            {
                seeds.Add(i);
            }
        }

        Console.WriteLine("Found seeds: " + seeds.Count);
        // Console.WriteLine("Are you sure you want to continue? (y/n)");
        // string input = Console.ReadLine();
        // if(input != "y")
        // {
        //     return;
        // }

        // parse multiple lines separated by blank lines, where the first line is the name of the farm map, and the rest of the lines are the ranges
        while((line = file.ReadLine()) != null)
        {
            if(string.IsNullOrEmpty(line) == false)
            {
                // parse the farm map
                FarmMap farmMap = new FarmMap();
                // parse farmMap name from word after to- in line that looks like this: seed-to-soil map:
                int indexOfName = line.IndexOf("to-") + 3;
                int indexOfEndOfName = line.IndexOf(" ");
                farmMap.name = line.Substring(indexOfName, (indexOfEndOfName - indexOfName));
                Console.WriteLine("Found farm map name: " + farmMap.name);

                // parse the ranges
                while((line = file.ReadLine()) != null)
                {
                    if(string.IsNullOrEmpty(line))
                    {
                        break;
                    }

                    // parse the range from line that looks like this: 0 0 1
                    Range range = new Range();
                    string[] rangeParts = line.Split(" ");
                    range.destinationStart = long.Parse(rangeParts[0]);
                    range.sourceStart = long.Parse(rangeParts[1]);
                    range.length = long.Parse(rangeParts[2]);
                    // add the range to the farm map
                    farmMap.ranges.Add(range);
                    Console.WriteLine("Found range: " + range.sourceStart + " " + range.destinationStart + " " + range.length);
                }
                // add the farm map to the list
                farmMaps.Add(farmMap);
            }
        }

        file.Close();

        // iterate through the farm maps
        List<long> mappedInts = new List<long>();
        mappedInts = seeds;
        foreach(FarmMap farmMap in farmMaps)
        {
            // map the ints to ints
            mappedInts = MapIntsToInts(mappedInts, farmMap);
        }

        List<Seed> mappedSeeds = new List<Seed>();
        mappedSeeds = seedSets;
        foreach(FarmMap farmMap in farmMaps)
        {
            //mappedSeeds = MapSeeds(mappedSeeds, farmMap);
        }

        long lowMappedSeed = long.MaxValue;
        foreach(Seed mappedSeed in mappedSeeds)
        {
            if(mappedSeed.seedStart < lowMappedSeed)
            {
              //  lowMappedSeed = mappedSeed.seedStart;
            }
        }

        // // find the low mappedInt
        long lowMappedInt = long.MaxValue;
        foreach(long mappedInt in mappedInts)
        {
            if(mappedInt < lowMappedInt)
            {
                lowMappedInt = mappedInt;
            }
        }

        // print the low mappedInt
        Console.WriteLine("Low location: " + lowMappedInt);
        // 993500720

        //Console.WriteLine("Low location: " + lowMappedSeed);
        // 0 is not right
    }

    // map ints to ints for farmranges
    public static List<long> MapIntsToInts(List<long> ints, FarmMap farmMap)
    {
        Console.WriteLine("Mapping ints to ints for farm map: " + farmMap.name + DateTime.Now.TimeOfDay.ToString());
        List<long> mappedInts = new List<long>();
        foreach(long sourceItem in ints)
        {
            bool foundMatch = false;
            // iterate through the ranges
            foreach(Range range in farmMap.ranges)
            {
                // check if the source item is in the range
                if(sourceItem >= range.sourceStart && sourceItem < (range.sourceStart + range.length))
                {
                    // map the source item to the destination item
                    long destinationItem = sourceItem - range.sourceStart + range.destinationStart;
                    mappedInts.Add(destinationItem);
                    //Console.WriteLine("Mapped source : " + sourceItem + " to " + farmMap.name + " : " + destinationItem);
                    foundMatch = true;
                    break;
                }
            }

            if(foundMatch == false)
            {
                mappedInts.Add(sourceItem);
                //Console.WriteLine("No match found for source : " + sourceItem + " in " + farmMap.name);
            }
        }

        return mappedInts;
    }

    public static List<Seed> MapSeeds(List<Seed> sourceSeeds, FarmMap farmMap)
    {
        List<Seed> mappedSeeds = new List<Seed>();
        foreach(Seed sourceSeed in sourceSeeds)
        {
            bool foundMatch = false;
            // iterate through the ranges
            foreach(Range range in farmMap.ranges)
            {
                // check if the source seed is in the range, either starting in the middle, 
                // or starting before and ending in the middle, or starting before and ending after
                if(sourceSeed.seedStart >= range.sourceStart && sourceSeed.seedStart < (range.sourceStart + range.length) ||
                sourceSeed.seedEnd >= range.sourceStart && sourceSeed.seedEnd < (range.sourceStart + range.length) ||
                sourceSeed.seedStart < range.sourceStart && sourceSeed.seedEnd > (range.sourceStart + range.length))
                {
                    // map the source seed to the destination seed and split it if necessary
                    if(sourceSeed.seedStart < range.sourceStart)
                    {
                        // starting before the range, split the seed into the before and the middle (and after if necessary)
                        // split the seed
                        Seed destinationSeed = new Seed();
                        destinationSeed.seedStart = sourceSeed.seedStart;
                        destinationSeed.seedEnd = range.sourceStart;
                        mappedSeeds.Add(destinationSeed);
                        Console.WriteLine("Split under : " + sourceSeed.seedStart + " to " + farmMap.name + " : " + destinationSeed.seedStart);

                        if(sourceSeed.seedEnd > (range.sourceStart + range.length))
                        {
                            // starts before, ends after the range, split the seed into the middle and the after
                            destinationSeed = new Seed();
                            destinationSeed.seedStart = range.destinationStart;
                            destinationSeed.seedEnd = range.destinationStart + range.length;
                            mappedSeeds.Add(destinationSeed);
                            Console.WriteLine("Split middle : " + sourceSeed.seedEnd + " to " + farmMap.name + " : " + destinationSeed.seedStart);

                            // split the rest of the seed
                            destinationSeed = new Seed();
                            destinationSeed.seedStart = range.sourceStart + range.length;
                            destinationSeed.seedEnd = sourceSeed.seedEnd;
                            mappedSeeds.Add(destinationSeed);
                            Console.WriteLine("Split over : " + sourceSeed.seedEnd + " to " + farmMap.name + " : " + destinationSeed.seedStart);
                        }
                        else
                        {
                            // starts before, ends during, map the second half of the seed
                            // e.g., source of 0 to 10, range source of 5 to 15, range destination of 25 to 35
                            destinationSeed = new Seed();
                            destinationSeed.seedStart = range.destinationStart;
                            destinationSeed.seedEnd = sourceSeed.seedEnd - range.sourceStart + range.destinationStart;
                            mappedSeeds.Add(destinationSeed);
                            Console.WriteLine("Split remainder : " + sourceSeed.seedEnd + " to " + farmMap.name + " : " + destinationSeed.seedStart);
                        }
                    }
                    else if(sourceSeed.seedEnd > (range.sourceStart + range.length))
                    {
                        // starts after the range begins, ends after it ends
                        // e.g., source of 7 to 17, range source of 5 to 15, range destination of 25 to 35
                        // map the first half of the seed
                        Seed destinationSeed = new Seed();
                        destinationSeed.seedStart = sourceSeed.seedStart - range.sourceStart + range.destinationStart;
                        destinationSeed.seedEnd = range.destinationStart + range.length;
                        mappedSeeds.Add(destinationSeed);
                        Console.WriteLine("Split first part : " + sourceSeed.seedEnd + " to " + farmMap.name + " : " + destinationSeed.seedStart);

                        // split the seed
                        destinationSeed = new Seed();
                        destinationSeed.seedStart = range.sourceStart + range.length;
                        destinationSeed.seedEnd = sourceSeed.seedEnd;
                        mappedSeeds.Add(destinationSeed);
                        Console.WriteLine("Split remainder : " + sourceSeed.seedStart + " to " + farmMap.name + " : " + destinationSeed.seedStart);
                    }
                    else
                    {
                        // must be fully within the range
                        // e.g., source of 7 to 12, range source of 5 to 15, range destination of 25 to 35
                        Seed destinationSeed = new Seed();
                        destinationSeed.seedStart = (sourceSeed.seedStart - range.sourceStart) + range.destinationStart;
                        destinationSeed.seedEnd = (sourceSeed.seedEnd - range.sourceStart) + range.destinationStart;
                        mappedSeeds.Add(destinationSeed);
                        Console.WriteLine("Mapped source : " + sourceSeed.seedStart + " to " + farmMap.name + " : " + destinationSeed.seedStart);
                    }

                    foundMatch = true;
                }
            }

            if(foundMatch == false)
            {
                mappedSeeds.Add(sourceSeed);
                Console.WriteLine("No match found for source : " + sourceSeed.seedStart + " in " + farmMap.name);
            }
        }

        return mappedSeeds;
    }

    public static void RunPart2()
    {
        int part2sum = 0;
              
        // print the sum
        Console.WriteLine("Part 2 Sum: " + part2sum);
        // 
    }
}