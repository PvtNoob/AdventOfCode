namespace Day03 {
    internal class Number(Schematic schematic) {
        public List<Field> Fields = [];
        private Schematic schematic = schematic;

        public Field GetAdjacentGearSymbol() => Fields.Select(schematic.FindAdjacentGearSymbol).FirstOrDefault(field => field != null);

        public int GetNumber() => int.Parse(new string(Fields.OrderBy(field => field.Coordinates.col).Select(field => field.Ch).ToArray()));
    }
}