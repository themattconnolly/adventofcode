namespace _2025;
public class Day1
{
    // initialize dial
    public static int dial = 50;
    private static List<string> instructions = new List<string>();
    private static void ParseFile()
    {
        string filename = System.IO.Path.Combine(AppContext.BaseDirectory, "day1/input.txt");
        string? line;
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

    private static int TurnDial2(string instruction)
    {
        int zerosHit = 0;
        // instruction looks like this: "L13" or "R7"
        char direction = instruction[0];
        int amount = int.Parse(instruction.Substring(1));
        int previousDial = dial;
        if (direction == 'L')
        {
            dial -= amount;
        }
        else if (direction == 'R')
        {
            dial += amount;
        }

        if(dial < 0 && previousDial > 0)
        {
            zerosHit = 1; // crossed zero
        }
        else if(dial == 0)
        {
            zerosHit = 1; // landed on zero exactly
        }

        int tempDial = dial;
        if(dial < 0)
        {
            tempDial = -1 * dial;
        }
        
        
        while(tempDial >= 100)
        {
            zerosHit++;
            tempDial -= 100;
        }
        
        if(dial < 0 && tempDial > 0)
        {
            dial = 100 - tempDial;
        }
        else
        {
            dial = tempDial;
        }

        return zerosHit;
    }

    public static void RunPart2()
    {
        ParseFile();

        int numberOfZeros = 0;
        foreach (string instruction in instructions)
        {
            int zeros = TurnDial2(instruction);
            numberOfZeros += zeros;
            Console.WriteLine("Dial is rotated by " + instruction + " and now at: " + dial + " Zeros hit: " + zeros);
        }


        Console.WriteLine("Part 2 : " + numberOfZeros); //6888 is too high, 6434 is too low
    }
}

