namespace AdventOfCode.Solutions.Year2024.Day05;

class Solution : SolutionBase
{
    private readonly List<(int ifPage, int thenPage)> _rules;
    private readonly List<int[]> _pageInstructions;
    public Solution() : base(05, 2024, "")
    {
        _rules = Input.SplitByParagraph()[0]
            .SplitByNewline()
            .Select(i => i.Split("|"))
            .Select(a => (int.Parse(a[0]), int.Parse(a[1])))
            .ToList();
        _pageInstructions = Input.SplitByParagraph()[1]
            .SplitByNewline()
            .Select(i => i.Split(",").Select(int.Parse).ToArray())
            .ToList();
    }

    protected override string? SolvePartOne()
    {
        var goodInstructions = new List<int[]>();
        foreach (var instruction in this._pageInstructions)
        {
            var rules = this._rules.Where(r => instruction.Contains(r.ifPage) && instruction.Contains(r.thenPage));
            var pageDependencyGraph = CreateRuleDependencyGraph(rules);
            var instructionResult = VerifyInstruction(instruction, pageDependencyGraph);
            if (instructionResult != null)
            {
                goodInstructions.Add(instructionResult);
            }
        }
        return goodInstructions
            .Select(i => i[(int)Math.Floor(i.Length / 2.0)])
            .Sum()
            .ToString();
    }

    private int[]? VerifyInstruction(int[] instruction, List<PageDependency> pageDependencies)
    {
        var printedPages = new List<int>();
        foreach (var page in instruction)
        {
            var pageDependency = pageDependencies
                .FirstOrDefault(p => p.Page == page);
            var allPageDependencies = pageDependency?.Prerequisites() ?? Enumerable.Empty<int>();
            if (!allPageDependencies.All(printedPages.Contains))
            {
                return null;
            }
            printedPages.Add(page);
        }
        return instruction;
    }

    protected override string? SolvePartTwo()
    {
        var goodInstructions = new List<int[]>();
        var fixedInstructions = new List<int[]>();
        foreach (var instruction in this._pageInstructions)
        {
            var rules = this._rules.Where(r => instruction.Contains(r.ifPage) && instruction.Contains(r.thenPage));
            var pageDependencyGraph = CreateRuleDependencyGraph(rules);
            var instructionResult = VerifyInstruction(instruction, pageDependencyGraph);
            if (instructionResult != null)
            {
                continue;
            }

            int[]? fixedInstruction = [.. instruction];
            // don't look at this 👀
            for (var i = 0; i < 5; i++)
            {
                fixedInstruction = FixPagesBasedOnRules(fixedInstruction, rules);
            }

            instructionResult = VerifyInstruction(fixedInstruction, pageDependencyGraph);
            if (instructionResult != null)
            {
                goodInstructions.Add(fixedInstruction);
            }
        }

        var mids = goodInstructions
            .Select(i => i[(int)Math.Floor(i.Length / 2.0)]);
        return mids.Sum().ToString();
    }



    private List<PageDependency> CreateRuleDependencyGraph(IEnumerable<(int ifPage, int thenPage)> rules)
    {
        var pageDependencies = new List<PageDependency>();
        var seenPages = new HashSet<int>();

        foreach (var (ifPage, thenPage) in rules)
        {
            var ifPageDependency = pageDependencies.FirstOrDefault(p => p.Page == ifPage);
            if (ifPageDependency == null)
            {
                ifPageDependency = new PageDependency(ifPage, null);
                pageDependencies.Add(ifPageDependency);
                seenPages.Add(ifPage);
            }

            var thenPageDependency = pageDependencies.FirstOrDefault(p => p.Page == thenPage);
            if (thenPageDependency == null)
            {
                thenPageDependency = new PageDependency(thenPage, [ifPageDependency]);
                pageDependencies.Add(thenPageDependency);
                seenPages.Add(thenPage);
            }
            else
            {
                thenPageDependency.DependentOn.Add(ifPageDependency);
            }
        }

        return pageDependencies;
    }

    private int[] FixPagesBasedOnRules(int[] pages, IEnumerable<(int ifPage, int thenPage)> rules)
    {
        int[] fixedPages = [.. pages];
        foreach (var (ifPage, thenPage) in rules)
        {
            var ifPageIndex = Array.IndexOf(fixedPages, ifPage);
            var thenPageIndex = Array.IndexOf(fixedPages, thenPage);
            if (ifPageIndex > thenPageIndex)
            {
                // TUPLES! (avoids using a temp variable to store the swap value)
                (fixedPages[thenPageIndex], fixedPages[ifPageIndex]) = (fixedPages[ifPageIndex], fixedPages[thenPageIndex]);
            }
        }
        return fixedPages;
    }
}


class PageDependency(int page, List<PageDependency>? dependentOn)
{
    public int Page { get; set; } = page;
    public List<PageDependency> DependentOn { get; set; } = dependentOn ?? [];
    public List<int> Prerequisites()
    {
        var allPrerequisites = new List<int>();
        foreach (var prerequisite in DependentOn)
        {
            allPrerequisites.Add(prerequisite.Page);
            allPrerequisites.AddRange(prerequisite.Prerequisites());
        }
        return allPrerequisites.Distinct().ToList();
    }
}