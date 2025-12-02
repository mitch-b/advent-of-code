using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020.Day04;

class Solution : SolutionBase
{
    private readonly IEnumerable<Passport> passports;
    public Solution() : base(04, 2020, "")
    {
        Func<string[], Passport> passportFromFields = (fields) => Passport.FromFields(fields);
        Func<string, string[]> actionOnRowAdd = (row) => row.Split(' ');
        passports = Input.SplitByEmptyLine(passportFromFields, actionOnRowAdd);
    }

    protected override string? SolvePartOne()
    {
        return passports.Count(p => p.IsValid(false)).ToString();
    }

    protected override string? SolvePartTwo()
    {
        return passports.Count(p => p.IsValid(true)).ToString();
    }
}

class PassportField
{
    public string ShortCode { get; set; }
    public string Value { get; set; }
    public bool IsValid(bool includeStricterChecks = false)
    {
        if (ValidationFunction != null)
        {
            if (includeStricterChecks)
            {
                return ValidationFunction(this);
            }
            return !string.IsNullOrWhiteSpace(this.Value) || ShortCode == "cid"; /* ew */
        }
        return false;
    }
    public Func<PassportField, bool> ValidationFunction = (passportField) => !string.IsNullOrWhiteSpace(passportField.Value);
}

class Passport
{
    public PassportField[] passportFields = new[]
    {
            new PassportField { ShortCode = "byr", ValidationFunction = (p) => {
                return int.TryParse(p.Value, out int year) && year >= 1920 && year <= 2002;
            } }, // Birth Year
            new PassportField { ShortCode = "iyr", ValidationFunction = (p) => {
                return int.TryParse(p.Value, out int year) && year >= 2010 && year <= 2020;
            } }, // Issue Year
            new PassportField { ShortCode = "eyr", ValidationFunction = (p) => {
                return int.TryParse(p.Value, out int year) && year >= 2020 && year <= 2030;
            } }, // Expiration Year
            new PassportField { ShortCode = "hgt", ValidationFunction = (p) => {
                if (p.Value?.Contains("cm") == true)
                {
                    return int.TryParse(p.Value.Replace("cm", ""), out int height) && height >= 150 && height <= 193;
                }
                else if (p.Value?.Contains("in") == true)
                {
                    return int.TryParse(p.Value.Replace("in", ""), out int height) && height >= 59 && height <= 76;
                }
                return false;
            } }, // Height
            new PassportField { ShortCode = "hcl", ValidationFunction = (p) => {
                return new Regex("#[0-9a-f]{6}").Match(p.Value ?? string.Empty).Success;
            } }, // Hair Color
            new PassportField { ShortCode = "ecl", ValidationFunction = (p) => {
                return (new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth" }).Contains(p.Value);
            } }, // Eye Color
            new PassportField { ShortCode = "pid", ValidationFunction = (p) => {
                return p.Value?.Length == 9 && int.TryParse(p.Value, out int _);
            } }, // Passport ID
            new PassportField { ShortCode = "cid", ValidationFunction = (p) => true }  // Country ID
        };

    public Passport()
    {

    }

    public static Passport FromFields(IEnumerable<string> fields)
    {
        var passport = new Passport();
        foreach (var field in fields)
        {
            var parts = field.Split(':').Select(s => s.Trim()).ToArray();
            var passportField = passport.passportFields.FirstOrDefault(f => f.ShortCode == parts[0]);
            passportField?.Value = parts[1];
        }
        return passport;
    }

    public bool IsValid(bool includeStricterChecks) => passportFields.All(field => field.IsValid(includeStricterChecks));
}