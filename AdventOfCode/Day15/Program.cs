using System.Drawing;
using System.Text.RegularExpressions;

namespace Day15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 15!");

            List<Sensor> sensors = new List<Sensor>();
            List<Point> beacons = new List<Point>();

            foreach(string line in File.ReadLines("input.txt"))
            {
                Match match = Regex.Match(line, @"Sensor at x=(-*\d+), y=(-*\d+): closest beacon is at x=(-*\d+), y=(-*\d+)");
                Point sensorPoint = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));

                int beaconX = int.Parse(match.Groups[3].Value);
                int beaconY = int.Parse(match.Groups[4].Value);
                Point beacon = beacons.FirstOrDefault(b => b.X == beaconY && b.Y == beaconY);
                if (beacon.IsEmpty)
                {
                    beacon = new Point(beaconY, beaconY);
                    beacons.Add(beacon);
                }

                int rangeX = Math.Abs(sensorPoint.X - beaconX);
                int rangeY = Math.Abs(sensorPoint.Y - beaconY);
                int range = rangeX + rangeY;

                sensors.Add(new Sensor()
                {
                    Location = sensorPoint,
                    NearestBeacon = beacon,
                    Range = range
                });
            }

            int rowToExamine = 2000000;

            int minX = int.MaxValue;
            int maxX = int.MinValue;
            foreach(Point beacon in beacons)
            {
                if(beacon.X < minX)
                {
                    minX = beacon.X;
                }
                if(beacon.X > maxX)
                {
                    maxX = beacon.X;
                }
            }
            foreach(Sensor sensor in sensors)
            {
                if(sensor.Location.X - sensor.Range < minX)
                {
                    minX = sensor.Location.X - sensor.Range;
                }
                if (sensor.Location.X + sensor.Range > maxX)
                {
                    maxX = sensor.Location.X + sensor.Range;
                }
            }

            // only check sensors range at all
            List<Sensor> sensorsToCheck = new List<Sensor>();
            foreach(Sensor sensor in sensors)
            {
                if(Math.Abs(sensor.Location.Y - rowToExamine) <= sensor.Range)
                {
                    sensorsToCheck.Add(sensor);
                }
            }

            List<int> inRangeXCoordinates = new List<int>();
            Sensor recentSensor = sensorsToCheck[0];
            for(int x = minX; x <= maxX; x++)
            {
                if(beacons.Exists(b => b.X == x && b.Y == rowToExamine) ||
                    sensorsToCheck.Exists(s => s.Location.X == x && s.Location.Y == rowToExamine))
                {
                    continue; // a beacon or sensor is here
                }

                // in range of the most recent sensor?
                int distanceFromSensor = Math.Abs(recentSensor.Location.X - x) + Math.Abs(recentSensor.Location.Y - rowToExamine);
                if(distanceFromSensor <= recentSensor.Range)
                {
                    // in range of sensor
                    inRangeXCoordinates.Add(x);
                    continue;
                }

                // check every other sensor
                foreach(Sensor sensor in sensorsToCheck)
                {
                    if(sensor == recentSensor)
                    {
                        continue; // skip
                    }

                    distanceFromSensor = Math.Abs(sensor.Location.X - x) + Math.Abs(sensor.Location.Y - rowToExamine);
                    if (distanceFromSensor <= sensor.Range)
                    {
                        // in range of sensor
                        recentSensor = sensor;
                        inRangeXCoordinates.Add(x);
                        break;
                    }
                }
            }

            //Console.WriteLine("X range = " + minX + " to " + maxX);
            Console.WriteLine("inRangeXCoordinates = " + inRangeXCoordinates.Count);
        }

        internal class Sensor
        {
            internal Point Location;
            internal Point NearestBeacon;
            internal int Range;
        }
    }
}