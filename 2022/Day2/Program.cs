namespace Day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 2! Expected score:");

            int totalScorePart1 = 0;
            int totalScorePart2 = 0;
            string filename = "input.txt";
            foreach (string line in File.ReadLines(filename))
            {
                var opponentMove = line[0];
                var suggestedMove = line[2];

                // opponentMove:  A for Rock, B for Paper, and C for Scissors
                // suggestedMove: X for Rock, Y for Paper, and Z for Scissors (part 1)

                // 1 for Rock, 2 for Paper, and 3 for Scissors
                // 0 if you lost, 3 if the round was a draw, and 6 if you won
                switch (suggestedMove)
                {
                    case 'X': // rock
                        totalScorePart1 += 1;
                        switch (opponentMove)
                        {
                            case 'A': // rock
                                totalScorePart1 += 3;
                                break;
                            case 'B': // paper
                                totalScorePart1 += 0;
                                break;
                            case 'C': // scissors
                                totalScorePart1 += 6;
                                break;
                        }
                        break;
                    case 'Y': // paper
                        totalScorePart1 += 2;
                        switch (opponentMove)
                        {
                            case 'A': // rock
                                totalScorePart1 += 6;
                                break;
                            case 'B': // paper
                                totalScorePart1 += 3;
                                break;
                            case 'C': // scissors
                                totalScorePart1 += 0;
                                break;
                        }
                        break;
                    case 'Z': // scissors
                        totalScorePart1 += 3;
                        switch (opponentMove)
                        {
                            case 'A': // rock
                                totalScorePart1 += 0;
                                break;
                            case 'B': // paper
                                totalScorePart1 += 6;
                                break;
                            case 'C': // scissors
                                totalScorePart1 += 3;
                                break;
                        }
                        break;
                }

                // 1 for Rock, 2 for Paper, and 3 for Scissors
                // 0 if you lost, 3 if the round was a draw, and 6 if you won
                // suggestedMove: X means you need to lose, Y means you need to end the round in a draw, and Z means you need to win. (part 1)
                switch (suggestedMove)
                {
                    case 'X': // lose
                        totalScorePart2 += 0;
                        switch (opponentMove)
                        {
                            case 'A': // rock
                                totalScorePart2 += 3; // lose with scissors
                                break;
                            case 'B': // paper
                                totalScorePart2 += 1; // lose with rock
                                break;
                            case 'C': // scissors
                                totalScorePart2 += 2; // lose with paper
                                break;
                        }
                        break;
                    case 'Y': // draw
                        totalScorePart2 += 3;
                        switch (opponentMove)
                        {
                            case 'A': // rock
                                totalScorePart2 += 1; // draw with rock
                                break;
                            case 'B': // paper
                                totalScorePart2 += 2; // draw with paper
                                break;
                            case 'C': // scissors
                                totalScorePart2 += 3; // draw with scissors
                                break;
                        }
                        break;
                    case 'Z': // win
                        totalScorePart2 += 6;
                        switch (opponentMove)
                        {
                            case 'A': // rock
                                totalScorePart2 += 2; // win with paper
                                break;
                            case 'B': // paper
                                totalScorePart2 += 3; // win with scissors
                                break;
                            case 'C': // scissors
                                totalScorePart2 += 1; // win with rock
                                break;
                        }
                        break;
                }
            }

            //Console.WriteLine(totalScorePart1);
            Console.WriteLine(totalScorePart2);
        }
    }
}