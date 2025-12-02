namespace AdventOfCode.Solutions.Year2021.Day09;

class Solution : SolutionBase
{
    public Solution() : base(09, 2021, "") { }

    protected override string? SolvePartOne()
    {
        var heatmap = ParseHeatmap(this.Input);
        var lowestPointCoordinates = GetLowestPoints(heatmap);
        var riskLevels = lowestPointCoordinates.Select(p => heatmap[p.y, p.x] + 1);
        return riskLevels.Sum().ToString();
    }

    protected override string? SolvePartTwo()
    {
        var heatmap = ParseHeatmap(this.Input);
        var lowestPointCoordinates = GetLowestPoints(heatmap);
        var basins = GetBasins(heatmap, lowestPointCoordinates);
        return basins
            .Select(b => b.Count())
            .OrderByDescending(b => b)
            .Take(3)
            .Aggregate((total, count) => total * count)
            .ToString();
    }

    private int[,] ParseHeatmap(string input)
    {
        var rows = input.SplitByNewline();
        var width = rows[0].Length;
        var heatmap = new int[rows.Length, rows[0].Length];
        for (var i = 0; i < rows.Length; i++)
        {
            for (var j = 0; j < rows[0].Length; j++)
            {
                heatmap[i, j] = int.Parse(rows[i][j].ToString());
            }
        }
        return heatmap;
    }

    private IEnumerable<(int y, int x)> GetLowestPoints(int[,] heatmap)
    {
        var lowestPoints = new List<(int y, int x)>();
        for (var i = 0; i < heatmap.GetLength(0); i++)
        {
            for (var j = 0; j < heatmap.GetLength(1); j++)
            {
                var point = heatmap[i, j];
                var surroundingPoints = new List<int>();

                // up
                if (i > 0)
                {
                    surroundingPoints.Add(heatmap[i - 1, j]);
                }
                // down
                if (i < heatmap.GetLength(0) - 1)
                {
                    surroundingPoints.Add(heatmap[i + 1, j]);
                }
                // left
                if (j > 0)
                {
                    surroundingPoints.Add(heatmap[i, j - 1]);
                }
                // right
                if (j < heatmap.GetLength(1) - 1)
                {
                    surroundingPoints.Add(heatmap[i, j + 1]);
                }

                if (surroundingPoints.Min() <= point)
                {
                    continue;
                }

                lowestPoints.Add((i, j));
            }
        }
        return lowestPoints;
    }

    private List<IEnumerable<int>> GetBasins(int[,] heatmap, IEnumerable<(int y, int x)> lowestPointCoordinates)
    {
        var basins = new List<IEnumerable<int>>();
        foreach (var coordinate in lowestPointCoordinates)
        {
            var traversedCoordinates = new List<(int y, int x)>();
            traversedCoordinates.AddRange(GetNearbyBasinPoints(heatmap, coordinate, traversedCoordinates));
            traversedCoordinates = traversedCoordinates.Distinct().ToList();
            basins.Add(traversedCoordinates.Select(c => heatmap[c.y, c.x]));
        }
        return basins;
    }

    private List<(int y, int x)> GetNearbyBasinPoints(int[,] heatmap, (int y, int x) point, List<(int y, int x)> traversedCoordinates)
    {
        if (!traversedCoordinates.Contains(point))
        {
            traversedCoordinates.Add(point);
        }
        // up
        if (point.y > 0 && heatmap[point.y - 1, point.x] != 9 && !traversedCoordinates.Contains((point.y - 1, point.x)))
        {
            traversedCoordinates.AddRange(GetNearbyBasinPoints(heatmap, (point.y - 1, point.x), traversedCoordinates));
        }
        // down
        if (point.y < heatmap.GetLength(0) - 1 && heatmap[point.y + 1, point.x] != 9 && !traversedCoordinates.Contains((point.y + 1, point.x)))
        {
            traversedCoordinates.AddRange(GetNearbyBasinPoints(heatmap, (point.y + 1, point.x), traversedCoordinates));
        }
        // left
        if (point.x > 0 && heatmap[point.y, point.x - 1] != 9 && !traversedCoordinates.Contains((point.y, point.x - 1)))
        {
            traversedCoordinates.AddRange(GetNearbyBasinPoints(heatmap, (point.y, point.x - 1), traversedCoordinates));
        }
        // right
        if (point.x < heatmap.GetLength(1) - 1 && heatmap[point.y, point.x + 1] != 9 && !traversedCoordinates.Contains((point.y, point.x + 1)))
        {
            traversedCoordinates.AddRange(GetNearbyBasinPoints(heatmap, (point.y, point.x + 1), traversedCoordinates));
        }
        return new List<(int y, int x)>(traversedCoordinates)
            .Distinct().ToList();
    }
}
