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

            string[] currentLocations = coordinates.Keys.Where(x => x.EndsWith('A')).ToArray();
            long[] p2AmountOfSteps = new long[currentLocations.Length];
            Array.Fill(p2AmountOfSteps, 0);

            long totalStepCounter = 1;
            for (int directionsIndex = 0; directionsIndex < directions.Length; directionsIndex = directionsIndex == directions.Length - 1 ? 0 : directionsIndex + 1) {
                for (int i = 0; i < currentLocations.Length; i++) {
                    currentLocations[i] = coordinates[currentLocations[i]][directions[directionsIndex]];
                    if (currentLocations[i].EndsWith('Z') && p2AmountOfSteps[i] == 0) {
                        if (currentLocations[i] == "ZZZ") {
                            p1_score = totalStepCounter;
                        }
                        p2AmountOfSteps[i] = totalStepCounter;
                    }
                }

                totalStepCounter++;

                if (p2AmountOfSteps.All(x => x > 0)) {
                    break;
                }
            }

            p2_score = FindLowestCommonMultiplier(p2AmountOfSteps);

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }

        private static long FindLowestCommonMultiplier(long[] numbers) => numbers.Aggregate((a, b) => a * b / FindGreatestCommonDivisor(a, b));

        private static long FindGreatestCommonDivisor(long a, long b) => b == 0 ? a : FindGreatestCommonDivisor(b, a % b);
    }
}
