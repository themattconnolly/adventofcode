using System.Collections;
using System.Drawing;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2024;
public class Day6
{
    private static char[][] Grid = new char[1000][];

    private static List<Point> Obstacles = new List<Point>();
    private static List<Point> GuardSteps = new List<Point>();

    private static Point GuardPosition = new Point();
    private static char GuardDirection = '^';

    private static void ParseFile()
    {
        string filename = @"C:\projects\adventofcode\2024\day6\input.txt";

        System.IO.StreamReader file = new System.IO.StreamReader(filename);

        // get the number of lines in the file
        int numLines = 0;
        while (file.ReadLine() != null)
        {
            numLines++;
        }
        Console.WriteLine("Number of lines: " + numLines);
        Grid = new char[numLines][];

        // reset file to beginning
        file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

        int rowIndex = numLines - 1;
        string line;
        while ((line = file.ReadLine()) != null)
        {
            char[] row = line.ToCharArray();
            Grid[rowIndex--] = row;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '#')
                {
                    Obstacles.Add(new Point(i, rowIndex + 1));
                }
                else if (line[i] == '^')
                {
                    GuardPosition = new Point(i, rowIndex + 1);
                    GuardSteps.Add(GuardPosition);
                }
            }
        }

        file.Close();

        PrintGrid();
    }

    private static void PrintGrid()
    {
        // print the grid to console
        for (int i = Grid.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < Grid[i].Length; j++)
            {
                Console.Write(Grid[i][j]);
            }
            Console.WriteLine();
        }
    }

    private static void MoveGuard()
    {
        bool guardInGrid = true;
        while (MoveGuardStep())
        {
        }
    }

    private static bool MoveGuardStep()
    {
        //if(GuardSteps.Count % 100 == 0)
        //{
        //    Console.WriteLine("");
        //    Console.WriteLine("Moved " + GuardSteps.Count + " steps:");
        //    PrintGrid();
        //    Console.ReadLine();
        //}

        // move the guard in the direction it is facing until it reaches an obstacle
        Point nextStep = new Point(GuardPosition.X, GuardPosition.Y);
        switch (GuardDirection)
        {
            case '^':
                nextStep.Y++;
                break;
            case 'v':
                nextStep.Y--;
                break;
            case '<':
                nextStep.X--;
                break;
            case '>':
                nextStep.X++;
                break;
        }

        if(Obstacles.Contains(nextStep))
        {
            // stop and turn
            switch (GuardDirection)
            {
                case '^':
                    GuardDirection = '>';
                    break;
                case '>':
                    GuardDirection = 'v';
                    break;
                case 'v':
                    GuardDirection = '<';
                    break;
                case '<':
                    GuardDirection = '^';
                    break;
            }

            Grid[GuardPosition.Y][GuardPosition.X] = GuardDirection;

            //Console.WriteLine("Turned at " + GuardSteps.Count + " steps");

            //if (GuardSteps.Count > 3800)
            //{
            //    Console.WriteLine("");
            //    Console.WriteLine("Turn:");
            //    PrintGrid();
            //    Console.ReadLine();
            //}
            return true;

        }
        else if(nextStep.X < 0 || nextStep.X >= Grid[0].Length || nextStep.Y < 0 || nextStep.Y >= Grid.Length)
        {
            // exit the grid
            Grid[GuardPosition.Y][GuardPosition.X] = 'X';
            return false;

            //Console.WriteLine("Leaving the grid at " + GuardSteps.Count + " steps");

            //Console.WriteLine("");
            //Console.WriteLine("FINAL Move:");
            //PrintGrid();
        }
        else
        {
            Grid[GuardPosition.Y][GuardPosition.X] = 'X';
            Grid[nextStep.Y][nextStep.X] = GuardDirection;

            GuardSteps.Add(nextStep);
            GuardPosition = nextStep;

            return true;

            //if (GuardSteps.Count > 3800)
            //{
            //    Console.WriteLine("");
            //    Console.WriteLine("Move:");
            //    PrintGrid();
            //    Console.ReadLine();
            //}

            //Console.WriteLine("Moved " + GuardSteps.Count + " steps");
        }
    }

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;

        MoveGuard();

        // count all the steps the guard took
        part1sum = GuardSteps.Distinct().Count();

        Console.WriteLine("Part 1 : " + part1sum);
    }

    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        Console.WriteLine("Part 2 : " + part2sum);
    }
}