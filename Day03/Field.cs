using Shared;

namespace Day03 {
    internal class Field {
        private static readonly List<char> possiblePartSymbols = ['#', '/', '*', '-', '+', '&', '$', '=', '@', '%'];
        private static readonly char gearSymbol = '*';

        public char Ch { get; set; }
        public bool IsPartSymbol { get; set; }
        public bool IsGearSymbol { get; set; }
        public bool IsPartOfNumber { get; set; }
        public int? Number { get; set; }
        public (int line, int col) Coordinates { get; set; }
        public (int, int)[] AdjacentCoordinates { get; set; }

        public (int line, int col) Right => new(Coordinates.line, Coordinates.col + 1);
        public (int line, int col) Left => new(Coordinates.line, Coordinates.col - 1);
        public (int line, int col) Up => new(Coordinates.line - 1, Coordinates.col);
        public (int line, int col) Down => new(Coordinates.line + 1, Coordinates.col);
        public (int line, int col) UpRight => new(Coordinates.line - 1, Coordinates.col + 1);
        public (int line, int col) UpLeft => new(Coordinates.line - 1, Coordinates.col - 1);
        public (int line, int col) DownRight => new(Coordinates.line + 1, Coordinates.col + 1);
        public (int line, int col) DownLeft => new(Coordinates.line + 1, Coordinates.col - 1);

        public Field(char ch, (int, int) coordinates) {
            Ch = ch;
            Coordinates = coordinates;
            IsPartSymbol = possiblePartSymbols.Contains(ch);
            IsGearSymbol = ch == gearSymbol;
            IsPartOfNumber = ch.IsNumeric(out int output);
            Number = output;
            AdjacentCoordinates = [Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight];
        }
    }
}
