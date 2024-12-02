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
                List<int> numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                //Determine IsAscending
                bool isAscending = true;
                short ascendingErrors = 0;
                short descendingErrors = 0;

                for(int i = 1; i < numbers.Count; i++) {
                    int difference = numbers[i] - numbers[i - 1];

                    if(difference < 0) {
                        ascendingErrors++;
                    } else if(difference > 0) {
                        descendingErrors++;
                    }
                }

                isAscending = ascendingErrors < descendingErrors;

                //Check Lines
                if(CheckLine(numbers, isAscending, out int? errorIndex)) {
                    p1_score++;
                    p2_score++;
                } else {
                    List<int> numbersWithoutRight = numbers.Select(x => x).ToList();
                    numbersWithoutRight.RemoveAt(errorIndex.Value);
                    List<int> numbersWithoutLeft = numbers.Select(x => x).ToList();
                    numbersWithoutLeft.RemoveAt(errorIndex.Value - 1);

                    if(CheckLine(numbersWithoutLeft, isAscending, out _) || CheckLine(numbersWithoutRight, isAscending, out _)) {
                        p2_score++;
                    }
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}\nFinished in {stopwatch.Elapsed}");
        }

        private static bool IsSafeAscend(int difference) {
            return difference >= 1 && difference <= 3;
        }

        private static bool IsSafeDescend(int difference) {
            return difference <= -1 && difference >= -3;
        }

        private static bool CheckLine(List<int> numbers, bool isAscending, out int? errorIndex) {
            errorIndex = null;

            for(int i = 1; i < numbers.Count; i++) {
                int difference = numbers[i] - numbers[i - 1];
                bool isSafe = isAscending ? IsSafeAscend(difference) : IsSafeDescend(difference);

                if(!isSafe) {
                    errorIndex = i;
                    return false;
                }
            }

            return true;
        }
    }
}