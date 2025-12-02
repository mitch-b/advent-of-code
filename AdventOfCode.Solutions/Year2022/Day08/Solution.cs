namespace AdventOfCode.Solutions.Year2022.Day08;

class Solution : SolutionBase
{
    public Solution() : base(08, 2022, "")
    {
        SetupForest();
    }

    private int _forestSize = 0;
    private int[,] _forest = default!;
    private HashSet<(int, int)> _visiblePairs = new HashSet<(int, int)>();

    protected override string? SolvePartOne()
    {
        // left/right
        for (var i = 1; i < _forestSize - 1; i++) // row (skipping top & bottom exterior rows)
        {
            // left
            var leftVisibleTrees = new List<Tree>() { new Tree((i, 0), _forest[i, 0]) };
            for (var j = 1; j < _forestSize - 1; j++) // column (skipping left & right exterior columns)
            {
                if (_forest[i, j] <= leftVisibleTrees.Max(t => t.Size))
                {
                    break;
                }

                leftVisibleTrees.Add(new Tree((i, j), _forest[i, j]));
            }
            if (leftVisibleTrees.Count != 0)
            {
                _visiblePairs.Add(leftVisibleTrees.Last().Position);
            }
            // right
            var rightVisibleTrees = new List<Tree>() { new Tree((i, (_forestSize - 1)), _forest[i, (_forestSize - 1)]) };
            for (var j = _forestSize - 1; j > 0; j--)
            {
                if (_forest[i, j] <= rightVisibleTrees.Max(t => t.Size))
                {
                    break;
                }

                rightVisibleTrees.Add(new Tree((i, j), _forest[i, j]));
            }
            if (rightVisibleTrees.Count != 0)
            {
                _visiblePairs.Add(rightVisibleTrees.Last().Position);
            }
        }

        // top/bottom
        for (var j = 1; j < _forestSize - 1; j++) // column (skipping left & right exterior columns)
        {
            // top
            var topVisibleTrees = new List<Tree>() { new Tree((0, j), _forest[0, j]) };
            for (var i = 1; i < _forestSize - 1; i++) // row (skipping top & bottom exterior rows)
            {
                if (_forest[i, j] <= topVisibleTrees.Max(t => t.Size))
                {
                    break;
                }

                topVisibleTrees.Add(new Tree((i, j), _forest[i, j]));
            }
            if (topVisibleTrees.Count != 0)
            {
                _visiblePairs.Add(topVisibleTrees.Last().Position);
            }
            // bottom
            var bottomVisibleTrees = new List<Tree>() { new Tree(((_forestSize - 1), j), _forest[(_forestSize - 1), j]) };
            for (var i = _forestSize - 1; i > 0; i--)
            {
                if (_forest[i, j] <= bottomVisibleTrees.Max(t => t.Size))
                {
                    break;
                }

                bottomVisibleTrees.Add(new Tree((i, j), _forest[i, j]));
            }
            if (bottomVisibleTrees.Count != 0)
            {
                _visiblePairs.Add(bottomVisibleTrees.Last().Position);
            }
        }
        return (_visiblePairs.Count).ToString();
    }

    protected override string? SolvePartTwo()
    {
        return "";
    }

    private void SetupForest()
    {
        _visiblePairs = new HashSet<(int, int)>();
        var forest = Input.SplitByNewline();
        _forestSize = forest[0].Length;
        _forest = new int[_forestSize, _forestSize];
        _visiblePairs.Clear();
        for (var i = 0; i < _forestSize; i++)
        {
            for (var j = 0; j < _forestSize; j++)
            {
                if ((i == 0 || i == _forestSize - 1) || (j == 0 || j == _forestSize - 1))
                {
                    _visiblePairs.Add((i, j));
                }
                _forest[i, j] = int.Parse(forest[i][j].ToString());
            }
        }
    }
}

record Tree((int, int) Position, int Size);