using AdventOfCode.Solutions.Models;

namespace AdventOfCode.Solutions.Year2025.Day09;

class Solution : SolutionBase
{
    public Solution() : base(09, 2025, "", useDebugInput: false) { }

    protected override string? SolvePartOne()
    {
        var grid = Input.SplitByNewline()
            .Select(line =>
            {
                var parts = line.Split(',').Select(decimal.Parse).ToArray();
                return new Coordinate<decimal>(parts[0], parts[1]);
            })
            .ToArray();

        var areas = new Dictionary<(Coordinate<decimal>, Coordinate<decimal>), decimal>();
        foreach (var c1 in grid)
        {
            foreach (var c2 in grid)
            {
                var area = Math.Abs((c1.X - c2.X + 1) * (c1.Y - c2.Y + 1));
                areas[(c1, c2)] = area;
            }
        }
        return areas.Max(a => a.Value).ToString();
    }

    protected override string? SolvePartTwo()
    {
        return null;
    }
}
