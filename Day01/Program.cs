using Day01.Properties;

namespace Day1 {
    internal class Program {

        static void Main(string[] args) {
            string[] lines = Resources.Input.Split("\r\n");
            int task1Value = 0;
            int task2Value = 0;

            List<(string word, char letter)> digitWords = [
                new("one", '1'),
                new("two", '2'),
                new("three", '3'),
                new("four", '4'),
                new("five", '5'),
                new("six", '6'),
                new("seven", '7'),
                new("eight", '8'),
                new("nine", '9')
            ];

            foreach (string line in lines) {
                LineExtractor lineExtractor = new(digitWords, line);
                task1Value += lineExtractor.CalculateNumber(checkDigitWords: false);
                task2Value += lineExtractor.CalculateNumber(checkDigitWords: true);
            }

            Console.WriteLine($"Task 1: {task1Value}\nTask 2: {task2Value}");
        }
    }
}
