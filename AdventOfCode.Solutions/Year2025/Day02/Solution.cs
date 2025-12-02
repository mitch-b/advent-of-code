using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2025.Day02;

class Solution : SolutionBase
{
    public Solution() : base(02, 2025, "", useDebugInput: false) { }

    protected override string? SolvePartOne()
    {
        var invalidIds = new List<long>();
        foreach (var numberRange in Input.Split(','))
        {
            var numbers = numberRange.Split('-');
            var range = new Range(long.Parse(numbers[0]), long.Parse(numbers[1]));
            foreach (var number in range)
            {
                var firstHalf = number.ToString()[..(number.ToString().Length / 2)];
                var secondHalf = number.ToString()[(number.ToString().Length / 2)..];
                if (firstHalf == secondHalf)
                {
                    invalidIds.Add(number);
                }
            }
        }
        return invalidIds.Sum().ToString();
    }

    protected override string? SolvePartTwo()
    {
        // muahahahaaha
        var repeatingPatternRegex = new Regex(@"^(?:(\d+?)\1+)$");
        var invalidIds = new List<long>();
        foreach (var numberRange in Input.Split(','))
        {
            var numbers = numberRange.Split('-');
            var range = new Range(long.Parse(numbers[0]), long.Parse(numbers[1]));
            foreach (var number in range)
            {
                if (repeatingPatternRegex.IsMatch(number.ToString()))
                {
                    invalidIds.Add(number);
                }
            }
        }
        return invalidIds.Sum().ToString();
    }
}

class Range(long start, long end)
{
    public long Start { get; set; } = start;
    public long End { get; set; } = end;
    public IEnumerator<long> GetEnumerator()
    {
        for (long i = Start; i <= End; i++)
        {
            yield return i;
        }
    }
}
