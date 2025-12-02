namespace AdventOfCode.Solutions.Year2022.Day06;

class Solution : SolutionBase
{
    public Solution() : base(06, 2022, "") { }

    protected override string? SolvePartOne()
    {
        var packetLen = 4;
        for (var i = 0; i < (Input.Length - packetLen); i++)
        {
            if (Input.Skip(i).Take(packetLen).Distinct().Count() == packetLen)
            {
                return (i + packetLen).ToString();
            }
        }

        return "";
    }

    protected override string? SolvePartTwo()
    {
        var packetLen = 14;
        for (var i = 0; i < (Input.Length - packetLen); i++)
        {
            if (Input.Skip(i).Take(packetLen).Distinct().Count() == packetLen)
            {
                return (i + packetLen).ToString();
            }
        }

        return "";
    }
}
