// See https://aka.ms/new-console-template for more information
using Day1;

List<Elf> elves = new List<Elf>();
bool newElf = true;
Elf currentElf = new Elf();

string filename = "";
if(args.Length < 1)
{
    Console.WriteLine("Please enter a filename:");
    filename = Console.ReadLine();
}
else
{
    filename = args[0];
}

foreach (string line in File.ReadLines(filename))
{
    if(string.IsNullOrEmpty(line))
    {
        newElf = true;
        currentElf = new Elf();
        continue;
    }
    else if (newElf)
    {
        elves.Add(currentElf);
        newElf = false;
    }

    Food newFood = new Food() { Calories = int.Parse(line) };
    currentElf.Foods.Add(newFood);
}

List<Elf> top3Elves = elves.OrderByDescending(x => x.TotalCalories).Take(3).ToList();

//Console.WriteLine("Elf carrying the most Calories: " + maxElfCalories);
Console.WriteLine("Top 3 Elves carrying the most Calories: ");
top3Elves.ForEach(x => Console.WriteLine(x.TotalCalories));
Console.WriteLine("Total: " + top3Elves.Sum(x => x.TotalCalories));