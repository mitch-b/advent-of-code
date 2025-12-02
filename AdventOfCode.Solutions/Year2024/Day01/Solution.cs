namespace AdventOfCode.Solutions.Year2024.Day01;

class Solution : SolutionBase
{
    List<long> leftSet = new List<long>();
    List<long> rightSet = new List<long>();
    public Solution() : base(01, 2024, "")
    {
        var lines = Input.SplitByNewline();
        foreach (var line in lines)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var left = long.Parse(parts[0]);
            var right = long.Parse(parts[1]);
            leftSet.Add(left);
            rightSet.Add(right);
        }
    }

    protected override string? SolvePartOne()
    {
        var sortedLeft = leftSet.OrderBy(x => x).ToList();
        var sortedRight = rightSet.OrderBy(x => x).ToList();
        var diffs = new List<long>();
        for (var i = 0; i < sortedLeft.Count; i++)
        {
            diffs.Add(Math.Abs(sortedLeft[i] - sortedRight[i]));
        }
        return diffs.Sum().ToString();
    }

    protected override string? SolvePartTwo()
    {
        long similarityScore = 0;
        for (var i = 0; i < leftSet.Count; i++)
        {
            similarityScore += leftSet[i] * rightSet.Count(a => a == leftSet[i]);
        }
        return similarityScore.ToString();
    }
}
