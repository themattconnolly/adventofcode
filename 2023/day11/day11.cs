using System.Collections;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day11
{   
    private static string filename = "day11/input.txt";

    private static char[][] Grid = new char[1000][];

    public class Galaxy{
        public Coordinate Coordinate { get; set; }
        public int Number { get; set; }
        public int ShortestPath = -1;
    }

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
        Grid = new char[numLines][];

        // reset file to beginning
        file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

        int rowIndex = numLines - 1;
        string line;
        while((line = file.ReadLine()) != null)
        {
            char[] row = line.ToCharArray();
            Grid[rowIndex--] = row;

            // for(int i = 0; i < line.Length; i++)
            // {
            //     if(line[i] == 'S')
            //     {
            //         startX = i;
            //         startY = rowIndex + 1;
            //         Console.WriteLine("Found start at " + startX + ", " + startY);
            //     }

            //     row[i] = line[i];
            // }
        }

        for(int i = Grid.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Row " + i + ": " + string.Join("", Grid[i]));
        }
    }

    public static void RunPart1()
    {
        ParseFile();

        // Create new grid by duplicating rows and columns that contain only periods

        List<int> rowsToExpand = new List<int>();
        List<int> colsToExpand = new List<int>();

        // scan each row and column of the grid to see if it contains only periods
        for(int i = 0; i < Grid.Length; i++)
        {
            bool rowIsAllPeriods = true;

            for(int j = 0; j < Grid[i].Length; j++)
            {
                if(Grid[i][j] != '.')
                {
                    rowIsAllPeriods = false;
                    break;
                }
            }

            if(rowIsAllPeriods)
            {
                rowsToExpand.Add(i);
            }
        }

        for(int i = 0; i < Grid[0].Length; i++)
        {
            bool colIsAllPeriods = true;

            for(int j = 0; j < Grid.Length; j++)
            {
                if(Grid[j][i] != '.')
                {
                    colIsAllPeriods = false;
                    break;
                }
            }

            if(colIsAllPeriods)
            {
                colsToExpand.Add(i);
            }
        }

        // create new grid to store expanded grid
        char[][] newGrid = new char[Grid.Length + rowsToExpand.Count][];
        int rowsToExpandProcessed = 0;
        char[] emptyRow = new char[Grid[0].Length + colsToExpand.Count];
        for(int i = 0; i < emptyRow.Length; i++)
        {
            emptyRow[i] = '.';
        }

        for(int i = 0; i < Grid.Length; i++)
        {
            char[] newRow = new char[Grid[0].Length + colsToExpand.Count];
            int colsToExpandProcessed = 0;
            for(int j = 0; j < Grid[i].Length; j++)
            {
                if(colsToExpand.Contains(j))
                {
                    newRow[j + colsToExpandProcessed++] = '.';
                    newRow[j + colsToExpandProcessed] = '.';
                }
                else
                {
                    newRow[j + colsToExpandProcessed] = Grid[i][j];
                }
            }

            if(rowsToExpand.Contains(i))
            {
                newGrid[i + rowsToExpandProcessed++] = emptyRow;
                newGrid[i + rowsToExpandProcessed] = emptyRow;
            }
            else
            {
                newGrid[i + rowsToExpandProcessed] = newRow;
            }
        }

        Console.WriteLine("Expanded grid:");
        for(int i = newGrid.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Row " + i + ": " + string.Join("", newGrid[i]));
        }

        // find all galaxies
        int galaxyCount = 0;
        for(int i = newGrid.Length - 1; i >= 0; i--)
        {
            for(int j = 0; j < newGrid[i].Length; j++)
            {
                if(newGrid[i][j] == '#')
                {
                    Galaxy galaxy = new Galaxy();
                    galaxy.Coordinate = new Coordinate { X = j, Y = i };
                    galaxy.Number = ++galaxyCount;
                    galaxies.Add(galaxy);

                    //Console.WriteLine("Found galaxy " + galaxy.Number + " at " + galaxy.Coordinate.X + ", " + galaxy.Coordinate.Y);
                }
            }
        }

        // Console.WriteLine("Galaxies:");
        foreach(Galaxy galaxy in galaxies)
        {
            Console.WriteLine("Galaxy " + galaxy.Number + " at " + galaxy.Coordinate.X + ", " + galaxy.Coordinate.Y);
            FindPathsToGalaxies(newGrid, galaxy);
        }

        Console.WriteLine("Galaxy pairs:" + galaxyPairCache.Count);

        long part1sum = 0;
        // sum the values of the galaxies hashtable as ints
        foreach(int pairShortPath in galaxyPairCache.Values)
        {
            part1sum += pairShortPath;
        }

        Console.WriteLine("Part 1 : " + part1sum);
        // 106030 is too low
        // 10490062 is right!
    }

    private static List<Galaxy> galaxies = new List<Galaxy>();

    private static Hashtable galaxyPairCache = new Hashtable();

    // find the nearest galaxy by searching the grid, starting at the current coordinate
    // and moving outwards in a spiral pattern
    private static void FindPathsToGalaxies(char[][] grid, Galaxy galaxy)
    {
        int steps = 0;
        //bool foundAllGalaxies = false;
        int foundGalaxies = 0;
        List<Galaxy> foundGalaxiesList = new List<Galaxy>();
        while(foundGalaxiesList.Count() < galaxies.Count - 1)
        {
            steps++;
            //Console.WriteLine("Searching for galaxy " + galaxy.Number + " at " + galaxy.Coordinate.X + ", " + galaxy.Coordinate.Y + " with " + steps + " steps");
            for(int k_y = Math.Max(galaxy.Coordinate.Y - steps, 0); k_y <= Math.Min(galaxy.Coordinate.Y + steps, grid.Length); k_y++)
            {
                // make sure it is inside the grid
                if(k_y >= 0 && k_y < grid.Length)
                {
                    int max_k_x = steps - Math.Abs(k_y - galaxy.Coordinate.Y);

                    for(int k_x = Math.Max(galaxy.Coordinate.X - max_k_x, 0); k_x <= Math.Min(galaxy.Coordinate.X + max_k_x, grid[0].Length); k_x++)
                    {
                        // make sure it is inside the grid
                        if(k_x >= 0 && k_x < grid[k_y].Length && grid[k_y][k_x] == '#' && (k_x != galaxy.Coordinate.X || k_y != galaxy.Coordinate.Y))
                        {
                            Galaxy matchingGalaxy = galaxies.Where(g => g.Coordinate.X == k_x && g.Coordinate.Y == k_y).First();

                            if(foundGalaxiesList.Contains(matchingGalaxy) == false)
                            {
                                foundGalaxiesList.Add(matchingGalaxy);

                                string key = string.Concat(Math.Min(galaxy.Number, matchingGalaxy.Number),"-",Math.Max(galaxy.Number, matchingGalaxy.Number));
                                if(galaxyPairCache.ContainsKey(key) == false)
                                {
                                    galaxyPairCache.Add(key, steps); // add this pair if it hasn't been found yet
                                }
                            }
                        }
                    }
                }
            }

            if(steps > (grid.Length + grid[0].Length))
            {
                throw new Exception("Steps is too high");
            }
        }
    }
    
    public static void RunPart2()
    {
        ParseFile();

        List<int> rowsToExpand = new List<int>();
        List<int> colsToExpand = new List<int>();

        // scan each row and column of the grid to see if it contains only periods
        for(int i = 0; i < Grid.Length; i++)
        {
            bool rowIsAllPeriods = true;

            for(int j = 0; j < Grid[i].Length; j++)
            {
                if(Grid[i][j] != '.')
                {
                    rowIsAllPeriods = false;
                    break;
                }
            }

            if(rowIsAllPeriods)
            {
                rowsToExpand.Add(i);
            }
        }

        for(int i = 0; i < Grid[0].Length; i++)
        {
            bool colIsAllPeriods = true;

            for(int j = 0; j < Grid.Length; j++)
            {
                if(Grid[j][i] != '.')
                {
                    colIsAllPeriods = false;
                    break;
                }
            }

            if(colIsAllPeriods)
            {
                colsToExpand.Add(i);
            }
        }

        Console.WriteLine("Rows to expand: " + string.Join(",", rowsToExpand));
        Console.WriteLine("Cols to expand: " + string.Join(",", colsToExpand));

        // find all galaxies
        int galaxyCount = 0;
        for(int i = Grid.Length - 1; i >= 0; i--)
        {
            for(int j = 0; j < Grid[i].Length; j++)
            {
                if(Grid[i][j] == '#')
                {
                    Galaxy galaxy = new Galaxy();
                    galaxy.Coordinate = new Coordinate { X = j, Y = i };
                    galaxy.Number = ++galaxyCount;
                    galaxies.Add(galaxy);

                    //Console.WriteLine("Found galaxy " + galaxy.Number + " at " + galaxy.Coordinate.X + ", " + galaxy.Coordinate.Y);
                }
            }
        }

        long part2sum = 0;
        long rowFactor = 1000000-1;
        for(int i = 0; i < galaxies.Count; i++)
        {
            for(int j = i + 1; j < galaxies.Count; j++)
            {
                Galaxy galaxy1 = galaxies[i];
                Galaxy galaxy2 = galaxies[j];

                long columnsPassed = colsToExpand.Where(c => (c > galaxy1.Coordinate.X && c < galaxy2.Coordinate.X) || (c < galaxy1.Coordinate.X && c > galaxy2.Coordinate.X)).Count();
                long rowsPassed = rowsToExpand.Where(r => (r > galaxy1.Coordinate.Y && r < galaxy2.Coordinate.Y) || (r < galaxy1.Coordinate.Y && r > galaxy2.Coordinate.Y)).Count();
                
                int shortestPath = Math.Abs(galaxy1.Coordinate.X - galaxy2.Coordinate.X) + Math.Abs(galaxy1.Coordinate.Y - galaxy2.Coordinate.Y);
                shortestPath += (int)(columnsPassed * rowFactor);
                shortestPath += (int)(rowsPassed * rowFactor);
                part2sum += shortestPath;
                Console.WriteLine("Shortest path from " + galaxy1.Number + " to " + galaxy2.Number + " is " + shortestPath);
            }
        }

        Console.WriteLine("Part 2 : " + part2sum);
        // 82000210 is too low
        // 382979724122 is right!
    }

}