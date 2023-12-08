using Shared;

namespace Day08 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            long p1_score = 0;
            long p2_score = 0;

            //Read coordinates
            string[] lines = File.ReadAllLines(args[0]);
            int[] directions = lines[0].Select(x => x == 'L' ? 0 : 1).ToArray();
            Dictionary<string, string[]> coordinates = [];
            char[] splits = ['=', '(', ',', ')'];
            for (int i = 2; i < lines.Length; i++) {
                string[] parts = lines[i].Split(splits, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                string[] coord = [parts[1], parts[2]];
                coordinates.Add(parts[0], coord);
            }

            //Part1
            string p1CurrentLocation = "AAA";
            for (int i = 0; i < directions.Length; i = i == directions.Length - 1 ? 0 : i + 1) {
                p1_score++;
                p1CurrentLocation = coordinates[p1CurrentLocation][directions[i]];
                if (p1CurrentLocation == "ZZZ") {
                    break;
                }
            }

            //Part2
            string[] p2CurrentLocations = coordinates.Keys.Where(x => x.EndsWith('A')).ToArray();
            long[] loopLengths = new long[p2CurrentLocations.Length];
            Array.Fill(loopLengths, 0);

            int currentDirectionIndex = 0;
            int directionLoop = 0;
            do {
                for (int j = 0; j < p2CurrentLocations.Length; j++) {
                    p2CurrentLocations[j] = coordinates[p2CurrentLocations[j]][directions[currentDirectionIndex]];
                    if (p2CurrentLocations[j].EndsWith('Z') && loopLengths[j] == 0) {
                        loopLengths[j] = directionLoop + 1;
                    }
                }

                if(currentDirectionIndex == directions.Length - 1) {
                    currentDirectionIndex = 0;
                    directionLoop++;
                } else {
                    currentDirectionIndex++;
                }
            } while (loopLengths.Any(x => x == 0));

            p2_score = FindLowestCommonMultiplier(loopLengths) * directions.Length;

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }

        private static long FindLowestCommonMultiplier(long[] numbers) => numbers.Aggregate((a, b) => a * b / FindGreatestCommonDivisor(a, b));

        private static long FindGreatestCommonDivisor(long a, long b) => b == 0 ? a : FindGreatestCommonDivisor(b, a % b);
    }
}
