namespace AdventOfCode.Solutions.Year2021.Day04;

class Solution : SolutionBase
{
    public Solution() : base(04, 2021, "") { }

    protected override string? SolvePartOne()
    {
        var drawnNumbers = this.Input.SplitByNewline()[0].Split(',').Select(n => int.Parse(n));
        var boardSets = SplitInputToBoards(this.Input.Split("\n").Skip(2));
        foreach (var number in drawnNumbers)
        {
            foreach (var board in boardSets)
            {
                board.Picks.Add(number);
                if (board.HasWin())
                {
                    return (board.UnpickedNumbers().Sum() * number).ToString();
                }
            }
        }
        return null;
    }

    protected override string? SolvePartTwo()
    {
        var drawnNumbers = this.Input.SplitByNewline()[0].Split(',').Select(n => int.Parse(n));
        var boardSets = SplitInputToBoards(this.Input.Split("\n").Skip(2));
        foreach (var number in drawnNumbers)
        {
            var openBoards = boardSets.Where(b => !b.HasWin());
            foreach (var board in openBoards)
            {
                board.Picks.Add(number);
                if (board.HasWin() && openBoards.Count() == 0)
                {
                    return (board.UnpickedNumbers().Sum() * number).ToString();
                }
            }
        }
        return null;
    }

    private List<Board> SplitInputToBoards(IEnumerable<string> inputString)
    {
        var boards = new List<Board>();
        var tempInput = new List<string>();
        foreach (var line in inputString)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (tempInput.Any())
                {
                    boards.Add(Board.Create(tempInput));
                }
                tempInput.Clear();
                continue;
            }
            tempInput.Add(line);
        }
        if (tempInput.Any())
        {
            boards.Add(Board.Create(tempInput));
        }
        return boards;
    }
}

class Board
{
    public int[][] Matrix;
    public HashSet<int> Picks = new HashSet<int>();
    public static Board Create(IEnumerable<string> inputString)
    {
        var board = new Board();
        board.Matrix = new int[inputString.Count()][];
        for (var i = 0; i < inputString.Count(); i++)
        {
            var rowValues = inputString.ElementAt(i).Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            board.Matrix[i] = new int[rowValues.Length];
            for (var j = 0; j < rowValues.Length; j++)
            {
                board.Matrix[i][j] = int.Parse(rowValues[j]);
            }
        }
        return board;
    }

    public bool HasWin()
    {
        for (int i = 0; i < Matrix.Length; i++)
        {
            // row match
            if (Matrix[i].All(Picks.Contains))
            {
                return true;
            }
        }
        for (int j = 0; j < Matrix[0].Length; j++)
        {
            // column match
            if (Matrix.Select(r => r[j]).All(Picks.Contains))
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerable<int> UnpickedNumbers() =>
        Matrix.SelectMany(row => row.Where(num => !Picks.Contains(num)));
}
