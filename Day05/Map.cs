using Shared;

namespace Day05 {
    internal class Map {
        private List<(long destinationStart, long sourceStart, long length)> Ranges = [];

        public void AddRange(string line) {
            string[] parts = line.Split(' ');
            Ranges.Add(new(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2])));
        }

        public long MapSourceToDestination(long source) {
            foreach((long destinationStart, long sourceStart, long length) in Ranges) {
                if(source.IsBetween(sourceStart, sourceStart + length)){
                    long modifier = destinationStart - sourceStart;
                    return source + modifier;
                }
            }
            return source;
        }
    }
}
