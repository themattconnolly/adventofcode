using System.Collections;

namespace Day6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 6!");

            List<byte> queue = new List<byte>();
            int index = 0;
            foreach (byte b in File.ReadAllBytes("input.txt"))
            {
                index++;
                queue.Add(b);
                if(queue.Count > 14)
                {
                    queue.RemoveAt(0);
                }
                else
                {
                    continue;
                }

                bool uniqueSet = false;
                for(int i = 0; i < queue.Count; i++)
                {
                    if (queue.Skip(i + 1).Contains(queue[i]))
                    {
                        // not a duplicate
                        uniqueSet = false;
                        break;
                    }
                    else
                    {
                        uniqueSet = true;
                    }
                }

                if(uniqueSet)
                {
                    Console.WriteLine("Found a signature with character " + index);
                    return;
                }
            }
        }
    }
}