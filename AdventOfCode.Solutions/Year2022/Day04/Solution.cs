namespace AdventOfCode.Solutions.Year2022.Day04;

class Solution : SolutionBase
{
    public Solution() : base(04, 2022, "")
    {
        _assignmentPairs = Input.SplitByNewline().Select(a => new AssignmentPair(a));
    }

    private readonly IEnumerable<AssignmentPair> _assignmentPairs = [];

    protected override string? SolvePartOne()
    {
        return _assignmentPairs
            .Count(p => p.HasCompletelyOverlappingAssignment)
            .ToString();
    }

    protected override string? SolvePartTwo()
    {
        return _assignmentPairs
            .Count(p => p.HasAnyOverlappingAssignment)
            .ToString();
    }
}

class AssignmentPair
{
    public Pair FirstPair { get; set; }
    public Pair SecondPair { get; set; }

    public AssignmentPair(string pairs)
    {
        var splitRegex = @"(\d+)-(\d+)";
        var firstSections = pairs.Split(',')[0].ExtractFromString<int>(splitRegex);
        var secondSections = pairs.Split(',')[1].ExtractFromString<int>(splitRegex);
        FirstPair = new Pair(firstSections.ElementAt(0), firstSections.ElementAt(1));
        SecondPair = new Pair(secondSections.ElementAt(0), secondSections.ElementAt(1));
    }
    public bool HasCompletelyOverlappingAssignment =>
        (FirstPair.From <= SecondPair.From && FirstPair.To >= SecondPair.To)
        || (SecondPair.From <= FirstPair.From && SecondPair.To >= FirstPair.To);

    public bool HasAnyOverlappingAssignment =>
        Enumerable.Range(FirstPair.From, FirstPair.To - FirstPair.From + 1)
        .Intersect(Enumerable.Range(SecondPair.From, SecondPair.To - SecondPair.From + 1))
        .Any();
}

record Pair(int From, int To);