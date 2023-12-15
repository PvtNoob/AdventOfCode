using System.Diagnostics;
using Shared;

namespace Day12 {
    internal class Program {
        private static List<int> _hashTagIndexes = [];
        private static List<int> _possiblePlaceIndexes = [];
        private static Dictionary<(int lengthIndex, int startIndex), long> combinationCache = [];

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();

            long p1_score = 0;
            long p2_score = 0;

            foreach (string line in File.ReadLines(args[0])) {
                //Part1
                string field = line.Split(' ')[0];
                BuildFieldCaches(field);
                int[] lenghts = line.Split(' ')[1].Split(',').Select(int.Parse).ToArray();
                p1_score += TryCombinations(field, lenghts, [], 0, _possiblePlaceIndexes.First());
                combinationCache.Clear();

                //Part2
                string unfoldField = $"{field}?{field}?{field}?{field}?{field}";
                BuildFieldCaches(unfoldField);

                int[] unfoldLengths = new int[lenghts.Length * 5];
                lenghts.CopyTo(unfoldLengths, 0);
                lenghts.CopyTo(unfoldLengths, lenghts.Length);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 2);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 3);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 4);

                long possibilities = TryCombinations(unfoldField, unfoldLengths, [], 0, _possiblePlaceIndexes.First());
                combinationCache.Clear();
                p2_score += possibilities;
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
            Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
        }

        private static long TryCombinations(string field, int[] lengths, List<int> indexList, int indexToSearch, int startFieldIndex) {
            //If indexToSearch is out of bounds, the max depths is reached
            if (indexToSearch >= lengths.Length) {
                return CheckCombination(indexList, partial: false) ? 1 : 0;
            }

            //Check, if this combination was searched for before
            if (combinationCache.TryGetValue((indexToSearch, startFieldIndex), out long combinations)) {
                return combinations;
            }

            //Check is partial combination is valid
            if (indexList.Count > 0 && !CheckCombination(indexList, partial: true)) {
                return 0;
            }

            long possibilities = 0;
            for (int index = startFieldIndex; index <= field.Length - lengths[indexToSearch]; index++) {
                List<int> nextIndexList = [.. indexList];
                for (int length = 0; length < lengths[indexToSearch]; length++) {
                    nextIndexList.Add(index + length);
                }

                possibilities += TryCombinations(field, lengths, nextIndexList, indexToSearch + 1, nextIndexList.Last() + 2);
            }
            combinationCache.Add((indexToSearch, startFieldIndex), possibilities);

            return possibilities;
        }

        private static bool CheckCombination(List<int> indexList, bool partial) {
            foreach(int index in indexList) {
                if (!_possiblePlaceIndexes.Contains(index)) {
                    return false;
                }
            }

            foreach (int hashTagIndex in _hashTagIndexes) {
                if (partial && hashTagIndex > indexList.Last()) {
                    continue;
                }
                if (!indexList.Contains(hashTagIndex)) {
                    return false;
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