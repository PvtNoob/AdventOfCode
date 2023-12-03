namespace Day03 {
    internal class Number(Schematic schematic) {
        public List<Field> Fields = [];
        private Schematic schematic = schematic;

        public Field? GetAdjacentGearSymbol() {
            Field[] gearSymbols = Fields.Where(schematic.HasGearSymbolAdjacent).ToArray();
            return gearSymbols.Length != 0 ? gearSymbols.First() : null;
        }

        public int GetNumber() {
            char[] number = Fields.OrderBy(field => field.Coordinates.col).Select(field => field.Ch).ToArray();
            return int.Parse(new string(number));
        }
    }
}
