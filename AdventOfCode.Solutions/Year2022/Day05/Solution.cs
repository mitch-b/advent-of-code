namespace AdventOfCode.Solutions.Year2022.Day05;

class Solution : SolutionBase
{
    public Solution() : base(05, 2022, "")
    {
        Setup();
    }

    private void Setup()
    {
        _containers = new List<Stack<char>>();
        _instructions = new List<Instruction>();
        var inputComponents =
            Input.SplitByParagraph();
        var inputContainers = inputComponents[0].SplitByNewline();
        var inputInstructions = inputComponents[1].SplitByNewline();
        BuildContainers(inputContainers);
        BuildInstructions(inputInstructions);
    }

    private List<Stack<char>> _containers = new List<Stack<char>>();
    private List<Instruction> _instructions = new List<Instruction>();

    protected override string? SolvePartOne()
    {
        foreach (var instruction in _instructions)
        {
            for (var i = 0; i < instruction.take; i++)
            {
                _containers[instruction.destinationContainer - 1]
                    .Push(_containers[instruction.fromContainer - 1].Pop());
            }
        }

        return string.Join("", _containers.Select(c => c.Pop()));
    }

    protected override string? SolvePartTwo()
    {
        Setup();
        var containerLists = _containers.Select(c => c.ToList()).ToList();
        foreach (var instruction in _instructions)
        {
            containerLists[instruction.destinationContainer - 1].InsertRange(0,
                containerLists[instruction.fromContainer - 1].Take(instruction.take)
            );
            containerLists[instruction.fromContainer - 1].RemoveRange(0, instruction.take);
        }
        return string.Join("", containerLists.Select(c => c.First()));
    }

    private void BuildContainers(IEnumerable<string> inputContainers)
    {
        var height = inputContainers.Count(l => l[1] != '1');
        var numContainers = inputContainers.Last()
            .Split(' ').Where(c => !string.IsNullOrWhiteSpace(c))
            .Count();
        for (var i = 0; i < numContainers; i++)
        {
            _containers.Add(new Stack<char>());
        }

        var regexBuilder = @"(\s\s\s|\[\w\])\s?";
        foreach (var inputLine in inputContainers.Take(height).Reverse())
        {
            var containers = inputLine
                .ExtractMatchesFromString<string>(regexBuilder);
            for (var i = 0; i < numContainers; i++)
            {
                if (string.IsNullOrWhiteSpace(containers[i]))
                {
                    continue;
                }

                _containers[i].Push(containers[i][1]);
            }
        }
    }

    private void BuildInstructions(IEnumerable<string> inputInstructions)
    {
        var regex = @"(\d+).*?(\d+).*?(\d+).*?";
        var parsedInstructions = inputInstructions.Select(i => i.ExtractFromString<int>(regex));
        _instructions.AddRange(parsedInstructions.Select(p => new Instruction(p[0], p[1], p[2])));
    }
}

record Instruction(int take, int fromContainer, int destinationContainer);