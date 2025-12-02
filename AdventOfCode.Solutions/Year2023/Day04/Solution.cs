using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2023.Day04;

class Solution : SolutionBase
{
    public Solution() : base(04, 2023, "") { }

    protected override string? SolvePartOne()
    {
        var cards = Input.SplitByNewline(true)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => new Card(c))
            .ToList();
        return cards.Sum(c => c.GetScore()).ToString();
    }

    protected override string? SolvePartTwo()
    {
        var cardPile = new List<Card>();
        var cards = Input.SplitByNewline(true)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => new Card(c))
            .ToList();
        foreach (var card in cards)
        {
            cardPile.Add(card);
            var countOfCardNumberInPile = cardPile.Count(c => c.CardNumber == card.CardNumber);
            var wonCardNumbers = card.WonExtraCardNumbers();
            for (var n = 0; n < countOfCardNumberInPile; n++)
            {
                foreach (var wonCardNumber in wonCardNumbers)
                {
                    cardPile.Add(cards.First(c => c.CardNumber == wonCardNumber));
                }
            }
        }
        return cardPile.Count.ToString();
    }
}


partial class Card
{
    private readonly Regex _cardStringRegex = CardRegex();
    public int CardNumber { get; set; }
    public List<int> WinningNumbers { get; set; }
    public List<int> PlayerNumbers { get; set; }

    public Card(string cardString)
    {
        if (!_cardStringRegex.IsMatch(cardString))
        {
            throw new ArgumentException("Invalid card string...uh oh");
        }
        var cardMatch = _cardStringRegex.Match(cardString);
        CardNumber = int.Parse(cardMatch.Groups[1].Value);
        WinningNumbers = cardMatch.Groups[2].Value.Split(" ")
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => int.Parse(v.Trim())).ToList();
        PlayerNumbers = cardMatch.Groups[3].Value.Split(" ")
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => int.Parse(v.Trim())).ToList();
    }

    public int GetScore()
    {
        int score = 0;
        foreach (var number in PlayerNumbers)
        {
            if (WinningNumbers.Contains(number))
            {
                if (score == 0)
                {
                    score = 1;
                }
                else
                {
                    score *= 2;
                }
            }
        }
        return score;
    }

    public int GetMatchingNumberCount()
    {
        return PlayerNumbers.Count(WinningNumbers.Contains);
    }

    public List<int> WonExtraCardNumbers()
    {
        if (GetMatchingNumberCount() > 0)
        {
            return Enumerable.Range(CardNumber + 1, GetMatchingNumberCount()).ToList();
        }
        return [];
    }

    // VSCode had me do this... never seen this before...
    [GeneratedRegex(@"Card\s+(\d+):\s+(.*?)\s+\|\s+(.*)", RegexOptions.Compiled)]
    private static partial Regex CardRegex();
}