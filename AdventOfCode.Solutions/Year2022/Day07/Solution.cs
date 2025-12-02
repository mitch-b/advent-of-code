using AdventOfCode.Solutions.Models;

namespace AdventOfCode.Solutions.Year2022.Day07;

class Solution : SolutionBase
{
    public Solution() : base(07, 2022, "") { }

    private void Setup()
    {
        _tree = new Node<long>(0, "");

        var lines = Input.SplitByNewline().Skip(1).ToArray(); // skip '$ cd /'
        var currentNodePath = "";
        var activeNode = _tree.TryGetNode("");
        for (var i = 0; i < lines.Count(); i++)
        {
            var line = lines[i];
            var parts = line.Split(' ');
            if (parts[0] == "$")
            {
                // command
                if (parts[1] == "ls") // we are capturing info!
                {
                    continue;
                }

                if (parts[1] == "cd")
                {
                    if (parts[2] == "..")
                    {
                        // pop up tree
                        currentNodePath = string.Join("/", currentNodePath.Split('/')[..^1]);
                        activeNode = _tree.TryGetNode(currentNodePath);
                        continue;
                    }
                    currentNodePath += $"/{parts[2]}";
                    activeNode = activeNode!.TryGetNode(parts[2]);
                    continue;
                }
            }
            if (parts[0] == "dir")
            {
                // add a node (that we likely haven't traversed yet)
                activeNode!.Children.Add(new Node<long>(default, parts[1]));
                continue;
            }

            var fileSize = long.Parse(parts[0]);
            var fileName = parts[1];
            activeNode?.Children.Add(new Node<long>(fileSize, fileName));
        }

        _ = UpdateDirectorySizes(_tree);
    }

    private Node<long> _tree = new Node<long>(0, "");

    protected override string? SolvePartOne()
    {
        Setup();

        var valueCount = GetDirectorySizesUnderLimit(_tree);
        return valueCount.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var totalDiskSpace = 70000000;
        var neededDiskSpace = 30000000;
        var usedDiskSpace = _tree.Value;
        var availableDiskSpace = totalDiskSpace - usedDiskSpace;
        var needToDeleteAtLeast = neededDiskSpace - availableDiskSpace;
        var directories = GetDirectoriesOverLimit(_tree, needToDeleteAtLeast);
        return directories.Min(d => d.Value).ToString();
    }

    private long UpdateDirectorySizes(Node<long> node)
    {
        var nodeValue = node.Value;
        foreach (var child in node.Children)
        {
            if (child.Children.Count > 0)
            {
                UpdateDirectorySizes(child);
            }
        }
        if (node.Children.Count > 0)
        {
            nodeValue = node.Children.Sum(n => n.Value);
        }

        node.Value = nodeValue;
        return node.Value;
    }

    private long GetDirectorySizesUnderLimit(Node<long> node)
    {
        var foundValue = node.Value <= 100000 ? node.Value : 0;
        foreach (var child in node.Children)
        {
            if (child.Children.Count != 0)
            {
                foundValue += GetDirectorySizesUnderLimit(child);
            }
        }
        return foundValue;
    }

    private List<Node<long>> GetDirectoriesOverLimit(Node<long> node, long limit)
    {
        if (node.Children.Count == 0)
        {
            return new List<Node<long>>();
        }

        var nodes = new List<Node<long>>();
        if (node.Value >= limit && node.Children.Count > 0)
        {
            nodes.Add(node);
        }

        foreach (var child in node.Children)
        {
            if (child.Children.Any())
            {
                nodes.AddRange(GetDirectoriesOverLimit(child, limit));
            }
        }
        return nodes;
    }
}
