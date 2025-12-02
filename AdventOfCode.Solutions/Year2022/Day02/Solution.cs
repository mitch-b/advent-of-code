namespace AdventOfCode.Solutions.Year2022.Day02;

class Solution : SolutionBase
{
    public Solution() : base(02, 2022, "") { }

    private readonly IEnumerable<Round> _rounds = [];

    protected override string? SolvePartOne()
    {
        return _rounds.Sum(r => GetScore(r)).ToString();
    }

    protected override string? SolvePartTwo()
    {
        return _rounds.Sum(r => GetScore(r, partTwo: true)).ToString();
    }

    static Move ParseMove(char move)
    {
        return move switch
        {
            _ when move is 'A' || move is 'X' => Move.Rock,
            _ when move is 'B' || move is 'Y' => Move.Paper,
            _ when move is 'C' || move is 'Z' => Move.Scissors,
            _ => throw new ArgumentException()
        };
    }

    static int GetScore(Round round, bool partTwo = false)
    {
        if (partTwo)
        {
            round.Me = UpdateMove(round);
        }
        return (IWon(round) ? 6 : (Tied(round) ? 3 : 0)) + (round.Me switch
        {
            Move.Rock => 1,
            Move.Paper => 2,
            Move.Scissors => 3,
            _ => throw new NotImplementedException()
        });
    }

    static bool IWon(Round round)
    {
        return round.Opponent == Move.Rock && round.Me == Move.Paper
        || round.Opponent == Move.Paper && round.Me == Move.Scissors
        || round.Opponent == Move.Scissors && round.Me == Move.Rock;
    }

    static bool Tied(Round round)
    {
        return round.Opponent == round.Me;
    }

    static Move GetWinMove(Move opponent)
    {
        return opponent switch
        {
            Move.Rock => Move.Paper,
            Move.Paper => Move.Scissors,
            Move.Scissors => Move.Rock,
            _ => throw new NotImplementedException()
        };
    }

    static Move GetLoseMove(Move opponent)
    {
        return opponent switch
        {
            Move.Rock => Move.Scissors,
            Move.Paper => Move.Rock,
            Move.Scissors => Move.Paper,
            _ => throw new NotImplementedException()
        };
    }

    static Move UpdateMove(Round round)
    {
        return round switch
        {
            _ when round.Me == Move.Rock => GetLoseMove(round.Opponent), // lose
            _ when round.Me == Move.Paper => round.Opponent, // tie
            _ when round.Me == Move.Scissors => GetWinMove(round.Opponent),
            _ => throw new ArgumentOutOfRangeException(nameof(round), "Unknown move selection.")
        };
    }
}

class Round
{
    public Move Opponent { get; set; }
    public Move Me { get; set; }
    public Round(Move opponent, Move me)
    {
        Opponent = opponent;
        Me = me;
    }
}

enum Move
{
    Rock, Paper, Scissors
}