namespace AdventOfCode.Solutions.Year2021.Day11;

class Solution : SolutionBase
{
    public Solution() : base(11, 2021, "") { }

    protected override string? SolvePartOne()
    {
        this.Grid = ParseInput(this.Input);
        var stepCount = 100;
        for (var i = 0; i < stepCount; i++)
        {
            Grid = TriggerStep(Grid);
        }

        return Flashes.ToString();
    }

    protected override string? SolvePartTwo()
    {
        this.Grid = ParseInput(this.Input);
        var i = 0;
        FullTrip = false;
        while (!FullTrip)
        {
            Grid = TriggerStep(Grid);
            i++;
        }
        return $"{i}";
    }

    private int[,] Grid = new int[10, 10];
    private int Flashes = 0;
    private bool FullTrip = false;

    private int[,] ParseInput(string input)
    {
        var rows = input.SplitByNewline();
        var width = rows[0].Length;
        var grid = new int[rows.Length, rows[0].Length];
        for (var i = 0; i < rows.Length; i++)
        {
            for (var j = 0; j < rows[0].Length; j++)
            {
                grid[i, j] = int.Parse(rows[i][j].ToString());
            }
        }
        return grid;
    }
    private int[,] TriggerStep(int[,] grid)
    {
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            for (var x = 0; x < grid.GetLength(1); x++)
            {
                grid[y, x]++;
            }
        }

        for (var y = 0; y < grid.GetLength(0); y++)
        {
            for (var x = 0; x < grid.GetLength(1); x++)
            {
                if (grid[y, x] > 9)
                {
                    grid = TriggerGlow((y, x), grid);
                }
            }
        }

        var stepFlashes = grid.Flatten().Count(n => n == 0);
        if (stepFlashes == 100)
        {
            FullTrip = true;
        }

        Flashes += stepFlashes;
        return grid;
    }
    private int[,] TriggerGlow((int y, int x) coordinate, int[,] grid)
    {
        grid[coordinate.y, coordinate.x] = 0;
        var nearbyPointTransforms = new[]
        {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1), (0, 1),
                (1, -1), (1, 0), (1, 1)
            };
        foreach (var c in nearbyPointTransforms.Select(t => Transform(coordinate, t)))
        {
            if (c == null)
            {
                continue;
            }

            if (grid[c.Value.y, c.Value.x] != 0)
            {
                // hasn't glowed yet
                grid[c.Value.y, c.Value.x]++;
                if (grid[c.Value.y, c.Value.x] > 9)
                {
                    grid = TriggerGlow(c.Value, grid);
                }
            }
        }
        return grid;
    }

    private (int y, int x)? Transform((int y, int x) point, (int y, int x) change) =>
        (point.y + change.y >= 0 && point.x + change.x >= 0) && (point.y + change.y < 10 && point.x + change.x < 10) ?
        (point.y + change.y, point.x + change.x)
        : null;

}
