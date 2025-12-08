using System.Collections;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2025;
public class Day8
{
    private static List<(int,Vector3)> BoxLocations = new List<(int,Vector3)>();

    private static List<List<(int,int)>> Circuits = new List<List<(int,int)>>();

    private static Dictionary<(int,int),long> DistanceCache = new Dictionary<(int,int),long>();

    private static void ParseFile()
    {
        string filename = System.IO.Path.Combine(AppContext.BaseDirectory, "day8/input.txt");
        List<string> lines = new List<string>();
        using(System.IO.StreamReader file = new System.IO.StreamReader(filename))
        {
            string? line;
            while((line = file.ReadLine()) != null && line.Trim().Length > 0)
            {
                lines.Add(line);
            }
        }

        for(int i = 0; i < lines.Count; i++)
        {
            string[] parts = lines[i].Split(',');
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);
            BoxLocations.Add((i,new Vector3(x, y, z)));
        }
    }


    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;

        int maxConnections = 10;
        maxConnections = 1000;

        // find the shortest connection between every box
        List<(int,int,long)> connections = new List<(int,int,long)>();
        for(int i = 0; i < BoxLocations.Count; i++)
        {
            for(int j = 0; j < BoxLocations.Count; j++)
            {
                if(i == j)
                {
                    continue;
                }
                long dist = (long)Vector3.Distance(BoxLocations[i].Item2, BoxLocations[j].Item2);
                if(!DistanceCache.ContainsKey((i,j)) && !DistanceCache.ContainsKey((j,i)))
                {
                    DistanceCache[(i,j)] = dist;
                    connections.Add((i,j,dist));
                }
            }
        }

        connections.Sort((a,b) => a.Item3.CompareTo(b.Item3));

            // List<int> circuit = new List<int>();
            // for(int c = 0; c < Math.Min(maxConnections, connections.Count); c++)
            // {
            //     circuit.Add(connections[c].Item1);
            // }
            // Circuits.Add(circuit);

        for(int i = 0; i < maxConnections; i++)
        {
            //var box1 = BoxLocations[connections[i].Item1];
            //var box2 = BoxLocations[connections[i].Item2];
            //Console.WriteLine($"Connection: Box {box1.Item2} <-> Box {box2.Item2} (Distance: {connections[i].Item3})");
            Console.WriteLine($"Connection: Box {connections[i].Item1} <-> Box {connections[i].Item2} (Distance: {connections[i].Item3})");
            
            List<List<(int,int)>> matchedCircuits = new List<List<(int,int)>>();
            // see if any circuit contains this connection
            foreach(var circuit in Circuits)
            {
                // if any item in the circuit contains either direction of the connection, potential connection
                if(circuit.Any(c => c.Item1 == connections[i].Item1 || c.Item2 == connections[i].Item1 || 
                    c.Item1 == connections[i].Item2 || c.Item2 == connections[i].Item2))
                {
                    matchedCircuits.Add(circuit);
                }
            }

            if(matchedCircuits.Count == 0)
            {
                // create a new circuit
                Circuits.Add(new List<(int,int)>() { (connections[i].Item1, connections[i].Item2) });
                Console.WriteLine("  Created new circuit.");
            }
            else if(matchedCircuits.Count == 1)
            {
                if(matchedCircuits[0].Any(c => c.Item1 == connections[i].Item1 || c.Item2 == connections[i].Item1) &&
                   matchedCircuits[0].Any(c => c.Item1 == connections[i].Item2 || c.Item2 == connections[i].Item2))
                {
                    // both items already in the circuit - skip
                    Console.WriteLine("  Both boxes already in the circuit - skipping.");
                    continue;
                }
                // add to existing circuit
                matchedCircuits[0].Add((connections[i].Item1, connections[i].Item2));
                Console.WriteLine("  Added to existing circuit.");
            }
            else if(matchedCircuits.Count == 2)
            {
                // confirm one matched circuit contains Item1 and the other contains Item2
                if(matchedCircuits[0].Any(c => c.Item1 == connections[i].Item1 || c.Item2 == connections[i].Item1) &&
                   matchedCircuits[1].Any(c => c.Item1 == connections[i].Item2 || c.Item2 == connections[i].Item2))
                {
                    // ok - merge the 2 circuits and add the connection
                    matchedCircuits[0].AddRange(matchedCircuits[1]);
                    Circuits.Remove(matchedCircuits[1]);
                    matchedCircuits[0].Add((connections[i].Item1, connections[i].Item2));
                    Console.WriteLine("  Merged two circuits.");
                }
                else if(matchedCircuits[1].Any(c => c.Item1 == connections[i].Item1 || c.Item2 == connections[i].Item1) &&
                        matchedCircuits[0].Any(c => c.Item1 == connections[i].Item2 || c.Item2 == connections[i].Item2))
                {
                    // ok - merge the 2 circuits and add the connection
                    matchedCircuits[0].AddRange(matchedCircuits[1]);
                    Circuits.Remove(matchedCircuits[1]);
                    matchedCircuits[0].Add((connections[i].Item1, connections[i].Item2));
                    Console.WriteLine("  Merged two circuits.");
                }
                else
                {
                    throw new Exception("Mismatched circuits!");
                }
            }
            else
            {
                throw new Exception("More than 2 matched circuits found!");
            }

            // // print out all the circuits
            // foreach(var circuit in Circuits)
            // {
            //     // count the number of boxes in the circuit
            //     int boxCount = circuit.SelectMany(c => new List<int>() { c.Item1, c.Item2 }).Distinct().Count();

            //     Console.WriteLine("Circuit: " + boxCount + " boxes");
            //     foreach(var conn in circuit)
            //     {
            //         Console.WriteLine($"  Box {conn.Item1} <-> Box {conn.Item2} (Distance: {DistanceCache[(conn.Item1, conn.Item2)]})");
            //     }

            //     part1sum += boxCount;
            // }

            // Console.WriteLine("Press Enter to continue...");
            // Console.ReadLine();
        }

        // sort circuits by number of boxes
        Circuits.Sort((a,b) => b.SelectMany(c => new List<int>() { c.Item1, c.Item2 }).Distinct().Count()
                                    .CompareTo(a.SelectMany(c => new List<int>() { c.Item1, c.Item2 }).Distinct().Count()));

        // print out all the circuits
        // Console.WriteLine("");
        // Console.WriteLine("Final Circuits:");
        part1sum = 1;
        for(int i = 0; i < 3; i++)
        {
            // count the number of boxes in the circuit
            int boxCount = Circuits[i].SelectMany(c => new List<int>() { c.Item1, c.Item2 }).Distinct().Count();

            // Console.WriteLine("Circuit: " + boxCount + " boxes");
            // foreach(var conn in circuit)
            // {
            //     Console.WriteLine($"  Box {conn.Item1} <-> Box {conn.Item2} (Distance: {DistanceCache[(conn.Item1, conn.Item2)]})");
            // }

            part1sum *= boxCount;
        }

        Console.WriteLine("Part 1 : " + part1sum);
    }
    
    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        int maxConnections = 10;
        maxConnections = 100000;

        // find the shortest connection between every box
        List<(int,int,long)> connections = new List<(int,int,long)>();
        for(int i = 0; i < BoxLocations.Count; i++)
        {
            for(int j = 0; j < BoxLocations.Count; j++)
            {
                if(i == j)
                {
                    continue;
                }
                long dist = (long)Vector3.Distance(BoxLocations[i].Item2, BoxLocations[j].Item2);
                if(!DistanceCache.ContainsKey((i,j)) && !DistanceCache.ContainsKey((j,i)))
                {
                    DistanceCache[(i,j)] = dist;
                    connections.Add((i,j,dist));
                }
            }
        }

        connections.Sort((a,b) => a.Item3.CompareTo(b.Item3));

            // List<int> circuit = new List<int>();
            // for(int c = 0; c < Math.Min(maxConnections, connections.Count); c++)
            // {
            //     circuit.Add(connections[c].Item1);
            // }
            // Circuits.Add(circuit);

        for(int i = 0; i < connections.Count(); i++)
        {
            //var box1 = BoxLocations[connections[i].Item1];
            //var box2 = BoxLocations[connections[i].Item2];
            //Console.WriteLine($"Connection: Box {box1.Item2} <-> Box {box2.Item2} (Distance: {connections[i].Item3})");
            Console.WriteLine($"Connection: Box {connections[i].Item1} <-> Box {connections[i].Item2} (Distance: {connections[i].Item3})");
            
            List<List<(int,int)>> matchedCircuits = new List<List<(int,int)>>();
            // see if any circuit contains this connection
            foreach(var circuit in Circuits)
            {
                // if any item in the circuit contains either direction of the connection, potential connection
                if(circuit.Any(c => c.Item1 == connections[i].Item1 || c.Item2 == connections[i].Item1 || 
                    c.Item1 == connections[i].Item2 || c.Item2 == connections[i].Item2))
                {
                    matchedCircuits.Add(circuit);
                }
            }

            if(matchedCircuits.Count == 0)
            {
                // create a new circuit
                Circuits.Add(new List<(int,int)>() { (connections[i].Item1, connections[i].Item2) });
                Console.WriteLine("  Created new circuit.");
            }
            else if(matchedCircuits.Count == 1)
            {
                if(matchedCircuits[0].Any(c => c.Item1 == connections[i].Item1 || c.Item2 == connections[i].Item1) &&
                   matchedCircuits[0].Any(c => c.Item1 == connections[i].Item2 || c.Item2 == connections[i].Item2))
                {
                    // both items already in the circuit - skip
                    Console.WriteLine("  Both boxes already in the circuit - skipping.");
                    continue;
                }
                // add to existing circuit
                matchedCircuits[0].Add((connections[i].Item1, connections[i].Item2));
                Console.WriteLine("  Added to existing circuit.");

                if(Circuits.Count == 1 && Circuits[0].SelectMany(c => new List<int>() { c.Item1, c.Item2 }).Distinct().Count() == BoxLocations.Count)
                {
                    // all boxes connected
                    Console.WriteLine("All boxes connected!");

                    var box1 = BoxLocations[connections[i].Item1];
                    var box2 = BoxLocations[connections[i].Item2];

                    Console.WriteLine($"Final Connection: Box {box1.Item2} <-> Box {box2.Item2} (Distance: {connections[i].Item3})");

                    long result = (long) box1.Item2.X * (long) box2.Item2.X;
                    Console.WriteLine("Part 2 Result: " + result);
                    break;
                }
            }
            else if(matchedCircuits.Count == 2)
            {
                // confirm one matched circuit contains Item1 and the other contains Item2
                if(matchedCircuits[0].Any(c => c.Item1 == connections[i].Item1 || c.Item2 == connections[i].Item1) &&
                   matchedCircuits[1].Any(c => c.Item1 == connections[i].Item2 || c.Item2 == connections[i].Item2))
                {
                    // ok - merge the 2 circuits and add the connection
                    matchedCircuits[0].AddRange(matchedCircuits[1]);
                    Circuits.Remove(matchedCircuits[1]);
                    matchedCircuits[0].Add((connections[i].Item1, connections[i].Item2));
                    Console.WriteLine("  Merged two circuits.");
                }
                else if(matchedCircuits[1].Any(c => c.Item1 == connections[i].Item1 || c.Item2 == connections[i].Item1) &&
                        matchedCircuits[0].Any(c => c.Item1 == connections[i].Item2 || c.Item2 == connections[i].Item2))
                {
                    // ok - merge the 2 circuits and add the connection
                    matchedCircuits[0].AddRange(matchedCircuits[1]);
                    Circuits.Remove(matchedCircuits[1]);
                    matchedCircuits[0].Add((connections[i].Item1, connections[i].Item2));
                    Console.WriteLine("  Merged two circuits.");
                }
                else
                {
                    throw new Exception("Mismatched circuits!");
                }

                if(Circuits.Count == 1 && Circuits[0].SelectMany(c => new List<int>() { c.Item1, c.Item2 }).Distinct().Count() == BoxLocations.Count)
                {
                    // all boxes connected
                    Console.WriteLine("All boxes connected!");

                    var box1 = BoxLocations[connections[i].Item1];
                    var box2 = BoxLocations[connections[i].Item2];

                    Console.WriteLine($"Final Connection: Box {box1.Item2} <-> Box {box2.Item2} (Distance: {connections[i].Item3})");

                    long result = (long) box1.Item2.X * (long) box2.Item2.X;
                    Console.WriteLine("Part 2 Result: " + result);
                    break;
                }
            }
            else
            {
                throw new Exception("More than 2 matched circuits found!");
            }

            // // print out all the circuits
            // foreach(var circuit in Circuits)
            // {
            //     // count the number of boxes in the circuit
            //     int boxCount = circuit.SelectMany(c => new List<int>() { c.Item1, c.Item2 }).Distinct().Count();

            //     Console.WriteLine("Circuit: " + boxCount + " boxes");
            //     foreach(var conn in circuit)
            //     {
            //         Console.WriteLine($"  Box {conn.Item1} <-> Box {conn.Item2} (Distance: {DistanceCache[(conn.Item1, conn.Item2)]})");
            //     }

            //     part1sum += boxCount;
            // }

            // Console.WriteLine("Press Enter to continue...");
            // Console.ReadLine();
        }

        // // sort circuits by number of boxes
        // Circuits.Sort((a,b) => b.SelectMany(c => new List<int>() { c.Item1, c.Item2 }).Distinct().Count()
        //                             .CompareTo(a.SelectMany(c => new List<int>() { c.Item1, c.Item2 }).Distinct().Count()));

        // // print out all the circuits
        // // Console.WriteLine("");
        // // Console.WriteLine("Final Circuits:");
        // part1sum = 1;
        // for(int i = 0; i < 3; i++)
        // {
        //     // count the number of boxes in the circuit
        //     int boxCount = Circuits[i].SelectMany(c => new List<int>() { c.Item1, c.Item2 }).Distinct().Count();

        //     // Console.WriteLine("Circuit: " + boxCount + " boxes");
        //     // foreach(var conn in circuit)
        //     // {
        //     //     Console.WriteLine($"  Box {conn.Item1} <-> Box {conn.Item2} (Distance: {DistanceCache[(conn.Item1, conn.Item2)]})");
        //     // }

        //     part1sum *= boxCount;
        // }
        
        // Console.WriteLine("Part 2 : " + part2sum); 
    }
}

