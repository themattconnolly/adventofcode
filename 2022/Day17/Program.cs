namespace Day17
{
    internal class Program
    {
        internal static List<char> Jets = new List<char>();
        static void Main(string[] args)
        {
            Console.WriteLine("Day 17!");

            foreach (byte fileByte in File.ReadAllBytes("input.txt"))
            {
                if(fileByte == '>' || fileByte == '<')
                {
                    Jets.Add((char)fileByte);
                }
            }

            Console.WriteLine(Jets.Count + " jets");

            List<Rock> rocks = new List<Rock>();
            rocks.Add(new Rock()
            {
                Name = "flat",
                Height = 1,
                Width = 4
            });
            rocks.Add(new Rock()
            {
                Name = "cross",
                Height = 3,
                Width = 3
            });
            rocks.Add(new Rock()
            {
                Name = "L",
                Height = 3,
                Width = 3
            });
            rocks.Add(new Rock()
            {
                Name = "line",
                Height = 4,
                Width = 1
            });
            rocks.Add(new Rock()
            {
                Name = "box",
                Height = 2,
                Width = 2
            });

            int nextRockIndex = 0;
            int rockX = 2;
            int rockY = ChamberHighestRow;

            // initialize chamber
            InitializeChamber();


            for(int i = 0; i < 2022; i++)
            {
                Rock nextRock = rocks[nextRockIndex];
                rockY = ChamberHighestRow + 4;
                ChamberHighestRow = Math.Max(ChamberHighestRow, DropRock(nextRock, rockX, rockY));

                nextRockIndex = (nextRockIndex + 1) % 5;
            }

            // draw chamber
            for (int i = ChamberHighestRow; i >= 0; i--)
            {
                //Console.WriteLine(string.Join(string.Empty, Chamber[i]));
            }

            Console.WriteLine("Height: " + ChamberHighestRow);
        }

        internal static List<char[]> Chamber = new List<char[]>();
        internal static int ChamberHighestRow = 0;
        internal static int JetIndex = 0;
        internal static int ChamberWidth = 7;

        internal static void InitializeChamber()
        {
            Chamber.Add("-------".ToCharArray());
        }

        internal static int DropRock(Rock rock, int rockX, int rockY)
        {
            bool rockIsDone = false;

            Chamber.Add(".......".ToCharArray());
            Chamber.Add(".......".ToCharArray());
            Chamber.Add(".......".ToCharArray());

            for(int i = 0; i < rock.Height; i++)
            {
                Chamber.Add(".......".ToCharArray());
            }

            while (rockIsDone == false)
            {
                char jet = Jets[JetIndex];
                if(++JetIndex == Jets.Count) { JetIndex = 0; } 
                if (CanRockSlide(rock, jet, rockX, rockY))
                {
                    if(jet == '>') { rockX++; }
                    else if (jet == '<') { rockX--; }
                }

                if(CanRockDrop(rock, rockX, rockY))
                {
                    rockY--;
                }
                else
                {
                    rockIsDone = true;
                }
            }

            // set chamber values
            switch (rock.Name)
            {
                case "flat":
                    Chamber[rockY][rockX] = '#';
                    Chamber[rockY][rockX + 1] = '#';
                    Chamber[rockY][rockX + 2] = '#';
                    Chamber[rockY][rockX + 3] = '#';
                    return rockY;
                case "cross":
                    Chamber[rockY][rockX + 1] = '#';
                    Chamber[rockY + 1][rockX] = '#';
                    Chamber[rockY + 1][rockX + 1] = '#';
                    Chamber[rockY + 1][rockX + 2] = '#';
                    Chamber[rockY + 2][rockX + 1] = '#';
                    return rockY + 2;
                case "L":
                    Chamber[rockY][rockX] = '#';
                    Chamber[rockY][rockX + 1] = '#';
                    Chamber[rockY][rockX + 2] = '#';
                    Chamber[rockY + 1][rockX + 2] = '#';
                    Chamber[rockY + 2][rockX + 2] = '#';
                    return rockY + 2;
                case "line":
                    Chamber[rockY][rockX] = '#';
                    Chamber[rockY + 1][rockX] = '#';
                    Chamber[rockY + 2][rockX] = '#';
                    Chamber[rockY + 3][rockX] = '#';
                    return rockY + 3;
                case "box":
                    Chamber[rockY][rockX] = '#';
                    Chamber[rockY][rockX + 1] = '#';
                    Chamber[rockY + 1][rockX] = '#';
                    Chamber[rockY + 1][rockX + 1] = '#';
                    return rockY + 1;
            }

            throw new Exception("oops");
        }

        internal static bool CanRockDrop(Rock rock, int rockX, int rockY)
        {
            if (rockY == 0) return false;

            switch (rock.Name)
            {
                case "flat":
                    return ((Chamber[rockY - 1][rockX] == '.') &&
                        (Chamber[rockY - 1][rockX + 1] == '.') &&
                        (Chamber[rockY - 1][rockX + 2] == '.') &&
                        (Chamber[rockY - 1][rockX + 3] == '.'));
                case "cross":
                    return ((Chamber[rockY][rockX] == '.') &&
                        (Chamber[rockY - 1][rockX + 1] == '.') &&
                        (Chamber[rockY][rockX + 2] == '.'));
                case "L":
                    return ((Chamber[rockY - 1][rockX] == '.') &&
                        (Chamber[rockY - 1][rockX + 1] == '.') &&
                        (Chamber[rockY - 1][rockX + 2] == '.'));
                case "line":
                    return (Chamber[rockY - 1][rockX] == '.');
                case "box":
                    return ((Chamber[rockY - 1][rockX] == '.') &&
                        (Chamber[rockY - 1][rockX + 1] == '.'));
            }

            throw new Exception("oops");
        }

        internal static bool CanRockSlide(Rock rock, char move, int rockX, int rockY)
        {
            if (move == '>')
            {
                if (rockX + rock.Width == ChamberWidth)
                {
                    return false;
                }
                else
                {
                    // check airspace
                    switch(rock.Name)
                    {
                        case "flat":
                            return (Chamber[rockY][rockX + rock.Width] == '.');
                        case "cross":
                            return ((Chamber[rockY + 1][rockX + rock.Width] == '.') &&
                                (Chamber[rockY][rockX + rock.Width - 1] == '.'));
                        case "L":
                            return ((Chamber[rockY + 2][rockX + rock.Width] == '.') &&
                                (Chamber[rockY + 1][rockX + rock.Width] == '.') &&
                                (Chamber[rockY][rockX + rock.Width] == '.'));
                        case "line":
                            return ((Chamber[rockY + 3][rockX + rock.Width] == '.') && 
                                (Chamber[rockY + 2][rockX + rock.Width] == '.') &&
                                (Chamber[rockY + 1][rockX + rock.Width] == '.') &&
                                (Chamber[rockY][rockX + rock.Width] == '.'));
                        case "box":
                            return ((Chamber[rockY + 1][rockX + rock.Width] == '.') &&
                                (Chamber[rockY][rockX + rock.Width] == '.'));
                    }
                }
            }
            else if (move == '<')
            {
                if (rockX == 0)
                {
                    return false;
                }
                else
                {
                    // check airspace
                    switch (rock.Name)
                    {
                        case "flat":
                            return (Chamber[rockY][rockX - 1] == '.');
                        case "cross":
                            return ((Chamber[rockY + 1][rockX - 1] == '.') &&
                                (Chamber[rockY][rockX] == '.'));
                        case "L":
                            return
                                (Chamber[rockY][rockX - 1] == '.');
                        case "line":
                            return ((Chamber[rockY + 3][rockX - 1] == '.') &&
                                (Chamber[rockY + 2][rockX - 1] == '.') &&
                                (Chamber[rockY + 1][rockX - 1] == '.') &&
                                (Chamber[rockY][rockX - 1] == '.'));
                        case "box":
                            return ((Chamber[rockY + 1][rockX - 1] == '.') &&
                                (Chamber[rockY][rockX - 1] == '.'));
                    }
                }
            }

            throw new Exception("oops");
        }

        internal class Rock
        {
            public string Name;
            public int Height;
            public int Width;
        }
    }
}