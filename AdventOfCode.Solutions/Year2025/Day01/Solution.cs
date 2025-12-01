namespace AdventOfCode.Solutions.Year2025.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2025, "", useDebugInput: false) { }

    protected override string? SolvePartOne()
    {
        var position = 50;
        var timesAtZero = 0;
        foreach (var line in Input.SplitByNewline())
        {
            var direction = line[0];
            var rotations = int.Parse(line[1..]);
            position += direction == 'L' 
                ? -rotations 
                : rotations;
            position = NormalizePosition(position);
            if (position == 0)
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

        foreach (var line in Input.SplitByNewline())
        {
            var direction = line[0];
            var rotations = int.Parse(line[1..]);

            var previousPosition = position;
            position += direction == 'L' 
                ? -rotations 
                : rotations;

            var newPosition = NormalizePosition(position);
            var zeroesHit = CountZeroCrossings(previousPosition, rotations, direction);

            timesAtZero += zeroesHit;

            Console.WriteLine($"From: {previousPosition} to {newPosition}, rotated {direction} {rotations} times. Touched zero {zeroesHit} times.");

            position = newPosition;
        }
        
        return timesAtZero.ToString();
    }

    private int NormalizePosition(int position)
    {
        if (position >= 0)
        {
            return position % 100;
        }
        var mod = Math.Abs(position) % 100;
        return mod == 0 
            ? 0 
            : 100 - mod;
    }

    private int CountZeroCrossings(int startPosition, int rotations, char direction)
    {
        if (startPosition == 0)
        {
            return rotations / 100;
        }
        
        var fullRotations = rotations / 100;
        var remainder = rotations % 100;
        var zeroesHit = fullRotations;

        if (direction == 'L' && remainder >= startPosition)
        {
            zeroesHit++;
        }
        else if (direction == 'R' && startPosition + remainder >= 100)
        {
            zeroesHit++;
        }

        return zeroesHit;
    }
}
