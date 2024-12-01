using Shared;

namespace Day01 {
    internal class Day01 {
        static void Main(string[] args) {
            if(!ArgsValidator.IsValidArgs(args)) return;

            int p1_score = 0;
            int p2_score = 0;

            List<int> leftList = new List<int>();
            List<int> rightList = new List<int>();

            foreach(string line in File.ReadLines(args[0])) {
                string[] split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                leftList.Add(int.Parse(split[0]));
                rightList.Add(int.Parse(split[1]));
            }

            leftList.Sort();
            rightList.Sort();

            //Part1
            for(int i = 0; i < leftList.Count; i++) {
                p1_score += Math.Abs(leftList[i] - rightList[i]);
            }

            //Part2
            for(int i = 0; i < leftList.Count; i++) {
                int occourences = rightList.Count(x => x == leftList[i]);
                p2_score += leftList[i] * occourences;
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }
    }
}