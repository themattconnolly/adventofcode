using System.Runtime.Serialization;

namespace _2024;
public class Day2
{
    static List<List<int>> reports = new List<List<int>>();

    private static void ParseFile()
    {
        string filename = @"C:\projects\adventofcode\2024\day2\input.txt";
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        while ((line = file.ReadLine()) != null)
        {
            // split the line into two strings
            string[] numbers = line.Split(" ");

            List<int> report = new List<int>();
            // parse all the integers in the line
            foreach (string number in numbers)
            {
                report.Add(int.Parse(number));
            }

            reports.Add(report);
        }
        file.Close();
    }

    public static void RunPart1()
    {
        ParseFile();

        long part1sum = 0;

        // iterate through the list of reports
        foreach (List<int> report in reports)
        {
            bool isSafe = true;

            // confirm that every number is either bigger than the previous number or every number is smaller than the previous number
            // and that the difference is at least one and no more than 3
            bool increasing = report[1] > report[0];
            for (int i = 1; i < report.Count; i++)
            {
                int delta = report[i] - report[i - 1];
                if (increasing && (delta < 1 || delta > 3))
                {
                    isSafe = false;
                    break;
                }
                if (increasing == false && (delta > -1 || delta < -3))
                {
                    isSafe = false;
                    break;
                }
            }
            
            if(isSafe)
            {
                part1sum++;
            }
        }

        Console.WriteLine("Part 1 : " + part1sum);
        // 376008 is right!
    }

    private static bool IsReportValid(List<int> report)
    {
        bool isSafe = true;
        bool increasing = report[1] > report[0];
        for (int i = 1; i < report.Count; i++)
        {
            int delta = report[i] - report[i - 1];
            if (increasing && (delta < 1 || delta > 3))
            {
                isSafe = false;
                break;
            }
            if (increasing == false && (delta > -1 || delta < -3))
            {
                isSafe = false;
                break;
            }
        }

        return isSafe;
    }


    public static void RunPart2()
    {
        ParseFile();

        long part2sum = 0;

        int unsavableReports = 0;

        // iterate through the list of reports
        foreach (List<int> report in reports)
        {
            List<int> errorIndexes = new List<int>();

            // confirm that every number is either bigger than the previous number or every number is smaller than the previous number
            // and that the difference is at least one and no more than 3
            bool increasing = report[1] > report[0];
            for (int i = 1; i < report.Count; i++)
            {
                int delta = report[i] - report[i - 1];
                if (increasing && (delta < 1 || delta > 3))
                {
                    errorIndexes.Add(i);
                }
                if (increasing == false && (delta > -1 || delta < -3))
                {
                    errorIndexes.Add(i);
                }
            }

            if (errorIndexes.Count == 0)
            {
                part2sum++;
            }
            else
            {
                // try removing each item and trying again
                for(int i = 0;i < report.Count; i++)
                {
                    List<int> revisedReport = report.ToList();
                    revisedReport.RemoveAt(i);

                    if(IsReportValid(revisedReport))
                    {
                        part2sum++;
                        break;
                    }
                }
            }
            //else if(errorIndexes.Count == 1)
            //{
            //    // try removing the previous number OR the current number and see if the report is valid
            //    List<int> revisedReport = report.ToList();
            //    revisedReport.RemoveAt(errorIndexes[0]-1);

            //    if(IsReportValid(revisedReport))
            //    {
            //        part2sum++;
            //    }
            //    else
            //    {
            //        revisedReport = report.ToList();
            //        revisedReport.RemoveAt(errorIndexes[0]);

            //        if (IsReportValid(revisedReport))
            //        {
            //            part2sum++;
            //        }
            //        else
            //        {
            //            unsavableReports++;
            //        }
            //    }
            //}
            //else if(errorIndexes.Count == 2 && errorIndexes[1] - errorIndexes[0] == 1)
            //{
            //    List<int> revisedReport = report.ToList();
            //    revisedReport.RemoveAt(errorIndexes[0]);

            //    if (IsReportValid(revisedReport))
            //    {
            //        part2sum++;
            //    }
            //    else
            //    {
            //        unsavableReports++;
            //    }
            //}
            //else
            //{
            //    // maybe the first direction was wrong, remove it and try for no errors
            //    List<int> revisedReport = report.ToList();
            //    revisedReport.RemoveAt(0);
            //    if (IsReportValid(revisedReport))
            //    {
            //        part2sum++;
            //    }
            //    else
            //    {
            //        unsavableReports++;
            //    }
            //}
        }

        Console.WriteLine("Part 2 : " + part2sum); // 712,713,715 are too low
    }
}