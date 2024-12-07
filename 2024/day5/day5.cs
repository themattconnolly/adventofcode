using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2024;
public class Day5
{
    private static List<Tuple<int,int>> Rules = new System.Collections.Generic.List<Tuple<int, int>>();

    private static List<List<int>> Updates = new System.Collections.Generic.List<List<int>>();

    private static void ParseFile()
    {
        string filename = @"C:\projects\adventofcode\2024\day5\input.txt";

        System.IO.StreamReader file = new System.IO.StreamReader(filename);

        // read in the rules, formatted like dd|dd until a blank line is reached
        string line;
        while ((line = file.ReadLine()) != null)
        {
            if (line == "")
            {
                break;
            }

            string[] parts = line.Split("|");

            Rules.Add(new Tuple<int, int>(int.Parse(parts[0]), int.Parse(parts[1])));
        }

        while ((line = file.ReadLine()) != null)
        {
            // read in the updates, formatted like "dd,dd,dd" but could be any odd number until the end of the file
            string[] parts = line.Split(",");
            List<int> update = new List<int>();
            foreach (string part in parts)
            {
                update.Add(int.Parse(part));
            }
            Updates.Add(update);
        }

        file.Close();
    }

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;

        // for each update, check if it is valid
        foreach (List<int> update in Updates)
        {
            // find any instances of Rules that have both values in the Update
            // contains with two different values

            List<Tuple<int,int>> applicableRules = Rules.Where(r => update.Contains(r.Item1) && update.Contains(r.Item2)).ToList();
            
            bool validUpdate = true;

            // for each applicable rule, confirm that the first value is before the second value
            foreach (Tuple<int,int> rule in applicableRules)
            {
                if (update.IndexOf(rule.Item1) > update.IndexOf(rule.Item2))
                {
                    validUpdate = false;
                    break;
                }
            }

            if (validUpdate)
            {
                // find the middle value of the update
                int odd = update.Count() % 2;
                if(odd == 1)
                {
                    int middleIndex = (update.Count() - 1) / 2;
                    part1sum += update[middleIndex];
                }
                else
                {
                    throw new Exception("Not Odd!");
                }
            }
        }

        Console.WriteLine("Part 1 : " + part1sum);
    }

    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        // for each update, check if it is valid
        foreach (List<int> update in Updates)
        {
            // find any instances of Rules that have both values in the Update
            // contains with two different values

            List<Tuple<int, int>> applicableRules = Rules.Where(r => update.Contains(r.Item1) && update.Contains(r.Item2)).ToList();

            var fixedUpdate = ValidUpdate(update, applicableRules, 0);

            if (fixedUpdate != update)
            {
                // find the middle value of the update
                int middleIndex = (fixedUpdate.Count() - 1) / 2;
                part2sum += fixedUpdate[middleIndex];
            }
        }

        Console.WriteLine("Part 2 : " + part2sum);
    }

    private static List<int> ValidUpdate(List<int> update, List<Tuple<int,int>> applicableRules, int swaps) 
    {
        bool validUpdate = true;

        if (swaps > 100)
        {
            throw new Exception("Too Many Swaps!");
        }

        // if every rule is valid, return the update
        // for each applicable rule, confirm that the first value is before the second value
        foreach (Tuple<int, int> rule in applicableRules)
        {
            if (update.IndexOf(rule.Item1) > update.IndexOf(rule.Item2))
            {
                validUpdate = false;
                
                // swap these values and run this again
                List<int> revisedUpdate = update.ToList();
                revisedUpdate[update.IndexOf(rule.Item1)] = update[update.IndexOf(rule.Item2)];
                revisedUpdate[update.IndexOf(rule.Item2)] = update[update.IndexOf(rule.Item1)];

                return ValidUpdate(revisedUpdate, applicableRules, swaps++);
            }
        }

        if (validUpdate)
        {
            return update;
        }
        else
        {
            throw new Exception("Not Valid!");
        }
    }
}