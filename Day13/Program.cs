using System.Diagnostics;
using Shared;

namespace Day13 {
    internal class Program {
        private static string[] _lines = [];
        private static int _lastBlankLine = -1;

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();

            long p1_score = 0;
            long p2_score = 0;

            _lines = File.ReadAllLines(args[0]);

            while (GetNextPattern(out List<string> patternLines)) {
                p1_score += SearchForHorizontalReflections(patternLines, -1, out int originalReflectionRowIndex);
                p1_score += SearchForVerticalReflections(patternLines, -1, out int originalReflectionColumnIndex);

                long oldP2Score = p2_score;
                for(int row = 0; row < patternLines.Count && oldP2Score == p2_score; row++) {
                    for(int column = 0; column < patternLines[row].Length && oldP2Score == p2_score; column++) {
                        List<string> newPatternLines = ModifyPatternLines(patternLines, row, column);
                        p2_score += SearchForHorizontalReflections(newPatternLines, originalReflectionRowIndex, out int _);
                        p2_score += SearchForVerticalReflections(newPatternLines, originalReflectionColumnIndex, out int _);
                    }
                }
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
            Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
        }

        private static bool GetNextPattern(out List<string> patternLines) {
            patternLines = [];
            for(int i = _lastBlankLine + 1; i < _lines.Length; i++) {
                if (string.IsNullOrWhiteSpace(_lines[i])) {
                    _lastBlankLine = i;
                    return true;
                } else {
                    patternLines.Add(_lines[i]);
                }
            }
            _lastBlankLine = _lines.Length;
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

        private static int SearchForHorizontalReflections(List<string> patternLines, int rowIndexToIgnore, out int rowIndex) {
            for (rowIndex = 0; rowIndex < patternLines.Count - 1; rowIndex++) {
                if (rowIndex != rowIndexToIgnore && patternLines[rowIndex] == patternLines[rowIndex + 1] && CheckHorizontalMirror(patternLines, rowIndex)) {
                    return (rowIndex + 1) * 100;
                }
            }
            rowIndex = -1;
            return 0;
        }

        private static int SearchForVerticalReflections(List<string> patternLines, int columnIndexToIgnore, out int columnIndex) {
            for (columnIndex = 0; columnIndex < patternLines[0].Length - 1; columnIndex++) {
                if (columnIndex != columnIndexToIgnore && GetColumnString(columnIndex, patternLines) == GetColumnString(columnIndex + 1, patternLines) && CheckVerticalMirror(patternLines, columnIndex)) {
                    return columnIndex + 1;
                }
            }
            columnIndex = -1;
            return 0;
        }

        private static List<string> ModifyPatternLines(List<string> patternLines, int row, int column) {
            string[] newPatternLines = new string[patternLines.Count];
            patternLines.CopyTo(newPatternLines);

            char[] chars = newPatternLines[row].ToCharArray();
            char replace = chars[column] == '#' ? '.' : '#';
            chars[column] = replace;
            newPatternLines[row] = new string(chars);
            return newPatternLines.ToList();
        }
    }
}
