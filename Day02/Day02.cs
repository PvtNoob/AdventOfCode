using System.Diagnostics;
using Shared;

namespace Day02 {
    internal class Day02 {
        static void Main(string[] args) {
            if(!ArgsValidator.IsValidArgs(args)) return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int p1_score = 0;
            int p2_score = 0;

            foreach(string line in File.ReadLines(args[0])) {
                int[] numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                bool isSafeLine = true;
                bool ascending = numbers[0] < numbers[1];
                int lastNumber = numbers[0];

                for(int i = 1; i < numbers.Length; i++) {
                    int difference = numbers[i] - numbers[i - 1];
                    if(ascending) {
                        isSafeLine &= difference >= 1 && difference <= 3;
                    } else {
                        isSafeLine &= difference <= -1 && difference >= -3;
                    }
                }

                if(isSafeLine) {
                    p1_score++;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}\nFinished in {stopwatch.Elapsed}");
        }
    }
}