namespace _2024;
public class Day1
{
    private static List<int> firstNumbers = new List<int>();
    private static List<int> secondNumbers = new List<int>();
    
    private static void ParseFile()
    {
        string filename = "day1/input.txt";
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        while((line = file.ReadLine()) != null)
        {
            // parse a line that looks like this int two integers: 39687   54930
            // split the line into two strings
            string[] numbers = line.Split("   ");
            // convert the strings to integers
            int firstNumber = int.Parse(numbers[0]);
            int secondNumber = int.Parse(numbers[1]);
            firstNumbers.Add(firstNumber);
            secondNumbers.Add(secondNumber);

        }
        file.Close();
    }

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;

        firstNumbers.Sort();
        secondNumbers.Sort();

        for(int i = 0; i < firstNumbers.Count; i++)
        {
            part1sum += Math.Abs(firstNumbers[i] - secondNumbers[i]);
        }

        Console.WriteLine("Part 1 : " + part1sum);
        // 376008 is right!
    }


    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        for (int i = 0; i < firstNumbers.Count; i++)
        {
            int matches = secondNumbers.Where(x => x == firstNumbers[i]).Count();
            part2sum += firstNumbers[i] * matches;
        }

        Console.WriteLine("Part 2 : " + part2sum);
    }

}