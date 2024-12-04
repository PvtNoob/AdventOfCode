namespace Shared {
    public static class TupleExtensions {
        private static (int rowMod, int colMod)[] _directions = [(-1, 0), (0, 1), (1, 0), (0, -1), (-1, 1), (-1, -1), (1, 1), (1, -1)];

        public static (int, int) Add(this (int, int) a, (int, int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2);
        public static (int, int) Move(this (int, int) position, Direction direction) => position.Add(_directions[(int)direction]);
        public static (int, int) Up(this (int, int) position) => position.Move(Direction.Up);
        public static (int, int) Right(this (int, int) position) => position.Move(Direction.Right);
        public static (int, int) Down(this (int, int) position) => position.Move(Direction.Down);
        public static (int, int) Left(this (int, int) position) => position.Move(Direction.Left);
        public static bool IsOutOfBounds(this (int row, int col) position, int maxRow, int maxCol) {
            return position.row < 0 || position.row > maxRow || position.col < 0 || position.col > maxCol;
        }
    }
}
