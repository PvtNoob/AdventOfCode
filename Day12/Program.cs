using System.Diagnostics;
using Shared;

namespace Day12 {
    internal class Program {
        private static List<int> _hashTagIndexes = [];
        private static List<int> _possiblePlaceIndexes = [];
        private static int _amountOfIndexesToPlace = 0;

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();
            Stopwatch fieldWatch = new();

            long p1_score = 0;
            long p2_score = 0;

            foreach (string line in File.ReadLines(args[0])) {
                string field = line.Split(' ')[0];
                BuildFieldCaches(field);
                int[] lenghts = line.Split(' ')[1].Split(',').Select(int.Parse).ToArray();
                _amountOfIndexesToPlace = lenghts.Sum();
                p1_score += TryCombinations(field, lenghts, [], 0);

                fieldWatch.Start();
                string unfoldField = $"{field}?{field}?{field}?{field}?{field}";
                BuildFieldCaches(unfoldField);
                int[] unfoldLengths = new int[lenghts.Length * 5];
                lenghts.CopyTo(unfoldLengths, 0);
                lenghts.CopyTo(unfoldLengths, lenghts.Length);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 2);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 3);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 4);
                _amountOfIndexesToPlace = unfoldLengths.Sum();
                long possibilities = TryCombinations(unfoldField, unfoldLengths, [], 0);
                Console.WriteLine($"Found after {fieldWatch.Elapsed} {possibilities} \tpossible solutions in field {field}.");
                fieldWatch.Reset();
                p2_score += possibilities;
            }

            Console.WriteLine($"Part1 Result: {p1_score} ({p1_score == 21})\nPart2 Result: {p2_score} ({p2_score == 525152})");
            Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
        }

        private static long TryCombinations(string field, int[] lenghts, int[] indexList, int indexToSearch) {
            if (_amountOfIndexesToPlace == indexList.Length) {
                return CheckHashTags(field, lenghts, indexList, partial: false) ? 1 : 0;
            } else if (!CheckHashTags(field, lenghts, indexList, partial: true)) {
                return 0;
            }

            long possibilities = 0;
            int startIndex = indexToSearch == 0 ? _possiblePlaceIndexes.First() : indexList.Last() + 2;

            for (int index = startIndex; index <= field.Length - lenghts[indexToSearch]; index++) {

                bool impossible = false;
                for (int checkIndex = index; checkIndex < index + lenghts[indexToSearch]; checkIndex++) {
                    if (!_possiblePlaceIndexes.Contains(checkIndex)) {
                        impossible = true;
                        break;
                    }
                }
                if (impossible) {
                    continue;
                }

                //Build lengthIndexes
                int[] nextLengthIndexes = new int[indexList.Length + lenghts[indexToSearch]];
                indexList.CopyTo(nextLengthIndexes, 0);
                for (int length = 0; length < lenghts[indexToSearch]; length++) {
                    nextLengthIndexes[indexList.Length + length] = index + length;
                }

                possibilities += TryCombinations(field, lenghts, nextLengthIndexes, indexToSearch + 1);
            }

            return possibilities;
        }

        private static bool CheckHashTags(string field, int[] lenghts, int[] indexList, bool partial) {
            if (indexList.Length == 0) return true;

            if (partial) {
                //Check if a # is in an empty space
                foreach (int hashTagIndex in _hashTagIndexes) {
                    if (hashTagIndex < indexList.Last() && !indexList.Contains(hashTagIndex)) {
                        return false;
                    }
                }
            } else {
                //Check if all # are placed
                foreach (int index in _hashTagIndexes) {
                    if (!indexList.Contains(index)) {
                        return false;
                    }
                }
            }

            return true;
        }

        private static void BuildFieldCaches(string field) {
            _hashTagIndexes.Clear();
            _possiblePlaceIndexes.Clear();

            for (int i = 0; i < field.Length; i++) {
                if (field[i] == '#') {
                    _hashTagIndexes.Add(i);
                    _possiblePlaceIndexes.Add(i);
                } else if (field[i] == '?') {
                    _possiblePlaceIndexes.Add(i);
                }
            }
        }
    }
}