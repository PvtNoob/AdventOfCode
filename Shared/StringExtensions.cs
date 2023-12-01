namespace Shared {
    public static class StringExtensions {
        public static bool IsNumeric(this string str, out int output) {
            return int.TryParse(str, out output);
        }

        public static bool IsNumeric(this char ch, out int output) {
            return int.TryParse(ch.ToString(), out output);
        }
    }
}
