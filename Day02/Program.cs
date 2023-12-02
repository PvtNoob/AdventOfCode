namespace Day02 {
    internal class Program {
        static void Main(string[] args) {
            int p1_score = 0;
            int p2_score = 0;

            foreach(string line in File.ReadLines(args[0])) {
                Game game = new(line);
                if (!game.HasAColorAtAnyPointMoreThan(redMax: 12, greenMax: 13, blueMax: 14)) {
                    p1_score += game.GameID;
                }
                int[] minimumSet = game.GetMinimumSet();
                p2_score += Game.GetPowerOf(minimumSet);
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }
    }
}