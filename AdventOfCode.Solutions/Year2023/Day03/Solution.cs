namespace AdventOfCode.Solutions.Year2023.Day03;

class Solution : SolutionBase
{
    public Solution() : base(03, 2023, "") { }

    protected override string? SolvePartOne()
    {
        var puzzleMatrix = GetPuzzleMatrix();
        var totalSum = 0;
        for (var y = 0; y < puzzleMatrix.Length; y++)
        {
            for (var x = 0; x < puzzleMatrix[y].Length; x++)
            {
                var currentSymbol = puzzleMatrix[y][x];
                if (!char.IsDigit(currentSymbol) && currentSymbol != '.')
                {
                    var neighborCoordinates = GetNeighborCoordinates(puzzleMatrix, x, y);
                    var neighborPartNumberCoordinates = neighborCoordinates
                        .Where(c => char.IsDigit(puzzleMatrix[c.y][c.x]))
                        .Select(c => (c.x, c.y));

                    while (neighborPartNumberCoordinates.Any())
                    {
                        var (partNumber, coordinatesUsed) = GetPartNumberFromCoordinate(puzzleMatrix, neighborPartNumberCoordinates.First());
                        totalSum += partNumber;
                        neighborCoordinates.RemoveAll(coordinatesUsed.Contains);
                    }
                }
            }
        }
        return totalSum.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var puzzleMatrix = GetPuzzleMatrix();
        var totalSum = 0;
        for (var y = 0; y < puzzleMatrix.Length; y++)
        {
            for (var x = 0; x < puzzleMatrix[y].Length; x++)
            {
                var currentSymbol = puzzleMatrix[y][x];
                if (!char.IsDigit(currentSymbol) && currentSymbol != '.')
                {
                    var neighborCoordinates = GetNeighborCoordinates(puzzleMatrix, x, y);
                    var neighborPartNumberCoordinates = neighborCoordinates
                        .Where(c => char.IsDigit(puzzleMatrix[c.y][c.x]))
                        .Select(c => (c.x, c.y))
                        .ToList();
                    neighborPartNumberCoordinates = RemoveDuplicativeDiagonalCoordinates(neighborPartNumberCoordinates, (x, y));
                    if (currentSymbol != '*' || neighborPartNumberCoordinates.Count() != 2)
                    {
                        continue;
                    }

                    var gearMultiplierValue = 1;
                    while (neighborPartNumberCoordinates.Any())
                    {
                        var (partNumber, coordinatesUsed) = GetPartNumberFromCoordinate(puzzleMatrix, neighborPartNumberCoordinates.First());
                        gearMultiplierValue *= partNumber;
                        neighborPartNumberCoordinates.RemoveAll(coordinatesUsed.Contains);
                    }
                    totalSum += gearMultiplierValue;
                }
            }
        }
        return totalSum.ToString();
    }

    private char[][] GetPuzzleMatrix()
    {
        var puzzleRows = Input.SplitByNewline();
        var puzzleMatrix = new char[puzzleRows.Count()][];
        for (var i = 0; i < puzzleRows.Count(); i++)
        {
            puzzleMatrix[i] = puzzleRows[i].Select(x => x).ToArray();
        }
        return puzzleMatrix;
    }

    private List<(int x, int y)> GetNeighborCoordinates(char[][] puzzleMatrix, int x, int y)
    {
        var neighborCoordinates = new List<(int x, int y)>();
        if (x > 0)
        {
            neighborCoordinates.Add((x - 1, y));
        }

        if (x < puzzleMatrix.Length - 1)
        {
            neighborCoordinates.Add((x + 1, y));
        }

        if (y > 0)
        {
            neighborCoordinates.Add((x, y - 1));
        }

        if (y < puzzleMatrix[y].Length - 1)
        {
            neighborCoordinates.Add((x, y + 1));
        }

        if (x > 0 && y > 0)
        {
            neighborCoordinates.Add((x - 1, y - 1));
        }

        if (x < puzzleMatrix.Length - 1 && y < puzzleMatrix[y].Length - 1)
        {
            neighborCoordinates.Add((x + 1, y + 1));
        }

        if (x > 0 && y < puzzleMatrix[y].Length - 1)
        {
            neighborCoordinates.Add((x - 1, y + 1));
        }

        if (x < puzzleMatrix.Length - 1 && y > 0)
        {
            neighborCoordinates.Add((x + 1, y - 1));
        }

        return neighborCoordinates;
    }

    private List<(int x, int y)> RemoveDuplicativeDiagonalCoordinates(List<(int x, int y)> neighborCoordinates, (int x, int y) coordinate)
    {
        if (neighborCoordinates.Contains((coordinate.x, coordinate.y - 1)))
        {
            neighborCoordinates.Remove((coordinate.x - 1, coordinate.y - 1));
            neighborCoordinates.Remove((coordinate.x + 1, coordinate.y - 1));
        }
        if (neighborCoordinates.Contains((coordinate.x, coordinate.y + 1)))
        {
            neighborCoordinates.Remove((coordinate.x - 1, coordinate.y + 1));
            neighborCoordinates.Remove((coordinate.x + 1, coordinate.y + 1));
        }
        return neighborCoordinates;
    }

    private (int, List<(int x, int y)>) GetPartNumberFromCoordinate(char[][] puzzleMatrix, (int x, int y) coordinate)
    {
        var x = coordinate.x;
        var y = coordinate.y;
        var partNumber = puzzleMatrix[y][x].ToString();
        var coordinatesUsed = new List<(int x, int y)> { coordinate };
        if (x > 0 && char.IsDigit(puzzleMatrix[y][x - 1]))
        {
            for (var i = x - 1; i >= 0; i--)
            {
                if (char.IsDigit(puzzleMatrix[y][i]))
                {
                    partNumber = puzzleMatrix[y][i] + partNumber;
                    coordinatesUsed.Add((i, y));
                }
                else
                {
                    break;
                }
            }
        }
        if (x < puzzleMatrix.Length - 1 && char.IsDigit(puzzleMatrix[y][x + 1]))
        {
            for (var i = x + 1; i < puzzleMatrix.Length; i++)
            {
                if (char.IsDigit(puzzleMatrix[y][i]))
                {
                    partNumber += puzzleMatrix[y][i];
                    coordinatesUsed.Add((i, y));
                }
                else
                {
                    break;
                }
            }
        }
        return (int.Parse(partNumber), coordinatesUsed);
    }
}
