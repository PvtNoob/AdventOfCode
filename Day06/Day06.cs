using System.Diagnostics;
using Shared;

namespace Day06 {
    internal class Day06 {
        static void Main(string[] args) {
            if(!ArgsValidator.IsValidArgs(args)) return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int p1_score = 0;
            int p2_score = 0;

            string[] lines = File.ReadAllLines(args[0]);
            Direction direction = Direction.Up;
            int maxRow = lines.Length - 1;
            int maxCol = lines[0].Length - 1;
            List<(int row, int col, Direction dir)> visited = [];
            (int row, int col) startPosition = (-1, -1);
            (int row, int col) position = (-1, -1);

            //Find Start
            for(int row = 0; row <= maxRow && position == (-1, -1); row++) {
                for(int col = 0; col <= maxCol && position == (-1, -1); col++) {
                    if(lines[row][col] == '^') {
                        startPosition = (row, col);
                        position = startPosition;
                        visited.Add((row, col, direction));
                    }
                }
            }

            //Run Part1
            do {
                (int row, int col) newPosition = position.Move(direction);
                char charAtNewPosition = lines[newPosition.row][newPosition.col];

                if(charAtNewPosition == '#') {
                    direction = TurnRight(direction);
                } else {
                    visited.Add((newPosition.row, newPosition.col, direction));
                    position = newPosition;
                }
            } while(!position.Move(direction).IsOutOfBounds(maxRow, maxCol));

            IEnumerable<(int row, int col)> distinctVisitedFields = visited.Select(x => (x.row, x.col)).Distinct();
            p1_score = distinctVisitedFields.Count();

            foreach((int row, int col) field in distinctVisitedFields) {
                //Reset Run
                position = startPosition;
                visited = [];
                direction = Direction.Up;

                //Do Run
                do {
                    (int row, int col) newPosition = position.Move(direction);
                    char charAtNewPosition = lines[newPosition.row][newPosition.col];

                    if(charAtNewPosition == '#' || newPosition == field) {
                        direction = TurnRight(direction);
                    } else {
                        if(visited.Contains((newPosition.row, newPosition.col, direction))) {
                            p2_score++;
                            break;
                        }

                        visited.Add((newPosition.row, newPosition.col, direction));
                        position = newPosition;
                    }
                } while(!position.Move(direction).IsOutOfBounds(maxRow, maxCol));
            }

            stopwatch.Stop();
            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}\nFinished in {stopwatch.Elapsed}");
        }

        private static Direction TurnRight(Direction direction) {
            return direction switch {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => Direction.Up
            };
        }
    }
}