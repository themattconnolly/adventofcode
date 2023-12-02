namespace Day4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 4! Fully overlapping sections:");

            int elfPairsWithFullOverlap = 0;
            int elfPairsWithAnyOverlap = 0;

            foreach (string line in File.ReadLines("input.txt"))
            {
                string[] ranges = line.Split(',');

                int elf1SectionLow = int.Parse(line.Split(',')[0].Split('-')[0]);
                int elf1SectionHigh = int.Parse(line.Split(',')[0].Split('-')[1]);
                int elf2SectionLow = int.Parse(line.Split(',')[1].Split('-')[0]);
                int elf2SectionHigh = int.Parse(line.Split(',')[1].Split('-')[1]);

                if(elf1SectionLow <= elf2SectionLow && elf1SectionHigh >= elf2SectionHigh)
                {
                    // elft 1 contains elf 2
                    elfPairsWithFullOverlap++;
                    elfPairsWithAnyOverlap++;
                }
                else if(elf1SectionLow >= elf2SectionLow && elf1SectionHigh <= elf2SectionHigh)
                {
                    // elft 2 contains elf 1
                    elfPairsWithFullOverlap++;
                    elfPairsWithAnyOverlap++;
                }
                else if(elf1SectionLow >= elf2SectionLow && elf1SectionLow <= elf2SectionHigh)
                {
                    // elf 1 is within elf 2
                    elfPairsWithAnyOverlap++;
                }
                else if(elf1SectionHigh >= elf2SectionLow && elf1SectionHigh <= elf2SectionHigh)
                {
                    // elf 1 is within elf 2
                    elfPairsWithAnyOverlap++;
                }
            }

            Console.WriteLine(elfPairsWithFullOverlap);
            Console.WriteLine("Elf Pairs with ANY overlap: " + elfPairsWithAnyOverlap);
        }
    }
}