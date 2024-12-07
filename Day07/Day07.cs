using System.Diagnostics;
using Shared;

namespace Day07 {
    internal class Day07 {
        static void Main(string[] args) {
            if(!ArgsValidator.IsValidArgs(args)) return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            long p1_score = 0;
            long p2_score = 0;

            foreach(string line in File.ReadLines(args[0])) {
                string[] split = line.Split(':');
                long result = long.Parse(split[0]);
                long[] numbers = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

                if(IsPossibleP1(numbers, 0, 0, result)) {
                    p1_score += result;
                }

                if(IsPossibleP2(numbers, 0, 0, result)) {
                    p2_score += result;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}\nFinished in {stopwatch.Elapsed}");
        }

        private static bool IsPossibleP1(long[] numbers, int index, long currentResult, long finalResult) {
            long plusResult = currentResult + numbers[index];
            long multiplyResult = currentResult * numbers[index];

            index++;
            if(index == numbers.Length) {
                return plusResult == finalResult || multiplyResult == finalResult;
            } else {
                return IsPossibleP1(numbers, index, plusResult, finalResult) || IsPossibleP1(numbers, index, multiplyResult, finalResult);
            }
        }

        private static bool IsPossibleP2(long[] numbers, int index, long currentResult, long finalResult) {
            long plusResult = currentResult + numbers[index];
            long multiplyResult = currentResult * numbers[index];
            long concatResult = index == 0 ? numbers[index] : long.Parse($"{currentResult}{numbers[index]}");

            index++;
            if(index == numbers.Length) {
                return plusResult == finalResult || multiplyResult == finalResult || concatResult == finalResult;
            } else {
                return IsPossibleP2(numbers, index, plusResult, finalResult) || IsPossibleP2(numbers, index, multiplyResult, finalResult) || IsPossibleP2(numbers, index, concatResult, finalResult);
            }
        }
    }
}
