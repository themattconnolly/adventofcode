using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2025;
public class Day7
{
    private static char[][] Grid = new char[10][];
    private static long[][] Part2Grid = new long[10][];

    private static void ParseFile()
    {
        string filename = System.IO.Path.Combine(AppContext.BaseDirectory, "day7/input.txt");
        List<string> lines = new List<string>();
        using(System.IO.StreamReader file = new System.IO.StreamReader(filename))
        {
            string? line;
            while((line = file.ReadLine()) != null && line.Trim().Length > 0)
            {
                lines.Add(line);
            }
        }

        Grid = new char[lines.Count][];
        for(int i = 0; i < lines.Count; i++)
        {
            Grid[i] = lines[i].ToCharArray();
        }

        Part2Grid = new long[lines.Count][];
        for(int i = 0; i < lines.Count; i++)
        {
            Part2Grid[i] = new long[lines[i].Length];
            for(int j = 0; j < lines[i].Length; j++)
            {
                switch(lines[i][j])
                {
                    case '.':
                        Part2Grid[i][j] = 0;
                        break;
                    case 'S':
                        Part2Grid[i][j] = -2;
                        break;
                    case '^':
                        Part2Grid[i][j] = -1;
                        break;
                }
            }
        }
    }

    private static int EvaluateLine(int row)
    {
        int splits = 0;
        for(int col = 0; col < Grid[row].Length; col++)
        {
            char c = Grid[row][col];
            // do something with c
            if(c == '|')
            {
                // check below cell
                if(row + 1 < Grid.Length)
                {
                    char below = Grid[row + 1][col];
                    if(below == '.')
                    {
                        Grid[row + 1][col] = '|';
                    }
                    else if(below == '^')
                    {
                        splits++;
                        // mark both left and right below cells as |
                        if(col - 1 >= 0)
                        {
                            Grid[row + 1][col - 1] = '|';
                        }
                        if(col + 1 < Grid[row].Length)
                        {
                            Grid[row + 1][col + 1] = '|';
                        }
                    }
                }
            }

            // ignore . and ^
        }

        return splits;
    }

    private static void PrintGrid()
    {
        for(int i = 0; i < Grid.Length; i++)
        {
            Console.WriteLine(new string(Grid[i]));
        }
    }

    public static void RunPart1()
    {
        ParseFile();

        PrintGrid();

        long part1sum = 0;

        for(int col = 0; col < Grid[0].Length; col++)
        {
            char c = Grid[0][col];
            if(c == 'S')
            {
                // check below cell
                if(1 < Grid.Length)
                {
                    char below = Grid[1][col];
                    if(below == '.')
                    {
                        Grid[1][col] = '|';
                    }
                }
            }
        }

        // Console.WriteLine("");
        // Console.WriteLine("Evaluated row 0");
        // PrintGrid();

        for(int row = 1; row < Grid.Length; row++)
        {
            part1sum += EvaluateLine(row);

            // Console.WriteLine("");
            // Console.WriteLine("Evaluated row " + row);
            // PrintGrid();
        }

        Console.WriteLine("Part 1 : " + part1sum);
    }

    private static int timelines = 1;

    private static void EvaluateLinePart2(int row, char[][] grid)
    {
        for(int col = 0; col < grid[row].Length; col++)
        {
            char c = grid[row][col];
            // do something with c
            if(c == '|')
            {
                // check below cell
                if(row + 1 < grid.Length)
                {
                    char below = grid[row + 1][col];
                    if(below == '.')
                    {
                        grid[row + 1][col] = '|';
                        Console.WriteLine("Timeline continues at row " + (row + 1) + ", col " + col);
                        EvaluateLinePart2(row + 1, grid);
                    }
                    else if(below == '^')
                    {
                        timelines++;

                        // clone grid for left split
                        char[][] leftGrid = new char[grid.Length][];
                        for(int r = 0; r < grid.Length; r++)
                        {
                            leftGrid[r] = (char[])grid[r].Clone();
                        }
                        leftGrid[row + 1][col - 1] = '|';
                        Console.WriteLine("Timeline split at row " + (row + 1) + ", col " + (col - 1) + ". Total timelines: " + timelines); 
                        EvaluateLinePart2(row + 1, leftGrid);

                        // clone grid for right split
                        char[][] rightGrid = new char[grid.Length][];
                        for(int r = 0; r < grid.Length; r++)
                        {
                            rightGrid[r] = (char[])grid[r].Clone();
                        }
                        rightGrid[row + 1][col + 1] = '|';
                        Console.WriteLine("Timeline split at row " + (row + 1) + ", col " + (col - 1) + ". Total timelines: " + timelines); 
                        EvaluateLinePart2(row + 1, rightGrid);
                    }
                }
            }
        }
    }

    private static void EvaluateLinePart2(int row)
    {
        for(int col = 0; col < Part2Grid[row].Length; col++)
        {
            long l = Part2Grid[row][col];
            // do something with c
            if(l > 0) // equivalent to '|'
            {
                // check below cell
                if(row + 1 < Part2Grid.Length)
                {
                    long below = Part2Grid[row + 1][col];
                    if(below == 0) // equivalent to '.'
                    {
                        Part2Grid[row + 1][col] = l;
                    }
                    else if(below > 0) // another already forked into here
                    {
                        Part2Grid[row + 1][col] += l;
                    }
                    else if(below == -1) // equivalent to '^'
                    {
                        long belowLeft = Part2Grid[row + 1][col - 1];
                        if(belowLeft == 0)
                        {
                            Part2Grid[row + 1][col - 1] = l;
                        }
                        else if(belowLeft > 0)
                        {
                            Part2Grid[row + 1][col - 1] += l;
                        }

                        long belowRight = Part2Grid[row + 1][col + 1];
                        if(belowRight == 0)
                        {
                            Part2Grid[row + 1][col + 1] = l;
                        }
                        else if(belowRight > 0)
                        {
                            Part2Grid[row + 1][col + 1] += l;
                        }
                    }
                }
            }
        }
    }
    
    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        Console.WriteLine("");
        Console.WriteLine("Starting Part 2");
        // print grid length
        Console.WriteLine("Grid length: " + Part2Grid.Length);

        for(int col = 0; col < Part2Grid[0].Length; col++)
        {
            long c = Part2Grid[0][col];
            if(c == -2) // 'S'
            {
                // check below cell
                if(1 < Part2Grid.Length)
                {
                    long below = Part2Grid[1][col];
                    if(below == 0)
                    {
                        Part2Grid[1][col] = 1;
                    }
                }
            }
        }

        for(int row = 1; row < Part2Grid.Length; row++)
        {
            EvaluateLinePart2(row);
            
            // Console.WriteLine("");
            // Console.WriteLine("Evaluated row " + row);
            // for(int i = 0; i < Part2Grid.Length; i++)
            // {
            //     Console.WriteLine(string.Join("\t", Part2Grid[i]));
            // }
        }

        // sum up all the values in the last row of Part2Grid
        for(int col = 0; col < Part2Grid[Part2Grid.Length - 1].Length; col++)
        {
            long l = Part2Grid[Part2Grid.Length - 1][col];
            if(l > 0)
            {
                part2sum += l;
            }
        }
        
        Console.WriteLine("Part 2 : " + part2sum); // 3311 is too low (n + n-1)
    }
}

