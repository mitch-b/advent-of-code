namespace AdventOfCode.Solutions.Year2021.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2021, "") { }

    protected override string? SolvePartOne()
    {
        var depths = this.Input.ToIntArray("\n");
        var increments = 0;
        for (var i = 0; i < depths.Length - 1; i++)
        {
            if (depths[i] < depths[i + 1])
            {
                increments++;
            }
        }
        return increments.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var depths = this.Input.ToIntArray("\n");
        var increments = 0;
        for (var i = 0; i < depths.Length - 3; i++)
        {
            var j = i + 1;
            var firstGroup = depths[i] + depths[i + 1] + depths[i + 2];
            var secondGroup = depths[j] + depths[j + 1] + depths[j + 2];
            if (firstGroup < secondGroup)
            {
                increments++;
            }
        }
        return increments.ToString();
    }
}
