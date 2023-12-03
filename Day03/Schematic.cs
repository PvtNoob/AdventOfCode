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

        public List<Number> GetAllNumbersWithSymbols() {
            List<Field> foundNumberFields = [];
            List<Number> foundNumbers = [];
            Number currentNumber = new(this);
            bool currentNumberHasSymbol = false;
            foreach (Field field in Fields) {
                if (field.IsPartOfNumber && !foundNumberFields.Contains(field)) {
                    Field? analyzingField = field;
                    do {
                        if (HasPartSymbolAdjacent(analyzingField.Value)) {
                            currentNumberHasSymbol = true;
                        }
                        foundNumberFields.Add(analyzingField.Value);
                        currentNumber.Fields.Add(analyzingField.Value);

                        analyzingField = GetField(analyzingField.Value.Right);
                    } while (analyzingField.HasValue && analyzingField.Value.IsPartOfNumber);

                    if (currentNumberHasSymbol) {
                        foundNumbers.Add(currentNumber);
                        currentNumberHasSymbol = false;
                    }
                    currentNumber = new(this);
                }
            }

            return foundNumbers;
        }

        public List<int> GetAllGearRatios(List<Number> numbers) {
            List<int> ratios = [];
            foreach(Field gearField in Fields.Where(field => field.IsGearSymbol)) {

            }

            return ratios;
        }

        private bool HasPartSymbolAdjacent(Field field) {
            return field.AdjacentCoordinates.Any(coord => {
                Field? field = GetField(coord);
                return field.HasValue && field.Value.IsPartSymbol;
            });
        }

        public bool HasGearSymbolAdjacent(Field field) {
            return field.AdjacentCoordinates.Any(coord => {
                Field? field = GetField(coord);
                return field.HasValue && field.Value.IsGearSymbol;
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
