using System.Diagnostics;
using System.Text.RegularExpressions;
using Shared;

namespace Day03 {
    internal partial class Day03 {
        [GeneratedRegex(@"mul\(\d{1,3},\d{1,3}\)")]
        private static partial Regex MulRegex();

        [GeneratedRegex(@"do\(\)")]
        private static partial Regex DoRegex();

        [GeneratedRegex(@"don't\(\)")]
        private static partial Regex DontRegex();

        static void Main(string[] args) {
            if(!ArgsValidator.IsValidArgs(args)) return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int p1_score = 0;
            int p2_score = 0;

            string input = File.ReadAllText(args[0]);

            MatchCollection matches = MulRegex().Matches(input);
            MatchCollection doMatches = DoRegex().Matches(input);
            MatchCollection dontMatches = DontRegex().Matches(input);

            foreach(Match match in matches) {
                string mul = match.Value.Replace("mul(", "").Replace(")", "");
                int[] factors = mul.Split(',').Select(int.Parse).ToArray();
                p1_score += factors[0] * factors[1];

                if(IsActive(doMatches, dontMatches, match.Index)) {
                    p2_score += factors[0] * factors[1];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}\nFinished in {stopwatch.Elapsed}");
        }

        private static bool IsActive(MatchCollection doMatches, MatchCollection dontMatches, int index) {
            IEnumerable<Match> doMatchesBeforeIndex = doMatches.Where(x => x.Index < index);
            IEnumerable<Match> dontMatchesBeforeIndex = dontMatches.Where(x => x.Index < index);

            if(!dontMatchesBeforeIndex.Any()) {
                return true;
            } else if(!doMatchesBeforeIndex.Any()) {
                return false;
            } else {
                return doMatchesBeforeIndex.MaxBy(x => x.Index).Index > dontMatchesBeforeIndex.MaxBy(x => x.Index).Index;
            }
        }
    }
}