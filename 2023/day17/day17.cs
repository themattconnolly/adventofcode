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
                row[i] = int.Parse(line[i].ToString());
            }
            Grid[rowIndex--] = row;
        }

        file.Close();

        for(int i = Grid.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Row " + i.ToString().PadLeft(2) + ": " + string.Join("", Grid[i]));
        }    
    }

    public static int[][] MinHeatLoss = new int[256][];
    
    public static void RunPart1()
    {
        ParseFile();

        // initialize MinHeatLoss
        MinHeatLoss = new int[Grid.Length][];
        for(int i = 0; i < MinHeatLoss.Length; i++)
        {
            MinHeatLoss[i] = new int[Grid[i].Length];
            for(int j = 0; j < MinHeatLoss[i].Length; j++)
            {
                MinHeatLoss[i][j] = int.MaxValue;
            }
        }

        FindNextStep(0, Grid.Length - 1, 0, "start", 0);

        for(int i = MinHeatLoss.Length - 1; i >= 0; i--)
        {
            Console.Write("Row " + i.ToString().PadLeft(2) + ": ");
            for(int j = 0; j < MinHeatLoss[i].Length; j++)
                Console.Write("[{0,3}]", MinHeatLoss[i][j]);
            Console.WriteLine();
        }

        Console.WriteLine("MinHeatLoss:" + MinHeatLoss[0][Grid[0].Length - 1]);
       
        //Console.WriteLine("Part 1 : " + part1sum);
        // 
    }

    public static void FindNextStep(int x, int y, int heatLossSoFar, string direction, int movesInARow)
    {
        if(x < 0 || y < 0 || y >= Grid.Length || x >= Grid[y].Length)
        {
            return;
        }

        int heatLoss = heatLossSoFar + Grid[y][x];

        if(heatLoss >= MinHeatLoss[y][x])
        {
            return;
        }

        MinHeatLoss[y][x] = heatLoss;

        if(x == Grid.Length - 1 && y == 0)
        {
            return;
        }

        // if(Grid[x][y] == (int)'#')
        // {
        //     return;
        // }
        if(direction == "right" || movesInARow < 3)
            FindNextStep(x + 1, y, heatLoss, "right", movesInARow + 1);
        else
            FindNextStep(x + 1, y, heatLoss, "right", 1);
        
        if(direction == "left" || movesInARow < 3)
            FindNextStep(x - 1, y, heatLoss, "left", movesInARow + 1);
        else
            FindNextStep(x - 1, y, heatLoss, "left", 1);
        
        if(direction == "up" || movesInARow < 3)
            FindNextStep(x, y + 1, heatLoss, "up", movesInARow + 1);
        else
            FindNextStep(x, y + 1, heatLoss, "up", 1);

        if(direction == "down" || movesInARow < 3)
            FindNextStep(x, y - 1, heatLoss, "down", movesInARow + 1);
        else
            FindNextStep(x, y - 1, heatLoss, "down", 1);
    }

    
    public static void RunPart2()
    {
        ParseFile();

        
        //Console.WriteLine("Part 2 : " + part2sum);
        // 
    }

}