namespace AdventOfCode.Solutions.Year2020.Day10;

class Solution : SolutionBase
{
    private IEnumerable<JoltageAdapter> joltageAdapters;
    public Solution() : base(10, 2020, "")
    {
        this.joltageAdapters = Input
                .SplitByNewline()
                .Select(j => new JoltageAdapter(int.Parse(j)))
                .Append(new JoltageAdapter(0)) // outlet itself
                .OrderBy(j => j);
    }

    protected override string? SolvePartOne()
    {
        var oneJoltDifferences = 0;
        var threeJoltDifferences = 1; // start at 1 to assume we've already counted our extra adapter that's 3 higher
        for (var i = 0; i < this.joltageAdapters.Count() - 1; i++)
        {
            var joltageDifference = this.joltageAdapters.ElementAt(i + 1).JoltageRating - this.joltageAdapters.ElementAt(i).JoltageRating;
            oneJoltDifferences += joltageDifference == 1 ? 1 : 0;
            threeJoltDifferences += joltageDifference == 3 ? 1 : 0;
        }
        return (oneJoltDifferences * threeJoltDifferences).ToString();
    }

    /// Stuck on this one. Needed help.
    /// https://www.reddit.com/r/adventofcode/comments/ka8z8x/2020_day_10_solutions/gfbo61q/?utm_source=reddit&utm_medium=web2x&context=3
    protected override string? SolvePartTwo()
    {
        // array of options that increases as we move forward toward the final adapter
        // first position is the adapter used to connect to outlet
        // continue to add number of adapter combinations as you progress through
        // the for-loop which would count up possibilities as we make our way to the
        // final adapter in our bag.
        var options = new long[this.joltageAdapters.Last().JoltageRating + 1];
        options[0] = 1; // first/direct connection
        for (var i = 0; i < this.joltageAdapters.Count(); i++)
        {
            var joltageRating = this.joltageAdapters.ElementAt(i).JoltageRating;
            options[joltageRating] += joltageRating >= 1 ? options[joltageRating - 1] : 0;
            options[joltageRating] += joltageRating >= 2 ? options[joltageRating - 2] : 0;
            options[joltageRating] += joltageRating >= 3 ? options[joltageRating - 3] : 0;
        }
        return options.Last().ToString();
    }
}

class JoltageAdapter : IComparable
{
    public readonly int JoltageRating;
    public readonly int JoltageFlux;
    public JoltageAdapter(int joltageRating, int joltageFlux = 3)
    {
        this.JoltageRating = joltageRating;
        this.JoltageFlux = joltageFlux;
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is JoltageAdapter other)
        {
            return this.JoltageRating.CompareTo(other.JoltageRating);
        }

        throw new ArgumentException("Object must be a JoltageAdapter.", nameof(obj));
    }

    public bool Handles(int joltageRating) => Math.Abs(joltageRating - this.JoltageRating) <= this.JoltageFlux;
}