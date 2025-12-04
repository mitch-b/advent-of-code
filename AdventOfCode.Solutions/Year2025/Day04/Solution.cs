namespace AdventOfCode.Solutions.Year2025.Day04;

class Solution : SolutionBase
{
    public Solution() : base(04, 2025, "", useDebugInput: false)
    {

    }

    private readonly string _rollMarker = "@";
    private readonly string _forkliftMarker = "x";

    protected override string? SolvePartOne()
    {
        var floor = Input.SplitToMatrix<string>();
        var forkliftManagedMatrix = PerformForkliftRemoval(floor);
        return forkliftManagedMatrix.SelectMany(row => row).Count(x => x == _forkliftMarker).ToString();
    }

    protected override string? SolvePartTwo()
    {
        var floor = Input.SplitToMatrix<string>();
        var lastRemoveCount = 0;
        while (true)
        {
            var forkliftManagedMatrix = PerformForkliftRemoval(floor);
            var currentRemoveCount = forkliftManagedMatrix.SelectMany(row => row).Count(x => x == _forkliftMarker);
            if (currentRemoveCount == lastRemoveCount)
            {
                return currentRemoveCount.ToString();
            }
            lastRemoveCount = currentRemoveCount;
            floor = forkliftManagedMatrix;
        }
    }

    private string[][] PerformForkliftRemoval(string[][] matrix)
    {
        var surroundingRollLimit = 4;

        var floor = matrix;
        var forkliftManagedMatrix = matrix.Select(row => row.ToArray()).ToArray();

        for (int row = 0; row < floor.Count(); row++)
        {
            for (int col = 0; col < floor[row].GetLength(0); col++)
            {
                if (floor.GetXY(col, row) != _rollMarker)
                {
                    continue;
                }
                var currentCoordinate = floor.GetXY(col, row);
                var surroundingCoordinates = floor.GetSurroundingCoordinates(col, row);
                var surroundingRolls = surroundingCoordinates.Count(c => floor.GetXY(c.X, c.Y) == _rollMarker);
                if (surroundingRolls < surroundingRollLimit)
                {
                    forkliftManagedMatrix.SetXY(col, row, _forkliftMarker);
                }
            }
        }
        return forkliftManagedMatrix;
    }
}
