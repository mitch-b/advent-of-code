namespace AdventOfCode.Solutions.Year2021.Day08;

class Solution : SolutionBase
{
    public Solution() : base(08, 2021, "") { }

    protected override string? SolvePartOne()
    {
        var entries = this.Input.SplitByNewline().Select(e => ParseInput(e));
        var validNums = new int?[] { 1, 4, 7, 8 };
        return entries
            .SelectMany(e => e.outputValues.Where(v => validNums.Contains(v.GetDigit())))
            .Count()
            .ToString();
    }

    protected override string? SolvePartTwo()
    {
        var entries = this.Input.SplitByNewline().Select(e => ParseInput(e));
        return entries
            .Select(e => int.Parse(string.Join("", e.GetOutputDigits())))
            .Sum()
            .ToString();
    }

    private Entry ParseInput(string inputLine)
    {
        var items = inputLine.Split(' ').Where(s => s != "|");
        var entry = new Entry(
            items.Take(10).Select(sd => new SegmentDisplay(sd)),
            items.Skip(10).Select(sd => new SegmentDisplay(sd)));
        entry.ParsePatterns();
        return entry;
    }
}

record class Entry(IEnumerable<SegmentDisplay> signalPatterns, IEnumerable<SegmentDisplay> outputValues)
{
    public Dictionary<int, HashSet<char>> DigitSegments { get; set; } = new Dictionary<int, HashSet<char>>();

    public IEnumerable<int> GetOutputDigits()
    {
        var knownDigits = new List<int>();
        foreach (var outputValue in outputValues)
        {
            var kvp = DigitSegments
                .FirstOrDefault(kvp => kvp.Value.SetEquals(outputValue.Segments));
            knownDigits.Add(kvp.Key);
        }
        return knownDigits;
    }


    public void ParsePatterns()
    {
        foreach (var signalPattern in signalPatterns)
        {
            var uniqueDigit = signalPattern.GetDigit();
            if (uniqueDigit.HasValue)
            {
                this.DigitSegments.Add(uniqueDigit.Value, signalPattern.Segments);
            }
        }
        for (var i = 0; i < 10; i++)
        {
            var signalPattern = signalPatterns.ElementAt(i);
            var digit = signalPattern.GetDigit(this.DigitSegments);
            if (digit.HasValue)
            {
                this.DigitSegments.TryAdd(digit.Value, signalPattern.Segments);
            }
        }
    }
};

record class SegmentDisplay(string segmentCode)
{
    public HashSet<char> Segments = new HashSet<char>(segmentCode.ToCharArray());
    public int? GetDigit(Dictionary<int, HashSet<char>>? digitSegments = null)
    {
        switch (segmentCode.Length)
        {
            case 2:
                return 1;
            case 3:
                return 7;
            case 4:
                return 4;
            case 5:
                if (digitSegments == null) return null;

                // if HashSet intersect a 7 == 3; 3
                // if HashSet intersect a 4 == 3; 5
                // else; 2
                // 2 or 3 or 5

                if (Segments.Intersect(digitSegments[7]).Count() == 3)
                    return 3;
                if (Segments.Intersect(digitSegments[4]).Count() == 3)
                    return 5;
                return 2;
            case 6:
                if (digitSegments == null) return null;

                // if HashSet intersect a 4 == 4; 9
                // if HashSet intersect a 7 == 3; 0
                // else; 6
                // 0 or 6 or 9

                if (Segments.Intersect(digitSegments[4]).Count() == 4)
                    return 9;
                if (Segments.Intersect(digitSegments[7]).Count() == 3)
                    return 0;
                return 6;
            case 7:
                return 8;
            default:
                return null;
        }
    }
};