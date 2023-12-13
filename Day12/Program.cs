using System.Diagnostics;
using Shared;

namespace Day12 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();

            long p1_score = 0;
            long p2_score = 0;

            foreach(string line in File.ReadLines(args[0])) {
                int possibilities = 0;



                p1_score += possibilities;
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
            Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
        }
    }
}
