namespace Day03 {
    internal class Number(Schematic schematic) {
        public List<Field> Fields = [];
        private Schematic schematic = schematic;

        public Field? GetAdjacentGearSymbol() {
            foreach(Field field in Fields) {
                Field? gearField = schematic.FindAdjacentGearSymbol(field);
                if(gearField.HasValue) {
                    return gearField;
                }
            }
            return null;
        }

        public int GetNumber() {
            char[] number = Fields.OrderBy(field => field.Coordinates.col).Select(field => field.Ch).ToArray();
            return int.Parse(new string(number));
        }
    }
}
