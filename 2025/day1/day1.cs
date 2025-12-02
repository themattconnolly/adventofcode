namespace _2025;
public class Day1
{
    // initialize dial
    public static int dial = 50;
    private static List<string> instructions = new List<string>();
    private static void ParseFile()
    {
        string filename = "day1/input.txt";
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        while((line = file.ReadLine()) != null)
        {
            instructions.Add(line);
        }
        file.Close();
    }

    private static void TurnDial(string instruction)
    {
        // instruction looks like this: "L13" or "R7"
        char direction = instruction[0];
        int amount = int.Parse(instruction.Substring(1));
        if (direction == 'L')
        {
            dial -= amount;
        }
        else if (direction == 'R')
        {
            dial += amount;
        }

        // wrap around dial, fix if negative
        dial = ((dial % 100) + 100) % 100;
    }

    public static void RunPart1()
    {
        ParseFile();

        int numberOfZeros = 0;
        foreach (string instruction in instructions)
        {
            TurnDial(instruction);
            Console.WriteLine("Dial is now at: " + dial);
            if (dial == 0)
            {
                numberOfZeros++;
            }
        }

        Console.WriteLine("Part 1 : " + numberOfZeros);
    }

    public static void RunPart2()
    {
        ParseFile();
        Console.WriteLine("Part 2 : Not implemented yet");
    }
}

