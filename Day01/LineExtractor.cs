using Shared;

namespace Day1 {
    internal class LineExtractor(List<(string word, char digit)> digitWords, string line) {
        private List<(string word, char digit)> digitWords = digitWords;
        private string line = line;
        private char? firstDigit;
        private char? secondDigit;

        public int CalculateNumber(bool checkDigitWords) {
            firstDigit = secondDigit = null;

            for (int i = 0; i < line.Length; i++) {
                if (line[i].IsNumeric(out _)) {
                    SaveNumber(line[i]);
                } else if (checkDigitWords && StartsADigitWord(i, out char? digit)) {
                    SaveNumber(digit.Value);
                }
            }

            if (!secondDigit.HasValue) {
                secondDigit = firstDigit;
            }

            int calibrationValue = int.Parse($"{firstDigit}{secondDigit}");
            return calibrationValue;
        }

        private void SaveNumber(char digit) {
            if (firstDigit.HasValue) {
                secondDigit = digit;
            } else {
                firstDigit = digit;
            }
        }

        private bool StartsADigitWord(int index, out char? digit) {
            string subString = line[index..];
            foreach ((string word, char digit) digitWord in digitWords) {
                if (subString.StartsWith(digitWord.word)) {
                    digit = digitWord.digit;
                    return true;
                }
            }
            digit = null;
            return false;
        }
    }
}
