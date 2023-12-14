using Shared;
using System.Diagnostics;

namespace Day14 {
    internal class Program {
        private static Field[][] _fields = [];
        private static (int, int) _north = (-1, 0);
        private static (int, int) _west = (0, -1);
        private static (int, int) _south = (1, 0);
        private static (int, int) _east = (0, 1);
        private static List<int> _loads = [];

        private static readonly int CYCLE_AMOUNT = 200;
        private static readonly int START_INDEX = 100;

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();

            _fields = File.ReadAllLines(args[0]).Select(line => line.Select(GetField).ToArray()).ToArray();

            TiltNorth();
            long p1_score = CalculateLoad();

            for (int i = 0; i < CYCLE_AMOUNT; i++) {
                TiltNorth();
                TiltWest();
                TiltSouth();
                TiltEast();
                _loads.Add(CalculateLoad());
            }
            long p2_score = CalculateLoadViaCycles();

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

        private static int CalculateLoadViaCycles() {
            //The first loads are never part of the cycle, skip the fist 100
            int startIndex = START_INDEX;

            //Take three loads
            int[] startCycle = [_loads[startIndex], _loads[startIndex + 1], _loads[startIndex + 2]];

            //Search the index, at that the three numbers can be found again
            int nextCycleIndex = 0;
            for(nextCycleIndex = startIndex + 3; nextCycleIndex < _loads.Count - 3; nextCycleIndex++) {
                if (_loads[nextCycleIndex] == startCycle[0] && _loads[nextCycleIndex + 1] == startCycle[1] && _loads[nextCycleIndex + 2] == startCycle[2]) {
                    break;
                }
            }

            //Make a list with the whole cycle
            List<int> wholeCycle = [];
            for (int i = startIndex; i < nextCycleIndex; i++) {
                wholeCycle.Add(_loads[i]);
            }

            //Search the beginning of the cycle
            List<int> cycleStartIndexList = [];
            //Shift the cycle numbers so every number is first at some time
            for (int shiftAmount = 0; shiftAmount < wholeCycle.Count; shiftAmount++) {
                //Go through the loads and look, if the cycle is starting there
                for (int loadIndex = 0; loadIndex < _loads.Count; loadIndex++) {
                    int cycleStartIndex = -1;
                    for (int cycleIndex = 0; cycleIndex <= 3; cycleIndex++) {
                        if (cycleIndex == 3) {
                            cycleStartIndex = loadIndex;
                            break;
                        }
                        if (_loads[loadIndex + cycleIndex] != wholeCycle[cycleIndex]) {
                            break;
                        }
                    }
                    if (cycleStartIndex > -1) {
                        cycleStartIndexList.Add(cycleStartIndex);
                        break;
                    }
                }

                //Shift the cycle
                int temp = wholeCycle[0];
                wholeCycle.RemoveAt(0);
                wholeCycle.Add(temp);
            }
            int firstCycleIndex = cycleStartIndexList.Min();

            //Build correct cycle
            List<int> correctCycle = [];
            for (int i = firstCycleIndex; i < firstCycleIndex + wholeCycle.Count; i++) {
                correctCycle.Add(_loads[i]);
            }

            int remainder = (1000000000 - firstCycleIndex) % correctCycle.Count;
            int result = correctCycle[remainder - 1];
            return result;
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

        private enum Field : byte {
            Nothing,
            CubeRock,
            RoundRock
        }
    }
}
