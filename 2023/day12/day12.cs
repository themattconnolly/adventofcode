using System.Collections;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace _2023;
public class Day12
{   
    private static string filename = "day12/input.txt";

    public static List<PartSet> PartSets = new List<PartSet>();

    public interface IPart
    {
        public int Position { get; set; }
        public int Length { get; set; }
    }

    public class Part : IPart
    {
        public int Position { get; set; }
        public int Length { get; set; }
    }

    public class UnknownPartSet : IPart
    {
        public string RawText { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
    }

    public class PartSet
    {
        public string RawText { get; set; }
        public List<IPart> Parts { get; set; }
        public List<int> PartLengths { get; set; }
    }

    private static void ParseFile()
    {
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        string line;
        while((line = file.ReadLine()) != null)
        {
            string[] sections = line.Split(" ");

            PartSet partSet = new PartSet();
            partSet.Parts = new List<IPart>();
            // parse a string like "1,2,3" into a list of ints
            string[] partLengthText = sections[1].Split(',');
            List<int> partLengths = new List<int>();
            foreach(string partLength in partLengthText)
            {
                partLengths.Add(int.Parse(partLength));
            }
            partSet.PartLengths = partLengths;

            partSet.RawText = sections[0];

            MatchCollection matchCollection = Regex.Matches(sections[0], @"[?#]+");
            foreach(Match match in matchCollection)
            {
                // if partText is entirely made of the character '#' create a part
                if(match.Value.All(c => c == '#'))
                {
                    Part part = new Part();
                    part.Position = match.Index;
                    part.Length = match.Value.Length;
                    partSet.Parts.Add(part);
                }
                else
                {
                    // otherwise create an unknown part
                    UnknownPartSet unknownPartSet = new UnknownPartSet();
                    unknownPartSet.RawText = match.Value;
                    unknownPartSet.Length = match.Value.Length;
                    unknownPartSet.Position = match.Index;
                    partSet.Parts.Add(unknownPartSet);
                }
            }

            PartSets.Add(partSet);
        }

        file.Close();

        // foreach(PartSet partSet in PartSets)
        // {
        //     Console.WriteLine("Part set: " + partSet.RawText + " with lengths: " + string.Join(",", partSet.PartLengths));
        //     foreach(IPart part in partSet.Parts)
        //     {
        //         if(part is Part)
        //         {
        //             Console.WriteLine("- Part: " + part.Position + ", " + part.Length);
        //         } else if(part is UnknownPartSet)
        //         {
        //             Console.WriteLine("- Unknown part: " + part.Position + ", " + part.Length + " with text: " + ((UnknownPartSet)part).RawText);
        //         }
        //     }

        //     int possibleArrangements = PossibleArrangements(partSet);
        //     Console.WriteLine("- Possible arrangements: " + possibleArrangements);
        // }
    }

