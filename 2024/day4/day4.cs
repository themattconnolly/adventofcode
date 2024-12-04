using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2024;
public class Day4
{
    private static char[][] Grid = new char[1000][];

    private static void ParseFile()
    {
        string filename = @"C:\projects\adventofcode\2024\day4\input.txt";

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
                //if (line[i] == 'S')
                //{
                //    startX = i;
                //    startY = rowIndex + 1;
                //    Console.WriteLine("Found start at " + startX + ", " + startY);
                //}

                row[i] = line[i];
            }
        }
    }

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;

        // for each row in the grid, find all the "X" characters
        for (int i = 0; i < Grid.Length; i++)
        {
            for (int j = 0; j < Grid[i].Length; j++)
            {
                if (Grid[i][j] == 'X')
                {
                    int XMASCount = GetXMASCount(j, i);
                    part1sum += XMASCount;
                }
            }
        }

        Console.WriteLine("Part 1 : " + part1sum);
    }

    private static int GetXMASCount(int x, int y)
    {
        int XMASCount = 0;
        // starting from the current coordinates, which is assumed to be an "X" character, check in the surrounding 8 directions to see 
        // if the next character is "M", while not going out of bounds
        if (x - 1 >= 0 && y - 1 >= 0 && Grid[y - 1][x - 1] == 'M')
        {
            // check if the next character in this direction an "A" and in bounds
            if (x - 2 >= 0 && y - 2 >= 0 && Grid[y - 2][x - 2] == 'A')
            {
                // check if the next character in this direction an "S" and in bounds
                if (x - 3 >= 0 && y - 3 >= 0 && Grid[y - 3][x - 3] == 'S')
                {
                    XMASCount++;
                }
            }
        }
        if (y - 1 >= 0 && Grid[y - 1][x] == 'M')
        {
            // check if the next two characters in this direction are "A" and "S" and in bounds
            if (y - 2 >= 0 && Grid[y - 2][x] == 'A')
            {
                if (y - 3 >= 0 && Grid[y - 3][x] == 'S')
                {
                    XMASCount++;
                }
            }
        }
        if (x + 1 < Grid[y].Length && y - 1 >= 0 && Grid[y - 1][x + 1] == 'M')
        {
            // check if the next two characters in this direction are "A" and "S" and in bounds
            if (x + 2 < Grid[y].Length && y - 2 >= 0 && Grid[y - 2][x + 2] == 'A')
            {
                if (x + 3 < Grid[y].Length && y - 3 >= 0 && Grid[y - 3][x + 3] == 'S')
                {
                    XMASCount++;
                }
            }
        }
        if (x - 1 >= 0 && Grid[y][x - 1] == 'M')
        {
            // check if the next two characters in this direction are "A" and "S" and in bounds
            if (x - 2 >= 0 && Grid[y][x - 2] == 'A')
            {
                if (x - 3 >= 0 && Grid[y][x - 3] == 'S')
                {
                    XMASCount++;
                }
            }
        }
        if (x + 1 < Grid[y].Length && Grid[y][x + 1] == 'M')
        {
            // check if the next two characters in this direction are "A" and "S" and in bounds
            if (x + 2 < Grid[y].Length && Grid[y][x + 2] == 'A')
            {
                if (x + 3 < Grid[y].Length && Grid[y][x + 3] == 'S')
                {
                    XMASCount++;
                }
            }
        }
        if (x - 1 >= 0 && y + 1 < Grid.Length && Grid[y + 1][x - 1] == 'M')
        {
            // check if the next two characters in this direction are "A" and "S" and in bounds
            if (x - 2 >= 0 && y + 2 < Grid.Length && Grid[y + 2][x - 2] == 'A')
            {
                if (x - 3 >= 0 && y + 3 < Grid.Length && Grid[y + 3][x - 3] == 'S')
                {
                    XMASCount++;
                }
            }
        }
        if (y + 1 < Grid.Length && Grid[y + 1][x] == 'M')
        {
            // check if the next two characters in this direction are "A" and "S" and in bounds
            if (y + 2 < Grid.Length && Grid[y + 2][x] == 'A')
            {
                if (y + 3 < Grid.Length && Grid[y + 3][x] == 'S')
                {
                    XMASCount++;
                }
            }
        }
        if (x + 1 < Grid[y].Length && y + 1 < Grid.Length && Grid[y + 1][x + 1] == 'M')
        {
            // check if the next two characters in this direction are "A" and "S" and in bounds
            if (x + 2 < Grid[y].Length && y + 2 < Grid.Length && Grid[y + 2][x + 2] == 'A')
            {
                if (x + 3 < Grid[y].Length && y + 3 < Grid.Length && Grid[y + 3][x + 3] == 'S')
                {
                    XMASCount++;
                }
            }
        }

        return XMASCount;
    }


    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        // for each row in the grid, find all the "X" characters
        for (int i = 0; i < Grid.Length; i++)
        {
            for (int j = 0; j < Grid[i].Length; j++)
            {
                if (Grid[i][j] == 'A')
                {
                    int crossMASCount = GetCrossMASCount(j, i);
                    if(crossMASCount == 2)
                    {
                        part2sum++;
                    }
                }
            }
        }

        Console.WriteLine("Part 2 : " + part2sum);
    }

    private static int GetCrossMASCount(int x, int y)
    {
        int MASCount = 0;


        // check if the opposite diagonal characters are "M" and "S" and in bounds
        if (x - 1 >= 0 && y - 1 >= 0 && Grid[y - 1][x - 1] == 'M')
        {
            if (x + 1 < Grid[y].Length && y + 1 < Grid.Length && Grid[y + 1][x + 1] == 'S')
            {
                MASCount++;
            }
        }
        else if (x - 1 >= 0 && y - 1 >= 0 && Grid[y - 1][x - 1] == 'S')
        {
            if (x + 1 < Grid[y].Length && y + 1 < Grid.Length && Grid[y + 1][x + 1] == 'M')
            {
                MASCount++;
            }
        }

        // check in the opposite diagonal direction
        if (x + 1 < Grid[y].Length && y - 1 >= 0 && Grid[y - 1][x + 1] == 'M')
        {
            if (x - 1 >= 0 && y + 1 < Grid.Length && Grid[y + 1][x - 1] == 'S')
            {
                MASCount++;
            }
        }
        else if (x + 1 < Grid[y].Length && y - 1 >= 0 && Grid[y - 1][x + 1] == 'S')
        {
            if (x - 1 >= 0 && y + 1 < Grid.Length && Grid[y + 1][x - 1] == 'M')
            {
                MASCount++;
            }
        }

        return MASCount;
    }
}