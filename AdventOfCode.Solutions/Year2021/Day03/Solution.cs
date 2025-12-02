namespace AdventOfCode.Solutions.Year2021.Day03;

class Solution : SolutionBase
{
    public Solution() : base(03, 2021, "") { }

    protected override string? SolvePartOne()
    {
        var codes = this.Input.SplitByNewline();
        if (codes.Length == 0)
        {
            return null;
        }

        var gammaRate = string.Empty;
        var epsilonRate = string.Empty;
        var majorityCount = codes.Length / 2;
        var codeLength = codes[0].Length;
        for (var i = 0; i < codeLength; i++)
        {
            gammaRate += codes.Count(c => c[i] == '1') >= majorityCount ? "1" : "0";
            epsilonRate += codes.Count(c => c[i] == '1') >= majorityCount ? "0" : "1";
        }
        var gammaRateValue = Convert.ToInt32(gammaRate, 2);
        var epsilonRateValue = Convert.ToInt32(epsilonRate, 2);
        return (gammaRateValue * epsilonRateValue).ToString();
    }

    protected override string? SolvePartTwo()
    {
        var codes = this.Input.SplitByNewline();
        if (codes.Length == 0)
        {
            return null;
        }

        var filteredCodes = new List<string>(codes);
        var codeLength = codes[0].Length;
        for (var i = 0; i < codeLength; i++)
        {
            var ones = filteredCodes.Count(c => c[i] == '1');
            var zeroes = filteredCodes.Count(c => c[i] == '0');
            var commonBit = ones >= zeroes ? '1' : '0';
            filteredCodes = filteredCodes.Where(c => c[i] == commonBit).ToList();
            if (filteredCodes.Count == 1) break;
        }
        var oxygenGeneratorRatingValue = Convert.ToInt32(filteredCodes.First(), 2);

        filteredCodes = new List<string>(codes);
        for (var i = 0; i < codeLength; i++)
        {
            var ones = filteredCodes.Count(c => c[i] == '1');
            var zeroes = filteredCodes.Count(c => c[i] == '0');
            var commonBit = ones >= zeroes ? '0' : '1';
            filteredCodes = filteredCodes.Where(c => c[i] == commonBit).ToList();
            if (filteredCodes.Count == 1) break;
        }
        var co2ScrubberRatingValue = Convert.ToInt32(filteredCodes.First(), 2);

        return (oxygenGeneratorRatingValue * co2ScrubberRatingValue).ToString();
    }
}
