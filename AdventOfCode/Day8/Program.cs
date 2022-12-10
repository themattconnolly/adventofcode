namespace Day8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 8!");

            List<List<Tree>> forest = new List<List<Tree>>();

            foreach (string line in File.ReadLines("input.txt"))
            {
                List<Tree> trees = new List<Tree>();
                foreach(char c in line)
                {
                    trees.Add(new Tree()
                    {
                        Height = (int)Char.GetNumericValue(c)
                    });
                }
                forest.Add(trees);
            }

            int width = forest[0].Count;
            int height = forest.Count;
            int visibleTrees = 0;
            int maxScenicScore = 0;
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    int viewUp = 0, viewDown = 0, viewLeft = 0, viewRight = 0;
                    for (int x = i - 1; x >= 0; x--)
                    {
                        if (x == 0 || forest[x][j].Height >= forest[i][j].Height)
                        {
                            viewUp = i - x;
                            break;
                        }
                    }

                    for (int x = i + 1; x < width; x++)
                    {
                        if (x == width - 1 || forest[x][j].Height >= forest[i][j].Height)
                        {
                            viewDown = x - i;
                            break;
                        }
                    }

                    for (int y = j - 1; y >= 0; y--)
                    {
                        if (y == 0 || forest[i][y].Height >= forest[i][j].Height)
                        {
                            viewLeft = j - y;
                            break;
                        }
                    }

                    for (int y = j + 1; y < height; y++)
                    {
                        if (y == height - 1 || forest[i][y].Height >= forest[i][j].Height)
                        {
                            viewRight = y - j;
                            break;
                        }
                    }

                    int scenicScore = viewUp * viewRight * viewDown * viewLeft;
                    if (scenicScore > maxScenicScore)
                    {
                        maxScenicScore = scenicScore;
                    }
                    forest[i][j].ScenicScore = scenicScore;

                    if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                    {
                        forest[i][j].Visible = true;
                        visibleTrees++;
                    }
                    else
                    {
                        bool isVisible = true;
                        for(int x = i - 1; x >= 0; x--)
                        {
                            if (forest[x][j].Height >= forest[i][j].Height)
                            {
                                // not visible from the left
                                isVisible = false;
                                break;
                            }
                            else
                            {
                                // keep checking
                            }
                        }

                        if(isVisible)
                        {
                            // did not find a taller tree in this direction so definitely visible
                            forest[i][j].Visible = true;
                            visibleTrees++;
                            continue;
                        }

                        isVisible = true;
                        for (int x = i + 1; x < width; x++)
                        {
                            if (forest[x][j].Height >= forest[i][j].Height)
                            {
                                // not visible from the right
                                isVisible = false;
                                break;
                            }
                            else
                            {
                                // keep checking
                            }
                        }

                        if (isVisible)
                        {
                            // did not find a taller tree in this direction so definitely visible
                            forest[i][j].Visible = true;
                            visibleTrees++;
                            continue;
                        }

                        isVisible = true;
                        for (int y = j - 1; y >= 0; y--)
                        {
                            if (forest[i][y].Height >= forest[i][j].Height)
                            {
                                // not visible from top
                                isVisible = false;
                                break;
                            }
                            else
                            {
                                // keep checking
                            }
                        }

                        if (isVisible)
                        {
                            // did not find a taller tree in this direction so definitely visible
                            forest[i][j].Visible = true;
                            visibleTrees++;
                            continue;
                        }

                        isVisible = true;
                        for (int y = j + 1; y < height; y++)
                        {
                            if (forest[i][y].Height >= forest[i][j].Height)
                            {
                                // not visible from bottom
                                isVisible = false;
                                break;
                            }
                            else
                            {
                                // keep checking
                            }
                        }

                        if (isVisible)
                        {
                            // did not find a taller tree in this direction so definitely visible
                            forest[i][j].Visible = true;
                            visibleTrees++;
                            continue;
                        }
                    }
                }
            }

            Console.WriteLine("Visible trees: " + visibleTrees);
            Console.WriteLine("Max Scenic Score: " + maxScenicScore);
        }

        internal class Tree
        {
            internal int Height;
            internal bool Visible = false;

            public int ScenicScore { get; internal set; }
        }
    }
}