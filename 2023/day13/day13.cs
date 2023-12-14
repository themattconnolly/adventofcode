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
            for(int i = 0; i < pattern.Rows[0].Length; i++)
            {
                pattern.Columns.Add(new char[pattern.Rows.Count]);
            }

            Console.WriteLine("Pattern:");
            for(int rowIndex = 0; rowIndex < pattern.Rows.Count; rowIndex++)
            {
                // write the contents of the row to the console
                Console.WriteLine((rowIndex + 1) + ": " + string.Join("", pattern.Rows[rowIndex]));

                for(int columnIndex = 0; columnIndex < pattern.Columns.Count; columnIndex++)
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

        long part1sum = 0;
        foreach(Pattern pattern in Patterns)
        {
            // look for two successive rows that are the same
            int horizontalMatch = GetPatternMatchResult(pattern.Rows);
            if(horizontalMatch != -1)
            {
                part1sum += 100 * horizontalMatch;
                Console.WriteLine("Found a horizontal match at row " + horizontalMatch);
            } else {
                Console.WriteLine("No horizontal match");
            }
            int verticalMatch = GetPatternMatchResult(pattern.Columns);
            if(verticalMatch != -1)
            {
                part1sum += verticalMatch;
                Console.WriteLine("Found a vertical match at column " + verticalMatch);
            } else {
                Console.WriteLine("No vertical match");}
        } 

        Console.WriteLine("Part 1 : " + part1sum);
        // 36041 is right!
    }

    private static int GetPatternMatchResult(List<char[]> rowOrColumns)
    {
        int result = -1;
        for(int rowIndex = 0; rowIndex < rowOrColumns.Count - 1; rowIndex++)
        {
            if(rowOrColumns[rowIndex].SequenceEqual(rowOrColumns[rowIndex + 1]))
            {
                Console.WriteLine("Found a match at rows " + rowIndex + " and " + (rowIndex + 1));

                int rowsFromStart = rowIndex;
                int rowsFromEnd = rowOrColumns.Count - rowIndex - 2;

                int rowsToCheck = Math.Min(rowsFromStart, rowsFromEnd);
                bool foundMismatch = false;
                for(int i = 1; i <= rowsToCheck; i++)
                {
                    int indexToCompare1 = rowIndex - i;
                    int indexToCompare2 = rowIndex + i + 1;
                    if(rowOrColumns[indexToCompare1].SequenceEqual(rowOrColumns[indexToCompare2]))
                    {
                        //Console.WriteLine("Found a match at rows " + (indexToCompare1) + " and " + (indexToCompare2));
                    }
                    else
                    {
                        Console.WriteLine("- Didn't match");
                        foundMismatch = true;
                        break;
                    }
                }

                if(!foundMismatch)
                {
                    Console.WriteLine("- Matched all the way to the end");
                    return rowIndex + 1; // this is the number of rows from the beginning
                }
            }
        }

        return result;
    }
    
    public static void RunPart2()
    {
        ParseFile();

        //Console.WriteLine("Part 2 : " + part2sum);
        // 
    }

}