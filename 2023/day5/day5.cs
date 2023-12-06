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

    public static void RunPart1()
    {
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        long part1sum = 0;

        List<FarmMap> farmMaps = new List<FarmMap>();
        // parse the first line into the n-digit seeds: seeds: 79 14 55 13
        line = file.ReadLine();

        List<long> seeds = new List<long>();
        MatchCollection seedMatches = Regex.Matches(line, @"\d+");
        foreach(Match seedMatch in seedMatches)
        {
            long seed = long.Parse(seedMatch.Value);
            seeds.Add(seed);
            Console.WriteLine("Found seed: " + seed);
        }

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

        // find the low mappedInt
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

    }

    // map ints to ints for farmranges
    public static List<long> MapIntsToInts(List<long> ints, FarmMap farmMap)
    {
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
                    Console.WriteLine("Mapped source : " + sourceItem + " to " + farmMap.name + " : " + destinationItem);
                    foundMatch = true;
                    break;
                }
            }

            if(foundMatch == false)
            {
                mappedInts.Add(sourceItem);
                Console.WriteLine("No match found for source : " + sourceItem + " in " + farmMap.name);
            }
        }

        return mappedInts;
    }

    public static void RunPart2()
    {
        int part2sum = 0;
              
        // print the sum
        Console.WriteLine("Part 2 Sum: " + part2sum);
        // 
    }
}