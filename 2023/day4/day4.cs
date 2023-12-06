using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day4
{   
    // function that reads in a file line by line and writes the output to the console
    public static void RunPart1()
    {
        string filename = "day4/input.txt";
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        int part1sum = 0;
        long part2sum = 0;
        
        int cardNumber = 0;

        while((line = file.ReadLine()) != null)
        {
            cardNumber++;

            //Console.WriteLine("Checking card: " + cardNumber);

            // parse a line like this into two sets of numbers, winning numbers and my numbers: Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            //int cardNumber = int.Parse(line.Substring(5, line.IndexOf(":") - 5));
            // split the rest of the line on the semicolon
            string[] cardSets = line.Substring(line.IndexOf(":") + 2).Split("|");
            // get the winning numbers
            string winningNumbers = cardSets[0].Trim();
            // get my numbers
            string myNumbers = cardSets[1].Trim();
            // trim multiple spaces down to one
            winningNumbers = Regex.Replace(winningNumbers, @"\s+", " ");
            myNumbers = Regex.Replace(myNumbers, @"\s+", " ");
            
            // split the winning numbers on the space
            string[] winningNumbersArray = winningNumbers.Split(" ");
            // split my numbers on the space
            string[] myNumbersArray = myNumbers.Split(" ");
            //convert the winning numbers to a list of ints
            List<int> winningNumbersList = new List<int>();
            foreach(string winningNumber in winningNumbersArray)
            {
                winningNumbersList.Add(int.Parse(winningNumber));
            }
            //convert my numbers to a list of ints
            List<int> myNumbersList = new List<int>();
            foreach(string myNumber in myNumbersArray)
            {
                myNumbersList.Add(int.Parse(myNumber));
            }

            // count how many of my numbers are in the winning numbers
            int myNumbersInWinningNumbers = 0;
            foreach(int myNumber in myNumbersList)
            {
                if(winningNumbersList.Contains(myNumber))
                {
                    myNumbersInWinningNumbers++;
                }
            }

            int points = 0;
            if(myNumbersInWinningNumbers > 0)
            {
                points = (int)Math.Pow((double)2,(double)myNumbersInWinningNumbers - 1);
                Console.WriteLine("Card " + cardNumber + ": Found " + myNumbersInWinningNumbers + " of my numbers in the winning numbers. Points: " + points);
            }
            else{
                Console.WriteLine("Card " + cardNumber + ": Found none of my numbers in the winning numbers.");
            }

            part1sum += points;
        }

        file.Close();

        // print the sum
        Console.WriteLine("Part 1 Sum: " + part1sum);
        // 24542 is correct
    }

    // class for a Gear that contains a key, list of numbers that have matched
    public class Gear
    {
        public string key;
        public List<int> numbers = new List<int>();
    }

    public static void AddGear(string key, int number, Hashtable gears, string direction)
    {
        // check if the gear exists
        if(gears.ContainsKey(key) == false)
        {
            // create the gear
            Gear gear = new Gear();
            gear.key = key;
            // add the number to the gear
            gear.numbers.Add(number);
            // add the gear to the hashtable
            gears.Add(key, gear);
            //Console.WriteLine("Found adjacent character: " + line[numberIndex - 1] + " before number: " + number + " with key: " + key);
            Console.WriteLine("Created gear with key: " + key + direction + " number: " + number);
        }
        else
        {
            // add the number to the gear
            ((Gear)gears[key]).numbers.Add(number);
            Console.WriteLine("Added to gear with key: " + key + direction + " number: " + number);
        }
    }

    public static void RunPart2()
    {
        string filename = "day4/input.txt";
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        int part1sum = 0;
        long part2sum = 0;
        
        int cardNumber = 0;


        // get the line count of the file
        int lineCount = 0;
        while((line = file.ReadLine()) != null)
        {
            lineCount++;
        }

        // reset the file
        file.Close();
        file = new System.IO.StreamReader(filename);

        int[] cards = new int[lineCount];
        int cardIndex = 0;

        while((line = file.ReadLine()) != null)
        {
            cards[cardIndex]++;
            cardNumber++;

            //Console.WriteLine("Checking card: " + cardNumber);

            // parse a line like this into two sets of numbers, winning numbers and my numbers: Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            //int cardNumber = int.Parse(line.Substring(5, line.IndexOf(":") - 5));
            // split the rest of the line on the semicolon
            string[] cardSets = line.Substring(line.IndexOf(":") + 2).Split("|");
            // get the winning numbers
            string winningNumbers = cardSets[0].Trim();
            // get my numbers
            string myNumbers = cardSets[1].Trim();
            // trim multiple spaces down to one
            winningNumbers = Regex.Replace(winningNumbers, @"\s+", " ");
            myNumbers = Regex.Replace(myNumbers, @"\s+", " ");
            
            // split the winning numbers on the space
            string[] winningNumbersArray = winningNumbers.Split(" ");
            // split my numbers on the space
            string[] myNumbersArray = myNumbers.Split(" ");
            //convert the winning numbers to a list of ints
            List<int> winningNumbersList = new List<int>();
            foreach(string winningNumber in winningNumbersArray)
            {
                winningNumbersList.Add(int.Parse(winningNumber));
            }
            //convert my numbers to a list of ints
            List<int> myNumbersList = new List<int>();
            foreach(string myNumber in myNumbersArray)
            {
                myNumbersList.Add(int.Parse(myNumber));
            }

            // count how many of my numbers are in the winning numbers
            int myNumbersInWinningNumbers = 0;
            foreach(int myNumber in myNumbersList)
            {
                if(winningNumbersList.Contains(myNumber))
                {
                    myNumbersInWinningNumbers++;
                }
            }

            

            int points = 0;
            if(myNumbersInWinningNumbers > 0)
            {
                for(int i = cardIndex + 1; i < cardIndex + myNumbersInWinningNumbers + 1 && i < lineCount; i++)
                {
                    cards[i] += cards[cardIndex];
                }

                //Console.WriteLine("Card " + cardNumber + ": Found " + myNumbersInWinningNumbers + " of my numbers in the winning numbers. Points: " + points);
            }
            else{
                //Console.WriteLine("Card " + cardNumber + ": Found none of my numbers in the winning numbers.");
            }

            Console.WriteLine("Card " + cardNumber + ": " + cards[cardIndex] + " copies.");

            part2sum += cards[cardIndex];

            cardIndex++;
        }

        file.Close();
        
        // print the sum
        Console.WriteLine("Part 2 Sum: " + part2sum);
        // 8736438 is correct
    }
}