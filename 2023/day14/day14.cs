using System.Collections;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day14
{   
    private static string filename = "day14/input.txt";

    public struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public static char[][] Grid = new char[1000][];

    public static List<Coordinate> RoundedRocks = new List<Coordinate>();
    public static List<Coordinate> CubeRocks = new List<Coordinate>();

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
        Grid = new char[numLines][];

        // reset file to beginning
        file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        
        int rowIndex = numLines - 1;
        string line;
        while((line = file.ReadLine()) != null)
        {
            for(int i = 0; i < line.Length; i++)
            {
                if(line[i] == '#')
                {
                    CubeRocks.Add(new Coordinate() { X = i, Y = rowIndex });
                }
                else if(line[i] == 'O')
                {
                    RoundedRocks.Add(new Coordinate() { X = i, Y = rowIndex });
                }
            }
            char[] row = line.ToCharArray();
            Grid[rowIndex--] = row;
        }

        file.Close();

        for(int i = Grid.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Row " + i + ": " + string.Join("", Grid[i]));
        }

        // Console.WriteLine("Rounded Rocks:");
        //Console.WriteLine("Rounded Rocks: " + RoundedRocks.Count);

        // Console.WriteLine("Cube Rocks:");
        //Console.WriteLine("Cube Rocks: " + CubeRocks.Count);
           
    }

    public static void ShiftNorth()
    {
        bool couldMove = true;
        while(couldMove == true)
        {
            couldMove = false;
            // move all rounded rocks north unless they are blocked by a cube rock, the grid boundary, or another rock
            for(int i = 0; i < RoundedRocks.Count; i++)
            {
                Coordinate rock = RoundedRocks[i];
                if(rock.Y == Grid.Length - 1)
                {
                    // rock is at the top of the grid, so it can't move
                    continue;
                }
                else if(Grid[rock.Y + 1][rock.X] == '#')
                {
                    // rock is blocked by a cube rock
                    continue;
                }
                else if(Grid[rock.Y + 1][rock.X] == 'O')
                {
                    // rock is blocked by another rounded rock
                    continue;
                }
                else
                {
                    couldMove = true;
                    // move the rock north
                    Grid[rock.Y][rock.X] = '.';
                    Grid[rock.Y + 1][rock.X] = 'O';
                    RoundedRocks[i] = new Coordinate() { X = rock.X, Y = rock.Y + 1 };
                }
            }
    }
        
    }
    
    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;
        
        ShiftNorth();

        Console.WriteLine("After shifting north:");
        for(int i = Grid.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Row " + (i+1).ToString().PadLeft(2) + ": " + string.Join("", Grid[i]));
        }

        // iterate over each row, multiplying the number of rounded rocks by the row number, adding that to part1sum
        for(int i = 0; i < Grid.Length; i++)
        {
            int numRoundedRocks = 0;
            for(int j = 0; j < Grid[i].Length; j++)
            {
                if(Grid[i][j] == 'O')
                {
                    numRoundedRocks++;
                }
            }
            part1sum += numRoundedRocks * (i + 1);
        }

        Console.WriteLine("Part 1 : " + part1sum);
        // 110274 is right
    }

  
    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;
        

        Console.WriteLine("Part 2 : " + part2sum);
        // 
    }

}