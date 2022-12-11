using System.Text.RegularExpressions;

namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 10!");

            Regex addxRegex = new Regex(@"addx (-*\d+)");
            int cycle = 0;
            int x = 1;
            int signalStrengthSum = 0;
            List<char[]> screen = new List<char[]>() { };
            screen.Add(new char[40]);
            int row = 0;
            int position = 0;
            foreach (string line in File.ReadLines("input.txt"))
            {
                
                cycle++;

                if (cycle % 40 == 20)
                {
                    signalStrengthSum += (cycle * x);
                }

                // drawPositionOfSprit
                UpdateScreen(screen, x, row, position++);
                if (position == 40)
                {
                    position = 0;
                    row++;
                    screen.Add(new char[40]);
                }

                if (line.Contains("noop"))
                {
                    // do nothing   
                }
                else
                {
                    int addX = int.Parse(addxRegex.Match(line).Groups[1].Value);
                    cycle++;

                    if (cycle % 40 == 20)
                    {
                        signalStrengthSum += (cycle * x);
                    }

                    UpdateScreen(screen, x, row, position++);
                    if (position == 40)
                    {
                        position = 0;
                        row++;
                        screen.Add(new char[40]);
                    }

                    x += addX;
                }
            }

            Console.WriteLine("Cycles: " + cycle);
            Console.WriteLine("X: " + x);
            Console.WriteLine("Signal Strength Sum: " + signalStrengthSum);
            Console.WriteLine("Screen:");

            foreach(char[] screenRow in screen)
            {
                Console.WriteLine(screenRow);
            }
        }

        internal static void UpdateScreen(List<char[]> screen, int x, int row, int position)
        {
            if(position >= x - 1 && position <= x + 1)
            {
                screen[row][position] = '#';
            }
            else
            {
                screen[row][position] = '.';
            }
        }
    }
}