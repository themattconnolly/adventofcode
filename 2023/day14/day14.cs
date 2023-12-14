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

    // ShiftWest
    public static void ShiftWest()
    {
        bool couldMove = true;
        while(couldMove == true)
        {
            couldMove = false;
            // move all rounded rocks west unless they are blocked by a cube rock, the grid boundary, or another rock
            for(int i = 0; i < RoundedRocks.Count; i++)
            {
                Coordinate rock = RoundedRocks[i];
                if(rock.X == 0)
                {
                    // rock is at the left of the grid, so it can't move
                    continue;
                }
                else if(Grid[rock.Y][rock.X - 1] == '#')
                {
                    // rock is blocked by a cube rock
                    continue;
                }
                else if(Grid[rock.Y][rock.X - 1] == 'O')
                {
                    // rock is blocked by another rounded rock
                    continue;
                }
                else
                {
                    couldMove = true;
                    // move the rock west
                    Grid[rock.Y][rock.X] = '.';
                    Grid[rock.Y][rock.X - 1] = 'O';
                    RoundedRocks[i] = new Coordinate() { X = rock.X - 1, Y = rock.Y };
                }
            }
        }
    }

    public static void ShiftSouth()
    {
        bool couldMove = true;
        while(couldMove == true)
        {
            couldMove = false;
            // move all rounded rocks south unless they are blocked by a cube rock, the grid boundary, or another rock
            for(int i = 0; i < RoundedRocks.Count; i++)
            {
                Coordinate rock = RoundedRocks[i];
                if(rock.Y == 0)
                {
                    // rock is at the bottom of the grid, so it can't move
                    continue;
                }
                else if(Grid[rock.Y - 1][rock.X] == '#')
                {
                    // rock is blocked by a cube rock
                    continue;
                }
                else if(Grid[rock.Y - 1][rock.X] == 'O')
                {
                    // rock is blocked by another rounded rock
                    continue;
                }
                else
                {
                    couldMove = true;
                    // move the rock south
                    Grid[rock.Y][rock.X] = '.';
                    Grid[rock.Y - 1][rock.X] = 'O';
                    RoundedRocks[i] = new Coordinate() { X = rock.X, Y = rock.Y - 1 };
                }
            }
        }
    }

    public static void ShiftEast()
    {
        bool couldMove = true;
        while(couldMove == true)
        {
            couldMove = false;
            // move all rounded rocks east unless they are blocked by a cube rock, the grid boundary, or another rock
            for(int i = 0; i < RoundedRocks.Count; i++)
            {
                Coordinate rock = RoundedRocks[i];
                if(rock.X == Grid[0].Length - 1)
                {
                    // rock is at the right of the grid, so it can't move
                    continue;
                }
                else if(Grid[rock.Y][rock.X + 1] == '#')
                {
                    // rock is blocked by a cube rock
                    continue;
                }
                else if(Grid[rock.Y][rock.X + 1] == 'O')
                {
                    // rock is blocked by another rounded rock
                    continue;
                }
                else
                {
                    couldMove = true;
                    // move the rock east
                    Grid[rock.Y][rock.X] = '.';
                    Grid[rock.Y][rock.X + 1] = 'O';
                    RoundedRocks[i] = new Coordinate() { X = rock.X + 1, Y = rock.Y };
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

    public static Hashtable GridStates = new Hashtable();
  
    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;
        
        long cycles = 1;
        while(cycles <= 1000000000)
        {
            if(cycles % 1000000 == 0) {
                Console.WriteLine("Cycle " + cycles);
            }
            ShiftNorth();
            ShiftWest();
            ShiftSouth();
            ShiftEast();

            //Console.WriteLine("After " + cycles + " cycles:");
            for(int i = Grid.Length - 1; i >= 0; i--)
            {
                //Console.WriteLine("Row " + (i+1).ToString().PadLeft(2) + ": " + string.Join("", Grid[i]));
            }

            // create a hash of the grid state
            string gridHash = "";
            for(int i = 0; i < Grid.Length; i++)
            {
                gridHash += string.Join("", Grid[i]);
            }

            // check if the grid state has been seen before
            if(GridStates.ContainsKey(gridHash))
            {
                // if so, we've found a cycle
                Console.WriteLine("Found a cycle at cycle " + cycles);
                // calculate the number of cycles remaining
                long remainingCycles = 1000000000 - cycles;
                // calculate the number of cycles in the cycle
                long cycleLength = cycles - (long)GridStates[gridHash];

                if(remainingCycles % cycleLength == 0)
                {
                    Console.WriteLine("Remaining cycles is a multiple of cycle length");
                    break;
                } else {
                    Console.WriteLine("Remaining cycles is not a multiple of cycle length");
                }

            } else {
                // if not, add the grid state to the hash table
                GridStates.Add(gridHash, cycles);
            }

            cycles++;
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
            part2sum += numRoundedRocks * (i + 1);
        }

        Console.WriteLine("Part 2 : " + part2sum);
        // 90982 is right!
    }

}