using Shared;

namespace Day10 {
    internal class Program {
        private static string[] map = [];
        private static int rowMaxIndex;
        private static int columnMaxIndex;

        private static char[][] expandMap = [];
        private static int expandedRowMaxIndex;
        private static int expandedColumnMaxIndex;

        private static char[][] debugMap = [];
        private static List<(int row, int column)> visitedPositions = [];

        private static (int, int) None = (-1, -1);

        static void Main(string[] args) {
            if (!ArgsValidator.IsValidArgs(args)) return;

            long p1_score = 0;
            long p2_score = 0;

            map = File.ReadAllLines(args[0]);
            debugMap = map.Select(x => x.ToArray()).ToArray();
            rowMaxIndex = map.Length - 1;
            columnMaxIndex = map[0].Length - 1;

            //Part1
            (int row, int col) startPosition = FindStartCoordinate();
            ReplaceStartWithPipe(startPosition);

            (int row, int col) lastPosition = None;
            (int row, int col) currentPosition = startPosition;
            visitedPositions.Add(currentPosition);

            for (p1_score = 0; currentPosition != startPosition || lastPosition == None; p1_score++) {
                (int row, int col) newPosition = FindNextPipeConnection(currentPosition, lastPosition, None);

                if (newPosition == None) {
                    Console.WriteLine("Could not find next pipe connection!");
                    PrintDebugMap();
                    return;
                }

                visitedPositions.Add(newPosition);
                lastPosition = currentPosition;
                currentPosition = newPosition;
            }
            p1_score /= 2;

            //Part2
            ExpandMap();
            Thread thread = new(new ThreadStart(FloodStart), int.MaxValue);
            thread.Start();
            while (thread.IsAlive) { }
            p2_score = CountInnerFields();

            Console.WriteLine($"Part1 Result: {p1_score}\nPart2 Result: {p2_score}");
        }

        private static void FloodStart() {
            Flood((0, 0));
        }

        private static (int, int) FindStartCoordinate() {
            for (int row = 0; row <= rowMaxIndex; row++) {
                for (int col = 0; col <= columnMaxIndex; col++) {
                    if (map[row][col] == 'S') {
                        return (row, col);
                    }
                }
            }
            Console.WriteLine("Could not found start position!");
            return None;
        }

        private static void ReplaceStartWithPipe((int row, int col) startPosition) {
            (int, int) firstPipe = FindNextPipeConnection(startPosition, None, None);
            (int, int) secondPipe = FindNextPipeConnection(startPosition, None, firstPipe);

            (int, int) north = GoNorth(startPosition);
            (int, int) east = GoEast(startPosition);
            (int, int) south = GoSouth(startPosition);
            (int, int) west = GoWest(startPosition);

            char newStart = 'S';

            if ((firstPipe == north || secondPipe == north) && (firstPipe == east || secondPipe == east)) {
                newStart = 'L';
            } else if ((firstPipe == north || secondPipe == north) && (firstPipe == south || secondPipe == south)) {
                newStart = '|';
            } else if ((firstPipe == north || secondPipe == north) && (firstPipe == west || secondPipe == west)) {
                newStart = 'J';
            } else if ((firstPipe == east || secondPipe == east) && (firstPipe == south || secondPipe == south)) {
                newStart = 'F';
            } else if ((firstPipe == east || secondPipe == east) && (firstPipe == west || secondPipe == west)) {
                newStart = '-';
            } else if ((firstPipe == south || secondPipe == south) && (firstPipe == west || secondPipe == west)) {
                newStart = '7';
            }

            map[startPosition.row] = map[startPosition.row].Replace('S', newStart);
            debugMap[startPosition.row][startPosition.col] = newStart;
        }

        private static (int, int) FindNextPipeConnection((int row, int col) currentPosition, (int row, int col) lastPosition, (int row, int col) ignorePosition) {
            char currentPipe = Get(currentPosition);

            if (ConnectsToNorth(currentPipe) && !IsAtNorthEnd(currentPosition)) {
                (int, int) northPosition = GoNorth(currentPosition);
                if (northPosition != lastPosition && northPosition != ignorePosition && ConnectsToSouth(Get(northPosition))) {
                    return northPosition;
                }
            }
            if (ConnectsToEast(currentPipe) && !IsAtEastEnd(currentPosition)) {
                (int, int) eastPosition = GoEast(currentPosition);
                if (eastPosition != lastPosition && eastPosition != ignorePosition && ConnectsToWest(Get(eastPosition))) {
                    return eastPosition;
                }
            }
            if (ConnectsToSouth(currentPipe) && !IsAtSouthEnd(currentPosition)) {
                (int, int) southPosition = GoSouth(currentPosition);
                if (southPosition != lastPosition && southPosition != ignorePosition && ConnectsToNorth(Get(southPosition))) {
                    return southPosition;
                }
            }
            if (ConnectsToWest(currentPipe) && !IsAtWestEnd(currentPosition)) {
                (int, int) westPosition = GoWest(currentPosition);
                if (westPosition != lastPosition && westPosition != ignorePosition && ConnectsToEast(Get(westPosition))) {
                    return westPosition;
                }
            }
            return None;
        }

