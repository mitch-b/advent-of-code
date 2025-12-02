namespace AdventOfCode.Solutions.Year2021.Day02;

class Solution : SolutionBase
{
    public Solution() : base(02, 2021, "") { }

    protected override string? SolvePartOne()
    {
        var instructions = this.Input.SplitByNewline();
        var horizontalPosition = 0;
        var depth = 0;
        foreach (var instruction in instructions)
        {
            var (action, value, _) = instruction.Split(' ');
            var num = int.Parse(value);
            switch (action)
            {
                case "forward":
                    horizontalPosition += num;
                    break;
                case "up":
                    depth -= num;
                    break;
                case "down":
                    depth += num;
                    break;
                default:
                    break;
            }
        }
        return (horizontalPosition * depth).ToString();
    }

    protected override string? SolvePartTwo()
    {
        var instructions = this.Input.SplitByNewline();
        var horizontalPosition = 0;
        var depth = 0;
        var aim = 0;
        foreach (var instruction in instructions)
        {
            var (action, value, _) = instruction.Split(' ');
            var num = int.Parse(value);
            switch (action)
            {
                case "forward":
                    horizontalPosition += num;
                    depth += (aim * num);
                    break;
                case "up":
                    aim -= num;
                    break;
                case "down":
                    aim += num;
                    break;
                default:
                    break;
            }
        }
        return (horizontalPosition * depth).ToString();
    }
}
