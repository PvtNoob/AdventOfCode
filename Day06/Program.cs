using System.Numerics;
using Shared;

namespace Day06 {
    internal class Program {
        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            BigInteger p1_score = 0;
            BigInteger p2_score = 0;

            string[] lines = File.ReadAllLines(args[0]);

            List<BigInteger> times = GetValues(lines[0]);
            List<BigInteger> distances = GetValues(lines[1]);
            BigInteger[] amountOfWinOptions = times.Select(x => new BigInteger(0)).ToArray();

            for (int race = 0; race < times.Count; race++) {
                BigInteger totalTime = times[race];
                BigInteger totalDistance = distances[race];
                BigInteger lowestWinTime = totalTime;

                BigInteger analyzingRangeStart = 0;
                BigInteger analyzingRangeEnd = totalTime / 2;
                do {
                    BigInteger middle = GetMiddle(analyzingRangeStart, analyzingRangeEnd);
                    BigInteger lowerMiddle = GetMiddle(analyzingRangeStart, middle);
                    BigInteger upperMiddle = GetMiddle(middle, analyzingRangeEnd);

                    if (IsInWinRange(totalTime, totalDistance, lowerMiddle)) {
                        lowestWinTime = lowerMiddle;
                        analyzingRangeEnd = lowerMiddle;
                    } else {
                        if(IsInWinRange(totalTime, totalDistance, upperMiddle)) {
                            analyzingRangeEnd = upperMiddle;
                            if (lowestWinTime > upperMiddle) {
                                lowestWinTime = upperMiddle;
                            }
                        }
                        analyzingRangeStart = lowerMiddle;
                    }
                } while (!IsLowestWinTime(totalTime, totalDistance, lowestWinTime));

                BigInteger biggestWinTime = totalTime - lowestWinTime;
                amountOfWinOptions[race] = biggestWinTime - lowestWinTime + 1;
            }

            p1_score = amountOfWinOptions.SkipLast(1).Aggregate(1, (BigInteger a, BigInteger b) => a * b);
            p2_score = amountOfWinOptions.Last();

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }

        private static List<BigInteger> GetValues(string line) {
            string numbers = line.Split(':')[1];
            List<BigInteger> values = numbers.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(BigInteger.Parse).ToList();
            values.Add(BigInteger.Parse(numbers.Replace(" ", "")));
            return values;
        }

        private static bool IsInWinRange(BigInteger totalTime, BigInteger totalDistance, BigInteger pushTime) {
            BigInteger runDistance = pushTime * (totalTime - pushTime);
            return runDistance > totalDistance;
        }

        private static bool IsLowestWinTime(BigInteger totalTime, BigInteger totalDistance, BigInteger pushTime) {
            return IsInWinRange(totalTime, totalDistance, pushTime) && !IsInWinRange(totalTime, totalDistance, pushTime - 1);
        }

        private static BigInteger GetMiddle(BigInteger lowerEnd, BigInteger upperEnd) {
            return (lowerEnd + upperEnd) / 2;
        }
    }
}