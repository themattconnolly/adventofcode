using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day3
{   
    // function that reads in a file line by line and writes the output to the console
    public static void Run()
    {
        string filename = "day3/input.txt";
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        int part1sum = 0;
        long part2sum = 0;
        
        string previousLine = "";
        string nonSymbolCharacters = "0123456789.";

        int lineNumber = 0;
        Hashtable foundNumbers = new Hashtable();

        while((line = file.ReadLine()) != null)
        {
            lineNumber++;

            Console.WriteLine("Checking line: " + lineNumber);
            // find all the multi-digit numbers in the line with regex
            MatchCollection matches = Regex.Matches(line, @"\d+");
            // iterate through the matches
            foreach(Match match in matches)
            {
                // get the number
                int number = int.Parse(match.Value);
                
                // get the index of the number in the line
                //int numberIndex = line.IndexOf(match.Value);
                int numberIndex = match.Index;

                string key = string.Concat(lineNumber + "-" + numberIndex);

                //Console.WriteLine("Checking adjacent to number: " + number);

                // check to see if a character other than a digit or period is adjacent to the number in the current line
                if(numberIndex > 0 && nonSymbolCharacters.Contains(line[numberIndex - 1]) == false)
                {    
                    foundNumbers.Add(key, number);
                    Console.WriteLine("Found adjacent character: " + line[numberIndex - 1] + " before number: " + number + " with key: " + key);
                    part1sum += number;
                }
                else if(numberIndex + match.Value.Length < line.Length && nonSymbolCharacters.Contains(line[numberIndex + match.Value.Length]) == false)
                {
                    foundNumbers.Add(key, number);
                    Console.WriteLine("Found adjacent character: " + line[numberIndex + match.Value.Length] + " after number: " + number + " with key: " + key);
                    part1sum += number;
                }
                
                if(foundNumbers.ContainsKey(key) == false && previousLine != "")
                {
                    //Console.WriteLine("Checking above number: " + number);
                    // check to see if a character other than a digit or period is adjacent to the number in the previous line
                    if(numberIndex > 0 && nonSymbolCharacters.Contains(previousLine[numberIndex - 1]) == false)
                    {
                        foundNumbers.Add(key, number);
                        Console.WriteLine("Found adjacent character: " + previousLine[numberIndex - 1] + " above number: " + number + " with key: " + key);
                        part1sum += number;
                    }
                    else if(numberIndex + match.Value.Length < previousLine.Length && nonSymbolCharacters.Contains(previousLine[numberIndex + match.Value.Length]) == false)
                    {
                        foundNumbers.Add(key, number);
                        Console.WriteLine("Found adjacent character: " + previousLine[numberIndex + match.Value.Length] + " above number: " + number + " with key: " + key);
                        part1sum += number;
                    }
                    else
                    {
                        for(int previousLineIndex = numberIndex; previousLineIndex < numberIndex + match.Value.Length; previousLineIndex++)
                        {
                            if(nonSymbolCharacters.Contains(previousLine[previousLineIndex]) == false)
                            {
                                foundNumbers.Add(key, number);
                                Console.WriteLine("Found adjacent character: " + previousLine[previousLineIndex] + " above number: " + number + " with key: " + key);
                                part1sum += number;
                                break;
                            }
                        }
                    }
                }

                if(foundNumbers.ContainsKey(key) == false)
                {
                    Console.WriteLine("No adjacent characters found next to or above number: " + number + " with key: " + key);
                }
            }

            if(previousLine != "")
            {
                // check the previous line for number matches that have characters other than digits or periods adjacent below them
                MatchCollection previousLineMatches = Regex.Matches(previousLine, @"\d+");
                // iterate through the matches
                foreach(Match previousLineMatch in previousLineMatches)
                {
                    // get the number
                    int previousLineNumber = int.Parse(previousLineMatch.Value);
                    
                    // get the index of the number in the line
                    //int previousLineNumberIndex = previousLine.IndexOf(previousLineMatch.Value);
                    int previousLineNumberIndex = previousLineMatch.Index;

                    string previousLineKey = string.Concat((lineNumber - 1) + "-" + previousLineNumberIndex);

                    //Console.WriteLine("Checking previous line number: " + previousLineNumber);

                    if(foundNumbers.ContainsKey(previousLineKey) == false)
                    {
                        //Console.WriteLine("Checking below previous line number: " + previousLineNumber);
                        if(previousLineNumberIndex > 0 && nonSymbolCharacters.Contains(line[previousLineNumberIndex - 1]) == false)
                        {
                            foundNumbers.Add(previousLineKey, previousLineNumber);
                            Console.WriteLine("Found adjacent character: " + line[previousLineNumberIndex - 1] + " below number: " + previousLineNumber + " with key:" + previousLineKey);
                            part1sum += previousLineNumber;
                        }
                        else if(previousLineNumberIndex + previousLineMatch.Value.Length < line.Length && nonSymbolCharacters.Contains(line[previousLineNumberIndex + previousLineMatch.Value.Length]) == false)
                        {
                            foundNumbers.Add(previousLineKey, previousLineNumber);
                            Console.WriteLine("Found adjacent character: " + line[previousLineNumberIndex + previousLineMatch.Value.Length] + " below number: " + previousLineNumber + " with key:" + previousLineKey);
                            part1sum += previousLineNumber;
                        }
                        else
                        {
                            for(int currentLineIndex = previousLineNumberIndex; currentLineIndex < previousLineNumberIndex + previousLineMatch.Value.Length; currentLineIndex++)
                            {
                                //Console.WriteLine("Checking subsequent line character: " + line[currentLineIndex]);
                                if(nonSymbolCharacters.Contains(line[currentLineIndex]) == false)
                                {
                                    foundNumbers.Add(previousLineKey, previousLineNumber);
                                    Console.WriteLine("Found adjacent character: " + line[currentLineIndex] + " below number: " + previousLineNumber + " with key:" + previousLineKey);
                                    part1sum += previousLineNumber;
                                    break;
                                }
                            }
                        }

                        if(foundNumbers.ContainsKey(previousLineKey) == false)
                        {
                            Console.WriteLine("No adjacent characters found below number: " + previousLineNumber + " with key: " + previousLineKey);
                        }
                    }
                }
            }

            previousLine = line;
        }

        file.Close();

        // print the sum
        Console.WriteLine("Part 1 Sum: " + part1sum);
        // 535962 is too low
        // 536202

        // print the sum
        //Console.WriteLine("Part 2 Sum: " + part2sum);
        // 
    }
}