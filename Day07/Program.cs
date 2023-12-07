using Shared;

namespace Day07 {
    internal class Program {
        private static readonly (char card, int value)[] p1CardValues = [('A', 12), ('K', 11), ('Q', 10), ('J', 9), ('T', 8), ('9', 7), ('8', 6), ('7', 5), ('6', 4), ('5', 3), ('4', 2), ('3', 1), ('2', 0),];
        private static readonly (char card, int value)[] p2CardValues = [('A', 12), ('K', 11), ('Q', 10), ('J', -1), ('T', 8), ('9', 7), ('8', 6), ('7', 5), ('6', 4), ('5', 3), ('4', 2), ('3', 1), ('2', 0),];

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            string[] lines = File.ReadAllLines(args[0]);
            Hand[] p1Hands = lines.Select(line => ParseLine(line, p1CardValues)).ToArray();
            Hand[] p2Hands = lines.Select(line => ParseLine(line, p2CardValues)).ToArray();

            Array.Sort(p1Hands);
            Array.Sort(p2Hands);

            long p1_score = 0;
            long p2_score = 0;

            for (int i = 1; i <= p1Hands.Length; i++) {
                p1_score += p1Hands[i - 1].Bid * i;
                p2_score += p2Hands[i - 1].Bid * i;
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }

        private static Hand ParseLine(string line, (char card, int value)[] cardValues) {
            string[] parts = line.Split(" ");
            return new(cardValues, parts[0], int.Parse(parts[1]));
        }
    }
}
