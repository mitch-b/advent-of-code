namespace AdventOfCode.Solutions.Year2025.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2025, "", useDebugInput: true) { }

    protected override string? SolvePartOne()
    {
        var startPosition = 50;
        var timesAtZero = 0;
        foreach (var line in Input.SplitByNewline())
        {
            var direction = line[0];
            var rotations = int.Parse(line[1..]);
            switch (direction)
            {
                case 'L':
                    startPosition += rotations;
                    break;
                case 'R':
                    startPosition -= rotations;
                    break;
                default:
                    throw new Exception("huh?");
            }
            startPosition = (startPosition + 100) % 100;
            if (startPosition == 0)
            {
                timesAtZero++;
            }
            
        }
        return timesAtZero.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var position = 50;
        var timesAtZero = 0;

        var lastPosition = position;
        foreach (var line in Input.SplitByNewline())
        {
            var direction = line[0];
            var rotations = int.Parse(line[1..]);
            switch (direction)
            {
                case 'L':
                    position += rotations;
                    break;
                case 'R':
                    position -= rotations;
                    break;
                default:
                    throw new Exception("huh?");
            }
            if (position < 0 || position >= 100)
            {
                var fullRotations = (int)Math.Round(position / 100f, MidpointRounding.ToZero);
                timesAtZero += Math.Abs(fullRotations);
            }
            position = (position + 100) % 100;
            if (position == 0)
            {
                timesAtZero++;
            }
        }
        return timesAtZero.ToString();
    }
}
