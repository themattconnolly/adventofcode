using System.Drawing;
using System.Text.RegularExpressions;

namespace Day9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 9!");

            Regex moveRegex = new Regex(@"(\w) (\d+)");

            Point headPos = new Point(0, 0);
            Point tailPos = new Point(0, 0);
            List<Point> visitedTailSpots = new List<Point>();

            Point[] rope = new Point[10];
            for (int i = 0; i < 10; i++)
            {
                rope[i] = new Point(0, 0);
            }

            foreach (string line in File.ReadLines("input.txt"))
            {
                Match match = moveRegex.Match(line);
                string direction = match.Groups[1].Value;
                int distance = int.Parse(match.Groups[2].Value);

                for (int i = 1; i <= distance; i++)
                {
                    switch (direction)
                    {
                        case "U":
                            headPos.Y++;
                            rope[0].Y++;
                            break;
                        case "D":
                            headPos.Y--;
                            rope[0].Y--;
                            break;
                        case "L":
                            headPos.X--;
                            rope[0].X--;
                            break;
                        case "R":
                            headPos.X++;
                            rope[0].X++;
                            break;
                    }

                    for(int knotIndex = 0; knotIndex < 9; knotIndex++)
                    {
                        UpdateKnotPosition(ref rope[knotIndex], ref rope[knotIndex + 1]);
                    }

                    if (visitedTailSpots.Contains(rope[9]) == false)
                    {
                        visitedTailSpots.Add(rope[9]);
                    }
                }
            }

            Console.WriteLine("Visited Tail Spots: " + visitedTailSpots.Count);
        }

        static void UpdateKnotPosition(ref Point knotA, ref Point knotB)
        {
            if (Math.Abs(knotA.Y - knotB.Y) > 1)
            {
                knotB.Y += Math.Sign(knotA.Y - knotB.Y);

                if (Math.Abs(knotA.X - knotB.X) > 0)
                {
                    knotB.X += Math.Sign(knotA.X - knotB.X);
                }
            }

            if (Math.Abs(knotA.X - knotB.X) > 1)
            {
                knotB.X += Math.Sign(knotA.X - knotB.X);

                if (Math.Abs(knotA.Y - knotB.Y) > 0)
                {
                    knotB.Y += Math.Sign(knotA.Y - knotB.Y);
                }
            }
        }
    }
}