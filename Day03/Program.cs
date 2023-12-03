using Shared;

namespace Day03 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            int p1_score = 0;
            int p2_score = 0;

            string[] lines = File.ReadAllLines(args[0]);
            Schematic schematic = new(lines);
            List<Number> numbers = schematic.GetAllNumbersWithSymbols();
            p1_score = numbers.Sum(x => x.GetNumber());

            p2_score = schematic.GetAllGearRatios(numbers).Sum();

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }
    }
}
