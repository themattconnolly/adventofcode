using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    internal class Elf
    {
        internal List<Food> Foods = new List<Food>();

        internal int TotalCalories => this.Foods.Sum(x => x.Calories);
    }
}
