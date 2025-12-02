namespace AdventOfCode.Solutions.Year2020.Day08;

class Solution : SolutionBase
{
    private readonly List<Instruction> Instructions;
    public Solution() : base(08, 2020, "")
    {
        this.Instructions = Input.SplitByNewline().Select(i => Instruction.FromInputLine(i)).ToList();
    }

    protected override string? SolvePartOne()
    {
        return GetAccumulatorFromProgram(Instructions).accumulator.ToString();
    }

    protected override string? SolvePartTwo()
    {
        // key: instruction index; value: step hit
        var modifiedInstructions = Instructions.Clone();
        var changedThisStep = false;
        for (var step = 0; step < Instructions.Count(); step++)
        {
            if (modifiedInstructions[step].operation == "jmp")
            {
                modifiedInstructions[step].operation = "nop";
            }
            else if (modifiedInstructions[step].operation == "nop")
            {
                modifiedInstructions[step].operation = "jmp";
            }
            changedThisStep = modifiedInstructions[step].operation != "acc";

            // if we made an adjustment, re-run our modified instructions
            if (changedThisStep)
            {
                (int accumulator, bool success) = GetAccumulatorFromProgram(modifiedInstructions);
                if (success)
                {
                    return accumulator.ToString();
                }
                modifiedInstructions = Instructions.Clone();
                continue;
            }
        }
        return null;
    }

    private (int accumulator, bool success) GetAccumulatorFromProgram(List<Instruction> instructions)
    {
        var accessedInstructions = new Dictionary<int, int>();
        (int index, int accumulator) position = (0, 0);
        var step = 0;
        for (; step < instructions.Count(); step++)
        {
            if (position.index == instructions.Count())
            {
                return (position.accumulator, true);
            }
            try
            {
                accessedInstructions.Add(position.index, step);
            }
            catch
            {
                return (position.accumulator, false);
            }
            position = instructions[position.index]
                .GetIndexAfterInstruction(position);
        }
        return (position.accumulator, false);
    }
}

class Instruction : ICloneable
{
    public string operation;
    public readonly int value;
    public Instruction(string operation, int value) // try long value?
    {
        this.operation = operation;
        this.value = value;
    }

    public static Instruction FromInputLine(string line) => new Instruction(line.Substring(0, 3), int.Parse(line.Substring(4)));

    public (int index, int accumulator) GetIndexAfterInstruction((int index, int accumulator) current)
    {
        switch (this.operation)
        {
            case "acc":
                return (current.index + 1, current.accumulator + value);
            case "jmp":
                return (current.index + value, current.accumulator);
            case "nop":
            default:
                return (current.index + 1, current.accumulator);
        }
    }

    public object Clone() => this.MemberwiseClone();
}