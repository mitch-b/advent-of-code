using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020.Day02;

class Solution : SolutionBase
{
    public Solution() : base(02, 2020, "") { }

    protected override string? SolvePartOne()
    {
        var records = this.Input.SplitByNewline(true);
        var passwords = records.Select(r => new Password(r));
        return passwords.Count(p => p.IsValid()).ToString();
    }

    protected override string? SolvePartTwo()
    {
        var records = this.Input.SplitByNewline(true);
        var passwords = records.Select(r => new Password(r));
        return passwords.Count(p => p.IsValidV2()).ToString();
    }
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

class Password
{
    Regex passwordPolicyExtractor = new Regex(@"(\d+)-(\d+)\s(\S): (.*)");
    private readonly int firstValue;
    private readonly int secondValue;
    private char requiredPhrase;
    private string password;
    public Password(string phrase)
    {
        var match = passwordPolicyExtractor.Match(phrase);
        if (match.Success)
        {
            firstValue = int.Parse(match.Groups[1].Value);
            secondValue = int.Parse(match.Groups[2].Value);
            requiredPhrase = char.Parse(match.Groups[3].Value);
            password = match.Groups[4].Value;
        }
    }

    public bool IsValid()
    {
        var instancesOfRequiredPhrase = password.ToCharArray().Count(c => c == requiredPhrase);
        return instancesOfRequiredPhrase >= firstValue && instancesOfRequiredPhrase <= secondValue;
    }

    public bool IsValidV2()
    {
        var inFirstPosition = password[firstValue - 1] == requiredPhrase;
        var inSecondPosition = password[secondValue - 1] == requiredPhrase;
        // character in one of the two positions, but not both
        return inFirstPosition != inSecondPosition;
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

