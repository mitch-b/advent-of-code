using System.Globalization;

namespace AdventOfCode.Solutions.Year2020.Day09;

class Solution : SolutionBase
{
    private readonly IEnumerable<long> xmasData;
    public Solution() : base(09, 2020, "")
    {
        this.xmasData = Input
            .SplitByNewline()
            .Select(n => long.Parse(n, CultureInfo.InvariantCulture));
    }

    protected override string? SolvePartOne()
    {
        var preambleLength = 25;
        for (var i = 0; i < xmasData.Count(); i++)
        {
            if (i < preambleLength) continue;
            if (!PreambleCanSumTarget(xmasData.Skip(i - preambleLength).Take(preambleLength), xmasData.ElementAt(i)))
            {
                return xmasData.ElementAt(i).ToString();
            }
        }
        return null;
    }

    protected override string? SolvePartTwo()
    {
        var partOneResult = SolvePartOne();
        if (string.IsNullOrEmpty(partOneResult))
        {
            throw new InvalidOperationException("Unable to determine the XMAS weakness without a target value.");
        }

        var breakpoint = long.Parse(partOneResult, CultureInfo.InvariantCulture);
        for (var i = 0; i < xmasData.Count(); i++)
        {
            var sequence = new List<long>();
            for (var j = i; j < xmasData.Count(); j++)
            {
                sequence.Add(xmasData.ElementAt(j));
                if (sequence.Sum() == breakpoint)
                {
                    return (sequence.OrderBy(s => s).First() + sequence.OrderByDescending(s => s).First()).ToString();
                }
                else if (sequence.Sum() > breakpoint)
                {
                    break;
                }
            }
        }
        return null;
    }

    private bool PreambleCanSumTarget(IEnumerable<long> preamble, long target)
    {
        for (var i = 0; i < preamble.Count(); i++)
        {
            for (var j = i + 1; j < preamble.Count(); j++)
            {
                if (preamble.ElementAt(i) + preamble.ElementAt(j) == target)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
