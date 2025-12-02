namespace AdventOfCode.Solutions.Year2021.Day07;

class Solution : SolutionBase
{
    public Solution() : base(07, 2021, "") { }

    protected override string? SolvePartOne()
    {
        var positions = this.Input.Split(',').Select(p => int.Parse(p));
        var maxPosition = positions.Max();
        var minPosition = positions.Min();
        var totalFuelCosts = new Dictionary<int, long>();

        for (var position = minPosition; position <= maxPosition; position++)
        {
            totalFuelCosts.Add(position, 0);
            foreach (var crab in positions)
            {
                totalFuelCosts[position] += Math.Abs(crab - position);
            }
        }

        return totalFuelCosts.OrderBy(k => k.Value).First().Value.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var positions = this.Input.Split(',').Select(p => int.Parse(p));
        var maxPosition = positions.Max();
        var minPosition = positions.Min();
        var totalFuelCosts = new Dictionary<int, long>();

        for (var position = minPosition; position <= maxPosition; position++)
        {
            totalFuelCosts.Add(position, 0);
            foreach (var crab in positions)
            {
                totalFuelCosts[position] += Enumerable.Range(1, Math.Abs(crab - position)).Sum();
            }
        }

        return totalFuelCosts.OrderBy(k => k.Value).First().Value.ToString();
    }
}
