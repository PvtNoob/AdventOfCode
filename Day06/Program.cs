using Shared;

namespace Day06 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            int p1_score = 0;
            int p2_score = 0;

            string[] lines = File.ReadAllLines(args[0]);
            int[] times = GetValues(lines[0]);
            int[] distances = GetValues(lines[1]);
            int[] amountOfWinOptions = times.Select(x => 0).ToArray();

            for (int race = 0; race < times.Length; race++) {
                for (int pushTime = 1; pushTime < times[race] - 1; pushTime++) {
                    int distance = pushTime * (times[race] - pushTime);
                    if (distance > distances[race]) {
                        amountOfWinOptions[race]++;
                    }
                }
            }

            p1_score = amountOfWinOptions.Aggregate(1, (a, b) => a * b);

            long p2Time = long.Parse(lines[0].Split(':')[1].Replace(" ", ""));
            long p2Distance = long.Parse(lines[1].Split(':')[1].Replace(" ", ""));

            for (long pushTime = 1; pushTime < p2Time - 1; pushTime++) {
                long distance = pushTime * (p2Time - pushTime);
                if (distance > p2Distance) {
                    p2_score++;
                }
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }

        private static int[] GetValues(string line) {
            return line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }
    }
}