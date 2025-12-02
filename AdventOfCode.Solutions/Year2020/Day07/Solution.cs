using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020.Day07;

class Solution : SolutionBase
{
    private readonly List<Bag> Bags;
    /// <summary>
    /// The key is the 'Bag' itself, the 'HashSet of Bag' are the bags which are a "parent" and contain the key Bag
    /// </summary>
    private Dictionary<Bag, HashSet<Bag>> ContainingBags = new Dictionary<Bag, HashSet<Bag>>();

    public Solution() : base(07, 2020, "")
    {
        this.Bags = Input.SplitByNewline()
                .Select(i => CreateBagFromInstruction(i))
                .ToList();
        ParseContainingBags();
    }

    protected override string? SolvePartOne()
    {
        return this.GetBagsWhichContain(GetBagByColor("shiny gold")).Count().ToString();
    }

    protected override string? SolvePartTwo()
    {
        var startingBag = GetBagByColor("shiny gold");
        var totalBags = GetRequiredBagsUnder(startingBag);
        return totalBags.count.ToString();
    }

    public Bag CreateBagFromInstruction(string instruction)
    {
        var extractor = new Regex(@"(.*?) bags contain (.*)");
        var endOfBags = "no other bags";
        var bagInstructionBase = extractor.Match(instruction);
        var bagColor = bagInstructionBase.Groups[1].Value;
        var contains = bagInstructionBase.Groups[2].Value;
        var bag = new Bag(bagColor);

        if (contains.Contains(endOfBags))
        {
            return bag;
        }

        var containedBags = Regex.Matches(contains, @"(\d+) (.*?) bags?");
        foreach (Match match in containedBags)
        {
            bag.BagContains.Add(match.Groups[2].Value, int.Parse(match.Groups[1].Value));
        }
        return bag;
    }

    private Bag GetBagByColor(string bagColor) => this.Bags.FirstOrDefault(b => b.BagColor == bagColor);

    private void ParseContainingBags()
    {
        foreach (var bag in this.Bags)
        {
            foreach (KeyValuePair<string, int> child in bag.BagContains)
            {
                var childBag = GetBagByColor(child.Key);
                if (!this.ContainingBags.ContainsKey(childBag))
                {
                    this.ContainingBags[childBag] = new HashSet<Bag>();
                }
                this.ContainingBags[childBag].Add(bag);
            }
        }
    }

    private IEnumerable<Bag> GetBagsWhichContain(Bag bag)
    {
        var nestedBags = new HashSet<Bag>();
        // loop each bag that "contains" the bag passed in
        if (ContainingBags.TryGetValue(bag, out HashSet<Bag> containedBags))
        {
            foreach (var containedBag in containedBags)
            {
                nestedBags.Add(containedBag);
                nestedBags.AddRange(GetBagsWhichContain(containedBag));
            }
        }
        return nestedBags;
    }

    private (Bag bag, int count) GetRequiredBagsUnder(Bag bag)
    {
        var numberOfBagsUnderThis = 0;
        foreach (var entry in bag.BagContains)
        {
            var containedBag = GetBagByColor(entry.Key);
            var bagCount = GetRequiredBagsUnder(containedBag).count + 1; // include containedBag itself
            numberOfBagsUnderThis += entry.Value * bagCount;
        }
        return (bag, numberOfBagsUnderThis);
    }
}

public class Bag : IEquatable<Bag>
{
    public string BagColor;
    public Dictionary<string, int> BagContains = new Dictionary<string, int>();
    public Bag(string bagColor)
    {
        this.BagColor = bagColor;
    }

    public int GetHashCode(Bag obj)
    {
        return obj.BagColor.GetHashCode();
    }

    public bool Equals(Bag other)
    {
        return other != null && other.BagColor == this.BagColor;
    }
}