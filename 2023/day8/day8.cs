using System.Collections;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day8
{   
    private static string filename = "day8/input.txt";
    
    // class with a source range start int, destination range start int, and range length int
    public class Node
    {
        public string key;
        public string left;
        public string right;

    }

    private static Hashtable nodes = new Hashtable();

    private static void ParseFile()
    {
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        Instructions = file.ReadLine();
        Console.WriteLine("Instructions: " + Instructions);

        file.ReadLine(); // blank
        
        string line;
        while((line = file.ReadLine()) != null)
        {
            // parse line like "TNX = (BBN, MXH)" into a node where left is BBN and right is MXH
            string[] lineSplits = line.Trim().Split(" ");
            Node node = new Node();
            node.key = lineSplits[0];
            node.left = lineSplits[2].TrimEnd(',').TrimStart('(');
            node.right = lineSplits[3].TrimEnd(')');
            
            nodes.Add(node.key, node);

            Console.WriteLine("Node " + node.key + " has left " + node.left + " and right " + node.right);
        }
    }

    private static string Instructions = "";
    public static void RunPart1()
    {
        ParseFile();
        

        int steps = 0;
        bool foundZZZ = false;
        Node currentNode = (Node)nodes["AAA"];
        while(foundZZZ == false)
        {
            for(int i = 0; i < Instructions.Length; i++)
            {
                if(Instructions[i] == 'L')
                {
                    currentNode = (Node)nodes[currentNode.left];
                }
                else
                {
                    currentNode = (Node)nodes[currentNode.right];
                }
                steps++;
                
                if(currentNode.key == "ZZZ")
                {
                    foundZZZ = true;
                    break;
                }
            }
        }

        Console.WriteLine("Part 1 : " + steps);
        // 19637 is right!

        // part 2:
        // 
    }

    public static void RunPart2()
    {
        ParseFile();
    }
}