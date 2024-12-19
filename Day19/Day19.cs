using System.Diagnostics;
using Shared;

namespace Day19 {
    internal class Day19 {
        static void Main(string[] args) {
            if(!ArgsValidator.IsValidArgs(args)) return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            long p1_score = 0;
            long p2_score = 0;

            string[] input = File.ReadAllLines(args[0]);

            string[] availablePatterns = input[0].Split(", ", StringSplitOptions.RemoveEmptyEntries);
            string[] designs = input[2..];

            HashSet<string> patternSet = new(availablePatterns);

            foreach(string design in designs) {
                if(IsDesignPossible(design, patternSet)) {
                    p1_score++;
                }

                p2_score += CountWaysToConstruct(design, patternSet, []);
            }

            stopwatch.Stop();
            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}\nFinished in {stopwatch.Elapsed}");
        }

        static bool IsDesignPossible(string design, HashSet<string> patternSet) {
            bool[] dp = new bool[design.Length + 1];
            dp[0] = true;

            for(int i = 1; i <= design.Length; i++) {
                for(int j = 0; j < i; j++) {
                    if(dp[j] && patternSet.Contains(design.Substring(j, i - j))) {
                        dp[i] = true;
                        break;
                    }
                }
            }

            return dp[design.Length];
        }

        static long CountWaysToConstruct(string design, HashSet<string> patternSet, Dictionary<string, long> memo) {
            if(design == "") return 1;
            if(memo.ContainsKey(design)) return memo[design];

            long totalWays = 0;
            for(int i = 1; i <= design.Length; i++) {
                string prefix = design.Substring(0, i);
                if(patternSet.Contains(prefix)) {
                    totalWays += CountWaysToConstruct(design.Substring(i), patternSet, memo);
                }
            }

            memo[design] = totalWays;
            return totalWays;
        }
    }
}
