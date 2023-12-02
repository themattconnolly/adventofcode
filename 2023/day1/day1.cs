namespace _2023;
public class Day1
{   
    // function that reads in a file line by line and writes the output to the console
    public static void RunDay1()
    {
        string filename = "day1/input.txt";
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        int sum = 0;
        while((line = file.ReadLine()) != null)
        {
            // find the first digit character in the line
            int firstDigitIndex = line.IndexOfAny("0123456789".ToCharArray());
            // convert the character to an integer
            int firstDigit = (int) Char.GetNumericValue(line[firstDigitIndex]);

            // find the last digit character in the line
            int lastDigitIndex = line.LastIndexOfAny("0123456789".ToCharArray());
            int lastDigit = (int) Char.GetNumericValue(line[lastDigitIndex]);

            // combine the first and last digit into a new number
            int newNumber = 10 * firstDigit + lastDigit;
            Console.WriteLine("Parsed number: " + newNumber + " from line: " + line + " with first digit: " + firstDigit + " and last digit: " + lastDigit + " at index: " + firstDigitIndex + " and " + lastDigitIndex);
            sum += newNumber;
        }
        file.Close();

        // print the sum
        Console.WriteLine("Sum: " + sum);
    }
}