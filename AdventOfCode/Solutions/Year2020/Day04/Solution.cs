using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return null;
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
            new PassportField { ShortCode = "byr" }, // Birth Year
            new PassportField { ShortCode = "iyr" }, // Issue Year
            new PassportField { ShortCode = "eyr" }, // Expiration Year
            new PassportField { ShortCode = "hgt" }, // Height
            new PassportField { ShortCode = "hcl" }, // Hair Color
            new PassportField { ShortCode = "ecl" }, // Eye Color
            new PassportField { ShortCode = "pid" }, // Passport ID
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
