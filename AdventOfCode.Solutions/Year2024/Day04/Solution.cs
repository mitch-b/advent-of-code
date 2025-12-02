namespace AdventOfCode.Solutions.Year2024.Day04;

class Solution : SolutionBase
{
    private readonly string[][] puzzle;
    public Solution() : base(04, 2024, "")
    {
        puzzle = Input.SplitToMatrix<string>();
    }

    protected override string SolvePartOne()
    {
        var pairs = new List<(int, int)>();
        var foundCount = 0;
        var xLen = puzzle.GetLength(0);
        var yLen = puzzle[0].Length;
        for (var y = 0; y < yLen; y++)
        {
            for (var x = 0; x < xLen; x++)
            {
                if (puzzle[y][x] == "X")
                {
                    var currentPairs = FindXmas("XMAS", x, y, new List<(Func<int, int> xAdder, Func<int, int> yAdder)>
                    {
                        (x => x, y => y - 1), // up
                        (x => x, y => y + 1), // down
                        (x => x - 1, y => y), // left
                        (x => x + 1, y => y), // right
                        (x => x - 1, y => y - 1), // left up
                        (x => x + 1, y => y - 1), // right up
                        (x => x - 1, y => y + 1), // left down
                        (x => x + 1, y => y + 1), // right down
                    });
                    if (currentPairs.Item2 != null && currentPairs.Item2.Any())
                    {
                        foundCount += currentPairs.count;
                        pairs.AddRange(currentPairs.Item2);
                    }
                }
            }
        }
        return foundCount.ToString();
    }

    protected override string SolvePartTwo()
    {
        var pairs = new List<(int, int)>();
        var foundCount = 0;
        var xLen = puzzle.GetLength(0);
        var yLen = puzzle[0].Length;
        for (var y = 0; y < yLen; y++)
        {
            for (var x = 0; x < xLen; x++)
            {
                if (puzzle[y][x] == "A")
                {
                    var oneSide = false;
                    var secondSide = false;
                    var oneSideResult = FindXmas("MAS", x - 1, y - 1, new List<(Func<int, int> xAdder, Func<int, int> yAdder)>
                    {
                        (x => x + 1, y => y + 1), // right down
                    });
                    if (oneSideResult.Item2 != null && oneSideResult.Item2.Any())
                    {
                        oneSide = true;
                    }
                    else
                    {
                        oneSideResult = FindXmas("SAM", x - 1, y - 1, new List<(Func<int, int> xAdder, Func<int, int> yAdder)>
                        {
                            (x => x + 1, y => y + 1), // right down
                        });
                        if (oneSideResult.Item2 != null && oneSideResult.Item2.Any())
                        {
                            oneSide = true;
                        }
                    }
                    var secondSideResult = FindXmas("MAS", x + 1, y - 1, new List<(Func<int, int> xAdder, Func<int, int> yAdder)>
                    {
                        (x => x - 1, y => y + 1), // left down
                    });
                    if (secondSideResult.Item2 != null && secondSideResult.Item2.Any())
                    {
                        secondSide = true;
                    }
                    else
                    {
                        secondSideResult = FindXmas("SAM", x + 1, y - 1, new List<(Func<int, int> xAdder, Func<int, int> yAdder)>
                        {
                            (x => x - 1, y => y + 1), // left down
                        });
                        if (secondSideResult.Item2 != null && secondSideResult.Item2.Any())
                        {
                            secondSide = true;
                        }
                    }
                    if (oneSide && secondSide)
                    {
                        foundCount++;
                    }
                }
            }
        }
        return foundCount.ToString();
    }

    private (int count, List<(int, int)>?) FindXmas(string stringToFind, int i, int j, List<(Func<int, int> xAdder, Func<int, int> yAdder)> pathFunctions)
    {
        var foundCount = 0;
        var pairs = new List<(int, int)>();

        foreach (var pathFunction in pathFunctions)
        {
            var (count, currentPairs) = FindWordInPuzzle(puzzle, stringToFind, i, j, pathFunction.xAdder, pathFunction.yAdder);
            foundCount += count;
            pairs.AddRange(currentPairs);
        }

        return (foundCount, pairs.Distinct().ToList());
    }

    private (int, List<(int x, int y)>) FindWordInPuzzle(string[][] puzzle, string wordMatch, int startX, int startY, Func<int, int> xAdder, Func<int, int> yAdder)
    {
        int xLen = puzzle.GetLength(0);
        int yLen = puzzle[0].Length;
        string currentMatch = string.Empty;
        var matchedPairs = new List<(int x, int y)>();
        int x = startX;
        int y = startY;

        while (x >= 0 && x < xLen && y >= 0 && y < yLen)
        {
            if (puzzle[y][x] == wordMatch[currentMatch.Length].ToString())
            {
                currentMatch += puzzle[y][x];
                matchedPairs.Add((x, y));
                if (currentMatch == wordMatch)
                {
                    return (1, matchedPairs);
                }
            }
            else
            {
                break;
            }
            x = xAdder(x);
            y = yAdder(y);
        }
        return (0, new List<(int, int)>());
    }
}
