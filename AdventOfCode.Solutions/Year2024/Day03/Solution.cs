using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2024.Day03;

class Solution : SolutionBase
{
    private readonly Regex _regex = new(@"(mul\((\d+),(\d+)\))", RegexOptions.Compiled);

    public Solution() : base(03, 2024, "") { }

    protected override string? SolvePartOne()
    {
        var outputs = GetMultValues(Input);
        return outputs.Sum().ToString();
    }

    protected override string? SolvePartTwo()
    {
        var instruction = string.Join("", Input).Trim();
        while (instruction.IndexOf("don't()") > -1)
        {
            var firstDont = instruction.IndexOf("don't()");
            var firstDo = instruction.IndexOf("do()", firstDont);
            if (firstDo == -1)
            {
                instruction = instruction.Remove(firstDont);
            }
            else
            {
                instruction = instruction.Remove(firstDont, firstDo - firstDont + 4);
            }
        }
        var outputs = GetMultValues(instruction);
        return outputs.Sum().ToString();
    }

    private List<long> GetMultValues(string input)
    {

        var matches = _regex.Matches(string.Join("", input).Trim());
        var outputs = new List<long>();
        foreach (Match match in matches)
        {
            var groups = match.Groups;
            var x = int.Parse(groups[2].Value);
            var y = int.Parse(groups[3].Value);
            outputs.Add(x * y);
        }
        return outputs;
    }
}
