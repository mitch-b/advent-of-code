namespace AdventOfCode.Solutions.Year2025.Day03;

class Solution : SolutionBase
{
    public Solution() : base(03, 2025, "", useDebugInput: false) { }

    protected override string? SolvePartOne()
    {
        var banks = Input.SplitByNewline()
            .SelectMany(line => line.Split());
        var maxJoltages = new List<int>();
        foreach (var bank in banks)
        {
            var combinations = bank.Combinations(2);
            var maxValue = 0;
            foreach (var combination in combinations)
            {
                var sum = int.Parse(string.Join("", combination));
                if (sum > maxValue)
                {
                    maxValue = sum;
                }
            }
            maxJoltages.Add(maxValue);
        }
        return maxJoltages.Sum().ToString();
    }

    protected override string? SolvePartTwo()
    {
        var banks = Input.SplitByNewline()
            .SelectMany(line => line.Split());
        var maxJoltages = new List<decimal>();
        foreach (var bank in banks)
        {
            decimal maxValue = decimal.Parse(MaxNumber(bank));
            maxJoltages.Add(maxValue);
        }
        return maxJoltages.Sum().ToString();
    }

    private string MaxNumber(string digits, int digitsToKeep = 12)
    {
        var selectedDigits = new List<char>();
        var digitsLeftToDrop = digits.Length - digitsToKeep;

        foreach (var digit in digits)
        {
            while (selectedDigits.Count != 0
                && digitsLeftToDrop > 0
                && selectedDigits.Last() < digit)
            {
                selectedDigits.RemoveAt(selectedDigits.Count - 1);
                digitsLeftToDrop--;
            }

            if (selectedDigits.Count < digitsToKeep)
            {
                selectedDigits.Add(digit);
                continue;
            }

            digitsLeftToDrop--;
        }

        return string.Join("", selectedDigits);
    }
}