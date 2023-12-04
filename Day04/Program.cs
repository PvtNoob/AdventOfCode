using System.Diagnostics;
using Shared;

namespace Day04 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            double p1_score = 0;
            int p2_score = 0;

            string[] lines = File.ReadAllLines(args[0]);
            int[] copies = lines.Select(x => 1).ToArray();

            for (int i = 0; i < lines.Length; i++) {
                string[] parts = lines[i].Split(':', '|');
                string[] winningNumbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string[] scratchNumbers = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int winningCounter = scratchNumbers.Where(x => winningNumbers.Contains(x)).Count();

                if (winningCounter > 0) {
                    p1_score += Math.Pow(2, winningCounter - 1);
                }

                p2_score += copies[i];
                for (int win = 1; win <= Math.Min(winningCounter, copies.Length) ; win++) {
                    copies[i + win] += copies[i];
                }
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }
    }
}