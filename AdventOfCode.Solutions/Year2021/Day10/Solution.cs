namespace AdventOfCode.Solutions.Year2021.Day10;

class Solution : SolutionBase
{

    private char[] OpenTokens = new char[] { '(', '{', '<', '[' };
    private char[] CloseTokens = new char[] { ')', '}', '>', ']' };

    public Solution() : base(10, 2021, "") { }
    protected override string? SolvePartOne()
    {
        return this.Input.SplitByNewline()
            .Sum(chunk => ChunkErrorScore(chunk))
            .ToString();
    }

    protected override string? SolvePartTwo()
    {
        var incompleteChunks = this.Input.SplitByNewline()
            .Where(chunk => ChunkErrorScore(chunk) == 0)
            .Select(chunk => ChunkFixScore(chunk))
            .OrderBy(c => c)
            .ToList();
        return incompleteChunks
            .ElementAt((incompleteChunks.Count / 2))
            .ToString();
    }

    private int ChunkErrorScore(string chunk)
    {
        var pendingOpenToken = new List<char>();
        for (var i = 0; i < chunk.Length; i++)
        {
            var openIndex = Array.IndexOf(OpenTokens, chunk[i]);
            var closeIndex = Array.IndexOf(CloseTokens, chunk[i]);
            if (openIndex >= 0)
            {
                pendingOpenToken.Add(chunk[i]);
            }
            else if (closeIndex >= 0)
            {
                if (pendingOpenToken.Last() == OpenTokens[closeIndex])
                {
                    pendingOpenToken.RemoveAt(pendingOpenToken.Count - 1);
                }
                else
                {
                    return CloseTokens[closeIndex] switch
                    {
                        ')' => 3,
                        ']' => 57,
                        '}' => 1197,
                        '>' => 25137,
                        _ => 0
                    };
                }
            }
        }
        return 0;
    }

    private ulong ChunkFixScore(string chunk)
    {
        var pendingOpenToken = new List<char>();
        for (var i = 0; i < chunk.Length; i++)
        {
            var openIndex = Array.IndexOf(OpenTokens, chunk[i]);
            var closeIndex = Array.IndexOf(CloseTokens, chunk[i]);
            if (openIndex >= 0)
            {
                pendingOpenToken.Add(chunk[i]);
            }
            else if (closeIndex >= 0)
            {
                if (pendingOpenToken.Last() == OpenTokens[closeIndex])
                {
                    pendingOpenToken.RemoveAt(pendingOpenToken.Count - 1);
                }
            }
        }
        pendingOpenToken.Reverse();
        var closeTokenOrder = pendingOpenToken
            .Select(openToken => CloseTokens[Array.IndexOf(OpenTokens, openToken)]);
        var score = (ulong)0;
        foreach (var closeToken in closeTokenOrder)
        {
            score = (score * 5) + (closeToken switch
            {
                ')' => 1,
                ']' => 2,
                '}' => 3,
                '>' => 4,
                _ => 0
            });
        }
        return score;
    }
}
