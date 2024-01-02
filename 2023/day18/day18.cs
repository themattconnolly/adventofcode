using System.Collections;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day18
{   
    private static string filename = "day18/input.txt";

    public static Hole[][] Grid = new Hole[256][];

    public struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Hole
    {
        public Coordinate HoleCoordinate { get; set; }
        public int Depth { get; set; }
        public string EdgeColor { get; set; }
    }

    public static List<Hole> Holes = new List<Hole>();

    private static void ParseFile()
    {
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        int x = 0;
        int y = 0;
        string line;
        while((line = file.ReadLine()) != null)
        {
            // parse a line that looks like "R 6 (#70c710)"
            // into a Hole object
            string[] parts = line.Split(" ");
            string direction = parts[0];
            int distance = int.Parse(parts[1]);
            string edgeColor = parts[2].Substring(1, 7);
            switch(direction)
            {
                case "R":
                    for(int i = 1; i <= distance; i++)
                    {
                        AddHole(x + i, y, 1, edgeColor);
                    }
                    x += distance;
                    break;
                case "L":
                    for(int i = 1; i <= distance; i++)
                    {
                        AddHole(x - i, y, 1, edgeColor);
                    }
                    x -= distance;
                    break;
                case "U":
                    for(int i = 1; i <= distance; i++)
                    {
                        AddHole(x, y + i, 1, edgeColor);
                    }
                    y += distance;
                    break;
                case "D":
                    for(int i = 1; i <= distance; i++)
                    {
                        AddHole(x, y - i, 1, edgeColor);
                    }
                    y -= distance;
                    break;
            }
        }

        file.Close();   
    }

    private static void AddHole(int x, int y, int depth, string edgeColor)
    {
        Hole hole = new Hole();
        hole.HoleCoordinate = new Coordinate() { X = x, Y = y };
        hole.Depth = depth;
        hole.EdgeColor = edgeColor;
        Holes.Add(hole);
    }
    
    public static void RunPart1()
    {
        ParseFile();

        //Console.WriteLine("Hole Count:" + Holes.Count);

        // find the min and max x and y values
        int minX = Holes.Min(x => x.HoleCoordinate.X);
        int maxX = Holes.Max(x => x.HoleCoordinate.X);
        int minY = Holes.Min(x => x.HoleCoordinate.Y);
        int maxY = Holes.Max(x => x.HoleCoordinate.Y);
        int gridHeight = maxY - minY;
        int gridLength = maxX - minX;
        
        Grid = new Hole[gridHeight + 1][];
        // add each Hole to its proper place in the Grid
        foreach(Hole hole in Holes)
        {
            int yOffset = hole.HoleCoordinate.Y - minY;
            int xOffset = hole.HoleCoordinate.X - minX;
            if(Grid[yOffset] == null)
            {
                Grid[yOffset] = new Hole[gridLength + 1];
            }
            Grid[yOffset][xOffset] = hole;
        }

        // flood fill from every edge piece that is a hole
        for(int y = 0; y < Grid.Length; y++)
        {
            if(y == 0 || y == Grid.Length - 1)
            {
                for(int x = 0; x < Grid[y].Length; x++)
                {
                    if(Grid[y][x] != null)
                    {
                        // flood fill
                        FloodFill(x, y);
                    }
                }
            }
            else
            {
                int x = 0;
                if(Grid[y][x] != null)
                {
                    // flood fill
                    FloodFill(x, y);
                }

                x = Grid[y].Length - 1;
                if(Grid[y][x] != null)
                {
                    // flood fill
                    FloodFill(x, y);
                }
            }
        }

        Console.WriteLine("Grid:");
        // sum the light grid
        int holeCount = 0;
        for(int i = Grid.Length - 1; i >= 0; i--)
        {
            Console.Write("Row " + i.ToString().PadLeft(2) + ": ");
            bool betweenHoles = false;
            bool onHole = false;
            for(int j = 0; j < Grid[i].Length; j++)
            {    
                if(Grid[i][j] != null)
                {
                    Console.Write("#");

                    //onHole = true;
                    holeCount++;
                }
                else
                {

                    // if(onHole)
                    // {
                    //     onHole = false;
                    //     if(betweenHoles)
                    //     {
                    //         betweenHoles = false;
                    //     }
                    //     else
                    //     {
                    //         betweenHoles = true;
                    //     }
                    // }

                    // if(betweenHoles)
                    // {
                    //     Console.Write("0");
                    //     holeCount++;
                    // } else {
                    //     Console.Write(".");
                    // }

                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }

        // find the area in the grid surrounded by the holes
        // that is not a hole
        

        
       
        Console.WriteLine("Part 1 : " + holeCount);
        // 93240 is too high
        // 4688 is too low
    }

    private static void FloodFill(int x, int y)
    {
        // if the character is not o or ., then return
        if(Grid[y][x] == null)
        {
            return;
        }

        // if the character is o, then change it to .
        // if(grid[y][x] == 'o')
        // {
        //     grid[y][x] = ' ';
        // }

        // // if the character is ., then change it to o
        // if(grid[y][x] == '.')
        // {
        //     grid[y][x] = ' ';
        // }

        // if the character is on the left edge, then flood fill right
        if(x == 0)
        {
            FloodFillHelper(x + 1, y);
        }
        // if the character is on the right edge, then flood fill left
        else if(x == Grid[y].Length - 1)
        {
            FloodFillHelper(x - 1, y);
        }
        // if the character is on the bottom edge, then flood fill up
        else if(y == 0)
        {
            FloodFillHelper(x, y + 1);
        }
        // if the character is on the top edge, then flood fill down
        else if(y == Grid.Length - 1)
        {
            FloodFillHelper(x, y - 1);
        }
        // otherwise, flood fill in all directions
        else
        {
            FloodFillHelper(x + 1, y);
            FloodFillHelper(x - 1, y);
            FloodFillHelper(x, y + 1);
            FloodFillHelper(x, y - 1);
        }
    }
    
    private static void FloodFillHelper(int x, int y)
    {
        // if it is a hole, return
        if(Grid[y][x] != null)
        {
            return;
        }

        Grid[y][x] = new Hole() { HoleCoordinate = new Coordinate() { X = x, Y = y }, Depth = 1, EdgeColor = "000000" };

        FloodFillHelper(x + 1, y);
        FloodFillHelper(x - 1, y);
        FloodFillHelper(x, y + 1);
        FloodFillHelper(x, y - 1);
    }
    
    public static void RunPart2()
    {
        ParseFile();

        
        //Console.WriteLine("Part 2 : " + part2sum);
        // 
    }

}