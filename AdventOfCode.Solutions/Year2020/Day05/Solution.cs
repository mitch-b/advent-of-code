namespace AdventOfCode.Solutions.Year2020.Day05;

class Solution : SolutionBase
{
    private readonly string[] instructions;
    public Solution() : base(05, 2020, "")
    {
        instructions = Input.SplitByNewline();
    }

    protected override string? SolvePartOne()
    {
        var highestSeatId = 0;
        for (var i = 0; i < instructions.Length; i++)
        {
            var seatId = GetSeatId(instructions[i]);
            if (seatId > highestSeatId)
            {
                highestSeatId = seatId;
            }
        }
        return highestSeatId.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var seats = new int[128 * 8];
        for (var i = 0; i < instructions.Length; i++)
        {
            seats[GetSeatId(instructions[i])] = 1;
        }
        return (string.Join("", seats).IndexOf("101") + 1).ToString();
    }

    private int GetSeatId(string instruction)
    {
        var rowInstructions = instruction.Substring(0, 7).Select(c => c).ToArray();
        var aisleInstructions = instruction.Substring(7, 3).Select(c => c).ToArray();
        var rowNumber = GetBinarySpacePartitionNumber(rowInstructions, (0, 127), 'F');
        var seatNumber = GetBinarySpacePartitionNumber(aisleInstructions, (0, 7), 'L');
        return rowNumber * 8 + seatNumber;
    }

    private int GetBinarySpacePartitionNumber(char[] instructions, (int min, int max) range, char lowerBoundInstruction)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            int midpoint = range.max - (int)((range.max - range.min) / 2);
            if (instructions[i] == lowerBoundInstruction)
            {
                range = (range.min, midpoint - 1);
            }
            else
            {
                range = (midpoint, range.max);
            }
        }
        return range.min;
    }
}
