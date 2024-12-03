using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2024;
public class Day3
{
    private static List<string> memorySections = new List<string>();

    private static void ParseFile()
    {
        string filename = @"C:\projects\adventofcode\2024\day3\input.txt";
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        while ((line = file.ReadLine()) != null)
        {
            memorySections.Add(line);
        }
        file.Close();
    }

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;

        foreach(string memorySection in memorySections)
        {
            // with regex parse out all instances of the text "mul(digits,digits)" where digits is a number OR matching "do()" or "don't()"




            MatchCollection matches = Regex.Matches(memorySection, "mul\\(\\d+,\\d+\\)");
            int subtotal = 0;
            foreach(Match match in matches)
            {
                // split the match into two numbers
                string[] numbers = match.Value.Substring(4, match.Value.Length - 5).Split(",");
                int firstNumber = int.Parse(numbers[0]);
                int secondNumber = int.Parse(numbers[1]);
                subtotal += firstNumber * secondNumber;
            }

            part1sum += subtotal;
        }

        Console.WriteLine("Part 1 : " + part1sum);
    }



    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        bool enabled = true;

        foreach (string memorySection in memorySections)
        {
            // with regex parse out all instances of the text "mul(digits,digits)" where digits is a number OR the text "do()" or the text "don't()"
            MatchCollection matches = Regex.Matches(memorySection, "mul\\(\\d+,\\d+\\)|do\\(\\)|don't\\(\\)");
            
            foreach (Match match in matches)
            {
                if (match.Value == "do()")
                {
                    enabled = true;
                }
                else if(match.Value == "don't()")
                {
                    enabled = false;
                }
                else if(enabled)
                {
                    // split the match into two numbers
                    string[] numbers = match.Value.Substring(4, match.Value.Length - 5).Split(",");
                    int firstNumber = int.Parse(numbers[0]);
                    int secondNumber = int.Parse(numbers[1]);
                    part2sum += firstNumber * secondNumber;
                }
            }
        }

        Console.WriteLine("Part 2 : " + part2sum); // 102631226 is too high
    }
}