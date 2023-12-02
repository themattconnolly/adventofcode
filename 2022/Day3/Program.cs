namespace Day3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 3! Total priority:");

            int totalPriorityPart1 = 0;
            int totalPriorityPart2 = 0;

            string theAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int groupIndex = 0;
            string firstLine = null;
            string secondLine = null;
            foreach (string line in File.ReadLines("input.txt"))
            {
                if (line.Length % 2 == 1)
                    throw new Exception("Not even!");

                string firstContainer = line.Substring(0, line.Length / 2);
                string secondContaner = line.Substring(line.Length / 2);

                if (firstContainer.Length + secondContaner.Length != line.Length)
                    throw new Exception("ya done goofed");

                string examinedCharacters = string.Empty;

                foreach(char c in firstContainer)
                {
                    if(examinedCharacters.Contains(c) == false && secondContaner.Contains(c))
                    {
                        // duplicate character
                        int priority = theAlphabet.IndexOf(c) + 1;
                        totalPriorityPart1 += priority;
                        examinedCharacters += c;
                    }
                }

                examinedCharacters = string.Empty;

                if (groupIndex == 0)
                {
                    firstLine = line;
                    groupIndex++;
                }
                else if(groupIndex == 1)
                {
                    secondLine = line;
                    groupIndex++;
                }
                else
                {
                    foreach (char c in line)
                    {
                        if (examinedCharacters.Contains(c) == false &&
                            firstLine.Contains(c) &&
                            secondLine.Contains(c))
                        {
                            // the badge
                            int priority = theAlphabet.IndexOf(c) + 1;
                            totalPriorityPart2 += priority;
                            examinedCharacters += c;
                        }
                    }
                    groupIndex = 0;
                }
            }

            //Console.WriteLine(totalPriorityPart1);
            Console.WriteLine(totalPriorityPart2);
        }
    }
}