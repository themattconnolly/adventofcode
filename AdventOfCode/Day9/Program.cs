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
                            break;
                        case "D":
                            headPos.Y--;
                            break;
                        case "L":
                            headPos.X--;
                            break;
                        case "R":
                            headPos.X++;
                            break;
                    }

                    if (Math.Abs(headPos.Y - tailPos.Y) > 1)
                    {
                        tailPos.Y += Math.Sign(headPos.Y - tailPos.Y);
                        
                        if (Math.Abs(headPos.X - tailPos.X) > 0)
                        {
                            tailPos.X += Math.Sign(headPos.X - tailPos.X);
                        }
                    }

                    if (Math.Abs(headPos.X - tailPos.X) > 1)
                    {
                        tailPos.X += Math.Sign(headPos.X - tailPos.X);

                        if (Math.Abs(headPos.Y - tailPos.Y) > 0)
                        {
                            tailPos.Y += Math.Sign(headPos.Y - tailPos.Y);
                        }
                    }

                    if(visitedTailSpots.Contains(tailPos) == false)
                    {
                        visitedTailSpots.Add(tailPos);
                    }
                    else
                    {
                        string noop = "";
                    }
                    
                    //if (headPos.X == tailPos.X)
                    //{
                    //    if(Math.Abs(headPos.Y - tailPos.Y) > 1)
                    //    {
                    //        tailPos.Y += Math.Sign(headPos.Y - tailPos.Y);
                    //    }
                    //}
                    //else if (headPos.Y == tailPos.Y)
                    //{
                    //    if (Math.Abs(headPos.X - tailPos.X) > 1)
                    //    {
                    //        tailPos.X += Math.Sign(headPos.X - tailPos.X);
                    //    }
                    //}
                }
            }

            Console.WriteLine("Visited Tail Spots: " + visitedTailSpots.Count);
        }

        static void UpdateTailPosition()
        {

        }
    }
}