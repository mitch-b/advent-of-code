namespace AdventOfCode.Solutions.Year2020.Day03;

class Solution : SolutionBase
{
    private readonly char[][] map;
    private int length;

    public Solution() : base(03, 2020, "")
    {
        length = Input.IndexOf("\n");
        // convert input string to addressable array
        map = [.. Input.SplitByNewline(true).Select(row => row.Select(c => c).ToArray())];
    }

    protected override string? SolvePartOne()
    {
        var trees = TreesHitBySlope((3, 1));
        return trees.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var trees = TreesHitBySlope((1, 1))
            * TreesHitBySlope((3, 1))
            * TreesHitBySlope((5, 1))
            * TreesHitBySlope((7, 1))
            * TreesHitBySlope((1, 2));
        return trees.ToString();
    }

    private long TreesHitBySlope((int x, int y) advanceBySlope)
    {
        var trees = 0;
        (int x, int y) position = (0, 0);
        while (position.y < map.Length)
        {
            trees += (TreeAtPosition(position)) ? 1 : 0;
            position = position.Add(advanceBySlope);
        }
        return trees;
    }

    private bool TreeAtPosition((int x, int y) pos) => map[pos.y][pos.x % length] == '#';
}
