using System.Diagnostics;
using Shared;

namespace Day13 {
    internal class Program {
        private static string[] lines = [];
        private static int lastBlankLine = -1;

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();

            long p1_score = 0;
            long p2_score = 0;

            lines = File.ReadAllLines(args[0]);

            int leftColumns = 0;
            int upperRows = 0;

            while(GetNextPattern(out List<string> patternLines)) {
                for(int row = 0; row < patternLines.Count - 1; row++) {
                    if (patternLines[row] == patternLines[row + 1] && CheckHorizontalMirror(patternLines, row)) {
                        upperRows += row + 1;
                        break;
                    }
                }
                for(int column = 0; column < patternLines[0].Length - 1; column++) {
                    if(GetColumnString(column, patternLines) == GetColumnString(column + 1, patternLines) && CheckVerticalMirror(patternLines, column)) {
                        leftColumns += column + 1;
                        break;
                    }
                }
            }

            p1_score = leftColumns + (100 * upperRows);

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
            Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
        }

        private static bool GetNextPattern(out List<string> patternLines) {
            patternLines = [];
            for(int i = lastBlankLine + 1; i < lines.Length; i++) {
                if (string.IsNullOrWhiteSpace(lines[i])) {
                    lastBlankLine = i;
                    return true;
                } else {
                    patternLines.Add(lines[i]);
                }
            }
            lastBlankLine = lines.Length;
            return patternLines.Count != 0;
        }

        private static string GetColumnString(int column, List<string> patternLines) {
            return new string(patternLines.Select(x => x[column]).ToArray());
        }

        private static bool CheckHorizontalMirror(List<string> patternLines, int row) {
            for(int i = 1; (row - i) >= 0 && (row + i + 1) < patternLines.Count; i++) {
                string up = patternLines[row - i];
                string down = patternLines[row + i + 1];
                if (up != down) {
                    return false;
                }
            }
            return true;
        }

        private static bool CheckVerticalMirror(List<string> patternLines, int column) {
            for (int i = 1; (column - i) >= 0 && (column + i + 1) < patternLines[0].Length; i++) {
                string left = GetColumnString(column - i, patternLines);
                string right = GetColumnString(column + i + 1, patternLines);
                if (left != right) {
                    return false;
                }
            }
            return true;
        }
    }
}
