using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Utils;

public static class RegexUtils
{
    public static IList<T> ExtractFromString<T>(this string str, string regexPattern, bool ignoreFirstCaptureGroup = true)
    {
        var matches = Regex.Match(str, regexPattern);
        var extractedData = new List<T>();
        var iterator = 0;
        foreach (Group match in matches.Groups.Cast<Group>())
        {
            if (++iterator <= 1 && ignoreFirstCaptureGroup)
            {
                continue;
            }

            var matchValue = match?.ToString();
            if (string.IsNullOrEmpty(matchValue))
            {
                continue;
            }

            var convertedValue = matchValue.ConvertTo<T>();
            if (convertedValue is { } value)
            {
                extractedData.Add(value);
            }
        }
        return extractedData;
    }
    public static IList<T> ExtractMatchesFromString<T>(this string str, string regexPattern)
    {
        var matches = Regex.Matches(str, regexPattern);
        var extractedData = new List<T>();
        foreach (Match match in matches)
        {
            var matchValue = match?.ToString();
            if (string.IsNullOrEmpty(matchValue))
            {
                continue;
            }

            var convertedValue = matchValue.ConvertTo<T>();
            if (convertedValue is { } value)
            {
                extractedData.Add(value);
            }
        }
        return extractedData;
    }
}
