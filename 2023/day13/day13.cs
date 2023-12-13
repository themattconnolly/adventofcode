using System.Collections;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day13
{   
    private static string filename = "day13/input.txt";

    public class Pattern
    {
        public List<char[]> Rows { get; set; }
        public List<char[]> Columns { get; set; }
    }

    public static List<Pattern> Patterns = new List<Pattern>();

    private static void ParseFile()
    {
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        Pattern newPattern = new Pattern();
        newPattern.Rows = new List<char[]>();
        newPattern.Columns = new List<char[]>();
        Patterns.Add(newPattern);
        string line;
        while((line = file.ReadLine()) != null)
        {
            if(line == "") {
                newPattern = new Pattern();
                Patterns.Add(newPattern);
                newPattern.Rows = new List<char[]>();
                newPattern.Columns = new List<char[]>();
            }
            else
            {
                newPattern.Rows.Add(line.ToCharArray());
            }
        }

        file.Close();

        foreach(Pattern pattern in Patterns)
        {
            char[] placeHolderColumn = new char[pattern.Rows.Count];
            
            for(int i = 0; i < pattern.Rows[0].Length; i++)
            {
                pattern.Columns.Add(placeHolderColumn);
            }

            Console.WriteLine("Pattern:");
            for(int rowIndex = 0; rowIndex < pattern.Rows.Count; rowIndex++)
            {
                // write the contents of the row to the console
                Console.WriteLine((rowIndex + 1) + ": " + string.Join("", pattern.Rows[rowIndex]));

                for(int columnIndex = 0; columnIndex < pattern.Rows[rowIndex].Length; columnIndex++)
                {
                    pattern.Columns[columnIndex][rowIndex] = pattern.Rows[rowIndex][columnIndex];
                }
            }
            Console.WriteLine();
        }
    }

    
    public static void RunPart1()
    {
        ParseFile();

        foreach(Pattern pattern in Patterns)
        {
            // look for two successive rows that are the same
            bool foundMatch = false;
            for(int rowIndex = 0; rowIndex < pattern.Rows.Count - 1; rowIndex++)
            {
                if(pattern.Rows[rowIndex].SequenceEqual(pattern.Rows[rowIndex + 1]))
                {
                    Console.WriteLine("Found a match at row " + (rowIndex + 1));
                }
            }
        } 

        //Console.WriteLine("Part 1 : " + part1sum);
        //
    }
    
    public static void RunPart2()
    {
        ParseFile();

        //Console.WriteLine("Part 2 : " + part2sum);
        // 
    }

}