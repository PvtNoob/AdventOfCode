using System.Diagnostics;
using Shared;

namespace Day06 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            string[] lines = File.ReadAllLines(args[0]);

            List<long> times = GetValues(lines[0]);
            List<long> distances = GetValues(lines[1]);
            long[] amountOfWinOptionsList = times.Select(x => 0L).ToArray();

            for (int race = 0; race < times.Count; race++) {
                long time = times[race];
                long distance = distances[race];
                long amountOfWinOptions = 0;

                for (int pushTime = 1; pushTime < time; pushTime++) {
                    long runDistance = pushTime * (time - pushTime);
                    if (runDistance > distance) {
                        amountOfWinOptions++;
                    } else if(amountOfWinOptions > 0) {
                        amountOfWinOptionsList[race] = amountOfWinOptions;
                        break;
                    }
                }
            }

            long p1_score = amountOfWinOptionsList.SkipLast(1).Aggregate(1, (long a, long b) => a * b);
            long p2_score = amountOfWinOptionsList.Last();

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }

        private static List<long> GetValues(string line) {
            string numbers = line.Split(':')[1];
            List<long> values = numbers.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
            values.Add(long.Parse(numbers.Replace(" ", "")));
            return values;
        }
    }
}