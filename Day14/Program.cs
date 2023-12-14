using Shared;
using System.Diagnostics;

namespace Day14 {
    internal class Program {
        private static Field[][] _fields = [];
        private static (int, int) _north = (-1, 0);
        private static (int, int) _west = (0, -1);
        private static (int, int) _south = (1, 0);
        private static (int, int) _east = (0, 1);

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();

            long p1_score = 0;
            long p2_score = 0;

            _fields = File.ReadAllLines(args[0]).Select(line => line.Select(GetField).ToArray()).ToArray();

            //Nach 2 cycles wiederholt sich das ergebniss alle 7 cycles
            int[] cycle = [69, 69, 65, 64, 65, 63, 68];
            int cycleStartIndex = 2;
            int remainder = 1000000000 % cycle.Length;
            int result = cycle[remainder - cycleStartIndex - 1];

            TiltNorth();
            p1_score = CalculateLoad();

            for (int i = 0; i < 1000000000; i++) {
                TiltNorth();
                TiltWest();
                TiltSouth();
                TiltEast();
            }

            p2_score = CalculateLoad();

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
            Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
        }

        private static Field GetField((int row, int col) coordinate) => _fields[coordinate.row][coordinate.col];

        private static Field GetField(char ch) => ch switch {
            '#' => Field.CubeRock,
            'O' => Field.RoundRock,
            _ => Field.Nothing
        };

        private static void TiltNorth() {
            for(int row = 0; row < _fields.Length; row++) {
                for(int col = 0;  col < _fields[row].Length; col++) {
                    if(GetField((row, col)) == Field.RoundRock) {
                        Roll((row, col), _north);
                    }
                }
            }
        }

        private static void TiltWest() {
            for (int col = 0; col < _fields[0].Length; col++) {
                for (int row = 0; row < _fields.Length; row++) {
                    if (GetField((row, col)) == Field.RoundRock) {
                        Roll((row, col), _west);
                    }
                }
            }
        }

        private static void TiltSouth() {
            for (int row = _fields.Length - 1; row >= 0; row--) {
                for (int col = 0; col < _fields[row].Length; col++) {
                    if (GetField((row, col)) == Field.RoundRock) {
                        Roll((row, col), _south);
                    }
                }
            }
        }

        private static void TiltEast() {
            for (int col = _fields[0].Length - 1; col >= 0; col--) {
                for (int row = 0; row < _fields.Length; row++) {
                    if (GetField((row, col)) == Field.RoundRock) {
                        Roll((row, col), _east);
                    }
                }
            }
        }

        private static void Roll((int row, int col) coordinate, (int, int) direction) {
            (int row, int col) lastFreePosition = FindLastFreeNorthernPosition(coordinate, direction);
            if(lastFreePosition != coordinate) {
                SwitchPositions(coordinate, lastFreePosition);
            }
        }

        private static (int row, int col) FindLastFreeNorthernPosition((int row, int col) coordinate, (int rowMod, int colMod) direction) {
            (int row, int col) lastFreePosition = coordinate;
            while (true) {
                (int row, int col) checkPosition = (lastFreePosition.row + direction.rowMod, lastFreePosition.col + direction.colMod);

                if(IsOutOfBounds(checkPosition) || GetField(checkPosition) != Field.Nothing) {
                    return lastFreePosition;
                }

                lastFreePosition = checkPosition;
            }
        }

        private static bool IsOutOfBounds((int row, int col) coordinate) {
            return coordinate.row < 0 || coordinate.row >= _fields.Length || coordinate.col < 0 || coordinate.col >= _fields[0].Length;
        }

        private static void SwitchPositions((int row, int col) a, (int row, int col) b) {
            (_fields[b.row][b.col], _fields[a.row][a.col]) = (_fields[a.row][a.col], _fields[b.row][b.col]);
        }

        private static int CalculateLoad() {
            int load = 0;
            int multiplier = _fields.Length;
            for(int i = 0; i < _fields.Length; i++) {
                load += _fields[i].Count(x => x == Field.RoundRock) * multiplier;
                multiplier--;
            }
            return load;
        }

        private enum Field : byte {
            Nothing,
            CubeRock,
            RoundRock
        }

        private static void PrintFields() {
            foreach (Field[] line in _fields) {
                line.ToList().ForEach(field => Console.Write(GetChar(field)));
                Console.Write('\n');
            }
        }

        private static char GetChar(Field field) => field switch {
            Field.CubeRock => '#',
            Field.RoundRock => 'O',
            _ => '.'
        };
    }
}
