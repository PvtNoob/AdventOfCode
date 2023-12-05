namespace Shared {
    public static class NumberExtensions {
        public static bool IsBetween(this long number, long min, long max) {
            return number >= min && number <= max;
        }
    }
}
