namespace AdventOfCode.Solutions.Year2020.Day01;

class Solution : SolutionBase
{
    private readonly int _idealExpenseReportSum = 2020;
    public Solution() : base(01, 2020, "") { }

    protected override string? SolvePartOne()
    {
        var expenses = this.Input.ToIntArray("\n");
        for (int i = 0; i < expenses.Length; i++)
        {
            for (int j = i + 1; j < expenses.Length; j++)
            {
                if (expenses[i] + expenses[j] == this._idealExpenseReportSum)
                {
                    return (expenses[i] * expenses[j]).ToString();
                }
            }
        }
        return null;
    }

    protected override string? SolvePartTwo()
    {
        var expenses = this.Input.ToIntArray("\n");
        for (int i = 0; i < expenses.Length; i++)
        {
            for (int j = i + 1; j < expenses.Length; j++)
            {
                for (int k = j + 1; k < expenses.Length; k++)
                {
                    if (expenses[i] + expenses[j] + expenses[k] == this._idealExpenseReportSum)
                    {
                        return (expenses[i] * expenses[j] * expenses[k]).ToString();
                    }
                }
            }
        }
        return null;
    }
}
