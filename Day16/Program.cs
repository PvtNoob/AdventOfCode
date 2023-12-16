using System.Diagnostics;
using Shared;

namespace Day16 {
    internal class Program {
        private static Tile[][] _field = [];
        private static bool[][] _energezedFields = [];
        private static List<((int row, int col), Direction)> _walkHistory = [];

        private static Dictionary<Direction, Direction> _slashPipeDirections = new() {
            { Direction.Up, Direction.Right },
            { Direction.Right, Direction.Up },
            { Direction.Down, Direction.Left },
            { Direction.Left, Direction.Down }
        };

        private static Dictionary<Direction, Direction> _backSlashPipeDirections = new() {
            { Direction.Up, Direction.Left },
            { Direction.Right, Direction.Down },
            { Direction.Down, Direction.Right },
            { Direction.Left, Direction.Up }
        };

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;
            Stopwatch stopwatch = Stopwatch.StartNew();

            _field = File.ReadLines(args[0]).Select(line => line.Select(GetTile).ToArray()).ToArray();
            ResetRun();

            long p1_score = 0;
            long p2_score = 0;

            (int row, int col) startPosition = (0, -1);
            Direction startDirection = Direction.Right;
            Move(startPosition, startDirection);
            p1_score = CountEnergizedFields();

            //Part2
            foreach(int col in Enumerable.Range(0, _field[0].Length - 1)) {
                ResetRun();
                startPosition = (-1, col);
                startDirection = Direction.Down;
                Move(startPosition, startDirection);
                p2_score = Math.Max(p2_score, CountEnergizedFields());
            }

            foreach (int col in Enumerable.Range(0, _field[0].Length - 1)) {
                ResetRun();
                startPosition = (_field.Length, col);
                startDirection = Direction.Up;
                Move(startPosition, startDirection);
                p2_score = Math.Max(p2_score, CountEnergizedFields());
            }

            foreach (int row in Enumerable.Range(0, _field.Length - 1)) {
                ResetRun();
                startPosition = (row, -1);
                startDirection = Direction.Right;
                Move(startPosition, startDirection);
                p2_score = Math.Max(p2_score, CountEnergizedFields());
            }

            foreach (int row in Enumerable.Range(0, _field.Length - 1)) {
                ResetRun();
                startPosition = (row, _field[0].Length);
                startDirection = Direction.Left;
                Move(startPosition, startDirection);
                p2_score = Math.Max(p2_score, CountEnergizedFields());
            }

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
            Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
        }

        private static int CountEnergizedFields() {
            return _energezedFields.Sum(row => row.Count(col => col));
        }

        private static void ResetRun() {
            _energezedFields = _field.Select(row => row.Select(col => false).ToArray()).ToArray();
            _walkHistory.Clear();
        }

        private static void Move((int row, int col) position, Direction direction) {
            if (!IsOutOfBounds(position)) {
                _energezedFields[position.row][position.col] = true;
            }

            (int, int) newPosition = position.Move(direction);
            if (IsOutOfBounds(newPosition) || _walkHistory.Contains((newPosition, direction))) {
                return;
            }
            _walkHistory.Add((newPosition, direction));

            switch (GetTile(newPosition)) {
                case Tile.SlashMirror:
                    Move(newPosition, _slashPipeDirections[direction]);
                    break;
                case Tile.BackshlashMirror:
                    Move(newPosition, _backSlashPipeDirections[direction]);
                    break;
                case Tile.PipeSplitter:
                    if (direction is Direction.Up or Direction.Down) {
                        Move(newPosition, direction);
                    } else {
                        Move(newPosition, Direction.Up);
                        Move(newPosition, Direction.Down);
                    }
                    break;
                case Tile.MinusSplitter:
                    if (direction is Direction.Right or Direction.Left) {
                        Move(newPosition, direction);
                    } else {
                        Move(newPosition, Direction.Right);
                        Move(newPosition, Direction.Left);
                    }
                    break;
                default:
                    Move(newPosition, direction);
                    break;
            }
        }

        private static bool IsOutOfBounds((int row, int col) position) {
            return position.IsOutOfBounds(_field.Length - 1, _field[0].Length - 1);
        }

        private static Tile GetTile((int row, int col) position) {
            return _field[position.row][position.col];
        }

        private static Tile GetTile(char ch) => ch switch {
            '/' => Tile.SlashMirror,
            '\\' => Tile.BackshlashMirror,
            '|' => Tile.PipeSplitter,
            '-' => Tile.MinusSplitter,
            _ => Tile.Nothing
        };

        private enum Tile {
            SlashMirror,
            BackshlashMirror,
            PipeSplitter,
            MinusSplitter,
            Nothing
        }

        private static void PrintEnergizedField() {
            foreach (bool[] row in _energezedFields) {
                foreach (bool col in row) {
                    Console.Write(col ? "#" : ".");
                }
                Console.Write('\n');
            }
        }
    }
}
