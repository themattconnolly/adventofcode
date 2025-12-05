using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2025;
public class Day4
{
    private static List<string> lines = new List<string>();
    private static char[][] grid = new char[10][];

    private static void ParseFile()
    {
        string filename = System.IO.Path.Combine(AppContext.BaseDirectory, "day4/input.txt");
        string? line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        while ((line = file.ReadLine()) != null)
        {
            lines.Add(line);
        }
        file.Close();

        grid = new char[lines.Count][];
        for(int i = 0; i < lines.Count; i++)
        {
            grid[i] = lines[i].ToCharArray();
        }

        
    }

    private static bool IsAccessible(int targetY,int targetX)
    {
        // evalute the 8 surrounding positions to count the number of @ characters, if there are fewer than 4 then the position is not accessible
        int @Count = 0;
        for(int shiftY = -1; shiftY <= 1; shiftY++)
        {
            if(@Count >= 4)
            {
                break;
            }
            for(int shiftX = -1; shiftX <= 1; shiftX++)
            {
                if(@Count >= 4)
                {
                    break;
                }
                // Skip the center position (0,0)
                if(shiftY == 0 && shiftX == 0)
                {
                    continue;
                }
                if(targetY + shiftY < 0 || targetY + shiftY >= grid.Length || targetX + shiftX < 0 || targetX + shiftX >= grid[targetY + shiftY].Length)
                {
                    continue;
                }
                if(grid[targetY + shiftY][targetX + shiftX] != '.')
                {
                    @Count++;
                }
            }
        }

        if(@Count < 4)
        {
            grid[targetY][targetX] = 'x';
            return true;
        }

        return false;
    }

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;
        
        for(int y = 0; y < grid.Length; y++)
        {
            for(int x = 0; x < grid[y].Length; x++)
            {
                if(grid[y][x] == '@')
                {
                    if(IsAccessible(y,x))
                    {
                        part1sum++;
                    }
                }
            }
        }

        // print the grid
        for(int i = 0; i < grid.Length; i++)
        {
            Console.WriteLine(string.Join("", grid[i]));
        }

        Console.WriteLine("Part 1 : " + part1sum);
    }

    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        bool ableToRemove = true;
        while(ableToRemove)
        {
            ableToRemove = false;
            for(int y = 0; y < grid.Length; y++)
            {
                for(int x = 0; x < grid[y].Length; x++)
                {
                    if(grid[y][x] == '@')
                    {
                        if(IsAccessible(y,x))
                        {
                        }
                    }
                }
            }

            for(int y = 0; y < grid.Length; y++)
            {
                for(int x = 0; x < grid[y].Length; x++)
                {
                    if(grid[y][x] == 'x')
                    {
                        grid[y][x] = '.';
                        ableToRemove = true;
                        part2sum++;
                    }
                }
            }
        }



        // print the grid
        for(int i = 0; i < grid.Length; i++)
        {
            Console.WriteLine(string.Join("", grid[i]));
        }

        Console.WriteLine("Part 2 : " + part2sum);
    }
}

