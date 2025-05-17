namespace Shared {
    public static class NumberExtensions {
        public static bool IsBetween(this long number, long min, long max) {
            return number >= min && number <= max;
        }

        public static int GetMiddleValue(this int[] values) {
            if(values.Length == 0) return -1;
            return values[(int)Math.Floor(new decimal(values.Length) / 2)];
        }

        public static int GetIndexOf(this int[] values, int value) {
            for(int i = 0; i < values.Length; i++) {
                if(values[i] == value) {
                    return i;
                }
            }

            return -1;
        }

        public static int[] Shuffle(this int[] values) {
            Random random = new();
            for(int i = 0; i < values.Length; i++) {
                int randomIndex = random.Next(0, values.Length);
                (values[i], values[randomIndex]) = (values[randomIndex], values[i]);
            }
            return values;
        }

        public static string GetString(this List<int> values) {
            return string.Join(", ", values);
        }
    }
}
