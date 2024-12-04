using System.Diagnostics;
using Shared;

namespace Day04 {
    internal class Day04 {
        private static string[] lines = null!;
        private static int maxRow, maxCol;

        static void Main(string[] args) {
            if(!ArgsValidator.IsValidArgs(args)) return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int p1_score = 0;
            int p2_score = 0;

            lines = File.ReadAllLines(args[0]);
            maxCol = lines[0].Length - 1;
            maxRow = lines.Length - 1;

            for(int row = 0; row <= maxRow; row++) {
                for(int col = 0; col <= maxCol; col++) {
                    if(lines[row][col] == 'X') {
                        p1_score += SearchXMAS(new(row, col));
                    } else if(lines[row][col] == 'A' && CheckIfX_MAS(new(row, col))) {
                        p2_score++;
                    }
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}\nFinished in {stopwatch.Elapsed}");
        }

        private static int SearchXMAS((int, int) startPosition) {
            int xmasCount = 0;
            if(CheckIfXMAS(startPosition, Direction.Up)) xmasCount++;
            if(CheckIfXMAS(startPosition, Direction.Right)) xmasCount++;
            if(CheckIfXMAS(startPosition, Direction.Down)) xmasCount++;
            if(CheckIfXMAS(startPosition, Direction.Left)) xmasCount++;
            if(CheckIfXMAS(startPosition, Direction.UpRight)) xmasCount++;
            if(CheckIfXMAS(startPosition, Direction.UpLeft)) xmasCount++;
            if(CheckIfXMAS(startPosition, Direction.DownRight)) xmasCount++;
            if(CheckIfXMAS(startPosition, Direction.DownLeft)) xmasCount++;
            return xmasCount;
        }

        private static bool CheckIfXMAS((int, int) startPosition, Direction direction) {
            (int row, int col) position = startPosition.Move(direction);
            if(position.IsOutOfBounds(maxRow, maxCol) || lines[position.row][position.col] != 'M') {
                return false;
            }
            position = position.Move(direction);
            if(position.IsOutOfBounds(maxRow, maxCol) || lines[position.row][position.col] != 'A') {
                return false;
            }
            position = position.Move(direction);
            if(position.IsOutOfBounds(maxRow, maxCol) || lines[position.row][position.col] != 'S') {
                return false;
            }
            return true;
        }

        private static bool CheckIfX_MAS((int, int) startPosition) {
            (int row, int col) upRight = startPosition.Move(Direction.UpRight);
            (int row, int col) upLeft = startPosition.Move(Direction.UpLeft);
            (int row, int col) downRight = startPosition.Move(Direction.DownRight);
            (int row, int col) downLeft = startPosition.Move(Direction.DownLeft);

            if(upRight.IsOutOfBounds(maxRow, maxCol) || upLeft.IsOutOfBounds(maxRow, maxCol) || downRight.IsOutOfBounds(maxRow, maxCol) || downLeft.IsOutOfBounds(maxRow, maxCol)) {
                return false;
            }

            bool upRightToDownLeft = lines[upRight.row][upRight.col] == 'M' && lines[downLeft.row][downLeft.col] == 'S';
            bool downLeftToUpRight = lines[upRight.row][upRight.col] == 'S' && lines[downLeft.row][downLeft.col] == 'M';
            bool upLeftToDownRight = lines[upLeft.row][upLeft.col] == 'M' && lines[downRight.row][downRight.col] == 'S';
            bool downRightToUpLeft = lines[upLeft.row][upLeft.col] == 'S' && lines[downRight.row][downRight.col] == 'M';

            return (upRightToDownLeft || downLeftToUpRight) && (upLeftToDownRight || downRightToUpLeft);
        }
    }
}