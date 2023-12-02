using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Day7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 7!");

            Regex cdRegex = new Regex(@"\$ cd (.+)");
            Regex lsRegex = new Regex(@"\$ ls");
            Regex dirRegex = new Regex(@"dir (.+)");
            Regex fileRegex = new Regex(@"(\d+) (.+)");

            ElfDirectory rootDirectory = new ElfDirectory() { Name = "/" };
            ElfDirectory currentElfDirectory = rootDirectory;

            foreach (string line in File.ReadLines("input.txt"))
            {
                // recru

                if (cdRegex.IsMatch(line))
                {
                    string relativeDirectory = cdRegex.Match(line).Groups[1].Value;

                    if (relativeDirectory == "..")
                    {
                        currentElfDirectory = currentElfDirectory.ParentElfDirectory;
                    }
                    else
                    {
                        if (relativeDirectory == "/")
                        {
                            currentElfDirectory = rootDirectory;
                        }
                        else
                        {
                            currentElfDirectory = currentElfDirectory.ElfDirectories.First(x => x.Name == relativeDirectory);
                        }
                    }
                }
                else if (lsRegex.IsMatch(line))
                {
                    // do nothing
                }
                else if (dirRegex.IsMatch(line))
                {
                    string relativeDirectory = dirRegex.Match(line).Groups[1].Value;
                    currentElfDirectory.ElfDirectories.Add(new ElfDirectory() { Name = relativeDirectory, ParentElfDirectory = currentElfDirectory });
                }
                else if (fileRegex.IsMatch(line))
                {
                    int fileSize = int.Parse(fileRegex.Match(line).Groups[1].Value);
                    string fileName = fileRegex.Match(line).Groups[2].Value;
                    currentElfDirectory.ElfFiles.Add(new ElfFile() { Name = fileName, Size = fileSize });

                    ElfDirectory temp = currentElfDirectory;
                    while (temp != null)
                    {
                        temp.Size += fileSize;
                        temp = temp.ParentElfDirectory;
                    }
                }
            }

            int totalSizeOf10kDirectorys = CalculatDirectorySize(rootDirectory);

            Console.WriteLine("total sizes over 100000 = " + totalSizeOf10kDirectorys);

            Console.WriteLine("root directory size = " + rootDirectory.Size);

            int availableSpace = 70000000 - rootDirectory.Size;
            int neededSpace = 30000000 - availableSpace;
            List<ElfDirectory> matchingDirectories = FindDirectoriesBigEnough(rootDirectory, neededSpace);
            ElfDirectory matchingDirectory = matchingDirectories.OrderBy(x => x.Size).First();
            Console.WriteLine("smallest Directory over " + neededSpace + " is " + matchingDirectory.Size);
        }

        static internal List<ElfDirectory> FindDirectoriesBigEnough(ElfDirectory elfDirectory, int minSize)
        {
            List<ElfDirectory> retVal = new List<ElfDirectory>();
            if (elfDirectory.Size >= minSize)
            {
                retVal.Add(elfDirectory);
            }

            foreach (ElfDirectory childDirectory in elfDirectory.ElfDirectories)
            {
                retVal.AddRange(FindDirectoriesBigEnough(childDirectory, minSize));
            }

            return retVal;
        }

        static internal int CalculatDirectorySize(ElfDirectory elfDirectory)
        {
            int size = 0;
            if(elfDirectory.Size <= 100000)
            {
                size += elfDirectory.Size;
            }

            foreach (ElfDirectory childDirectory in elfDirectory.ElfDirectories)
            {
                size += CalculatDirectorySize(childDirectory);
            }

            return size;
        }

        internal class ElfDirectory
        {
            internal int Size;
            internal string Name;
            internal List<ElfDirectory> ElfDirectories = new List<ElfDirectory>();
            internal List<ElfFile> ElfFiles = new List<ElfFile>();
            internal ElfDirectory ParentElfDirectory { get; set; }
        }

        internal class ElfFile
        {
            internal string Name;
            internal int Size;
        }
    }
}