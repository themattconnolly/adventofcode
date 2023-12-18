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
        public long sourceEnd;
        public long destinationEnd;

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
        Console.WriteLine("Mapping ints to ints for farm map: " + farmMap.name + " @" + DateTime.Now.TimeOfDay.ToString());
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
        Console.WriteLine("===== Mapping " + sourceSeeds.Count + " seeds to : " + farmMap.name + " with " + farmMap.ranges.Count + " ranges @" + DateTime.Now.TimeOfDay.ToString());
        List<Seed> mappedSeeds = new List<Seed>();
        List<Seed> unmappedSeeds = sourceSeeds;
        // foreach(Seed sourceSeed in sourceSeeds)
        // {
            //bool foundMatch = false;
            // iterate through the ranges
        int rangeIndex = 0;
        foreach(Range range in farmMap.ranges)
        {
            Console.WriteLine("Evaluating range " + rangeIndex++ + " of " + farmMap.ranges.Count + " @" + DateTime.Now.TimeOfDay.ToString());
            foreach(Seed sourceSeed in sourceSeeds)
            {
                bool foundMatch = false;
                // check if the source seed is in the range, either starting in the middle, 
                // or starting before and ending in the middle, or starting before and ending after
                long rangeEnd = range.sourceStart + range.length;
                if(sourceSeed.seedStart >= range.sourceStart && sourceSeed.seedStart < rangeEnd || // start is within range
                sourceSeed.seedEnd >= range.sourceStart && sourceSeed.seedEnd < rangeEnd || // end is within range
                sourceSeed.seedStart < range.sourceStart && sourceSeed.seedEnd >= rangeEnd) // start is before and end is after
                {
                    if(foundMatch == true)
                    {
                        throw new Exception("Found second match!"); // really hope this doesn't happen
                    }

                    // map the source seed to the destination seed and split it if necessary
                    if(sourceSeed.seedStart < range.sourceStart)
                    {
                        // starting before the range, split the seed into the before and the middle (and after if necessary)
                        // split the seed
                        Seed destinationSeed = new Seed();
                        destinationSeed.seedStart = sourceSeed.seedStart;
                        destinationSeed.seedEnd = range.sourceStart;
                        mappedSeeds.Add(destinationSeed);
                        //Console.WriteLine("Split 1 - under : " + sourceSeed.seedStart + " to " + farmMap.name + " : " + destinationSeed.seedStart + "-" + destinationSeed.seedEnd);

                        if(sourceSeed.seedEnd > rangeEnd)
                        {
                            // starts before, ends after the range, split the seed into the middle and the after
                            destinationSeed = new Seed();
                            destinationSeed.seedStart = range.destinationStart;
                            destinationSeed.seedEnd = range.destinationStart + range.length;
                            mappedSeeds.Add(destinationSeed);
                            //Console.WriteLine("Split 2 - middle : " + sourceSeed.seedEnd + " to " + farmMap.name + " : " + destinationSeed.seedStart + "-" + destinationSeed.seedEnd);

                            // split the rest of the seed
                            destinationSeed = new Seed();
                            destinationSeed.seedStart = range.sourceStart + range.length;
                            destinationSeed.seedEnd = sourceSeed.seedEnd;
                            mappedSeeds.Add(destinationSeed);
                            //Console.WriteLine("Split 3 - over : " + sourceSeed.seedEnd + " to " + farmMap.name + " : " + destinationSeed.seedStart + "-" + destinationSeed.seedEnd);
                        }
                        else
                        {
                            // starts before, ends during, map the second half of the seed
                            // e.g., source of 0 to 10, range source of 5 to 15, range destination of 25 to 35
                            destinationSeed = new Seed();
                            destinationSeed.seedStart = range.destinationStart;
                            destinationSeed.seedEnd = sourceSeed.seedEnd - range.sourceStart + range.destinationStart;
                            mappedSeeds.Add(destinationSeed);
                            //Console.WriteLine("Split 2 - remainder : " + sourceSeed.seedEnd + " to " + farmMap.name + " : " + destinationSeed.seedStart + "-" + destinationSeed.seedEnd);

                            //0 2359144752 26906322
                            //  2364061876
                        }
                    }
                    else if(sourceSeed.seedEnd >= rangeEnd)
                    {
                        // starts after the range begins, ends after it ends
                        // e.g., source of 7 to 17, range source of 5 to 15, range destination of 25 to 35
                        // map the first half of the seed
                        Seed destinationSeed = new Seed();
                        destinationSeed.seedStart = sourceSeed.seedStart - range.sourceStart + range.destinationStart;
                        destinationSeed.seedEnd = range.destinationStart + range.length;
                        mappedSeeds.Add(destinationSeed);
                        //Console.WriteLine("Split 1 - first part : " + sourceSeed.seedEnd + " to " + farmMap.name + " : " + destinationSeed.seedStart + "-" + destinationSeed.seedEnd);

                        // split the seed
                        destinationSeed = new Seed();
                        destinationSeed.seedStart = range.sourceStart + range.length;
                        destinationSeed.seedEnd = sourceSeed.seedEnd;
                        mappedSeeds.Add(destinationSeed);
                        //Console.WriteLine("Split 2 - remainder : " + sourceSeed.seedStart + " to " + farmMap.name + " : " + destinationSeed.seedStart + "-" + destinationSeed.seedEnd);
                    }
                    else
                    {
                        // must be fully within the range
                        // e.g., source of 7 to 12, range source of 5 to 15, range destination of 25 to 35
                        Seed destinationSeed = new Seed();
                        destinationSeed.seedStart = (sourceSeed.seedStart - range.sourceStart) + range.destinationStart;
                        destinationSeed.seedEnd = (sourceSeed.seedEnd - range.sourceStart) + range.destinationStart;
                        mappedSeeds.Add(destinationSeed);
                        //Console.WriteLine("Mapped all : " + sourceSeed.seedStart + "-" + sourceSeed.seedEnd + " to " + farmMap.name + " : " + destinationSeed.seedStart + "-" + destinationSeed.seedEnd);
                    }

                    foundMatch = true;
                }

                if(foundMatch == false)
                {
                    Seed destinationSeed = new Seed();
                    destinationSeed.seedStart = sourceSeed.seedStart;
                    destinationSeed.seedEnd = sourceSeed.seedEnd;
                    mappedSeeds.Add(destinationSeed);
                    //Console.WriteLine("No match found for source : " + sourceSeed.seedStart + "-" + sourceSeed.seedEnd + " in " + farmMap.name);
                }
            }
        }

        return mappedSeeds;
    }

    private static int CurrentReverseMapDestinationIndex = 0;
    public static void ReverseMappedSeeds()
    {
        FarmMap destinationFarmMap = FarmMaps[CurrentReverseMapDestinationIndex];
        FarmMap sourceFamMap = FarmMaps[CurrentReverseMapDestinationIndex - 1];
        List<Seed> mappedSeeds = new List<Seed>();
        
        List<Range> destinationRanges = destinationFarmMap.ranges.OrderBy(r => r.destinationStart).ToList();
        foreach(Range destinationRange in destinationRanges) // locations
        {
            // find all source ranges that overlap the destination range
            // source destination of 4 to 9, destination source of 0 to 10
            // source destination of 4 to 9, destination source of 5 to 6
            // source destination of 4 to 9, destination source of 3 to 5
            // source destination of 4 to 9, destination source of 8 to 10
            // source destination of 4 to 9, destination source of 10 to 11
            // source destination of 4 to 9, destination source of 1 to 2
            List<Range> sourceRanges = sourceFamMap.ranges.Where(sourceRange => 
                sourceRange.destinationStart <= destinationRange.sourceEnd && 
                sourceRange.destinationEnd >= destinationRange.sourceStart).ToList(); // 

            if(sourceRanges.Count > 0)
            {
                // do stuff
                //Console.WriteLine("Found " + sourceRanges.Count + " source ranges for destination range " + destinationRange.destinationStart + "-" + (destinationRange.destinationStart + destinationRange.length));

                // if any of these source ranges map all the way back to a seed, return that seed. The first match will be the lowest location.
                foreach(Range sourceRange in sourceRanges)
                {
                    Range overlapRange = new Range();
                    overlapRange.destinationStart = Math.Max(sourceRange.destinationStart, destinationRange.sourceStart);
                    overlapRange.destinationEnd = Math.Min(sourceRange.destinationEnd, destinationRange.sourceEnd);
                    overlapRange.sourceStart = sourceRange.sourceStart + (overlapRange.destinationStart - sourceRange.destinationStart);
                    overlapRange.sourceEnd = sourceRange.sourceEnd - (sourceRange.destinationEnd - overlapRange.destinationEnd);
                    // if any of these source ranges map all the way back to a seed, return that seed. The first match will be the lowest location.
                    long result = ReverseMappedSeedsHelper(CurrentReverseMapDestinationIndex - 2, overlapRange);
                    if(result != -1)
                    {
                        long winningLocation = result + (destinationRange.destinationStart - destinationRange.sourceStart);
                        // Winning destination range! Find the overlap
                        Console.WriteLine("Found winning location: " + winningLocation);
                        return;
                    }
                }
            }
        }
    }

    public static long ReverseMappedSeedsHelper(int reverseMapDestinationIndex, Range destinationRange)
    {

        if(reverseMapDestinationIndex == -1)
        {
            // do we have a seed that maps all the way back to the beginning?
            // 
            List<Seed> matchingSeeds = SeedSets.Where(seedSet => 
                seedSet.seedStart <= destinationRange.sourceEnd && 
                seedSet.seedEnd >= destinationRange.sourceStart).OrderBy(x => x.seedStart).ToList();

            if(matchingSeeds.Count > 0)
            {
                long result = Math.Max(matchingSeeds[0].seedStart, destinationRange.destinationStart);
                return result;
            }

            return -1;
        }
        else
        {   
            FarmMap sourceFamMap = FarmMaps[reverseMapDestinationIndex];
            Console.WriteLine("Looking for matches in " + sourceFamMap.name);
        
            List<Range> sourceRanges = sourceFamMap.ranges.Where(sourceRange => 
                sourceRange.destinationStart <= destinationRange.sourceEnd && 
                sourceRange.destinationEnd >= destinationRange.sourceStart).ToList();

            if(sourceRanges.Count > 0)
            {
                // do stuff
                Console.WriteLine("Found " + sourceRanges.Count + " source ranges for destination range " + destinationRange.destinationStart + "-" + (destinationRange.destinationStart + destinationRange.length));

                foreach(Range sourceRange in sourceRanges)
                {
                    Range overlapRange = new Range();
                    overlapRange.destinationStart = Math.Max(sourceRange.destinationStart, destinationRange.sourceStart);
                    overlapRange.destinationEnd = Math.Min(sourceRange.destinationEnd, destinationRange.sourceEnd);
                    overlapRange.sourceStart = sourceRange.sourceStart + (overlapRange.destinationStart - sourceRange.destinationStart);
                    overlapRange.sourceEnd = sourceRange.sourceEnd - (sourceRange.destinationEnd - overlapRange.destinationEnd);

                    // if any of these source ranges map all the way back to a seed, return that seed. The first match will be the lowest location.
                    long result = ReverseMappedSeedsHelper(reverseMapDestinationIndex - 1, overlapRange);

                    if(result != -1)
                    {
                        // result is within overlaptRange
                        result = result + (destinationRange.destinationStart - destinationRange.sourceStart);
                        return result;
                    }
                }
            }
        }

        return -1;
    }

    private static List<FarmMap> FarmMaps = new List<FarmMap>();

    private static List<Seed> SeedSets = new List<Seed>();

    public static void RunPart2()
    {
        Help1.Run();
    }

    public static void RunPart2_bad()
    {
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        long part1sum = 0;

        //List<FarmMap> farmMaps = new List<FarmMap>();
        // parse the first line into the n-digit seeds: seeds: 79 14 55 13
        line = file.ReadLine();

        //List<long> seeds = new List<long>();
        
        long maxSeed = 0;
        MatchCollection seedMatches = Regex.Matches(line, @"(\d+) (\d+)");
        foreach(Match seedMatch in seedMatches)
        {
            long seed = long.Parse(seedMatch.Groups[1].Value);
            long range = long.Parse(seedMatch.Groups[2].Value);

            Seed newSeed = new Seed();
            newSeed.seedStart = seed;
            newSeed.seedEnd = seed + range - 1; // -1 because the end is inclusive
            SeedSets.Add(newSeed);

            if(newSeed.seedEnd > maxSeed)
            {
                maxSeed = newSeed.seedEnd;
            }

            Console.WriteLine("Found seed: " + string.Format("{0:n0}", newSeed.seedStart) + " to " + string.Format("{0:n0}", newSeed.seedEnd));

            // for(long i = seed; i < seed + range; i++)
            // {
            //     seeds.Add(i);
            // }
        }

        Console.WriteLine("Found seed sets: " + SeedSets.Count + " with max seed of " + string.Format("{0:n0}", maxSeed));
        //Console.WriteLine("Found seeds: " + seeds.Count);

        // parse multiple lines separated by blank lines, where the first line is the name of the farm map, and the rest of the lines are the ranges
        long maxRangeDestination = 0;
        long maxRangeSource = 0;
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
                    range.destinationEnd = range.destinationStart + range.length - 1; // -1 because the end is inclusive
                    range.sourceEnd = range.sourceStart + range.length - 1; // -1 because the end is inclusive
                    // add the range to the farm map
                    farmMap.ranges.Add(range);
                    //Console.WriteLine("Found range: " + range.sourceStart + " " + range.destinationStart + " " + range.length);

                    if(range.destinationStart + range.length > maxRangeDestination)
                    {
                        Console.WriteLine("New max range destination: " + string.Format("{0:n0}", range.destinationStart) + "-" + string.Format("{0:n0}", (range.destinationStart + range.length)));
                        maxRangeDestination = range.destinationStart + range.length;
                    }
                    if(range.sourceStart + range.length > maxRangeSource)
                    {
                        Console.WriteLine("New max range source: " + string.Format("{0:n0}", range.sourceStart) + "-" + string.Format("{0:n0}", (range.sourceStart + range.length)));
                        maxRangeSource = range.sourceStart + range.length;
                    }
                }
                // sort by the source start
                //farmMap.ranges = farmMap.ranges.OrderBy(x => x.sourceStart).ToList();
                // add the farm map to the list
                FarmMaps.Add(farmMap);

                Console.WriteLine("Found farm map name: " + farmMap.name + " with " + farmMap.ranges.Count + " ranges");
            }
        }

        // console write maxRangeDestination nicely formatted
        Console.WriteLine("Max range destination: " + string.Format("{0:n0}", maxRangeDestination));

        //Console.WriteLine("Max range destination: " + string.Format(maxRangeDestination, "N0"));

        file.Close();

        long maxRange = Math.Max(maxSeed, Math.Max(maxRangeDestination, maxRangeSource));
        //BitArray pots = new BitArray()
        // iterate through the farm maps
        // List<long> mappedInts = new List<long>();
        // mappedInts = seeds;
        // foreach(FarmMap farmMap in farmMaps)
        // {
        //     // map the ints to ints
        //     //mappedInts = MapIntsToInts(mappedInts, farmMap);
        // }

        List<Seed> mappedSeeds = new List<Seed>();
        mappedSeeds = SeedSets;
        foreach(FarmMap farmMap in FarmMaps)
        {
            //mappedSeeds = MapSeeds(mappedSeeds, farmMap);

            // Console.WriteLine("Are you sure you want to continue? (y/n)");
            // string input = Console.ReadLine();
            // if(input != "y")
            // {
            //     return;
            // }
        }

        // go backwards from the last farmmap to the first, mapping the seeds to seeds
        //List<
        // for(int i = farmMaps.Count - 1; i >= 0; i--)
        // {

        // }

        CurrentReverseMapDestinationIndex = FarmMaps.Count - 1;
        ReverseMappedSeeds();

        // long lowMappedSeed = long.MaxValue;
        // foreach(Seed mappedSeed in mappedSeeds)
        // {
        //     if(mappedSeed.seedStart < lowMappedSeed)
        //     {
        //         lowMappedSeed = mappedSeed.seedStart;
        //     }
        // }

        // // find the low mappedInt
        // long lowMappedInt = long.MaxValue;
        // foreach(long mappedInt in mappedInts)
        // {
        //     if(mappedInt < lowMappedInt)
        //     {
        //         lowMappedInt = mappedInt;
        //     }
        // }

        //Console.WriteLine("Low location: " + lowMappedSeed);
        // 0 is not right
        // 60756547 is too high
        // 4917124 is right! Thanks Mic1010
    }
}