using Shared;

namespace Day03 {
    internal struct Field {
        private static List<char> possiblePartSymbols = ['#', '/', '*', '-', '+', '&', '$', '=', '@', '%'];
        private static char gearSymbol = '*';

        public char Ch;
        public bool IsPartSymbol;
        public bool IsGearSymbol;
        public bool IsPartOfNumber;
        public int? Number;
        public (int line, int col) Coordinates;
        public (int, int)[] AdjacentCoordinates;

        public (int line, int col) Right;
        public (int line, int col) Left;
        public (int line, int col) Up;
        public (int line, int col) Down;
        public (int line, int col) UpRight;
        public (int line, int col) UpLeft;
        public (int line, int col) DownRight;
        public (int line, int col) DownLeft;

        public Field(char ch, (int, int) coordinates) {
            Ch = ch;
            Coordinates = coordinates;
            IsPartSymbol = possiblePartSymbols.Contains(ch);
            IsGearSymbol = ch == gearSymbol;
            IsPartOfNumber = ch.IsNumeric(out int output);
            Number = output;

            Up = new(Coordinates.line - 1, Coordinates.col);
            Down = new(Coordinates.line + 1, Coordinates.col);
            Left = new(Coordinates.line, Coordinates.col - 1);
            Right = new(Coordinates.line, Coordinates.col + 1);
            UpLeft = new(Coordinates.line - 1, Coordinates.col - 1);
            UpRight = new(Coordinates.line - 1, Coordinates.col + 1);
            DownLeft = new(Coordinates.line + 1, Coordinates.col - 1);
            DownRight = new(Coordinates.line + 1, Coordinates.col + 1);

            AdjacentCoordinates = [Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight];
        }
    }
}
