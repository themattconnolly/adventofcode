using System;
using System.Collections.Generic;

namespace _2025;
public class Day3
{
    private static List<string> lines = new List<string>();

    private static void ParseFile()
    {
        string filename = "day3/input.txt";
        string? line;
        using (var file = new System.IO.StreamReader(filename))
        {
            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }
    }

    public static void RunPart1()
    {
        ParseFile();
        
        int totalJoltage = 0;
        foreach (var line in lines)
        {
            // two integers representing joltage ratings
            var maxJoltageNums = (0, 0);
            for (int i = 0; i < line.Length - 1; i++)
            {
                // convert char to int
                char c = line[i];
                int num1 = int.Parse(c.ToString());
                if(num1 < maxJoltageNums.Item1)
                {
                    continue;
                }

                // evaluate the highest max joltage
                int biggestNum2 = line.Substring(i + 1).Select(ch => int.Parse(ch.ToString())).Max();

                if(num1 > maxJoltageNums.Item1)
                {
                    maxJoltageNums = (num1, biggestNum2);
                }
                else if(num1 == maxJoltageNums.Item1 && biggestNum2 > maxJoltageNums.Item2)
                {
                    maxJoltageNums = (num1, biggestNum2);
                }
            }

            int maxJoltage = maxJoltageNums.Item1 * 10 + maxJoltageNums.Item2;
            Console.WriteLine($"Max joltage: {maxJoltage}");

            totalJoltage += maxJoltage;
        }

        Console.WriteLine("Day 3 - Part 1: " + totalJoltage);
    }

    private static long FindLargestNumber(string line, int digits)
    {
        // find the biggest single digit before digits-1 from the end of the line
        if(line.Length < digits)
        {
            return -1;
        }
        if(line.Length == digits)
        {
            return long.Parse(line);
        }
        
        int biggestFirstNum = line.Take(line.Length - digits + 1).Select(ch => int.Parse(ch.ToString())).Max();
        
        if(digits == 1)
        {
            return biggestFirstNum;
        }

        long biggestNum = -1;
        // now evaluate all the possible numbers starting with biggestNum
        for(int i = 0; i <= line.Length - digits; i++)
        {
            char c = line[i];
            int num = int.Parse(c.ToString());
            if(num == biggestFirstNum)
            {
                // candidate
                long largestNextNum = FindLargestNumber(line.Substring(i + 1), digits - 1);
                long candidateNum = biggestFirstNum * (long)Math.Pow(10, digits - 1) + largestNextNum;

                if(candidateNum > biggestNum)
                {
                    biggestNum = candidateNum;
                }
            }
        }

        return biggestNum;
    }

    public static void RunPart2()
    {
        ParseFile();
        
        long totalJoltage = 0;
        foreach (var line in lines)
        {
            // find the largest 12 digit number that can be formed from the digits in the line
            long maxJoltage = FindLargestNumber(line, 12);

            Console.WriteLine($"Max joltage: {maxJoltage}");

            totalJoltage += maxJoltage;
        }

        Console.WriteLine("Day 3 - Part 2: " + totalJoltage);
    }
}
