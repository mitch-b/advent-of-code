namespace AdventOfCode.Solutions.Year2023.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2023, "") { }

    protected override string? SolvePartOne()
    {
        var sum = 0;
        var rows = Input.SplitByNewline();
        foreach (var row in rows)
        {
            var digits = row.Where(Char.IsDigit).Select(c => c.ToString());
            sum += int.Parse(digits.First() + digits.Last());
        }
        return sum.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var spelledOutDigits = new Dictionary<string, string>
        {
            {"zero", "0"},
            {"one", "1"},
            {"two", "2"},
            {"three", "3"},
            {"four", "4"},
            {"five", "5"},
            {"six", "6"},
            {"seven", "7"},
            {"eight", "8"},
            {"nine", "9"}
        };
        var sum = 0;
        var rows = Input.SplitByNewline();
        foreach (var row in rows)
        {
            string digitizedRow = row;
            foreach (var key in spelledOutDigits.Keys)
            {
                if (row.Contains(key))
                {
                    var replaceDigitText = key.First() + spelledOutDigits[key] + key.Last();
                    digitizedRow = digitizedRow.Replace(key, replaceDigitText);
                }
            }
            var digits = digitizedRow.Where(Char.IsDigit).Select(c => c.ToString());
            sum += int.Parse(digits.First() + digits.Last());
        }
        return sum.ToString();
    }
}
