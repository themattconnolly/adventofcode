using System.Collections;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day19
{   
    private static string filename = "day19/input.txt";

    public static int[][] Grid = new int[256][];

    public struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class rule{
        public char category;
        public char comparison;
        public int threshold;
        public string nextWorkflow;
    }

    public class workflow {
        public string name;
        public List<rule> rules;
    }

    public class part {
        public int x;
        public int m;
        public int a;
        public int s;
    }

    public static List<workflow> workflows = new List<workflow>();

    public static List<part> parts = new List<part>();

    private static void ParseFile()
    {
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        string line;
        while((line = file.ReadLine()) != null)
        {
            if(string.IsNullOrEmpty(line))
            {
                break;
            }
            
            // parse this line into a rule: px{a<2006:qkq,m>2090:A,rfg}
            // everything before the brace is a workflow name
            workflow wf = new workflow();
            wf.rules = new List<rule>();
            wf.name = line.Substring(0, line.IndexOf('{'));
            string rulesText = line.Substring(line.IndexOf('{') + 1, line.IndexOf('}') - line.IndexOf('{') - 1);
            string[] ruleTexts = rulesText.Split(',');
            foreach(string ruleText in ruleTexts)
            {
                rule r = new rule();
                if(ruleText.Contains(':'))
                {
                    // split ruleText using regex on these characters <,>,:
                    string[] ruleParts = Regex.Split(ruleText, @"(<)|(>)|(:)");
                    r.category = char.Parse(ruleParts[0]);
                    r.comparison = char.Parse(ruleParts[1]);
                    r.threshold = int.Parse(ruleParts[2]);
                    r.nextWorkflow = ruleParts[4];
                }
                else
                {
                    r.nextWorkflow = ruleText; // should be another workflow name
                }
                wf.rules.Add(r);
            }

            workflows.Add(wf);
        }

        Regex regex = new Regex(@"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}");
        while((line = file.ReadLine()) != null)
        {
            // regex the integers out of this: {x=787,m=2655,a=1222,s=2876}    
            Match match = regex.Match(line);
            part p = new part();
            p.x = int.Parse(match.Groups[1].Value);
            p.m = int.Parse(match.Groups[2].Value);
            p.a = int.Parse(match.Groups[3].Value);
            p.s = int.Parse(match.Groups[4].Value);
            parts.Add(p);
        }

        file.Close();
    }

    public static string ProcessPart(part part, string workflowName)
    {
        if(workflowName == "A" || workflowName == "R")
        {
            return workflowName;
        }

        workflow wf = workflows.Find(w => w.name == workflowName);

        for(int ruleIndex = 0; ruleIndex < wf.rules.Count; ruleIndex++)
        {
            rule r = wf.rules[ruleIndex];
            if(r.category == 'x')
            {
                if(r.comparison == '<')
                {
                    if(part.x < r.threshold)
                    {
                        return ProcessPart(part, r.nextWorkflow);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if(r.comparison == '>')
                {
                    if(part.x > r.threshold)
                    {
                        return ProcessPart(part, r.nextWorkflow);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else if(r.category == 'm')
            {
                if(r.comparison == '<')
                {
                    if(part.m < r.threshold)
                    {
                        return ProcessPart(part, r.nextWorkflow);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if(r.comparison == '>')
                {
                    if(part.m > r.threshold)
                    {
                        return ProcessPart(part, r.nextWorkflow);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else if(r.category == 'a')
            {
                if(r.comparison == '<')
                {
                    if(part.a < r.threshold)
                    {
                        return ProcessPart(part, r.nextWorkflow);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if(r.comparison == '>')
                {
                    if(part.a > r.threshold)
                    {
                        return ProcessPart(part, r.nextWorkflow);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else if(r.category == 's')
            {
                if(r.comparison == '<')
                {
                    if(part.s < r.threshold)
                    {
                        return ProcessPart(part, r.nextWorkflow);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if(r.comparison == '>')
                {
                    if(part.s > r.threshold)
                    {
                        return ProcessPart(part, r.nextWorkflow);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                if(ruleIndex != wf.rules.Count - 1)
                    throw new Exception("Invalid rule!");
                return ProcessPart(part, r.nextWorkflow); // last step
            }
        }

        throw new Exception("Something bad happened");
    }
    
    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;
        foreach(part part in parts)
        {
            if(ProcessPart(part, "in") == "A")
            {
                part1sum += part.x + part.m + part.a + part.s;
            }
        }

        Console.WriteLine("Part 1 : " + part1sum);
        // 376008 is right!
    }

    
    public static void RunPart2()
    {
        ParseFile();

        
        //Console.WriteLine("Part 2 : " + part2sum);
        // 
    }

}