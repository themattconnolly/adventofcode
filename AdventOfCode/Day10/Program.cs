using System.Text.RegularExpressions;

namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 10!");

            Regex addxRegex = new Regex(@"addx (-*\d+)");
            int cycle = 1;
            int x = 1;
            int signalStrengthSum = 0;
            foreach (string line in File.ReadLines("input.txt"))
            {
                if(line.Contains("noop"))
                {
                    cycle++;
                }
                else
                {
                    int addX = int.Parse(addxRegex.Match(line).Groups[1].Value);
                    cycle++;

                    if(cycle % 40 == 20)
                    {
                        signalStrengthSum += (cycle * x);
                    }

                    x += addX;
                    cycle++;
                }

                if (cycle % 40 == 20)
                {
                    signalStrengthSum += (cycle * x);
                }
            }

            Console.WriteLine("Cycles: " + cycle);
            Console.WriteLine("X: " + x);
            Console.WriteLine("Signal Strength Sum: " + signalStrengthSum);
        }
    }
}