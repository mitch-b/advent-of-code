namespace AdventOfCode.Solutions.Year2025.Day07;

class Solution : SolutionBase
{
    public Solution() : base(07, 2025, "", useDebugInput: false) { }

    private readonly string _startMarker = "S";
    private readonly string _beamMarker = "|";
    private readonly string _splitterMarker = "^";
    private readonly string _emptySpaceMarker = ".";

    protected override string? SolvePartOne()
    {
        var manifold = Run(Input.SplitToMatrix<string>());
        PrintMap(manifold.Map);
        return manifold.SplittersHit.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var manifold = Run(Input.SplitToMatrix<string>());
        var startPosition = manifold.Map[0].IndexOf(_startMarker);
        manifold.Map.SetXY(startPosition, 1, _beamMarker);
        return CountPaths(manifold.Map, startPosition, 0).ToString();
    }

    private TachyonManifold Run(string[][] map)
    {
        var newMap = map.Select(row => row.ToArray()).ToArray();

        var startPosition = newMap[0].IndexOf(_startMarker);
        newMap.SetXY(startPosition, 1, _beamMarker);

        var splittersHit = 0;

        for (var rowIndex = 1; rowIndex < newMap.Length; rowIndex++)
        {
            var row = newMap[rowIndex];
            if (rowIndex + 1 == newMap.Length)
            {
                break;
            }

            for (var colIndex = 0; colIndex < row.Length; colIndex++)
            {
                if (newMap.GetXY(colIndex, rowIndex) != _beamMarker)
                {
                    continue;
                }
                if (newMap.GetXY(colIndex, rowIndex + 1) == _splitterMarker)
                {
                    splittersHit++;
                    newMap.SetXY(colIndex - 1, rowIndex + 1, _beamMarker);
                    newMap.SetXY(colIndex + 1, rowIndex + 1, _beamMarker);
                }
                else if (newMap.GetXY(colIndex, rowIndex + 1) == _emptySpaceMarker)
                {
                    newMap.SetXY(colIndex, rowIndex + 1, _beamMarker);
                }
            }
        }

        return new TachyonManifold(splittersHit, newMap);
    }

    private long CountPaths(string[][] map, int startX, int startY)
    {
        var activeBeams = new Dictionary<int, long>
        {
            [startX] = 1
        };

        for (var currentY = startY; currentY < map.Length - 1; currentY++)
        {
            var nextY = currentY + 1;
            var rowLength = map[nextY].Length;
            var nextRow = new Dictionary<int, long>();

            foreach (var beam in activeBeams)
            {
                var x = beam.Key;
                var count = beam.Value;

                if (x < 0 || x >= rowLength)
                {
                    continue;
                }

                var nextCell = map.GetXY(x, nextY);

                if (nextCell == _splitterMarker)
                {
                    AddBeam(nextRow, x - 1, count, rowLength);
                    AddBeam(nextRow, x + 1, count, rowLength);
                }
                else
                {
                    AddBeam(nextRow, x, count, rowLength);
                }
            }

            activeBeams = nextRow;
        }

        return activeBeams.Values.Sum();
    }

    private void AddBeam(Dictionary<int, long> row, int x, long value, int rowLength)
    {
        if (x < 0 || x >= rowLength)
        {
            return;
        }

        if (row.TryGetValue(x, out var existing))
        {
            row[x] = existing + value;
        }
        else
        {
            row[x] = value;
        }
    }

    private void PrintMap(string[][] map)
    {
        for (var rowIndex = 0; rowIndex < map.Length; rowIndex++)
        {
            var row = map[rowIndex];
            for (var colIndex = 0; colIndex < row.Length; colIndex++)
            {
                Console.Write(map.GetXY(colIndex, rowIndex));
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

record TachyonManifold(int SplittersHit, string[][] Map);