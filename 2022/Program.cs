using System.Diagnostics;

namespace AOC2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set Current Day Number and whether to use the actual data or test data file
            int DayNumber = 16;
            bool TestData = true;

            List<string> InputData = SupportRoutines.LoadDataIntoArray(DayNumber, TestData);
            Stopwatch t = Stopwatch.StartNew();
            switch (DayNumber)
            {
                case 1:
                    Solutions.Day01aSolution(ref InputData);
                    Solutions.Day01bSolution(ref InputData);
                    break;
                case 2:
                    Solutions.Day02aSolution(ref InputData);
                    Solutions.Day02bSolution(ref InputData);
                    break;
                case 3:
                    Solutions.Day03aSolution(ref InputData);
                    Solutions.Day03bSolution(ref InputData);
                    break;
                case 4:
                    Solutions.Day04aSolution(ref InputData);
                    Solutions.Day04bSolution(ref InputData);
                    break;
                case 5:
                    Solutions.Day05aSolution(ref InputData);
                    Solutions.Day05bSolution(ref InputData);
                    break;
                case 6:
                    Solutions.Day06aSolution(ref InputData);
                    Solutions.Day06bSolution(ref InputData);
                    break;
                case 7:
                    Solutions.Day07Solution(ref InputData);
                    Solutions.Day07SolutionNSW(ref InputData);
                    break;
                case 8:
                    Solutions.Day08aSolution(ref InputData);
                    Solutions.Day08bSolution(ref InputData);
                    break;
                case 9:
                    //Solutions.Day09aSolutionBounded(ref InputData);
                    Solutions.Day09aSolution(ref InputData);
                    Solutions.Day09bSolution(ref InputData);
                    break;
                case 12:
                    Solutions.Day12Solution('a', ref InputData);
                    Solutions.Day12Solution('b', ref InputData);
                    break;
                case 13:
                    Solutions.Day13Solution();
                    break;
                case 14:
                    Solutions.Day14Solution('a', ref InputData);
                    Solutions.Day14Solution('b', ref InputData);
                    break;
                case 15:
                    Solutions.Day15Solution(ref InputData);
                    break;
                case 16:
                    Solutions.Day16Solution(ref InputData);
                    break;
                default:
                    Console.WriteLine("Error: There is no solution for this day yet.");
                    break;
            }
            t.Stop();
            Console.WriteLine("=== Completed in " + t.ElapsedMilliseconds.ToString() + "ms ===");
            Console.WriteLine("===============================");

            //Test random stuff down here..
            //Console.WriteLine("\nRandom stuff down here..");
            //var graph = SupportRoutines.LoadFileIntoArray("graph");
            //Solutions.Dijkstra(ref graph);
        }
    }
}