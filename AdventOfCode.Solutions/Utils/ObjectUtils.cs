using System.ComponentModel;

namespace AdventOfCode.Solutions.Utils;

public static class ObjectUtils
{
    public static T? ConvertTo<T>(this string input) =>
        (T?)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(input);
}
