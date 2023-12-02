namespace _2023;
public class Day1
{   
    // function that reads in a file line by line and writes the output to the console
    public static void RunDay1()
    {
        string filename = "day1/input.txt";
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        int part1sum = 0;
        int part2sum = 0;
        // initalize string array with digits spelled out
        string[] digits = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
        while((line = file.ReadLine()) != null)
        {
            int firstLineDigit = 0;
            int lastLineDigit = 0;
            int firstLineDigitIndex = -1;
            int lastLineDigitIndex = int.MaxValue;
            // find the first digit character in the line
            int firstDigitIndex = line.IndexOfAny("0123456789".ToCharArray());
            if(firstDigitIndex != -1)
            {
                // convert the character to an integer
                firstLineDigit = (int) Char.GetNumericValue(line[firstDigitIndex]);
                firstLineDigitIndex = firstDigitIndex;
            }

            // find the last digit character in the line
            int lastDigitIndex = line.LastIndexOfAny("0123456789".ToCharArray());
            if(lastDigitIndex != -1)
            {
                // convert the character to an integer
                lastLineDigit = (int) Char.GetNumericValue(line[lastDigitIndex]);
                lastLineDigitIndex = lastDigitIndex;
            }
            
            // find the first digit name in the line
            for(int digitIndex = 0; digitIndex < digits.Length; digitIndex++)
            {
                int firstDigitNameIndex = -1;
                int lastDigitNameIndex = int.MaxValue;
                int currentDigitNameIndex = line.IndexOf(digits[digitIndex]);
                while(currentDigitNameIndex != -1)
                {
                    if(firstDigitNameIndex == -1)
                    {
                        firstDigitNameIndex = currentDigitNameIndex;
                        lastDigitNameIndex = currentDigitNameIndex;
                        //Console.WriteLine("Found first and last digit name: " + digits[digitIndex] + " at index: " + firstDigitNameIndex);
                    }
                    else
                    {
                        if(currentDigitNameIndex > lastDigitNameIndex)
                        {
                            lastDigitNameIndex = currentDigitNameIndex;
                        }
                        //Console.WriteLine("Found last digit name: " + digits[digitIndex] + " at index: " + lastDigitNameIndex);
                    }

                    currentDigitNameIndex = line.IndexOf(digits[digitIndex], currentDigitNameIndex + 1);
                }
                
                if(firstDigitNameIndex != -1 &&
                (firstLineDigitIndex == -1 || firstDigitNameIndex < firstLineDigitIndex))
                {
                    firstLineDigitIndex = firstDigitNameIndex;
                    firstLineDigit = digitIndex;
                    //Console.WriteLine("Set first digit name: " + digits[digitIndex] + " at index: " + firstDigitNameIndex);
                }
                if(lastDigitNameIndex != int.MaxValue &&
                (lastLineDigitIndex == int.MaxValue || lastDigitNameIndex > lastLineDigitIndex))
                {
                    lastLineDigitIndex = lastDigitNameIndex;
                    lastLineDigit = digitIndex;
                    //Console.WriteLine("Set last digit name: " + digits[digitIndex] + " at index: " + lastDigitNameIndex);
                }
            }
            

            // combine the first and last digit into a new number
            //int newNumber = 10 * firstDigit + lastDigit;
            //Console.WriteLine("Part 1 Parsed number: " + newNumber + " from line: " + line + " with first digit: " + firstDigit + " and last digit: " + lastDigit + " at index: " + firstDigitIndex + " and " + lastDigitIndex);
            //part1sum += newNumber;

            int part2Number = 10 * firstLineDigit + lastLineDigit;
            Console.WriteLine("Part 2 Parsed number: " + part2Number + " from line: " + line);
            part2sum += part2Number;
        }
        file.Close();

        // print the sum
        //Console.WriteLine("Part 1 Sum: " + part1sum);

        // print the sum
        Console.WriteLine("Part 2 Sum: " + part2sum);
    }
}