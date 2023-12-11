using Shared;

namespace Day11 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            long p1_score = 0;
            long p2_score = 0;

            string[] lines = File.ReadAllLines(args[0]);

            List<(int row, int col)> p1Galaxies = [];
            List<(int row, int col)> p2Galaxies = [];

            int p1RowExpansionModificator = 0;
            int p2RowExpansionModificator = 0;
            for (int row = 0; row < lines.Length; row++) {
                int galaxyIndex = lines[row].IndexOf('#');
                if (galaxyIndex == -1) {
                    p1RowExpansionModificator++;
                    p2RowExpansionModificator += 999999;
                    continue;
                }
                do {
                    p1Galaxies.Add((row + p1RowExpansionModificator, galaxyIndex));
                    p2Galaxies.Add((row + p2RowExpansionModificator, galaxyIndex));
                    galaxyIndex = lines[row].IndexOf('#', galaxyIndex + 1);
                } while (galaxyIndex != -1);
            }

            List<(int row, int col)> originalGalaxies = p1Galaxies.Select(x => x).ToList();
            for (int col = 0; col < lines[0].Length; col++) {
                if (!originalGalaxies.Any(x => x.col >= col)) {
                    break;
                }
                if (!originalGalaxies.Any(x => x.col == col)) {
                    for (int i = 0; i < originalGalaxies.Count; i++) {
                        if (originalGalaxies[i].col > col) {
                            p1Galaxies[i] = (p1Galaxies[i].row, p1Galaxies[i].col + 1);
                            p2Galaxies[i] = (p2Galaxies[i].row, p2Galaxies[i].col + 999999);
                        }
                    }
                }
            }

            for (int i = 0; i < p1Galaxies.Count; i++) {
                for (int j = i + 1; j < p1Galaxies.Count; j++) {
                    p1_score += CalculateDistance(p1Galaxies[i], p1Galaxies[j]);
                    p2_score += CalculateDistance(p2Galaxies[i], p2Galaxies[j]);
                }
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }

        private static int CalculateDistance((int row, int col) a, (int row, int col) b) {
            int p1RowDistance = Math.Abs(a.row - b.row);
            int p1ColumnDistance = Math.Abs(a.col - b.col);
            //Console.WriteLine($"Distance of {a} and {b} is {p1RowDistance} + {p1ColumnDistance} = {p1RowDistance + p1ColumnDistance}");
            return p1RowDistance + p1ColumnDistance;
        }
    }
}
