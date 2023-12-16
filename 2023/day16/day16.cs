using System.Collections;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day16
{   
    private static string filename = "day16/input.txt";

    public static char[][] Grid = new char[256][];

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
            // for(int i = 0; i < line.Length; i++)
            // {
                
            // }
            char[] row = line.ToCharArray();
            Grid[rowIndex--] = row;
        }

        file.Close();

        for(int i = Grid.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Row " + i + ": " + string.Join("", Grid[i]));
        }
    }

    public class LightSquare
    {
        public int LightCount {
            get {
                int count = 0;
                if(LightUp) count++;
                if(LightDown) count++;
                if(LightLeft) count++;
                if(LightRight) count++;
                return count;
            }
        }
        public bool LightRight { get; set; }
        public bool LightLeft { get; set; }
        public bool LightUp { get; set; }
        public bool LightDown { get; set; }
    }

    public static void LightGo(int x, int y, string direction)
    {
        // if x  or y are out of bounds, return
        if(x < 0 || y < 0 || y >= Grid.Length || x >= Grid[y].Length)
        {
            return;
        }

        // if lightgrid is a number, add one to it. Otherwise set it to 1
        if(LightGrid[y] == null)
        {
            //LightGrid[y] = new char[Grid[y].Length];
            // initialize the row with .
            //LightGrid[y] = new string('.', Grid[y].Length).ToCharArray();
            LightGrid[y] = new LightSquare[Grid[y].Length];
        }
        if(LightGrid[y][x] == null)
        {
            LightGrid[y][x] = new LightSquare();
            switch(direction)
            {
                case "up":
                    LightGrid[y][x].LightUp = true;
                    break;
                case "down":
                    LightGrid[y][x].LightDown = true;
                    break;
                case "left":
                    LightGrid[y][x].LightLeft = true;
                    break;
                case "right":
                    LightGrid[y][x].LightRight = true;
                    break;
            }
        }
        else
        {
            switch(direction)
            {
                case "up":
                    if(LightGrid[y][x].LightUp)
                    {
                        return;
                    }
                    else
                    {
                        LightGrid[y][x].LightUp = true;
                    }
                    break;
                case "down":
                    if(LightGrid[y][x].LightDown)
                    {
                        return;
                    }
                    else
                    {
                        LightGrid[y][x].LightDown = true;
                    }
                    break;
                case "left":
                    if(LightGrid[y][x].LightLeft)
                    {
                        return;
                    }
                    else
                    {
                        LightGrid[y][x].LightLeft = true;
                    }
                    break;
                case "right":
                    if(LightGrid[y][x].LightRight)
                    {
                        return;
                    }
                    else
                    {
                        LightGrid[y][x].LightRight = true;
                    }
                    break;
            }
        }

        // switch based on current grid char
        switch(Grid[y][x])
        {
            case '.':
                // keep going
                switch(direction)
                {
                    case "up":
                        LightGo(x, y + 1, direction);
                        break;
                    case "down":
                        LightGo(x, y - 1, direction);
                        break;
                    case "left":
                        LightGo(x - 1, y, direction);
                        break;
                    case "right":
                        LightGo(x + 1, y, direction);
                        break;
                }
                break;
            case '/':
                // change direction
                switch(direction)
                {
                    case "up":
                        LightGo(x + 1, y, "right");
                        break;
                    case "down":
                        LightGo(x - 1, y, "left");
                        break;
                    case "left":
                        LightGo(x, y - 1, "down");
                        break;
                    case "right":
                        LightGo(x, y + 1, "up");
                        break;
                }
                break;
            case '\\':
                // change direction
                switch(direction)
                {
                    case "up":
                        LightGo(x - 1, y, "left");
                        break;
                    case "down":
                        LightGo(x + 1, y, "right");
                        break;
                    case "left":
                        LightGo(x, y + 1, "up");
                        break;
                    case "right":
                        LightGo(x, y - 1, "down");
                        break;
                }
                break;
            case '-':
                // split if going up or down, continue if going left or right
                switch(direction)
                {
                    case "up":
                        LightGo(x - 1, y, "left");
                        LightGo(x + 1, y, "right");
                        break;
                    case "down":
                        LightGo(x - 1, y, "left");
                        LightGo(x + 1, y, "right");
                        break;
                    case "left":
                        LightGo(x - 1, y, "left");
                        break;
                    case "right":
                        LightGo(x + 1, y, "right");
                        break;
                }
                break;
            case '|':
                // split if going left or right, continue if going up or down
                switch(direction)
                {
                    case "up":
                        LightGo(x, y + 1, "up");
                        break;
                    case "down":
                        LightGo(x, y - 1, "down");
                        break;
                    case "left":
                        LightGo(x, y + 1, "up");
                        LightGo(x, y - 1, "down");
                        break;
                    case "right":
                        LightGo(x, y + 1, "up");
                        LightGo(x, y - 1, "down");
                        break;
                }
                break;
        }
    }

    public static LightSquare[][] LightGrid = new LightSquare[256][];
    
    public static void RunPart1()
    {
        ParseFile();

        LightGrid = new LightSquare[Grid.Length][];

        LightGo(0, Grid.Length - 1, "right");

        long part1sum = 0;
        
        Console.WriteLine("Light Grid:");
        // sum the light grid
        for(int i = LightGrid.Length - 1; i >= 0; i--)
        {
            Console.Write("Row " + i.ToString().PadLeft(2) + ": ");
            for(int j = 0; j < LightGrid[i].Length; j++)
            {
                if(LightGrid[i][j] != null)
                {
                    Console.Write(LightGrid[i][j].LightCount);
                    // parse the char at LightGrid[i][j] as an int and add it to the sum
                    int lightCount = LightGrid[i][j].LightCount;

                    part1sum++;
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine("Part 1 : " + part1sum);
        // 7034 is right
    }

    public static int GetLightCount()
    {
        int lightCount = 0;
        for(int i = LightGrid.Length - 1; i >= 0; i--)
        {
            if(LightGrid[i] == null)
            {
                continue;
            }
            for(int j = 0; j < LightGrid[i].Length; j++)
            {
                if(LightGrid[i][j] != null)
                {
                    lightCount++;
                }
            }
        }
        return lightCount;
    }
    
    public static void RunPart2()
    {
        ParseFile();

        int maxSquares = 0;

        for(int i = 0; i < Grid.Length; i++)
        {
            LightGrid = new LightSquare[Grid.Length][];
            LightGo(0, i, "right");
            if(GetLightCount() > maxSquares)
            {
                maxSquares = GetLightCount();
            }

            LightGrid = new LightSquare[Grid.Length][];
            LightGo(Grid[i].Length - 1, i, "left");
            if(GetLightCount() > maxSquares)
            {
                maxSquares = GetLightCount();
            }
        }

        for(int i = 0; i < Grid[0].Length; i++)
        {
            LightGrid = new LightSquare[Grid.Length][];
            LightGo(i, 0, "up");
            if(GetLightCount() > maxSquares)
            {
                maxSquares = GetLightCount();
            }

            LightGrid = new LightSquare[Grid.Length][];
            LightGo(i, Grid.Length - 1, "down");
            if(GetLightCount() > maxSquares)
            {
                maxSquares = GetLightCount();
            }
        }
        
        Console.WriteLine("Part 2 : " + maxSquares);
        // 7759 is right!
    }

}