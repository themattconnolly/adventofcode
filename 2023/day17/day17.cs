using System.Collections;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day17
{   
    private static string filename = "day17/input.txt";

    public static int[][] Grid = new int[256][];

    public struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    private static void ParseFile()
    {
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        // get the number of lines in the file
        int numLines = 0;
        while(file.ReadLine() != null)
        {
            numLines++;
        }
        Console.WriteLine("Number of lines: " + numLines);
        Grid = new int[numLines][];

        // reset file to beginning
        file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        
        int rowIndex = numLines - 1;
        string line;
        while((line = file.ReadLine()) != null)
        {
            // for(int i = 0; i < line.Length; i++)
            // {
                
            // }
            int[] row = new int[line.Length];
            for(int i = 0; i < line.Length; i++)
            {
                row[i] = (int)line[i];
            }
            Grid[rowIndex--] = row;
        }

        file.Close();

        for(int i = Grid.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Row " + i.ToString().PadLeft(2) + ": " + string.Join("", Grid[i]));
        }    
    }

    
    public static void RunPart1()
    {
        ParseFile();

       
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