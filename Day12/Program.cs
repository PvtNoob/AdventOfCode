using System.Diagnostics;
using Shared;

namespace Day12 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();

            long p1_score = 0;
            long p2_score = 0;

            foreach(string line in File.ReadLines(args[0])) {
                string field = line.Split(' ')[0];
                int[] lenghts = line.Split(' ')[1].Split(',').Select(int.Parse).ToArray();
                p1_score += TryCombinations(field, lenghts, []);

                string unfoldField = $"{field}?{field}?{field}?{field}?{field}";
                int[] unfoldLengths = new int[lenghts.Length * 5];
                lenghts.CopyTo(unfoldLengths, 0);
                lenghts.CopyTo(unfoldLengths, lenghts.Length);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 2);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 3);
                lenghts.CopyTo(unfoldLengths, lenghts.Length * 4);
                long possibilities = TryCombinations(unfoldField, unfoldLengths, []);
                Console.WriteLine($"Unfold field {field} has {possibilities} possible solutions");
                p2_score += possibilities;
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
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
            //Very first index is zero. Then start after the length before with one space between
            int startIndex = lengthIndexes.Length > 0 ? lengthIndexes.Last() + lenghts[indexToSearch - 1] + 1 : 0;

            for(int i = startIndex; i <= field.Length - lenghts[indexToSearch]; i++) {
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
                if (field[index] is not ('#' or '?')) {
                    return false;
                }
            }

            if(partial) {
                //Check if a # is in an empty space
                List<int> hashTagIndexList = [];
                for(int i = 0; i < lenghts.Length; i++) {
                    if (field[i] == '#') {
                        hashTagIndexList.Add(i);
                    }
                }
                foreach(int hashTagIndex in hashTagIndexList) {
                    if(hashTagIndex < lengthIndexes.Last() && !indexList.Contains(hashTagIndex)) {
                        return false;
                    }
                }
            } else {
                //Check if all # are placed
                for (int i = 0; i < field.Length; i++) {
                    if (field[i] is '#' && !indexList.Contains(i)) {
                        return false;
                    }
                }
            }
            
            return true;
        }

        private static string GetString(int[] indexes) => string.Join(", ", indexes);
    }
}
