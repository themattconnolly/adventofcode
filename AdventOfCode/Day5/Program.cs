using System.Linq;
using System.Text.RegularExpressions;

namespace Day5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 5!");

            //int elfPairsWithFullOverlap = 0;
            //int elfPairsWithAnyOverlap = 0;

            List<Stack<string>> stacks = new List<Stack<string>>();
            List<List<string>> lists = new List<List<string>>();

            for (int i = 0; i < 9; i++)
            {
                stacks.Add(new Stack<string>());
                lists.Add(new List<string>());
            }

            int lineNumber = 1;
            bool initializingStacks = true;
            Regex stackRegex = new Regex(@"^.(.).\s.(.).\s.(.).\s.(.).\s.(.).\s.(.).\s.(.).\s.(.).\s.(.).$");
            Regex moveRegex = new Regex(@"^move (\d+) from (\d+) to (\d+)$");
            foreach (string line in File.ReadLines("input.txt"))
            {
                if (lineNumber <= 8)
                {
                    // initialize stacks

                    //var match = regex.Match(line);
                    //match.Captures
                    GroupCollection groupCollection = stackRegex.Match(line).Groups;
                    for (int i = 1; i < groupCollection.Count; i++)
                    {
                        Group group = groupCollection[i];
                        if (Regex.IsMatch(group.Value, @"\w"))
                        {
                            stacks[i-1].Push(group.Value);
                            lists[i-1].Add(group.Value);
                        }

                        //if (Regex.IsMatch(capture.Value, "/w"))
                        //{
                        //    stacks[capture.Index].Append(capture.Value);
                        //}
                    }
                }
                else if(lineNumber == 9)
                {
                    for (int i = 0; i < stacks.Count; i++)
                    {
                        stacks[i] = new Stack<string>(stacks[i]);
                    }
                }
                else if(lineNumber >= 11)
                {
                    GroupCollection groupCollection = moveRegex.Match(line).Groups;
                    int numToMove = int.Parse(groupCollection[1].Value);
                    int stackToMoveFrom = int.Parse(groupCollection[2].Value);
                    int stackToMoveTo = int.Parse(groupCollection[3].Value);

                    for(int i = 0; i < numToMove; i++)
                    {
                        stacks[stackToMoveTo - 1].Push(stacks[stackToMoveFrom - 1].Pop());

                        //if (stacks[stackToMoveFrom - 1].Count > 0)
                        //{
                        //    stacks[stackToMoveTo - 1].Push(stacks[stackToMoveFrom - 1].Pop());
                        //}
                    }

                    lists[stackToMoveTo - 1].InsertRange(0, (lists[stackToMoveFrom - 1].GetRange(0, numToMove)));
                    lists[stackToMoveFrom - 1].RemoveRange(0, numToMove);
                }

                lineNumber++;
            }

            int stackIndex = 0;
            

            stackIndex = 0;
            foreach (var stack in stacks)
            {
                string stackString = string.Join(",", stack.ToArray());
                Console.WriteLine("Stack " + stackIndex + " " + stackString);

                stackIndex++;
            }

            Console.WriteLine("Part 2:");

            stackIndex = 0;
            foreach (var stack in lists)
            {
                string stackString = string.Join(",", stack.ToArray());
                Console.WriteLine("Stack " + stackIndex + " " + stackString);

                stackIndex++;
            }
        }
    }
}