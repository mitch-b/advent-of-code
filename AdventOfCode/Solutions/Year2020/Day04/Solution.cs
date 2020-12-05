using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{
    class Day04 : ASolution
    {
        private readonly IEnumerable<Passport> passports;
        public Day04() : base(04, 2020, "")
        {
            passports = GetPassports(Input.SplitByNewline(true, false));
        }

        protected override string SolvePartOne()
        {
            return passports.Count(p => p.IsValid).ToString();
        }

        protected override string SolvePartTwo()
        {
            return passports.Count(p => p.IsValid).ToString();
        }

        private IEnumerable<Passport> GetPassports(string[] inputRows)
        {
            var passports = new List<Passport>();
            var tempFields = new List<string>();
            foreach(var row in inputRows)
            {
                if (string.IsNullOrWhiteSpace(row))
                {
                    passports.Add(Passport.FromFields(tempFields));
                    tempFields.Clear();
                }
                else
                {
                    tempFields.AddRange(row.Split(' '));
                }
            }
            return passports;
        }
    }
    class PassportField
    {
        public string ShortCode { get; set; }
        public string Value { get; set; }
        public bool IsValid()
        {
            if (ValidationFunction != null)
            {
                return ValidationFunction(this);
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
                if (passportField != null)
                {
                    passportField.Value = parts[1];
                }
            }
            return passport;
        }

        public bool IsValid => passportFields.All(field => field.IsValid());
    }
}
