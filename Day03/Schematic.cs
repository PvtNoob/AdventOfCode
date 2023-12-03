namespace Day03 {
    internal class Schematic {
        private List<Field> Fields;
        private int NumberOfLines;
        private int NumberOfColumns;

        public Schematic(string[] lines) {
            NumberOfLines = lines.Length;
            NumberOfColumns = lines.First().Length;
            Fields = [];

            for (int lineNo = 0; lineNo < NumberOfLines; lineNo++) {
                for (int colNo = 0; colNo < NumberOfColumns; colNo++) {
                    char ch = lines[lineNo][colNo];
                    if(ch != '.') {
                        Fields.Add(new Field(ch, new(lineNo, colNo)));
                    }
                }
            }
        }

        public List<int> GetAllNumbersWithSymbols() {
            List<Field> foundNumberFields = [];
            List<int> foundNumbers = [];
            string currentNumber = string.Empty;
            bool currentNumberHasSymbol = false;
            foreach (Field field in Fields) {
                if (field.IsPartOfNumber && !foundNumberFields.Contains(field)) {
                    Field? analyzingField = field;
                    do {
                        if (HasPartSymbolAdjacent(analyzingField.Value)) {
                            currentNumberHasSymbol = true;
                        }
                        foundNumberFields.Add(analyzingField.Value);
                        currentNumber += analyzingField.Value.Ch;

                        analyzingField = GetField(analyzingField.Value.Right);
                    } while (analyzingField.HasValue && analyzingField.Value.IsPartOfNumber);

                    if (currentNumberHasSymbol) {
                        foundNumbers.Add(int.Parse(currentNumber));
                        currentNumberHasSymbol = false;
                    }
                    currentNumber = string.Empty;
                }
            }

            return foundNumbers;
        }

        private bool HasPartSymbolAdjacent(Field field) {
            return field.AdjacentCoordinates.Any(coord => {
                Field? field = GetField(coord);
                return field.HasValue && field.Value.IsPartSymbol;
            });
        }

        private Field? GetField((int line, int col) coordinate) {
            IEnumerable<Field> found = Fields.Where(field => field.Coordinates == coordinate);
            if (found.Any()) {
                return found.First();
            } else {
                return null;
            }
        }
    }
}
