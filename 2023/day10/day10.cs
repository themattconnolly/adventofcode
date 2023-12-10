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

    private static List<Coordinate> theLoop = new List<Coordinate>();

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

        theLoop.Add(new Coordinate() { X = startX, Y = startY });

        List<Coordinate> nextSteps = FindPossibleSteps(startX, startY);
        distanceFromStart++;

        theLoop.AddRange(nextSteps);
        
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
                if(tempNextSteps.Count == 0)
                {
                    // loop fragment that hopefully only happens from S
                    continue;
                }
                else if(previousSteps.Contains(tempNextSteps[0]))
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

            theLoop.AddRange(nextSteps);
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
        int part2tiles = 0;

        ParseFilePart2();
        FindLoopSteps();

        // find the boundaries of the loop
        // int minX = theLoop.Min(c => c.X);
        // int maxX = theLoop.Max(c => c.X);
        // int minY = theLoop.Min(c => c.Y);
        // int maxY = theLoop.Max(c => c.Y);

        // write to console the boundaries of the loop
        //Console.WriteLine("Min X: " + minX + ", Max X: " + maxX + ", Min Y: " + minY + ", Max Y: " + maxY);

        // write the length of the loop to console
        //Console.WriteLine("Loop length: " + theLoop.Count);

        // find the number of tiles in the loop that are not the loop
        // for(int y = minY; y <= maxY; y++)
        // {
        //     for(int x = minX; x <= maxX; x++)
        //     {
        //         if(theLoop.Contains(new Coordinate() { X = x, Y = y }) == false)
        //         {
        //             part2tiles++;
        //         }
        //     }
        // }

        // create new grid with only pipe or ground characters
        char[][] newGrid = new char[Grid.Length][];
        for(int y = 1; y < newGrid.Length; y += 2)
        {
            char[] row1 = new char[Grid[0].Length];
            char[] row2 = new char[Grid[0].Length];
            for(int x = 0; x < Grid[0].Length; x += 2)
            {
                if(theLoop.Contains(new Coordinate() { X = x, Y = y }))
                {
                    row1[x] = Grid[y][x];
                    row1[x + 1] = Grid[y][x + 1];
                    row2[x] = Grid[y - 1][x];
                    row2[x + 1] = Grid[y - 1][x + 1];
                }
                else // either ground or pipe fragment - maybe I don't clean it?
                {
                    row1[x] = '.';
                    row1[x + 1] = 'o';
                    row2[x] = 'o';
                    row2[x + 1] = 'o';
                }
            }

            newGrid[y] = row1;
            newGrid[y - 1] = row2;
        }

        Console.WriteLine("New grid:");
        // write the new grid to console
        for(int y = newGrid.Length - 1; y >= 0; y--)
        {
            Console.WriteLine(string.Join("", newGrid[y]));
        }

        // flood fill from every edge piece that is either o or . to any character that is o or .
        for(int y = 0; y < newGrid.Length; y++)
        {
            if(y == 0 || y == newGrid.Length - 1)
            {
                for(int x = 0; x < newGrid[y].Length; x++)
                {
                    if(newGrid[y][x] == 'o' || newGrid[y][x] == '.')
                    {
                        // flood fill
                        FloodFill(newGrid, x, y);
                    }
                }
            }
            else
            {
                int x = 0;
                if(newGrid[y][x] == 'o' || newGrid[y][x] == '.')
                {
                    // flood fill
                    FloodFill(newGrid, x, y);
                }

                x = newGrid[y].Length - 1;
                if(newGrid[y][x] == 'o' || newGrid[y][x] == '.')
                {
                    // flood fill
                    FloodFill(newGrid, x, y);
                }
            }
        }

        Console.WriteLine("Grid after flood :");
        // write the new grid to console
        for(int y = newGrid.Length - 1; y >= 0; y--)
        {
            Console.WriteLine(string.Join("", newGrid[y]));
        }

        // count the number of . in the grid
        for(int y = 0; y < newGrid.Length; y++)
        {
            for(int x = 0; x < newGrid[y].Length; x++)
            {
                if(newGrid[y][x] == '.')
                {
                    part2tiles++;
                }
            }
        }

        Console.WriteLine("Part 2 : " + part2tiles);
        // 1000 is too high
        // 100 is too low
        // 600 is too high
        // 350 is wrong
        // 493 is right!
    }

    private static void FloodFill(char[][] grid, int x, int y)
    {
        // if the character is not o or ., then return
        if(grid[y][x] != 'o' && grid[y][x] != '.')
        {
            return;
        }

        // if the character is o, then change it to .
        if(grid[y][x] == 'o')
        {
            grid[y][x] = ' ';
        }

        // if the character is ., then change it to o
        if(grid[y][x] == '.')
        {
            grid[y][x] = ' ';
        }

        // if the character is on the left edge, then flood fill right
        if(x == 0)
        {
            FloodFill(grid, x + 1, y);
        }
        // if the character is on the right edge, then flood fill left
        else if(x == grid[y].Length - 1)
        {
            FloodFill(grid, x - 1, y);
        }
        // if the character is on the bottom edge, then flood fill up
        else if(y == 0)
        {
            FloodFill(grid, x, y + 1);
        }
        // if the character is on the top edge, then flood fill down
        else if(y == grid.Length - 1)
        {
            FloodFill(grid, x, y - 1);
        }
        // otherwise, flood fill in all directions
        else
        {
            FloodFill(grid, x + 1, y);
            FloodFill(grid, x - 1, y);
            FloodFill(grid, x, y + 1);
            FloodFill(grid, x, y - 1);
        }
    }

    private static void ParseFilePart2()
    {
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        // get the number of lines in the file
        int numLines = 0;
        while(file.ReadLine() != null)
        {
            numLines++;
        }
        Console.WriteLine("Number of lines: " + numLines);
        Grid = new char[numLines*2][];

        // reset file to beginning
        file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

        int rowIndex = (2 * numLines) - 1;
        string line;
        while((line = file.ReadLine()) != null)
        {
            char[] row1 = new char[line.Length * 2];
            char[] row2 = new char[line.Length * 2];
            Grid[rowIndex--] = row1;
            Grid[rowIndex--] = row2;

            for(int i = 0; i < line.Length; i++)
            {
                switch(line[i])
                {
                    // for each of the pipe characters
                    case '|':
                        row1[i * 2] = '|';
                        row1[i * 2 + 1] = 'o';
                        row2[i * 2] = '|';
                        row2[i * 2 + 1] = 'o';
                        break;
                    case '-':
                        row1[i * 2] = '-';
                        row1[i * 2 + 1] = '-';
                        row2[i * 2] = 'o';
                        row2[i * 2 + 1] = 'o';
                        break;
                    case 'L':
                        row1[i * 2] = 'L';
                        row1[i * 2 + 1] = '-';
                        row2[i * 2] = 'o';
                        row2[i * 2 + 1] = 'o';
                        break;
                    case 'J':
                        row1[i * 2] = 'J';
                        row1[i * 2 + 1] = 'o';
                        row2[i * 2] = 'o';
                        row2[i * 2 + 1] = 'o';
                        break;
                    case '7':
                        row1[i * 2] = '7';
                        row1[i * 2 + 1] = 'o';
                        row2[i * 2] = '|';
                        row2[i * 2 + 1] = 'o';
                        break;
                    case 'F':
                        row1[i * 2] = 'F';
                        row1[i * 2 + 1] = '-';
                        row2[i * 2] = '|';
                        row2[i * 2 + 1] = 'o';
                        break;
                    case 'S':
                        row1[i * 2] = 'S';
                        row1[i * 2 + 1] = '-';
                        row2[i * 2] = '|';
                        row2[i * 2 + 1] = 'o';
                        startX = i * 2;
                        startY = rowIndex + 2;
                        Console.WriteLine("Found start at " + startX + ", " + startY);
                        break;
                    default:
                        row1[i * 2] = '.';
                        row1[i * 2 + 1] = 'o';
                        row2[i * 2] = 'o';
                        row2[i * 2 + 1] = 'o';
                        break;
                }
            }
        }

        for(int i = Grid.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Row " + i + ": " + string.Join("", Grid[i]));
        }
    }
}