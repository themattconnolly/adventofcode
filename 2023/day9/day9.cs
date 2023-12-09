using System.Collections;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day9
{   
    private static string filename = "day9/input.txt";

    private static List<int[]> sequences = new List<int[]>();

    private static void ParseFile()
    {
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        string line;
        while((line = file.ReadLine()) != null)
        {
            // parse a line like "-1 10 45 -25"
            string[] lineSplits = line.Trim().Split(" ");
            int[] sequence = new int[lineSplits.Length];
            for(int i = 0; i < lineSplits.Length; i++)
            {
                sequence[i] = int.Parse(lineSplits[i]);
            }
            sequences.Add(sequence);

            //Console.WriteLine("Parsed sequence: " + sequence.ToString());
        }        
    }

    private static int DetermineNextNumber(int[] sequence)
    {
        int nextNumber = 0;

        List<int[]> currentSequences = new List<int[]>();
        currentSequences.Add(sequence);
        int[] currentSequence = sequence;

        while(currentSequence.Any(n => n != 0))
        {
            // create a new sequence that is populated with the differences between subsequent numbers
            int[] newSequence = new int[currentSequence.Length - 1];
            for(int i = 0; i < currentSequence.Length - 1; i++)
            {
                newSequence[i] = currentSequence[i + 1] - currentSequence[i];
            }
            currentSequence = newSequence;
            currentSequences.Add(currentSequence);
        }

        // write out all the previous sequences
        // foreach(int[] seq in previousSequences)
        // {
        //     Console.WriteLine("Previous sequence: " + string.Join(',', seq));
        // }

        // go back up the chain and add the last number in each sequence to the next number
        for(int i = currentSequences.Count - 1; i >= 0; i--)
        {
            int[] newSequence = new int[currentSequences[i].Length + 1];
            // copy the old sequence into the new one
            for(int j = 0; j < currentSequences[i].Length; j++)
            {
                newSequence[j] = currentSequences[i][j];
            }

            int newLastNumber = 0;
            // add the last number in the previous sequence to the last number in the current sequence
            if(i != currentSequences.Count - 1)
            {
                newLastNumber = currentSequences[i + 1].Last() + currentSequences[i].Last();
            }
            
            newSequence[newSequence.Length - 1] = newLastNumber;
            currentSequences[i] = newSequence;
        }

        // write out all the sequences
        foreach(int[] seq in currentSequences)
        {
            Console.WriteLine("After: " + string.Join(',', seq));
        }

        nextNumber = currentSequences[0].Last();

        return nextNumber;
    }

    private static int DeterminePreviousNumber(int[] sequence)
    {
        int nextNumber = 0;

        List<int[]> currentSequences = new List<int[]>();
        currentSequences.Add(sequence);
        int[] currentSequence = sequence;

        while(currentSequence.Any(n => n != 0))
        {
            // create a new sequence that is populated with the differences between subsequent numbers
            int[] newSequence = new int[currentSequence.Length - 1];
            for(int i = 0; i < currentSequence.Length - 1; i++)
            {
                newSequence[i] = currentSequence[i + 1] - currentSequence[i];
            }
            currentSequence = newSequence;
            currentSequences.Add(currentSequence);
        }

        // write out all the previous sequences
        // foreach(int[] seq in previousSequences)
        // {
        //     Console.WriteLine("Previous sequence: " + string.Join(',', seq));
        // }

        // go back up the chain and add the last number in each sequence to the next number
        for(int i = currentSequences.Count - 1; i >= 0; i--)
        {
            int[] newSequence = new int[currentSequences[i].Length + 1];
            // copy the old sequence into the new one
            for(int j = 0; j < currentSequences[i].Length; j++)
            {
                newSequence[j+1] = currentSequences[i][j];
            }

            int newFirstNumber = 0;
            if(i != currentSequences.Count - 1)
            {
                newFirstNumber = currentSequences[i][0] - currentSequences[i + 1][0];
            }
            
            newSequence[0] = newFirstNumber;
            currentSequences[i] = newSequence;
        }

        // write out all the sequences
        foreach(int[] seq in currentSequences)
        {
            Console.WriteLine("After: " + string.Join(',', seq));
        }

        nextNumber = currentSequences[0][0];

        return nextNumber;
    }
    public static void RunPart1()
    {
        ParseFile();
        int part1sum = 0;
        
        foreach(int[] seq in sequences)
        {

            int nextNumber = DetermineNextNumber(seq);
            part1sum += nextNumber;
            Console.WriteLine("Next number: " + nextNumber);
        }

        Console.WriteLine("Part 1 : " + part1sum);
        // 1980437560 is right!
    }

    public static void RunPart2()
    {
        int part2sum = 0;

        ParseFile();
        
        foreach(int[] seq in sequences)
        {
            int nextNumber = DeterminePreviousNumber(seq);
            part2sum += nextNumber;
            Console.WriteLine("Previous number: " + nextNumber);
        }

        Console.WriteLine("Part 2 : " + part2sum);
        // 977 is right!
    }
}