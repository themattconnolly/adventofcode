using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day6
{   
    private static string filename = "day6/input.txt";
    
    // class with a source range start int, destination range start int, and range length int
    public class Range
    {
        public long sourceStart;
        public long destinationStart;
        public long length;
    }

    // class with a name and a list of Range
    public class FarmMap
    {
        public string name;
        public List<Range> ranges = new List<Range>();
    }

    public static void RunPart1()
    {
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        long part1 = 1;

        
        line = file.ReadLine();
        // parse the numbers from this line: "Time:      7  15   30"
        MatchCollection timeMatches = Regex.Matches(line, @"\d+");

        int[] times = new int[timeMatches.Count];
        int timeIndex = 0;
        foreach(Match timeMatch in timeMatches)
        {
            int time = int.Parse(timeMatch.Value);
            times[timeIndex] = time;
            timeIndex++;;
            Console.WriteLine("Found time: " + time);
        }

        line = file.ReadLine();
        MatchCollection distanceMatches = Regex.Matches(line, @"\d+");
        int[] distances = new int[distanceMatches.Count];
        int distanceIndex = 0;
        foreach(Match distanceMatch in distanceMatches)
        {
            int distance = int.Parse(distanceMatch.Value);
            distances[distanceIndex] = distance;
            distanceIndex++;;
            Console.WriteLine("Found distance: " + distance);
        }

        file.Close();

        for(int i = 0; i < times.Length; i++)
        {
            long raceWins = CountWins(times[i], distances[i]);
            Console.WriteLine("Race " + (i+1) + " wins: " + raceWins);
            part1 *= raceWins;
        }

        // print the low mappedInt
        Console.WriteLine("Part 1 : " + part1);
        // 861300 is right!
    }

    private static long CountWins(long raceRecord, long raceDistance)
    {
        long wins = 0;
        long holdTime = 1;
        long speed = 1;
        long remainingTime = raceRecord - holdTime;
        long travelDistance = speed * remainingTime;

        while(holdTime < raceRecord)
        {
            if(travelDistance > raceDistance)
            {
                wins++;
                //Console.WriteLine("Hold time: " + holdTime + ", speed: " + speed + ", remaining time: " + remainingTime + ", travel distance: " + travelDistance + " WINNER!");
            }
            // else
            // {
            //     Console.WriteLine("Hold time: " + holdTime + ", speed: " + speed + ", remaining time: " + remainingTime + ", travel distance: " + travelDistance + " LOSER..");
            // }

            holdTime++;
            speed++;
            remainingTime--;
            travelDistance = speed * remainingTime;
        }

        return wins;
    }


    public static void RunPart2()
    {
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        line = file.ReadLine();
        // parse the numbers from this line: "Time:      7  15   30"
        MatchCollection timeMatches = Regex.Matches(line, @"\d+");

        // concatenate the times into a single int
        string timeString = "";
        foreach(Match timeMatch in timeMatches)
        {
            timeString += timeMatch.Value;
        }
        long time = long.Parse(timeString); 

        Console.WriteLine("Found time: " + time);

        line = file.ReadLine();
        MatchCollection distanceMatches = Regex.Matches(line, @"\d+");
        
        // concatenate the distances into a single int
        string distanceString = "";
        foreach(Match distanceMatch in distanceMatches)
        {
            distanceString += distanceMatch.Value;
        }
        long distance = long.Parse(distanceString);

        Console.WriteLine("Found distance: " + distance);

        file.Close();

        long raceWins = CountWins(time, distance);

        // print the low mappedInt
        Console.WriteLine("Part 2 : " + raceWins);
        // 861300 is right!
    }
}