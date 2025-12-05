using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2025;
public class Day5
{
    private static List<(long,long)> FreshIngredientRanges = new List<(long,long)>();
    private static List<long> AvailableIngredients = new List<long>();

    private static void ParseFile()
    {
        string filename = System.IO.Path.Combine(AppContext.BaseDirectory, "day5/input.txt");
        string? line;
        using(System.IO.StreamReader file = new System.IO.StreamReader(filename))
        {
            while ((line = file.ReadLine()) != null && line.Trim().Length > 0)
            {
                // parse input of format "1234-5678" into fresh ingredient ranges
                string[] parts = line.Split('-');
                FreshIngredientRanges.Add((long.Parse(parts[0]), long.Parse(parts[1])));
            }

            while ((line = file.ReadLine()) != null && line.Trim().Length > 0)
            {
                AvailableIngredients.Add(long.Parse(line.Trim()));
            }
        }
    }

    

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;
        
        foreach(var ingredient in AvailableIngredients)
        {
            bool isFresh = false;
            foreach(var range in FreshIngredientRanges)
            {
                if(ingredient >= range.Item1 && ingredient <= range.Item2)
                {
                    Console.WriteLine("Fresh ingredient found: " + ingredient + " in range " + range.Item1 + "-" + range.Item2);
                    isFresh = true;
                    part1sum++;
                    break;
                }
            }

            if(!isFresh)
            {
                Console.WriteLine("Spolied ingredient found: " + ingredient);
            }
        }

        Console.WriteLine("Part 1 : " + part1sum);
    }

    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        var orderedFreshIngredients = FreshIngredientRanges.OrderBy(r => r.Item1).ThenBy(r => r.Item2).ToList();
        var normalizedFreshIngredients = new List<(long,long)>();
        foreach(var range in orderedFreshIngredients)
        {
            if(normalizedFreshIngredients.Count() == 0)
            {
                normalizedFreshIngredients.Add(range);
                continue;
            }

            var lastRange = normalizedFreshIngredients[normalizedFreshIngredients.Count - 1];
            if(range.Item1 <= lastRange.Item2 + 1) // overlaps
            {
                // merge ranges
                normalizedFreshIngredients[normalizedFreshIngredients.Count - 1] = (lastRange.Item1, Math.Max(lastRange.Item2, range.Item2));
            }
            else
            {
                normalizedFreshIngredients.Add(range);
            }
        }

        foreach(var range in normalizedFreshIngredients)
        {
            long rangeSize = range.Item2 - range.Item1 + 1;
            part2sum += rangeSize;
        }

        Console.WriteLine("Part 2 : " + part2sum); // 442902654125346 is too high
    }
}

