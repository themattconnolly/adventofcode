using System.Collections;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day15
{   
    private static string filename = "day15/input.txt";

    public static List<string> inputs = new List<string>();

    private static void ParseFile()
    {
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        string line;
        while((line = file.ReadLine()) != null)
        {
            // split line on commas and add each to inputs
            inputs.AddRange(line.Split(","));
        }

        file.Close();

        Console.WriteLine("Inputs count: " + inputs.Count);        
    }

    private static int PerformHASH(string input)
    {
        int hashValue = 0;
        for(int i = 0; i < input.Length; i++)
        {
            hashValue += (int)input[i];
            hashValue *= 17;
            hashValue %= 256;
        }
        return hashValue;
    }
    
    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;
        
        foreach(string input in inputs)
        {
            int hashValue = PerformHASH(input);
            part1sum += hashValue;

            Console.WriteLine("Input: " + input + " Hash: " + hashValue);
        }

        Console.WriteLine("Part 1 : " + part1sum);
        // 510013 is right!
    }

    public class lens{
        public string label { get; set; }
        public int focalLength { get; set; }
    }

    public class box{
        public List<lens> lenses { get; set; }
    }
    
    public static void RunPart2()
    {
        ParseFile();

        

        box[] boxes = new box[256];

        foreach(string input in inputs)
        {
            // split input on either = or - keeping the split characters
            string[] splitInput = Regex.Split(input, "(=|-)");

            //StringSplitOptions St = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.;
            //string[] splitInput = input.Split(new string[] { "=", "-" }, St);
            int hashValue = PerformHASH(splitInput[0]);
            string label = splitInput[0];
            string operation = splitInput[1];

            if(operation == "=")
            {
                if(boxes[hashValue] == null)
                {
                    boxes[hashValue] = new box();
                    boxes[hashValue].lenses = new List<lens>();
                }

                int focalLength = int.Parse(splitInput[2]);
                lens lens = new lens();
                lens.label = label;
                lens.focalLength = focalLength;
                lens foundLens = boxes[hashValue].lenses.Find(x => x.label == lens.label);
                if(foundLens != null)
                {
                    int indexOfLens = boxes[hashValue].lenses.IndexOf(foundLens);
                    boxes[hashValue].lenses[indexOfLens].focalLength = focalLength;
                }
                else
                {
                    boxes[hashValue].lenses.Add(lens);
                }
            }
            else if(operation == "-")
            {
                if(boxes[hashValue] != null)
                {
                    lens foundLens = boxes[hashValue].lenses.Find(x => x.label == label);
                    if(foundLens != null)
                    {
                        boxes[hashValue].lenses.Remove(foundLens);
                    }
                }
            }
        }

        long part2sum = 0;
        for(int boxNumber = 0; boxNumber < 256; boxNumber++)
        {
            if(boxes[boxNumber] != null)
            {
                for(int lensSlot = 0; lensSlot < boxes[boxNumber].lenses.Count; lensSlot++)
                {
                    int focusingPower = 1 + boxNumber;
                    focusingPower *= (lensSlot + 1);
                    focusingPower *= boxes[boxNumber].lenses[lensSlot].focalLength;

                    part2sum += focusingPower;

                    // write the lens and focusing power to the console
                    Console.WriteLine("Box: " + boxNumber + " Lens: " + boxes[boxNumber].lenses[lensSlot].label + " Focusing Power: " + focusingPower);
                }
            }
        }

        Console.WriteLine("Part 2 : " + part2sum);
        // 268497 is right!
    }

}