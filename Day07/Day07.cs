using System.Diagnostics;
using Shared;

namespace Day07 {
    internal class Day07 {
        static void Main(string[] args) {
            if(!ArgsValidator.IsValidArgs(args)) return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int p1_score = 0;
            int p2_score = 0;

            //Do stuff

            stopwatch.Stop();
            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}\nFinished in {stopwatch.Elapsed}");
        }
    }
}
