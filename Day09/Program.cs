using Shared;

namespace Day09 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            long p1_score = 0;
            long p2_score = 0;

            foreach(string line in File.ReadLines(args[0])) {
                List<List<long>> numbers = [line.Split(' ').Select(long.Parse).ToList()];

                do {
                    List<long> newNumbers = [];
                    for (int i = 0; i < numbers.Last().Count - 1; i++) {
                        newNumbers.Add(numbers.Last()[i + 1] - numbers.Last()[i]);
                    }
                    numbers.Add(newNumbers);
                }while(numbers.Last().Any(x => x != 0));

                for (int i = numbers.Count - 2; i >= 0; i--) {
                    numbers[i].Add(numbers[i].Last() + numbers[i + 1].Last());
                    numbers[i].Insert(0, numbers[i].First() - numbers[i + 1].First());
                }

                p1_score += numbers.First().Last();
                p2_score += numbers.First().First();
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }
    }
}
