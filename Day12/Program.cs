using System.Diagnostics;
using Shared;

namespace Day12 {
    internal class Program {
        private static List<int> _hashTagIndexes = [];
        private static List<int> _possiblePlaceIndexes = [];

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();
            Stopwatch fieldWatch = new();

            long p1_score = 0;
            long p2_score = 0;

            foreach(string line in File.ReadLines(args[0])) {
                string field = line.Split(' ')[0];
                BuildFieldCaches(field);
                int[] lenghts = line.Split(' ')[1].Split(',').Select(int.Parse).ToArray();
                p1_score += TryCombinations(field, lenghts, []);

                fieldWatch.Start();
                string unfoldField = $"{field}?{field}?{field}?{field}?{field}";
                BuildFieldCaches(unfoldField);
                int[] unfoldLengths = new int[lenghts.Length * 5];
                lenghts.CopyTo(unfoldLengths, 0);
                lenghts.CopyTo(unfoldLengths, lenghts.Length);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 2);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 3);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 4);
                long possibilities = TryCombinations(unfoldField, unfoldLengths, []);
                Console.WriteLine($"Found after {fieldWatch.Elapsed} {possibilities} \tpossible solutions in field {field}.");
                fieldWatch.Reset();
                p2_score += possibilities;
            }

            Console.WriteLine($"Part1 Result: {p1_score} ({p1_score == 21})\nPart2 Result: {p2_score} ({p2_score == 525152})");
            Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
        }

        private static long TryCombinations(string field, int[] lenghts, int[] lengthIndexes) {
            if (lenghts.Length == lengthIndexes.Length) {
                return CheckCombination(field, lenghts, lengthIndexes, partial: false) ? 1 : 0;
            } else if(!CheckCombination(field, lenghts, lengthIndexes, partial: true)) {
                return 0;
            }

            long possibilities = 0;
            int indexToSearch = lengthIndexes.Length;

            int startIndex;
            if(indexToSearch == 0) {
                //Very first index. Use first possible place, that has enough space
                startIndex = _possiblePlaceIndexes.First();
            } else {
                startIndex = lengthIndexes.Last() + lenghts[indexToSearch - 1] + 1;
            }

            for(int i = startIndex; i <= field.Length - lenghts[indexToSearch]; i++) {
                if (!_possiblePlaceIndexes.Contains(i)) {
                    continue;
                }

                //Build lengthIndexes
                int[] nextLengthIndexes = new int[indexToSearch + 1];
                lengthIndexes.CopyTo(nextLengthIndexes, 0);
                nextLengthIndexes[indexToSearch] = i;

                possibilities += TryCombinations(field, lenghts, nextLengthIndexes);
            }

            return possibilities;
        }

        private static bool CheckCombination(string field, int[] lenghts, int[] lengthIndexes, bool partial) {
            if (lengthIndexes.Length == 0) return true;
            List<int> indexList = [];
            for (int i = 0; i < lengthIndexes.Length; i++) {
                for (int j = 0; j < lenghts[i]; j++) {
                    indexList.Add(lengthIndexes[i] + j);
                }
            }

            //Check if all lengths are possible
            foreach(int index in indexList) {
                if (!_possiblePlaceIndexes.Contains(index)) {
                    return false;
                }
            }

            if(partial) {
                //Check if a # is in an empty space
                foreach(int hashTagIndex in _hashTagIndexes) {
                    if(hashTagIndex < lengthIndexes.Last() && !indexList.Contains(hashTagIndex)) {
                        return false;
                    }
                }
            } else {
                //Check if all # are placed
                foreach(int index in _hashTagIndexes) {
                    if(!indexList.Contains(index)) {
                        return false;
                    }
                }
            }
            
            return true;
        }

        private static void BuildFieldCaches(string field) {
            _hashTagIndexes.Clear();
            _possiblePlaceIndexes.Clear();
            
            for(int i = 0; i < field.Length; i++) {
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
