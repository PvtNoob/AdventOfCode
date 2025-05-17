using System.Data;
using System.Diagnostics;
using Shared;

namespace Day05 {
    internal class Day05 {
        private static List<Rule> _rules = [];

        static void Main(string[] args) {
            if(!ArgsValidator.IsValidArgs(args)) return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int p1_score = 0;
            int p2_score = 0;

            bool rulesFinished = false;
            foreach(string line in File.ReadLines(args[0])) {
                if(line.Length == 0) {
                    rulesFinished = true;
                    continue;
                }

                int[] pages = line.Split('|', ',').Select(int.Parse).ToArray();

                if(!rulesFinished) {
                    _rules.Add(new Rule(pages[0], pages[1]));
                } else if(pages.Length > 0) {
                    if(IsUpdateValid(pages)) {
                        p1_score += pages.GetMiddleValue();
                    } else {
                        p2_score += FixUpdate(pages).GetMiddleValue();
                    }
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}\nFinished in {stopwatch.Elapsed}");
        }

        private static bool IsUpdateValid(int[] update) {
            for(int i = 0; i < update.Length; i++) {
                foreach(Rule rule in _rules.Where(x => x.Before == update[i])) {
                    int pageIndex = update.GetIndexOf(rule.After);

                    if(pageIndex >= 0 && pageIndex < i) {
                        return false;
                    }
                }
            }

            return true;
        }

        private static int[] FixUpdate(int[] update) {
            List<int> brokenUpdate = new(update);
            List<int> fixedUpdate = new();
            List<Rule> relevantRules = _rules.Where(rule => update.Contains(rule.Before) && update.Contains(rule.After)).ToList();

            do {
                int pageNotInAfterRules = brokenUpdate.First(page => !relevantRules.Any(rule => rule.After == page));

                brokenUpdate.Remove(pageNotInAfterRules);
                fixedUpdate.Add(pageNotInAfterRules);

                relevantRules.RemoveAll(rule => rule.Before == pageNotInAfterRules);
            } while(brokenUpdate.Count > 0);

            return fixedUpdate.ToArray();
        }
    }

    internal record Rule(int Before, int After);
}
