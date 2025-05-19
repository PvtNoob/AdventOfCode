using System.Diagnostics;
using Shared;

namespace Day08 {
    internal class Day08 {
        private const char EMPTY = '.';

        static void Main(string[] args) {
            if(!ArgsValidator.IsValidArgs(args)) return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int p1_score = 0;
            int p2_score = 0;

            List<Signal> signals = [];
            List<Antinode> antinodes = [];

            string[] lines = File.ReadAllLines(args[0]);

            int maxX = lines[0].Length - 1;
            int maxY = lines.Length - 1;

            //READ SIGNALS
            for(int y = 0; y < lines.Length; y++) {
                for(int x = 0; x < lines[y].Length; x++) {
                    char position = lines[y][x];
                    if(position != EMPTY) {
                        signals.Add(new(x, y, position));
                    }
                }
            }

            //CALCULATE ANTINODES
            IEnumerable<IGrouping<char, Signal>> frequenceGroups = signals.GroupBy(x => x.Frequency);

            foreach(IGrouping<char, Signal> group in frequenceGroups) {
                Signal[] signalsForFrequence = group.ToArray();

                for(int i = 0; i < signalsForFrequence.Length - 1; i++) {
                    for(int j = i + 1; j < signalsForFrequence.Length; j++) {
                        Signal iSignal = signalsForFrequence[i];
                        Signal jSignal = signalsForFrequence[j];

                        (int x, int y) vector = (jSignal.X - iSignal.X, jSignal.Y - iSignal.Y);

                        (int x, int y) minusVector = vector;
                        while(true) {
                            Antinode a = new(iSignal.X - minusVector.x, iSignal.Y - minusVector.y);

                            if(a.IsOutOfBounds(maxX, maxY)) {
                                break;
                            } else {
                                antinodes.Add(a);
                                minusVector = (minusVector.x - vector.x, minusVector.y - vector.y);
                            }
                        }

                        (int x, int y) plusVector = vector;
                        while(true) {
                            Antinode b = new(jSignal.X + plusVector.x, jSignal.Y + plusVector.y);

                            if(b.IsOutOfBounds(maxX, maxY)) {
                                break;
                            } else {
                                antinodes.Add(b);
                                plusVector = (plusVector.x + vector.x, plusVector.y + vector.y);
                            }
                        }
                    }
                }
            }

            //OUTPUT
            for(int y = 0; y <= maxY; y++) {
                for(int x = 0; x <= maxX; x++) {
                    char position = EMPTY;

                    Antinode? antinode = antinodes.FirstOrDefault(ant => ant.X == x && ant.Y == y);

                    if(antinode != null) {
                        position = '#';
                    }

                    Signal? signal = signals.FirstOrDefault(sig => sig.X == x && sig.Y == y);

                    if(signal != null) {
                        position = signal.Frequency;
                    }

                    Console.Write(position);
                }
                Console.Write('\n');
            }

            p1_score = antinodes.Distinct().Count();

            stopwatch.Stop();
            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}\nFinished in {stopwatch.Elapsed}");
        }
    }

    internal record Signal(int X, int Y, char Frequency);

    internal record Antinode(int X, int Y) {
        public bool IsOutOfBounds(int maxX, int maxY) {
            return X < 0 || X > maxX || Y < 0 || Y > maxY;
        }
    }
}
