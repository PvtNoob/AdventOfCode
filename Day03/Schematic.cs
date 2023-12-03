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
                    Field analyzingField = field;
                    do {
                        if (HasPartSymbolAdjacent(analyzingField)) {
                            currentNumberHasSymbol = true;
                        }
                        foundNumberFields.Add(analyzingField);
                        currentNumber.Fields.Add(analyzingField);

                        analyzingField = GetField(analyzingField.Right);
                    } while (analyzingField != null && analyzingField.IsPartOfNumber);

                    if (currentNumberHasSymbol) {
                        foundNumbers.Add(currentNumber);
                        currentNumberHasSymbol = false;
                    }
                    currentNumber = new(this);
                }
            }

            return foundNumbers;
        }

        public List<int> GetAllGearRatios(List<Number> allNumbers) {
            List<int> ratios = [];
            Dictionary<Field, List<Number>> gearsWithNumbers = [];
            Fields.Where(field => field.IsGearSymbol).ToList().ForEach(gearField => gearsWithNumbers.Add(gearField, []));

            foreach(Number number in allNumbers) {
                Field gearSymbol = number.GetAdjacentGearSymbol();
                if(gearSymbol != null) {
                    gearsWithNumbers[gearSymbol].Add(number);
                }
            }

            foreach (List<Number> numbersOfGear in gearsWithNumbers.Values) {
                if(numbersOfGear.Count >= 2) {
                    int gearRatio = 0;
                    numbersOfGear.ForEach(number => {
                        if (gearRatio == 0) {
                            gearRatio = number.GetNumber();
                        } else {
                            gearRatio *= number.GetNumber();
                        }
                    });
                    ratios.Add(gearRatio);
                }
            }

            return ratios;
        }

        private bool HasPartSymbolAdjacent(Field field) {
            return field.AdjacentCoordinates.Any(coord => {
                Field field = GetField(coord);
                return field != null && field.IsPartSymbol;
            });
        }

        public Field FindAdjacentGearSymbol(Field field) {
            foreach((int, int) coord in field.AdjacentCoordinates) {
                Field adjacentField = GetField(coord);
                if(adjacentField != null && adjacentField.IsGearSymbol) {
                    return adjacentField;
                }
            }
            return null;
        }

        private Field GetField((int line, int col) coordinate) => Fields.FirstOrDefault(field => field.Coordinates == coordinate);
    }
}
