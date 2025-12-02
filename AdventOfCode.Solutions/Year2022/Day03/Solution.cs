namespace AdventOfCode.Solutions.Year2022.Day03;

class Solution : SolutionBase
{
    public Solution() : base(03, 2022, "")
    {
        _rucksacks.AddRange(Input.SplitByNewline().Select(i => new Rucksack(i)));
    }

    private readonly List<Rucksack> _rucksacks = [];

    protected override string? SolvePartOne()
    {
        return _rucksacks
            .Sum(r => r.SharedItems.Sum(item => GetItemTypeValue(item)))
            .ToString();
    }

    protected override string? SolvePartTwo()
    {
        return _rucksacks.Chunk(3)
            .Select<IEnumerable<Rucksack>, int>(rucksackGroup =>
            GetItemTypeValue(rucksackGroup
                .Select(r => r.UniqueItems)
                .Aggregate((prev, next) => prev.Intersect(next))
                .First()
            )
        ).Sum().ToString();
    }

    private int GetItemTypeValue(char item) => item switch
    {
        _ when (int)item >= (int)'a' => (int)item % 32,
        _ => ((int)item % 32) + 26
    };
}

class Rucksack
{
    public List<char> FirstCompartment = [];
    public List<char> SecondCompartment = [];
    public IEnumerable<char> SharedItems => FirstCompartment.Intersect(SecondCompartment).Distinct();
    public IEnumerable<char> AllItems => FirstCompartment.Union(SecondCompartment);
    public IEnumerable<char> UniqueItems => AllItems.Distinct();
    public Rucksack(string items)
    {
        var compartmentSize = items.Length / 2;
        FirstCompartment.AddRange(items.Take(compartmentSize));
        SecondCompartment.AddRange(items.Skip(compartmentSize));
    }
}