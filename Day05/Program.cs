using Shared;

namespace Day05 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            long p1_score = long.MaxValue;
            long p2_score = long.MaxValue;

            string[] lines = File.ReadAllLines(args[0]);

            //Read maps
            Map[] maps = new Map[7];
            int mapCounter = 0;
            Map currentMap = new();
            for(int i = 3; i < lines.Length; i++) {
                if (lines[i].Contains("map")){
                    maps[mapCounter] = currentMap;
                    currentMap = new Map();
                    mapCounter++;
                    continue;
                }
                
                if (string.IsNullOrWhiteSpace(lines[i])) {
                    continue;
                }

                currentMap.AddRange(lines[i]);
            }
            maps[mapCounter] = currentMap;

            //Read seeds
            long[] seeds = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();

            //Map seeds
            for (int i = 0; i < seeds.Length; i++) {
                long seed = seeds[i];
                for(int j = 0; j < maps.Length; j++) {
                    seed = maps[j].MapSourceToDestination(seed);
                }
                p1_score = Math.Min(p1_score, seed);
            }

            List<long> totalSeeds = [];
            for(int seedIndex = 0; seedIndex < seeds.Length; seedIndex += 2) {
                for (int seedCounter = 0; seedCounter < seeds[seedIndex + 1]; seedCounter++) {
                    long seed = seeds[seedIndex] + seedCounter;
                    for(int mapIndex = 0; mapIndex < maps.Length; mapIndex++) {
                        seed = maps[mapIndex].MapSourceToDestination(seed);
                    }
                    p2_score = Math.Min(p2_score, seed);
                }
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }
    }
}
