using System.Runtime.Serialization.Formatters;
using System.Text.RegularExpressions;

namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 13!");

            int orderedPairIndexTotal = 0;
            int pairIndex = 0;
            string[] lines = File.ReadAllLines("input.txt");
            for(int lineNumber = 0; lineNumber < lines.Length; lineNumber+=3)
            {
                pairIndex++;
                // parse left
                Packet leftPacket = new Packet(lines[lineNumber]);
                // parse right
                Packet rightPacket = new Packet(lines[lineNumber + 1]);

                // first pair
                if (leftPacket.CompareTo(rightPacket) > 0)
                {
                    orderedPairIndexTotal += pairIndex;
                }
                else
                {
                    // not ordered
                }
            }

            Console.WriteLine("Ordered pair index total: " + orderedPairIndexTotal);
        }


        internal class Packet : IComparable<Packet>
        {
            internal Packet(string value)
            {
                Match match = Regex.Match(value, @"\[(.*)\]");
                if (match.Success)
                {
                    foreach(char c in match.Groups[1].Value)
                    {

                    }
                    //Match sublistMatch = Regex.Match(, @"\[(.*)\]");
                    //foreach (string subvalue in match.Groups[1].Value.Split(','))
                    //{
                    //    Packets.Add(new Packet(subvalue));
                    //}
                }

                this.Value = value;
            }

            internal string Value;

            internal List<Packet> Packets = new List<Packet>();

            public int CompareTo(Packet? other)
            {
                if(Packets.Count > other.Packets.Count)
                {
                    return -1;
                }

                for (int i = 0; i < Packets.Count; i++)
                {
                    if(int.TryParse(Packets[i].Value, out int intA))
                    {
                        if(int.TryParse(other.Packets[i].Value, out int intB))
                        {
                            if(intA > intB)
                            {
                                // not valid
                                return -1;
                            }
                        }
                        else
                        {
                            // pretend A is a list
                        }
                    }
                }
                return 1;
            }

            internal void ParseValue()
            {

            }


        }
    }
}