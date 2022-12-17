using System.Drawing;
using System.Text.RegularExpressions;

namespace Day14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 14!");

            string[] lines = File.ReadAllLines("input.txt");
            List<Point> allRocks = new List<Point>();

            foreach (string line in lines)
            {
                List<Point> rockPoints = new List<Point>();
                foreach (Match match in Regex.Matches(line, @"(\d*),(\d*)"))
                {
                    rockPoints.Add(new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));
                }

                Point? previousPoint = null;
                foreach (Point point in rockPoints)
                {
                    if (previousPoint != null)
                    {
                        int previousX = previousPoint.Value.X;
                        int previousY = previousPoint.Value.Y;
                        int nextX = point.X;
                        int nextY = point.Y;

                        if (previousX < nextX)
                        {
                            for (int i = previousX; i <= nextX; i++)
                            {
                                if (previousY < nextY)
                                {
                                    for (int j = previousY; j <= nextY; j++)
                                    {
                                        allRocks.Add(new Point { X = i, Y = j });
                                    }
                                }
                                else
                                {
                                    for (int j = previousY; j >= nextY; j--)
                                    {
                                        allRocks.Add(new Point { X = i, Y = j });
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = previousX; i >= nextX; i--)
                            {
                                if (previousY <= nextY)
                                {
                                    for (int j = previousY; j <= nextY; j++)
                                    {
                                        if (allRocks.Exists(r => r.X == i && r.Y == j) == false)
                                        {
                                            allRocks.Add(new Point { X = i, Y = j });
                                        }
                                    }
                                }
                                else
                                {
                                    for (int j = previousY; j >= nextY; j--)
                                    {
                                        if (allRocks.Exists(r => r.X == i && r.Y == j) == false)
                                        {
                                            allRocks.Add(new Point { X = i, Y = j });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    previousPoint = point;
                }
            }

            int minX = allRocks.Min(r => r.X);
            int minY = 0;
            int maxX = allRocks.Max(r => r.X);
            int maxY = allRocks.Max(r => r.Y);

            List<Point> allSand = new List<Point>();
            Point nextSand = NextSandMove(new Point(500, 0), allRocks, allSand);

            while(nextSand.Y <= maxY)
            {
                // have not yet reached infinity
                Point previousSand = Point.Empty;
                while(nextSand.IsEmpty == false && nextSand.Y <= maxY)
                {
                    previousSand = nextSand;
                    nextSand = NextSandMove(nextSand, allRocks, allSand);
                }

                if(nextSand.Y > maxY)
                {
                    continue;
                }

                // sand cannot move further, drop a new grain
                allSand.Add(previousSand);
                nextSand = NextSandMove(new Point(500, 0), allRocks, allSand);
            }


            // draw grid
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (y == 0 && x == 500)
                    {
                        Console.Write("+");
                        continue;
                    }

                    Point rock = allRocks.FirstOrDefault(r => r.X == x && r.Y == y);
                    if (rock.IsEmpty)
                    {
                        Point sand = allSand.FirstOrDefault(s => s.X == x && s.Y == y);
                        if (sand.IsEmpty)
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            Console.Write("o");
                        }
                    }
                    else
                    {
                        Console.Write("#");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine(allSand.Count + " grains of sand");
        }

        internal static Point NextSandMove(Point currentSand, List<Point> allRocks, List<Point> allSand)
        {
            Point nextMove = Point.Empty;
            int nextMoveX = currentSand.X;
            int nextMoveY = currentSand.Y + 1;

            if(allRocks.FirstOrDefault(r => r.X == nextMoveX && r.Y == nextMoveY).IsEmpty &&
                allSand.FirstOrDefault(s => s.X == nextMoveX && s.Y == nextMoveY).IsEmpty)
            {
                nextMove = new Point(nextMoveX, nextMoveY);
            }
            else
            {
                nextMoveX = nextMoveX - 1; // try to the left

                if (allRocks.FirstOrDefault(r => r.X == nextMoveX && r.Y == nextMoveY).IsEmpty &&
                allSand.FirstOrDefault(s => s.X == nextMoveX && s.Y == nextMoveY).IsEmpty)
                {
                    nextMove = new Point(nextMoveX, nextMoveY);
                }
                else
                {
                    nextMoveX = nextMoveX + 2; // try to the right
                    if (allRocks.FirstOrDefault(r => r.X == nextMoveX && r.Y == nextMoveY).IsEmpty &&
                        allSand.FirstOrDefault(s => s.X == nextMoveX && s.Y == nextMoveY).IsEmpty)
                    {
                        nextMove = new Point(nextMoveX, nextMoveY);
                    }
                }
            }

            return nextMove;
        }
    }
}