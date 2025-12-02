namespace AdventOfCode.Solutions.Year2022.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2022, "")
    {
        Elves = Input.SplitByParagraph()
            .Select(p => p.SplitByNewline())
            .Select(e => new Elf(e.Sum(calories => int.Parse(calories)), e))
            .OrderByDescending(e => e.sumOfCalories);
    }

    private IEnumerable<Elf> Elves;

    protected override string? SolvePartOne()
    {
        return Elves.Take(1).Sum(e => e.sumOfCalories).ToString();
    }

    protected override string? SolvePartTwo()
    {
        return Elves.Take(3).Sum(e => e.sumOfCalories).ToString();
    }

    protected record Elf(long sumOfCalories, IEnumerable<string> snacks);
}
