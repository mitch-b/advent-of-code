using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2024.Day07;

class Solution : SolutionBase
{
    private List<(string op, Func<long, long, long> action)> validOperators =
    [
        ("+", (a, b) => a + b),
        ("*", (a, b) => a * b),
        ("||", (a, b) => long.Parse($"{a}{b}"))
    ];
    public Solution() : base(07, 2024, "") { }

    protected override string? SolvePartOne() => EvaluateEquations(["+", "*"]).ToString();

    protected override string? SolvePartTwo() => EvaluateEquations(["+", "*", "||"]).ToString();

    private long EvaluateEquations(List<string> canUseOperands)
    {
        var successfulNumbers = new List<long>();
        foreach (var c in Input.SplitByNewline())
        {
            var equationSplit = new Regex(@"(\d+)\: (.*)", RegexOptions.Compiled);
            var match = equationSplit.Match(c);
            if (match.Success)
            {
                var targetResult = long.Parse(match.Groups[1].Value);
                var equationString = match.Groups[2].Value;
                var numbers = equationString.Split(" ").Select(long.Parse).ToArray();

                var operatorPath = new List<string>();
                if (SolveEquation(numbers, targetResult, canUseOperands, operatorPath))
                {
                    successfulNumbers.AddRange(targetResult);
                }
            }
        }
        return successfulNumbers.Sum();
    }

    private bool SolveEquation(long[] numbers, long targetResult, List<string> canUseOperators, List<string> operatorPath)
    {
        if (numbers.Length == 1)
            return numbers[0] == targetResult;

        if (numbers.Length == 0)
            return false;

        foreach (var op in validOperators.Where(o => canUseOperators.Contains(o.op)))
        {
            long result = op.action(numbers[0], numbers[1]);

            if (result > targetResult) // hopefully other will work
            {
                continue;
            }

            long[] newNumbers = [result, .. numbers.Skip(2)];

            operatorPath.Add(op.op);

            if (SolveEquation(newNumbers, targetResult, canUseOperators, operatorPath))
            {
                return true;
            }

            operatorPath.RemoveAt(operatorPath.Count - 1);
        }

        return false;
    }
}