    private static int PossibleArrangementsSimple(string remainingPartText, List<int> remainingPartLengths)
    {
        if(remainingPartText.Length == 0 && remainingPartLengths.Count == 0)
        {
            return 1;
        }
        else if(remainingPartText.Length > 0)
        {
            if(remainingPartLengths.Sum() > remainingPartText.Length)
            {
                return 0;
            }

            if(remainingPartLengths.Count == 0)
            {
                // if remainingPartText only contains . and ? this is possible
                if(remainingPartText.All(c => c == '.' || c == '?'))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            char nextChar = remainingPartText[0];
            if(nextChar == '.') // skip until something interesting
            {
                return PossibleArrangementsSimple(remainingPartText.Substring(1), remainingPartLengths);
            }
            else if(nextChar == '?')
            {
                // if the next character is a ?, then we can either skip it or replace it with a #
                int optionA = PossibleArrangementsSimple(remainingPartText.Substring(1), remainingPartLengths);
                int optionB = PossibleArrangementsSimple(string.Concat('#',remainingPartText.Substring(1)), remainingPartLengths);

                optionA = PossibleArrangementsSimple(remainingPartText.Substring(1), remainingPartLengths);
                optionB = PossibleArrangementsSimple(string.Concat('#',remainingPartText.Substring(1)), remainingPartLengths);
                return optionA + optionB;
            }
            else if(nextChar == '#')
            {
                int nextRemainingPartLength = remainingPartLengths[0];
                if(remainingPartLengths.Count > 0 && remainingPartText.Length >= nextRemainingPartLength)
                {
                    if(remainingPartText.Substring(0,nextRemainingPartLength).Contains('.') == false)
                    {
                        if(remainingPartText.Length == nextRemainingPartLength && remainingPartLengths.Count == 1)
                        {
                            return 1;
                        }
                        else
                        {
                            string remainingPartTextAfterPart = remainingPartText.Substring(nextRemainingPartLength);
                            if(remainingPartTextAfterPart.Length > 0 && remainingPartTextAfterPart[0] == '#')
                            {
                                return 0;
                            }
                            else
                            {
                                return PossibleArrangementsSimple(remainingPartTextAfterPart.Substring(1), remainingPartLengths.Skip(1).ToList());
                            }
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                // invalid character
                throw new Exception("Invalid character");
            }
        }
        else
        {
            return 0;
        }
    }

    private static int PossibleArrangements(PartSet partSet)
    {
        int possibleArrangements = 1;
        
        List<int> partLengths = partSet.PartLengths;

        if(partLengths.Count == partSet.Parts.Count)
        {
            // we know the mapping of part/uknown part to part length, easy calculation
            for(int i = 0; i < partSet.Parts.Count; i++)
            {
                if(partSet.Parts[i] is UnknownPartSet)
                {
                    UnknownPartSet unknownPartSet = (UnknownPartSet)partSet.Parts[i];
                    int possiblePlacements = PossiblePlacementsOfPart(unknownPartSet.RawText, partLengths[i]);
                    possibleArrangements *= possiblePlacements;
                }
            }
        }
        else {
            bool foundExactPart = false;
            for(int i = 0; i < partSet.Parts.Count; i++)
            {
                if(partSet.Parts[i] is Part && partLengths.Count(p => p == partSet.Parts[i].Length) == 1)
                {
                    int partLengthIndex = partLengths.IndexOf(partSet.Parts[i].Length);
                    Console.WriteLine("-- Only 1 partLength with length " + partLengths[partLengthIndex] + " found");

                    // split the partLengths into 2 parts, before and after the partLength
                    List<int> partLengthsBefore = partLengths.GetRange(0, partLengthIndex);
                    List<int> partLengthsAfter = partLengths.GetRange(partLengthIndex + 1, partLengths.Count - partLengthIndex - 1);

                    // split the parts into 2 parts, before and after the part
                    List<IPart> partsBefore = partSet.Parts.GetRange(0, i);
                    List<IPart> partsAfter = partSet.Parts.GetRange(i + 1, partSet.Parts.Count - i - 1);

                    // calculate the number of possible arrangements for the parts before and after the part
                    int possibleArrangementsBefore = PossibleArrangements(new PartSet { PartLengths = partLengthsBefore, Parts = partsBefore });
                    int possibleArrangementsAfter = PossibleArrangements(new PartSet { PartLengths = partLengthsAfter, Parts = partsAfter });

                    // the number of possible arrangements is the number of possible arrangements before the part multiplied by the number of possible arrangements after the part
                    possibleArrangements *= possibleArrangementsBefore * possibleArrangementsAfter;

                    foundExactPart = true;
                    break;
                }
            }

            if(foundExactPart == false)
            {
                for(int i = 0; i < partSet.Parts.Count; i++)
                {
                    if(partSet.Parts[i] is Part)
                    {
                        if(partLengths.Count(p => p == partSet.Parts[i].Length) > 1)
                        {
                            // if there are multiple parts with the same length, then the number of possible arrangements is the number of parts with that length
                            Console.WriteLine("-- Multiple parts with length " + partLengths[i] + " found");
                            for(int j = 0; j < partLengths.Count; j++)
                            {
                                if(partLengths[j] == partSet.Parts[i].Length)
                                {
                                    // j represents the number of part lengths before this match
                                    int remainingPartLengths  = partLengths.Count - j;

                                    // i represents the number of parts before this match
                                    int remainingParts = partSet.Parts.Count - i;

                                    if(i == j || remainingPartLengths == remainingParts)
                                    {
                                        // part i matches partLength j
                                        Console.WriteLine("-- Part " + i + ", length: " + partSet.Parts[i].Length + " matches part length " + j + ", length : " + partLengths[j] );
                                    }

                                    // // is there a match before or after this part?
                                    // if(j > 0 && partLengths[j - 1] == partSet.Parts[i].Length)
                                    // {
                                    //     // if there is a match before this part, then the number of possible arrangements is the number of parts with that length
                                    //     possibleArrangements *= partLengths[j];
                                    // }
                                    // else if(j < partLengths.Count - 1 && partLengths[j + 1] == partSet.Parts[i].Length)
                                    // {
                                    //     // if there is a match after this part, then the number of possible arrangements is the number of parts with that length
                                    //     possibleArrangements *= partLengths[j];
                                    // }
                                    // else
                                    // {
                                    //     // otherwise the number of possible arrangements is the length of the part
                                    //     possibleArrangements *= partLengths[j];
                                    // }
                                    // possibleArrangements *= partLengths[j];
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("-- ???");
                        }

                        //possibleArrangements *= partLengths[i];
                    }
                    else // uknown part
                    {
                        UnknownPartSet unknownPartSet = (UnknownPartSet)partSet.Parts[i];
                        if(unknownPartSet.RawText.Contains("#"))
                        {
                            Console.WriteLine("-- Unknown part " + i + " contains a #");
                        }
                        else {
                            Console.WriteLine("-- Unknown part " + i + " does not contain a #");
                        }

                        Console.WriteLine("-- Need to figure this out");
                    }
                }        
            }
        }

        return possibleArrangements;
    }

    public static int PossiblePlacementsOfPart(string unknownPartText, int partLength)
    {
        int possibleArrangements = 0;

        for(int i = 0; i <= unknownPartText.Length - partLength; i++)
        {
            possibleArrangements++;
        }

        return possibleArrangements;
    }

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;
        foreach(PartSet partSet in PartSets)
        {
            int possibleArrangements = PossibleArrangementsSimple(partSet.RawText, partSet.PartLengths);
            Console.WriteLine("Part set: " + partSet.RawText + " with lengths: " + string.Join(",", partSet.PartLengths) + " has " + possibleArrangements + " possible arrangements");
            part1sum += possibleArrangements;
        }
        
        Console.WriteLine("Part 1 : " + part1sum);
        // 7169 is right!
    }
    
    public static void RunPart2()
    {
        ParseFile();

        //Console.WriteLine("Part 2 : " + part2sum);
        // 
    }

}