using System;
using System.Collections.Generic;

namespace _2025;
public class Day2
{
    // store pairs of integers for ranges
    private static List<(long, long)> ranges = new List<(long, long)>();

    private static List<string> lines = new List<string>();

    private static void ParseFile()
    {
        string filename = System.IO.Path.Combine(AppContext.BaseDirectory, "day2/input.txt");
        using (var file = new System.IO.StreamReader(filename))
        {
            string? line = file.ReadLine();
            // split on commas
            foreach(string range in line.Split(',').ToList())
            {
                // split on hyphen
                string[] parts = range.Split('-');
                long start = long.Parse(parts[0]);
                long end = long.Parse(parts[1]);
                ranges.Add((start, end));
            }
        }
    }

    public static void RunPart1()
    {
        ParseFile();

        long sum = 0;
        foreach (var range in ranges)
        {
            long start = range.Item1;
            long end = range.Item2;

            for (long i = start; i <= end; i++)
            {
                string number = i.ToString();
                if(number.Length % 2 == 0)
                {
                    // check if first half matches second half
                    string firstHalf = number.Substring(0, number.Length / 2);
                    string secondHalf = number.Substring(number.Length / 2);
                    if(firstHalf == secondHalf)
                    {
                        // found a match
                        Console.WriteLine("Found matching number: " + number);
                        sum += i;
                    }
                }
            }
        }
        
        Console.WriteLine("Day 2 - Part 1: " + sum);
    }

    public static void RunPart2()
    {
        ParseFile();
        
        long sum = 0;
        foreach (var range in ranges)
        {
            long start = range.Item1;
            long end = range.Item2;

            for (long i = start; i <= end; i++)
            {
                string number = i.ToString();
                for(int j = 0; j < number.Length / 2; j++)
                {
                    string pattern = number.Substring(0, j + 1);
                    // if entire number is made of this pattern repeated
                    if(number.Length % pattern.Length == 0)
                    {
                        int repeats = number.Length / pattern.Length;

                        bool match = true;
                        for(int k = 0; k < repeats; k++)
                        {
                            if(number.Substring(k * pattern.Length, pattern.Length) != pattern)
                            {
                                match = false;
                                break;
                            }
                        }
                        
                        if(match)
                        {
                            // found a match
                            Console.WriteLine("Found matching number: " + number);
                            sum += i;
                            break;
                        }
                    }
                }
            }
        }

        Console.WriteLine("Day 2 - Part 2: " + sum);
    }
}
