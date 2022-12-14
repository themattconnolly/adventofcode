using System.Drawing;
using System.Text.RegularExpressions;

namespace Day12
{
    internal class Program
    {
        internal static List<char[]> grid = new List<char[]>();
        internal static List<Location> EvaluatedLocations = new List<Location>();


        static void Main(string[] args)
        {
            Console.WriteLine("Day 12!");

            Point startingSpot = new Point();
            Point endingSpot = new Point();

            foreach (string line in File.ReadLines("input.txt"))
            {
                grid.Add(Regex.Match(line, @"\w+").Value.ToCharArray());

                if (line.Contains("S"))
                {
                    startingSpot.X = line.IndexOf("S");
                    startingSpot.Y = grid.Count - 1;
                }
            }

            List<List<Point>> allMoveHistories = new List<List<Point>>();

            Location ogStartingLocation = new Location()
            {
                Point = startingSpot,
                Height = 'S',
                MinimumMoves = 0
            };
            EvaluatedLocations.Add(ogStartingLocation);

            List<Location> startingLocations = new List<Location>();
            startingLocations.Add(ogStartingLocation);

            // get all starting a locations
            for (int i = 0; i < grid.Count; i++)
            {
                for(int j = 0; j < grid[0].Length; j++)
                {
                    if (grid[i][j] == 'a')
                    {
                        Location newLocation = new Location()
                        {
                            Point = new Point(j, i),
                            Height = grid[i][j],
                            MinimumMoves = 0
                        };

                        EvaluatedLocations.Add(newLocation);
                        startingLocations.Add(newLocation);
                    }
                }
            }

            foreach(Location startingLocation in startingLocations)
            {
                EvaluateMoves(startingLocation);
            }
            
            //EvaluateMoves(startingLocation);

            Console.WriteLine("TADA?");

            Location endingLocation = EvaluatedLocations.FirstOrDefault(x => x.Height == 'E');
            if (endingLocation == null)
            {
                Console.WriteLine("Ya done goofed");
            }
            else
            {
                Console.WriteLine("minimum moves: " + endingLocation.MinimumMoves);
            }
        }

        internal static void EvaluateMoves(Location currentLocation)
        {
            foreach (Point validMove in GetValidMoves(currentLocation))
            {
                Location evaluatedLocation = EvaluatedLocations.FirstOrDefault(x => x.Point == validMove);
                if (evaluatedLocation == null)
                {
                    evaluatedLocation = new Location()
                    {
                        Point = validMove,
                        Height = grid[validMove.Y][validMove.X],
                        MinimumMoves = currentLocation.MinimumMoves + 1
                    };

                    EvaluatedLocations.Add(evaluatedLocation);
                    EvaluateMoves(evaluatedLocation);
                }
                else if(evaluatedLocation.MinimumMoves > currentLocation.MinimumMoves + 1)
                {
                    // re-evaluate from here
                    evaluatedLocation.MinimumMoves = currentLocation.MinimumMoves + 1;
                    EvaluateMoves(evaluatedLocation);
                }
                else
                {
                    // location has been evaluated and isn't better
                }
            }
        }

        //internal static List<Location> locatToEvaluate = new List<Point>();
        //internal static List<Point> moves= new List<Point>();

        internal static List<Point> GetValidMoves(Location currentLocation)
        {
            List<Point> validMoves = new List<Point>();

            if (currentLocation.Point.X < grid[0].Length - 1)
            {
                Point possibleStep = new Point(currentLocation.Point.X + 1, currentLocation.Point.Y);
                if (GetSpotValue(possibleStep) <= currentLocation.Height + 1)
                {
                    validMoves.Add(possibleStep);
                }
            }

            if (currentLocation.Point.Y > 0)
            {
                Point possibleStep = new Point(currentLocation.Point.X, currentLocation.Point.Y - 1);
                if (GetSpotValue(possibleStep) <= currentLocation.Height + 1)
                {
                    validMoves.Add(possibleStep);
                }
            }

            if (currentLocation.Point.Y < grid.Count - 1)
            {
                Point possibleStep = new Point(currentLocation.Point.X, currentLocation.Point.Y + 1);
                if (GetSpotValue(possibleStep) <= currentLocation.Height + 1)
                {
                    validMoves.Add(possibleStep);
                }
            }

            if (currentLocation.Point.X > 0)
            {
                Point possibleStep = new Point(currentLocation.Point.X - 1, currentLocation.Point.Y);
                if (GetSpotValue(possibleStep) <= currentLocation.Height + 1)
                {
                    validMoves.Add(possibleStep);
                }
            }

            return validMoves;
        }

        internal static char GetSpotValue(Point point)
        {
            return GetSpotValue(point.X, point.Y);
        }

        internal static char GetSpotValue(int x, int y)
        {
            char retVal = grid[y][x];
            if(retVal == 'S') { retVal = 'a'; }
            if(retVal == 'E') { retVal = 'z'; }
            return retVal;
        }

        internal class Move
        {
            internal Point MoveFrom;
            internal Point MoveTo;
        }

        internal class Location
        {
            internal Point Point;
            internal char Height;
            internal int MinimumMoves;
        }
    }
}