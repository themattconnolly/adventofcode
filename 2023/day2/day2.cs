namespace _2023;
public class Day2
{   
    // function that reads in a file line by line and writes the output to the console
    public static void Run()
    {
        string filename = "day2/input.txt";
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        
        int part1sum = 0;
        long part2sum = 0;
        string[] cubeColors = {"red", "green", "blue"};
        int[] maxCubeCounts = {12, 13, 14};

        while((line = file.ReadLine()) != null)
        {
            // parse out the game number from this line: Game 26: 7 red, 1 blue; 2 red, 1 blue; 9 red, 1 green, 2 blue; 5 red, 2 blue; 4 red, 2 green; 8 red, 1 green, 2 blue
            int gameNumber = int.Parse(line.Substring(5, line.IndexOf(":") - 5));

            // split the rest of the line on the semicolon
            string[] cubeGroups = line.Substring(line.IndexOf(":") + 2).Split(";");

            // iterate through the cube groups
            int[] maxCubesInGame = {0, 0, 0};
            bool gamePossible = true;
            for(int cubeGroupIndex = 0; cubeGroupIndex < cubeGroups.Length; cubeGroupIndex++)
            {
                // split the cube group on the comma
                string[] cubes = cubeGroups[cubeGroupIndex].Split(",");
                for(int cubeIndex = 0; cubeIndex < cubes.Length; cubeIndex++)
                {
                    // split the cube on the space
                    string[] cube = cubes[cubeIndex].Trim().Split(" ");
                    // get the color
                    string color = cube[1];
                    // get the count
                    int count = int.Parse(cube[0]);

                    // get the index of the color
                    int colorIndex = Array.IndexOf(cubeColors, color);
                    // check if the count is greater than the max
                    int maxCubeCount = maxCubeCounts[colorIndex];
                    if(count > maxCubeCount)
                    {
                        //Console.WriteLine("Game " + gameNumber + " has " + count + " " + color + " color cubes in group " + cubeGroupIndex + ", max of " + maxCubeCount + "!");
                        gamePossible = false;
                        //break;
                    }

                    if(maxCubesInGame[colorIndex] < count)
                    {
                        maxCubesInGame[colorIndex] = count;
                    }
                }

                if(gamePossible == false)
                {
                    //break;
                }
            }

            // game is possible, add the game number
            if(gamePossible == true)
            {
                //Console.WriteLine("Game " + gameNumber + " is possible!");
                part1sum += gameNumber;
            }

            long product = maxCubesInGame[0] * maxCubesInGame[1] * maxCubesInGame[2];
            part2sum += product;
        }
        file.Close();

        // print the sum
        Console.WriteLine("Part 1 Sum: " + part1sum);
        // 2687 is too low
        // 2879 is right

        // print the sum
        Console.WriteLine("Part 2 Sum: " + part2sum);
        // 65122 is right
    }
}