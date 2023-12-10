using System.Collections;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day10
{   
    private static string filename = "day10/input.txt";

    private static char[][] Grid = new char[1000][];

    // declare x, y coordinate field
    private static int startX = 0;
    private static int startY = 0;

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
            char[] row = line.ToCharArray();
            Grid[rowIndex--] = row;

            for(int i = 0; i < line.Length; i++)
            {
                if(line[i] == 'S')
                {
                    startX = i;
                    startY = rowIndex + 1;
                    Console.WriteLine("Found start at " + startX + ", " + startY);
                }

                row[i] = line[i];
            }
        }

        for(int i = Grid.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Row " + i + ": " + string.Join("", Grid[i]));
        }
    }

    private static int FindLoopSteps()
    {
        // start at the start position
        int distanceFromStart = 0;
        // traverse in both directions

        // pipe characters
        // | is a vertical pipe connecting north and south.
        // - is a horizontal pipe connecting east and west.
        // L is a 90-degree bend connecting north and east.
        // J is a 90-degree bend connecting north and west.
        // 7 is a 90-degree bend connecting south and west.
        // F is a 90-degree bend connecting south and east.
        // . is ground; there is no pipe in this tile.
        // S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.

        List<Coordinate> nextSteps = FindPossibleSteps(startX, startY);
        distanceFromStart++;
        
        List<Coordinate> previousSteps = new List<Coordinate>();

        bool foundLoop = false;
        while(foundLoop == false)
        {
            Console.WriteLine("Distance: " + distanceFromStart + ", Next steps: " + string.Join(", ", nextSteps.Select(c => c.X + ", " + c.Y)));
            List<Coordinate> nextNextSteps = new List<Coordinate>();

            foreach(Coordinate nextStep in nextSteps)
            {
                List<Coordinate> tempNextSteps = FindPossibleSteps(nextStep.X, nextStep.Y);

                // find the Coordinate that is not the previous step
                if(previousSteps.Contains(tempNextSteps[0]))
                {
                    tempNextSteps.Remove(tempNextSteps[0]);
                }
                else if(tempNextSteps.Count > 1 && previousSteps.Contains(tempNextSteps[1]))
                {
                    tempNextSteps.Remove(tempNextSteps[1]);
                }

                if(nextNextSteps.Contains(tempNextSteps[0]) == false)
                {
                    nextNextSteps.Add(tempNextSteps[0]);
                }
                else
                {
                    foundLoop = true;
                    break;
                }

                if(tempNextSteps.Count != 1)
                {
                    throw new Exception("Invalid Loop");
                }

                //nextNextSteps.Add(tempNextSteps[0]);
            }

            distanceFromStart++;
            previousSteps = nextSteps;
            nextSteps = nextNextSteps;
        }

        return distanceFromStart;
    }

    private struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    private static List<Coordinate> FindPossibleSteps(int x, int y)
    {
        List<Coordinate> possibleSteps = new List<Coordinate>();
        char currentStep = Grid[y][x];

        // check north
        if(y + 1 < Grid.Length && Grid[y + 1][x] != '.')
        {
            // if the pipe is a vertical pipe, a 7, or an F, then we can go north
            if(Grid[y + 1][x] == '|' || Grid[y + 1][x] == '7' || Grid[y + 1][x] == 'F')
            {
                // if the currentStep is S or goes north, then we can go north
                if(currentStep == 'S' || currentStep == '|' || currentStep == 'L' || currentStep == 'J')
                {
                    possibleSteps.Add(new Coordinate() { X = x, Y = y + 1 });
                }
            }
        }

        // check south
        if(y - 1 >= 0 && Grid[y - 1][x] != '.')
        {
            // if the pipe is a vertical pipe, a L, or a J, then we can go south
            if(Grid[y - 1][x] == '|' || Grid[y - 1][x] == 'L' || Grid[y - 1][x] == 'J')
            {
                // if the currentStep is S or goes south, then we can go south
                if(currentStep == 'S' || currentStep == '|' || currentStep == '7' || currentStep == 'F')
                {
                    possibleSteps.Add(new Coordinate() { X = x, Y = y - 1 });
                }
            }
        }

        // check east
        if(x + 1 < Grid[y].Length && Grid[y][x + 1] != '.')
        {
            // if the pipe is a horizontal pipe, a J, or a 7, then we can go east
            if(Grid[y][x + 1] == '-' || Grid[y][x + 1] == 'J' || Grid[y][x + 1] == '7')
            {
                // if the currentStep is S or goes east, then we can go east
                if(currentStep == 'S' || currentStep == '-' || currentStep == 'L' || currentStep == 'F')
                {
                    possibleSteps.Add(new Coordinate() { X = x + 1, Y = y });
                }
            }
        }

        // check west
        if(x - 1 >= 0 && Grid[y][x - 1] != '.')
        {
            // if the pipe is a horizontal pipe, a L, or an F, then we can go west
            if(Grid[y][x - 1] == '-' || Grid[y][x - 1] == 'L' || Grid[y][x - 1] == 'F')
            {
                // if the currentStep is S or goes west, then we can go west
                if(currentStep == 'S' || currentStep == '-' || currentStep == 'J' || currentStep == '7')
                {
                    possibleSteps.Add(new Coordinate() { X = x - 1, Y = y });
                }
            }
        }

        // write possibleSteps to console
        Console.WriteLine("- Possible steps from " + x + ", " + y + ": " + string.Join(", ", possibleSteps.Select(c => c.X + ", " + c.Y)));

        return possibleSteps;
    }

    public static void RunPart1()
    {
        ParseFile();
        int part1steps = FindLoopSteps();

        Console.WriteLine("Part 1 : " + part1steps);
        // 6773
    }

    public static void RunPart2()
    {
        int part2steps = 0;

        ParseFile();

        Console.WriteLine("Part 2 : " + part2steps);
        // 
    }
}