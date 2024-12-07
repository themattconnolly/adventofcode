using System.Collections;
using System.Data;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2024;
public class Day7
{
    private static Dictionary<long, List<int>> equations = new Dictionary<long, List<int>>();

    private static void ParseFile()
    {
        string filename = @"C:\projects\adventofcode\2024\day7\input.txt";

        System.IO.StreamReader file = new System.IO.StreamReader(filename);

        string line;
        while ((line = file.ReadLine()) != null)
        {
            // each line is formatted like ddd: dd dd dd
            string[] parts = line.Split(":");
            long key = long.Parse(parts[0]);

            List<int> values = new List<int>();
            string[] valueParts = parts[1].Split(" ");
            foreach (string value in valueParts)
            {
                if (value != "")
                {
                    values.Add(int.Parse(value));
                }
            }

            equations.Add(key, values);
        }

        file.Close();
    }

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;

        foreach(long key in equations.Keys)
        {
            if (HasValidEquation(key))
            {
                part1sum += key;
            }
        }

        Console.WriteLine("Part 1 : " + part1sum);
    }

    private static List<string> BuildPossibleEquations(List<int> values)
    {
        List<string> newEquations = new List<string>();
        if (values.Count == 2)
        {
            newEquations.Add(values[0].ToString() + " + " + values[1].ToString());
            newEquations.Add(values[0].ToString() + " * " + values[1].ToString());
        }
        else
        {
            List<string> subEquations = BuildPossibleEquations(values.GetRange(1, values.Count - 1));
            foreach(string subEquation in subEquations)
            {
                newEquations.Add(values[0].ToString() + " + " + subEquation);
                newEquations.Add(values[0].ToString() + " * " + subEquation);
            }
        }
        return newEquations;

    }

    private static List<long> BuildPossibleAnswers(List<long> values)
    {
        List<long> answers = new List<long>();
        if (values.Count == 2)
        {
            answers.Add(values[0] + values[1]);
            answers.Add(values[0] * values[1]);
        }
        else
        {
            List<long> newAddValues = new List<long>();
            long addValue = values[0] + values[1];
            newAddValues.Add(addValue);
            newAddValues.AddRange(values.GetRange(2, values.Count - 2));
            answers.AddRange(BuildPossibleAnswers(newAddValues));

            List<long> newMultiplyValues = new List<long>();
            long multiplyValue = values[0] * values[1];
            newMultiplyValues.Add(multiplyValue);
            newMultiplyValues.AddRange(values.GetRange(2, values.Count - 2));
            answers.AddRange(BuildPossibleAnswers(newMultiplyValues));
        }
        return answers;

    }

    private static bool HasValidEquation(long key)
    {
        // build all the possible combinations of the values
        List<long> convertedInput = equations[key].Select(i => (long)i).ToList();
        List<long> possibleAnswers = BuildPossibleAnswers(convertedInput);

        if(possibleAnswers.Contains(key))
        {
            return true;
        }

        return false;
    }

    private static bool HasValidPart2Equation(long key)
    {
        // build all the possible combinations of the values
        List<long> convertedInput = equations[key].Select(i => (long)i).ToList();
        List<long> possibleAnswers = BuildPossiblePart2Answers(convertedInput);

        if (possibleAnswers.Contains(key))
        {
            return true;
        }

        return false;
    }

    private static List<long> BuildPossiblePart2Answers(List<long> values)
    {
        List<long> answers = new List<long>();
        if (values.Count == 2)
        {
            answers.Add(values[0] + values[1]);
            answers.Add(values[0] * values[1]);
            answers.Add(long.Parse(values[0].ToString() + values[1].ToString()));
        }
        else
        {
            List<long> newAddValues = new List<long>();
            long addValue = values[0] + values[1];
            newAddValues.Add(addValue);
            newAddValues.AddRange(values.GetRange(2, values.Count - 2));
            answers.AddRange(BuildPossiblePart2Answers(newAddValues));

            List<long> newMultiplyValues = new List<long>();
            long multiplyValue = values[0] * values[1];
            newMultiplyValues.Add(multiplyValue);
            newMultiplyValues.AddRange(values.GetRange(2, values.Count - 2));
            answers.AddRange(BuildPossiblePart2Answers(newMultiplyValues));

            List<long> newCombineValues = new List<long>();
            long combineValue = long.Parse(values[0].ToString() + values[1].ToString());
            newCombineValues.Add(combineValue);
            newCombineValues.AddRange(values.GetRange(2, values.Count - 2));
            answers.AddRange(BuildPossiblePart2Answers(newCombineValues));

            //List<long> newSplitValues = new List<long>();
            //List<long> possibleRemainingAnswers = BuildPossiblePart2Answers(values.GetRange(1, values.Count - 1));
            //foreach(long possibleRemainingAnswer in possibleRemainingAnswers)
            //{
            //    long newAnswer = long.Parse(values[0].ToString() + possibleRemainingAnswer.ToString());
            //    answers.Add(newAnswer);
            //}
        }
        return answers;

    }

    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        foreach (long key in equations.Keys)
        {
            if (HasValidPart2Equation(key))
            {
                part2sum += key;
            }
        }

        Console.WriteLine("Part 2 : " + part2sum);
    }

}