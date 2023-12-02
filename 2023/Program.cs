// See https://aka.ms/new-console-ddtemplate for more information
// cycle through the args
if(args.Length > 0)
{
    if(args[0].StartsWith("day"))
    {
        Console.WriteLine("Day 1");
        _2023.Day1.RunDay1();
    }
} else
{
    Console.WriteLine("No args");
}