        private static void ExpandMap() {
            expandedRowMaxIndex = (rowMaxIndex * 3) + 2;
            expandedColumnMaxIndex = (columnMaxIndex * 3) + 2;
            expandMap = new char[map.Length * 3][];
            for (int i = 0; i < expandMap.Length; i++) {
                expandMap[i] = new char[map[0].Length * 3];
            }

            for (int row = 0; row <= rowMaxIndex; row++) {
                for (int col = 0; col <= columnMaxIndex; col++) {
                    string[] newTile;
                    if (visitedPositions.Contains((row, col))) {
                        newTile = GetExpandedVersion(Get((row, col)));
                    } else {
                        newTile = GetNoPipe();
                    }

                    expandMap[row * 3][col * 3] = newTile[0][0];
                    expandMap[row * 3][(col * 3) + 1] = newTile[0][1];
                    expandMap[row * 3][(col * 3) + 2] = newTile[0][2];
                    expandMap[(row * 3) + 1][col * 3] = newTile[1][0];
                    expandMap[(row * 3) + 1][(col * 3) + 1] = newTile[1][1];
                    expandMap[(row * 3) + 1][(col * 3) + 2] = newTile[1][2];
                    expandMap[(row * 3) + 2][col * 3] = newTile[2][0];
                    expandMap[(row * 3) + 2][(col * 3) + 1] = newTile[2][1];
                    expandMap[(row * 3) + 2][(col * 3) + 2] = newTile[2][2];
                }
            }
        }

        private static string[] GetExpandedVersion(char ch) => ch switch {
            '|' => GetNorthSouthPipe(),
            '-' => GetEastWestPipe(),
            'L' => GetNorthEastPipe(),
            'J' => GetNorthWestPipe(),
            '7' => GetSouthWestPipe(),
            'F' => GetEastSouthPipe(),
            _ => GetNoPipe()
        };

        private static string[] GetNorthSouthPipe() => [
            "X|X",
            "X|X",
            "X|X"];

        private static string[] GetEastWestPipe() => [
            "XXX",
            "---",
            "XXX"];

        private static string[] GetNorthEastPipe() => [
            "X|X",
            "XL-",
            "XXX"];

        private static string[] GetNorthWestPipe() => [
            "X|X",
            "-JX",
            "XXX"];

        private static string[] GetEastSouthPipe() => [
            "XXX",
            "XF-",
            "X|X"];

        private static string[] GetSouthWestPipe() => [
            "XXX",
            "-7X",
            "X|X"];

        private static string[] GetNoPipe() => [
            "XXX",
            "XXX",
            "XXX"];

        private static void Flood((int row, int col) position) {
            char ch = GetExpanded(position);
            if (ch == 'O' || IsPipe(ch)) {
                return;
            } else {
                expandMap[position.row][position.col] = 'O';
            }

            if (!IsAtNorthEnd(position)) {
                (int, int) north = GoNorth(position);
                Flood(north);
            }

            if (!IsAtEastEnd(position, expanded: true)) {
                (int, int) east = GoEast(position);
                Flood(east);
            }

            if (!IsAtSouthEnd(position, expanded: true)) {
                (int, int) south = GoSouth(position);
                Flood(south);
            }

            if (!IsAtWestEnd(position)) {
                (int, int) west = GoWest(position);
                Flood(west);
            }
        }

        private static bool IsPipe(char ch) {
            return ch is '|' or 'L' or 'J' or '-' or 'F' or '7';
        }

        private static long CountInnerFields() {
            long count = 0;
            for (int row = 1; row <= expandedRowMaxIndex; row += 3) {
                for (int col = 1; col <= expandedColumnMaxIndex; col += 3) {
                    if (GetExpanded((row, col)) == 'X') {
                        count++;
                    }
                }
            }
            return count;
        }

        private static char Get((int row, int col) position) => map[position.row][position.col];
        private static char GetDebug((int row, int col) position) => debugMap[position.row][position.col];
        private static char GetExpanded((int row, int col) position) => expandMap[position.row][position.col];

        private static bool ConnectsToNorth(char ch) => ch is '|' or 'L' or 'J' or 'S';
        private static bool ConnectsToEast(char ch) => ch is '-' or 'L' or 'F' or 'S';
        private static bool ConnectsToSouth(char ch) => ch is '|' or '7' or 'F' or 'S';
        private static bool ConnectsToWest(char ch) => ch is '-' or '7' or 'J' or 'S';

        private static bool IsAtNorthEnd((int row, int col) position) => position.row == 0;
        private static bool IsAtEastEnd((int row, int col) position, bool expanded = false) => position.col == (expanded ? expandedColumnMaxIndex : columnMaxIndex);
        private static bool IsAtSouthEnd((int row, int col) position, bool expanded = false) => position.row == (expanded ? expandedRowMaxIndex : rowMaxIndex);
        private static bool IsAtWestEnd((int row, int col) position) => position.col == 0;

        private static (int, int) GoNorth((int row, int col) position) => (position.row - 1, position.col);
        private static (int, int) GoEast((int row, int col) position) => (position.row, position.col + 1);
        private static (int, int) GoSouth((int row, int col) position) => (position.row + 1, position.col);
        private static (int, int) GoWest((int row, int col) position) => (position.row, position.col - 1);

        private static void PrintDebugMap() {
            for (int row = 0; row <= rowMaxIndex; row++) {
                for (int col = 0; col <= columnMaxIndex; col++) {
                    (int, int) position = (row, col);
                    char ch = GetDebug(position);
                    if (visitedPositions.Contains(position)) {
                        ch = ReplaceDebugSymbols(ch);
                    } else {
                        ch = 'X';
                    }
                    Console.Write(ch); ;
                }
                Console.Write('\n');
            }
        }

        private static void PrintExpandedMap() {
            foreach (char[] line in expandMap) {
                line.ToList().ForEach(ch => Console.Write(ReplaceDebugSymbols(ch)));
                Console.Write('\n');
            }
        }

        private static char ReplaceDebugSymbols(char ch) => ch switch {
            '|' => '│',
            '-' => '─',
            'L' => '└',
            'J' => '┘',
            '7' => '┐',
            'F' => '┌',
            'S' => 'S',
            'X' => 'X',
            'O' => 'O',
            _ => '?'
        };
    }
}