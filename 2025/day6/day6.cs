using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2025;
public class Day6
{
    private static int[][] NumberGrid = new int[10][];
    private static char[] Operators = new char[10];

    private static char[][][] StupidGrid = new char[10][][];

    private static void ParseFile()
    {
        string filename = System.IO.Path.Combine(AppContext.BaseDirectory, "day6/input.txt");
        List<string> lines = new List<string>();
        using(System.IO.StreamReader file = new System.IO.StreamReader(filename))
        {
            string? line;
            while((line = file.ReadLine()) != null && line.Trim().Length > 0)
            {
                lines.Add(line);
            }
        }

        // parse input of format "123 456   78" into int array
        string[] parts = lines[0].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        NumberGrid = new int[lines.Count - 1][];
        Operators = new char[parts.Length];
        NumberGrid[0] = parts.Select(c => int.Parse(c.ToString())).ToArray();

        for(int row = 1; row < lines.Count; row++)
        {
            // parse input of format "123 456 78" into lint array
            parts = lines[row].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

            // if first part is an integer, parse the line into NumberGrid, else parse into Operators
            if(int.TryParse(parts[0], out int _))
            {
                NumberGrid[row] = parts.Select(c => int.Parse(c.ToString())).ToArray();
            }
            else
            {
                for(int i = 0; i < parts.Length; i++)
                {
                    Operators[i] = parts[i][0];
                }
            }
        }
    }

    

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;
        
        for(int column = 0; column < NumberGrid[0].Length; column++)
        {
            long result = NumberGrid[0][column];
            for(int row = 1; row < NumberGrid.Length; row++)
            {
                switch(Operators[column])
                {
                    case '+':
                        result += NumberGrid[row][column];
                        break;
                    case '*':
                        result *= NumberGrid[row][column];
                        break;
                }
            }

            Console.WriteLine("Result for column " + column + ": " + result);
            part1sum += result;
        }

        Console.WriteLine("Part 1 : " + part1sum);
    }


    private static void ParseFilePart2()
    {
        string filename = System.IO.Path.Combine(AppContext.BaseDirectory, "day6/input.txt");
        List<string> lines = new List<string>();
        using(System.IO.StreamReader file = new System.IO.StreamReader(filename))
        {
            string? line;
            while((line = file.ReadLine()) != null && line.Trim().Length > 0)
            {
                lines.Add(line);
            }
        }

        int numCount = lines[0].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
        StupidGrid = new char[numCount][][];
        Operators = new char[numCount];
        
        //NumberGrid[0] = parts.Select(c => int.Parse(c.ToString())).ToArray();

        // parse last line into Operators but the number of spaces between each operator is significant and is the
        // size of the corresponding number in StupidGrid
        int spacesAfter = 0;
        int operationIndex = 0;

        for(int i = 0; i < lines[lines.Count - 1].Length; i++)
        {
            
            char c = lines[lines.Count - 1][i];
            if(c == ' ')
            {
                spacesAfter++;
                continue;
            }

            if(spacesAfter > 0)
            {
                // allocate space in StupidGrid for previous operator, initialize
                StupidGrid[operationIndex] = new char[lines.Count -1][];
                for(int row = 0; row < lines.Count -1; row++)
                {
                    StupidGrid[operationIndex][row] = new char[spacesAfter];
                }

                //StupidGrid[operationIndex] = new char[lines.Count -1][];
                spacesAfter = 0;
                operationIndex++;   
            }
            Operators[operationIndex] = c;
        }

        StupidGrid[operationIndex] = new char[lines.Count -1][];
        for(int row = 0; row < lines.Count -1; row++)
        {
            StupidGrid[operationIndex][row] = new char[spacesAfter+1];
        }

        operationIndex = 0;

        for(int row = 0; row < lines.Count - 1; row++)
        {
            int col = 0;
            for(operationIndex = 0; operationIndex < Operators.Length; operationIndex++)
            {
                char[][] stupidNumber = StupidGrid[operationIndex];
                int numWidth = stupidNumber[0].Length;
                int colEnd = col + numWidth;
                for(; col < colEnd; col++)
                {
                    stupidNumber[row][col - (colEnd - numWidth)] = lines[row][col];
                }
                col++; // for the space between numbers
            }
        }
    }

    public static void RunPart2()
    {
        ParseFilePart2();

        long part2sum = 0;

        for(int column = 0; column < StupidGrid.Length; column++)
        {
            char[][] stupidNumber = StupidGrid[column];
            long result = 0;
            // build each number by starting with the last column, appending down, skipping blanks
            for(int stupidCol = stupidNumber[0].Length - 1; stupidCol >= 0; stupidCol--)
            {
                string digitStr = "";
                for(int row = 0; row < stupidNumber.Length; row++)
                {
                    char c = stupidNumber[row][stupidCol];
                    if(c == ' ')
                    {
                        continue;
                    }
                    // append c to number string
                    digitStr += c;
                }

                int stupidNum = int.Parse(digitStr);
                if(result == 0)
                {
                    result = stupidNum;
                }
                else
                {
                    switch(Operators[column])
                    {
                        case '+':
                            result += stupidNum;
                            break;
                        case '*':
                            result *= stupidNum;
                            break;
                    }
                }
            }

            part2sum += result;
            Console.WriteLine("Result for column " + column + ": " + result);
        }
        
        Console.WriteLine("Part 2 : " + part2sum); 
    }
}

