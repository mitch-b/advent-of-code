using AdventOfCode.Solutions.Models;

namespace AdventOfCode.Solutions.Year2025.Day05;

class Solution : SolutionBase
{
    public Solution() : base(05, 2025, "", useDebugInput: false) { }

    protected override string? SolvePartOne()
    {
        var freshRanges = ParseFreshRanges(Input.SplitByNewline(shouldTrim: true));
        var productIds = ParseProductIds(Input.SplitByNewline(shouldTrim: true));
        var freshProducts = new List<decimal>();
        foreach (var productId in productIds)
        {
            foreach (var range in freshRanges)
            {
                if (range.Contains(productId))
                {
                    freshProducts.Add(productId);
                    break;
                }
            }
        }
        return freshProducts.Count.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var freshRanges = ParseFreshRanges(Input.SplitByNewline(shouldTrim: true));
        freshRanges = [.. freshRanges.OrderBy(r => r.Start)];

        var mergedRanges = new List<ComparableRange<decimal>>();
        foreach (var range in freshRanges)
        {
            if (mergedRanges.Count == 0)
            {
                mergedRanges.Add(range);
                continue;
            }

            var lastRange = mergedRanges.Last();
            if (range.Start.CompareTo(lastRange.End) <= 0)
            {
                lastRange.End = Math.Max(lastRange.End, range.End);
            }
            else
            {
                mergedRanges.Add(range);
            }
        }

        return mergedRanges.Sum(r => r.GetSize()).ToString();
    }

    private static List<decimal> ParseProductIds(string[] input)
    {
        var productIds = new List<decimal>();
        foreach (var line in input)
        {
            if (decimal.TryParse(line, out var id))
            {
                productIds.Add(id);
            }
        }
        return productIds;
    }

    private static List<ComparableRange<decimal>> ParseFreshRanges(string[] input)
    {
        var ranges = new List<ComparableRange<decimal>>();
        foreach (var line in input)
        {
            var parts = line.Split("-", StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
            {
                break;
            }
            ranges.Add(new ComparableRange<decimal>(
                decimal.Parse(parts[0]),
                decimal.Parse(parts[1])
            ));
        }
        return ranges;
    }
}
