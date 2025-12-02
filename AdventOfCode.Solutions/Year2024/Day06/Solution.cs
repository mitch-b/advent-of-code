namespace AdventOfCode.Solutions.Year2024.Day06;

class Solution : SolutionBase
{
    private readonly char[][] puzzle;
    private readonly char[][] traversedPuzzle;
    private char start = '^';
    private char traversedMarker = 'X';
    public Solution() : base(06, 2024, "")
    {
        puzzle = Input.SplitToMatrix<char>();
        traversedPuzzle = Input.SplitToMatrix<char>();
    }

    protected override string? SolvePartOne()
    {
        char[] obstacles = ['#'];
        (int x, int y) startCoordinate = GetStart(this.puzzle);
        (int x, int y) currentCoordinate = startCoordinate;
        (int x, int y) previousCoordinate = startCoordinate;

        var direction = Direction.Up;
        (Func<int, int> xAdder, Func<int, int> yAdder) = GetDirectionAdders(direction);

        while (InBounds(this.puzzle, currentCoordinate.x, currentCoordinate.y))
        {
            if (obstacles.Contains(puzzle.GetXY(currentCoordinate.x, currentCoordinate.y)))
            {
                direction = TurnRight(direction);
                (xAdder, yAdder) = GetDirectionAdders(direction);
                currentCoordinate = previousCoordinate;
            }
            traversedPuzzle[currentCoordinate.y][currentCoordinate.x] = traversedMarker;
            previousCoordinate = currentCoordinate;
            currentCoordinate = (xAdder(currentCoordinate.x), yAdder(currentCoordinate.y));
        }

        if (Debug)
        {
            foreach (var line in traversedPuzzle)
            {
                Console.WriteLine(string.Join("", line));
            }
        }

        return string.Join("", traversedPuzzle.SelectMany(p => string.Join("", p))).Count(x => x == traversedMarker).ToString();
    }

    protected override string? SolvePartTwo()
    {
        // to solve Part 2, at each step, I first check and pretend i'm about to hit an obstacle and need to turn right.
        // if I turned right and ran into an obstacle i've already hit, I know that's a spot to put a marker.
        // if I turned right and ran into an obstacle which also is on a path i've traversed before, I know that's a spot to put a marker.
        // otherwise, it's not a good spot to put a marker

        char[] obstacles = ['#'];
        List<(int x, int y)> obstaclesHit = new();
        List<(int x, int y)> couldPlaceObstacleForLoop = new();
        (int x, int y) startCoordinate = GetStart(this.puzzle);
        (int x, int y) currentCoordinate = startCoordinate;
        (int x, int y) previousCoordinate = startCoordinate;

        var direction = Direction.Up;
        (Func<int, int> xAdder, Func<int, int> yAdder) = GetDirectionAdders(direction);

        while (InBounds(this.puzzle, currentCoordinate.x, currentCoordinate.y))
        {
            if (currentCoordinate != startCoordinate)
            {
                // check if we can place an obstacle here
                var (x, y) = currentCoordinate;
                var (prevX, prevY) = previousCoordinate;

                var pretendDirection = TurnRight(direction);
                var (deltaX, deltaY) = GetDirectionAdders(pretendDirection);
                while (InBounds(this.puzzle, x, y))
                {
                    if (Debug)
                    {
                        WriteCurrentGraphToConsole(currentCoordinate, pretendDirection);
                    }

                    if (obstaclesHit.Contains((x, y)) && traversedPuzzle.GetXY(prevX, prevY) == traversedMarker)
                    {
                        couldPlaceObstacleForLoop.Add(currentCoordinate);
                        break;
                    }
                    if (obstacles.Contains(puzzle.GetXY(x, y)))
                    {
                        break;
                    }
                    (x, y) = (deltaX(x), deltaY(y));
                }
            }
            if (obstacles.Contains(puzzle.GetXY(currentCoordinate.x, currentCoordinate.y)))
            {
                obstaclesHit.Add(currentCoordinate);
                direction = TurnRight(direction);
                (xAdder, yAdder) = GetDirectionAdders(direction);
                currentCoordinate = previousCoordinate;
            }
            traversedPuzzle[currentCoordinate.y][currentCoordinate.x] = traversedMarker;
            previousCoordinate = currentCoordinate;
            currentCoordinate = (xAdder(currentCoordinate.x), yAdder(currentCoordinate.y));
        }

        if (Debug)
        {
            foreach (var line in traversedPuzzle)
            {
                Console.WriteLine(string.Join("", line));
            }
        }

        return couldPlaceObstacleForLoop.Distinct().Count().ToString();
    }

    private (int x, int y) GetStart(char[][] puzzle)
    {
        var startRow = puzzle.First(row => row.Contains(start));
        return (startRow.ToList().IndexOf(start), Array.IndexOf(puzzle, startRow));
    }

    private Direction TurnRight(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            Direction.Right => Direction.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private char GetDirectionMarker(Direction direction)
    {
        return direction switch
        {
            Direction.Up => '^',
            Direction.Down => 'v',
            Direction.Left => '<',
            Direction.Right => '>',
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private (Func<int, int> xAdder, Func<int, int> yAdder) GetDirectionAdders(Direction direction)
    {
        return direction switch
        {
            Direction.Up => (x => x, y => y - 1),
            Direction.Down => (x => x, y => y + 1),
            Direction.Left => (x => x - 1, y => y),
            Direction.Right => (x => x + 1, y => y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private bool InBounds(char[][] puzzle, int x, int y) =>
        y >= 0 && y < puzzle.Length && x >= 0 && x < puzzle[y].Length;

    private void WriteCurrentGraphToConsole((int x, int y) currentCoordinate, Direction direction)
    {
        var (x, y) = currentCoordinate;
        var currPuzzle = Input.SplitToMatrix<char>();
        currPuzzle[y][x] = GetDirectionMarker(direction);
        Console.WriteLine("======");
        foreach (var line in currPuzzle)
        {
            Console.WriteLine(string.Join("", line));
        }
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}