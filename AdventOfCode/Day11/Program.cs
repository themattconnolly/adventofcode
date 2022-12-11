using System.Text.RegularExpressions;

namespace Day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 11!");

            // initialize
            Monkey currentMonkey = null;
            int step = 0;
            List<Monkey> monkeys = new List<Monkey>();
            foreach (string line in File.ReadLines("input.txt"))
            {
                Match match = Regex.Match(line, @"Monkey (\d+)");
                if(match.Success)
                {
                    int monkeyNumber = int.Parse(match.Groups[1].Value);
                    currentMonkey = new Monkey() { MonkeyNumber = monkeyNumber };
                    monkeys.Add(currentMonkey);
                }

                match = Regex.Match(line, @"Starting items:((?: \d+,?)+)");
                if(match.Success)
                {
                    MatchCollection matches = Regex.Matches(match.Groups[1].Value, @"\d+");
                    foreach(Match itemMatch in matches)
                    {
                        currentMonkey.Items.Add(int.Parse(itemMatch.Value));
                    }
                }

                match = Regex.Match(line, @"Operation: new = old ([\*\+]) (\d+|old)");
                if (match.Success)
                {
                    currentMonkey.WorryOperation = match.Groups[1].Value;
                    if (match.Groups[2].Value == "old")
                    {
                        currentMonkey.WorryValueSelf = true;
                    }
                    else
                    {
                        currentMonkey.WorryValue = int.Parse(match.Groups[2].Value);
                    }
                }

                match = Regex.Match(line, @"Test: divisible by (\d+)");
                if (match.Success)
                {
                    currentMonkey.TestDivisor = int.Parse(match.Groups[1].Value);
                }

                match = Regex.Match(line, @"If true: throw to monkey (\d+)");
                if (match.Success)
                {
                    currentMonkey.TrueMonkey = int.Parse(match.Groups[1].Value);
                }

                match = Regex.Match(line, @"If false: throw to monkey (\d+)");
                if (match.Success)
                {
                    currentMonkey.FalseMonkey = int.Parse(match.Groups[1].Value);
                }
            }

            for(int round = 1; round <= 20; round++)
            {
                foreach(Monkey monkey in monkeys)
                {
                    // every item is thrown
                    foreach(int item in monkey.Items)
                    {
                        int newItem = monkey.ProcessWorry(item);
                        newItem = (int)Math.Floor((double) newItem / 3);
                        if(newItem % monkey.TestDivisor == 0)
                        {
                            monkeys[monkey.TrueMonkey].Items.Add(newItem);
                        }
                        else
                        {
                            monkeys[monkey.FalseMonkey].Items.Add(newItem);
                        }

                        monkey.Inspections++;
                    }
                    monkey.Items.Clear();
                }

                Console.WriteLine("After round " + round + ":");
                foreach (Monkey monkey in monkeys)
                {
                    Console.WriteLine("Monkey " + monkey.MonkeyNumber + ": " + string.Join(", ", monkey.Items));
                }
            }

            List<Monkey> top2Monkeys = monkeys.OrderByDescending(x => x.Inspections).Take(2).ToList();
            Console.WriteLine("Monkey Business: " + top2Monkeys[0].Inspections * top2Monkeys[1].Inspections);
        }

        internal class Monkey
        {
            internal int MonkeyNumber;
            internal List<int> Items = new List<int>();
            internal int WorryValue;
            internal bool WorryValueSelf = false;
            internal string WorryOperation;
            internal int TestDivisor;
            internal int TrueMonkey;
            internal int FalseMonkey;
            internal int Inspections;
            internal int ProcessWorry(int input)
            {
                switch(this.WorryOperation)
                {
                    case "+":
                        if (WorryValueSelf)
                        {
                            return input + input;
                        }
                        else
                        {
                            return input + this.WorryValue;
                        }
                    case "*":
                        if (WorryValueSelf)
                        {
                            return input * input;
                        }
                        else
                        {
                            return input * this.WorryValue;
                        }
                    default:
                        throw new Exception("Unhandled operation: " + this.WorryOperation);
                }
            }
        }
    }
}