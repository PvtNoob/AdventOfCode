namespace Day02 {
    internal class Game {
        public const int RED = 0;
        public const int GREEN = 1;
        public const int BLUE = 2;

        public int GameID { get; }
        public List<int[]> Rounds {  get; }

        public Game(string line) {
            Rounds = [];
            GameID = int.Parse(line.Split(':')[0].Replace("Game", "").Trim());
            line = line.Split(':')[1];
            foreach(string roundStr in line.Split(";")) {
                int[] round = [0, 0, 0];
                string[] colors = roundStr.Split(",");
                foreach(string color in colors) {
                    if (color.Contains("red")) {
                        round[RED] += int.Parse(color.Replace("red", "").Trim());
                    }else if (color.Contains("green")) {
                        round[GREEN] += int.Parse(color.Replace("green", "").Trim());
                    }else if (color.Contains("blue")) {
                        round[BLUE] += int.Parse(color.Replace("blue", "").Trim());
                    }
                }
                Rounds.Add(round);
            }
        }

        public bool HasMoreThanAtAnyPoint(int index, int max) => Rounds.Any(x => x[index] > max);

        public bool HasAColorAtAnyPointMoreThan(int redMax, int greenMax, int blueMax) {
            return HasMoreThanAtAnyPoint(RED, redMax) || HasMoreThanAtAnyPoint(GREEN, greenMax) || HasMoreThanAtAnyPoint(BLUE, blueMax);
        }

        public int[] GetMinimumSet() {
            int[] minimumSet = [Rounds.Max(x => x[RED]), Rounds.Max(x => x[GREEN]), Rounds.Max(x => x[BLUE])];
            return minimumSet;
        }

        public static int GetPowerOf(int[] set) {
            return set[RED] * set[GREEN] * set[BLUE];
        }
    }
}
