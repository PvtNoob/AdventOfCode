using Shared;
using System.Diagnostics;

namespace Day15 {
    internal class Program {
        private static Dictionary<string, int> labelHashCache = [];
        private static List<(string label, int focalLength)>[] boxes = [];

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();

            string[] steps = File.ReadAllText(args[0]).Split(',');
            long p1_score = steps.Sum(GetHash);

            boxes = Enumerable.Range(0, 256).Select(x => new List<(string, int)>()).ToArray();
            DoProcedure(steps);
            long p2_score = CalculateFocusingPower();

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
            Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
        }

        private static int GetHash(string @string) {
            int hash = 0;
            for(int i = 0; i < @string.Length; i++) {
                if (@string[i] is '=' or '-') {
                    labelHashCache.TryAdd(@string[..i], hash);
                }

                hash += @string[i];
                hash *= 17;
                hash %= 256;
            }
            
            return hash;
        }

        private static void DoProcedure(string[] steps) {
            foreach (string step in steps) {
                string label = string.Empty;
                char operation = ' ';

                for(int i = 0; i < step.Length; i++) {
                    if(step[i] is '=' or '-') {
                        label = step[..i];
                        operation = step[i];
                        break;
                    }
                }

                int boxIndex = labelHashCache[label];

                if (operation == '-') {
                    //Remove
                    for (int i = 0; i < boxes[boxIndex].Count; i++) {
                        if (boxes[boxIndex][i].label == label) {
                            boxes[boxIndex].RemoveAt(i);
                            break;
                        }
                    }
                } else {
                    int focalLength = int.Parse(step.Last().ToString());

                    //Replace if existing
                    bool replaced = false;
                    for (int lensIndex = 0; lensIndex < boxes[boxIndex].Count; lensIndex++) {
                        if (boxes[boxIndex][lensIndex].label == label) {
                            boxes[boxIndex][lensIndex] = (label, focalLength);
                            replaced = true;
                            break;
                        }
                    }

                    //Add if not existing
                    if (!replaced) {
                        boxes[boxIndex].Add((label, focalLength));
                    }
                }
            }
        }

        private static long CalculateFocusingPower() {
            long totalPower = 0;
            for(int i = 0; i < boxes.Length; i++) {
                for(int j = 0; j < boxes[i].Count; j++) {
                    totalPower += (i + 1) * (j + 1) * boxes[i][j].focalLength; 
                }
            }
            return totalPower;
        }
    }
}
