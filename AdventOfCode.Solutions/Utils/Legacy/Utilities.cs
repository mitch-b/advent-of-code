/**
 * This utility class is largely based on:
 * https://github.com/jeroenheijmans/advent-of-code-2018/blob/master/AdventOfCode2018/Util.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public static class Utilities
    {
        public static int[] ToIntArray(this string str, string delimiter = "")
        {
            if (delimiter == "")
            {
                var result = new List<int>();
                foreach (char c in str)
                {
                    if (int.TryParse(c.ToString(), out int n))
                    {
                        result.Add(n);
                    }
                }

                return [.. result];
            }
            else
            {
                return str
                    .Split(delimiter)
                    .Where(n => int.TryParse(n, out int v))
                    .Select(n => Convert.ToInt32(n))
                    .ToArray();
            }

        }


        public static int MinOfMany(params int[] items)
        {
            var result = items[0];
            for (int i = 1; i < items.Length; i++)
            {
                result = Math.Min(result, items[i]);
            }
            return result;
        }

        public static int MaxOfMany(params int[] items)
        {
            var result = items[0];
            for (int i = 1; i < items.Length; i++)
            {
                result = Math.Max(result, items[i]);
            }
            return result;
        }

        // https://stackoverflow.com/a/3150821/419956 by @RonWarholic
        public static IEnumerable<T> Flatten<T>(this T[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    yield return map[row, col];
                }
            }
        }

        public static string JoinAsStrings<T>(this IEnumerable<T> items)
        {
            return string.Join("", items);
        }

        public static string[] SplitByNewline(this string input, bool shouldTrim = false, bool shouldIgnoreEmpty = true)
        {
            return [.. input
                .Split(["\r", "\n", "\r\n"], StringSplitOptions.None)
                .Where(s => !shouldIgnoreEmpty || !string.IsNullOrWhiteSpace(s))
                .Select(s => shouldTrim ? s.Trim() : s)];
        }

        /// <summary>
        /// I thought this was clever, but I'm sure this will make no sense to me the next time I try to use it
        /// </summary>
        /// <typeparam name="T">Type to return groups in (typical expectation, string[])</typeparam>
        /// <param name="input">Action to perform on, like Input</param>
        /// <param name="resultAction">Optional function where the output object can be manipulated from the collection of rows before being placed in output</param>
        /// <param name="rowAdd">Optional transformation applied to the row before being added to collection of row data passed to resultAction</param>
        /// <returns></returns>
        public static IEnumerable<T> SplitByEmptyLine<T>(this string input, Func<string[], T>? resultAction = null, Func<string, string[]>? rowAdd = null)
        {
            // default collate action (add string row to array output)
            rowAdd ??= (r => [r]);
            // default result action (add string array as-is)
            resultAction ??= lines => (T)(object)lines;

            var splitInput = input.SplitByNewline(true, false);
            var output = new List<T>();
            var lines = new List<string>();
            foreach (var row in splitInput)
            {
                if (string.IsNullOrWhiteSpace(row))
                {
                    output.Add(resultAction(lines.ToArray()));
                    lines.Clear();
                }
                else
                {
                    lines.AddRange(rowAdd(row));
                }
            }
            if (lines.Count != 0)
            {
                output.Add(resultAction([.. lines]));
            }
            return [.. output];
        }

        public static string Reverse(this string str)
        {
            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static int ManhattanDistance((int x, int y) a, (int x, int y) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public static double FindGCD(double a, double b) => (a % b == 0) ? b : FindGCD(b, a % b);

        public static double FindLCM(double a, double b) => a * b / FindGCD(a, b);

        public static void Repeat(this Action action, int count)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }

        // https://github.com/tslater2006/AdventOfCode2019
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var comparer = EqualityComparer<T>.Default;
            return (values.Count() == 1)
                ? [values]
                : values.SelectMany(v => Permutations(values.Where(x => comparer.Equals(x, v) == false)), (v, p) => p.Prepend(v));
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> array, int size)
        {
            for (var i = 0; i < (float)array.Count() / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }

        // https://stackoverflow.com/questions/49190830/is-it-possible-for-string-split-to-return-tuple
        public static void Deconstruct<T>(this IList<T> list, out T? first, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default;
            rest = list.Skip(1).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default!; // or throw
            second = list.Count > 1 ? list[1] : default!; // or throw
            rest = list.Skip(2).ToList();
        }

        public static (int, int) Add(this (int x, int y) a, (int x, int y) b) => (a.x + b.x, a.y + b.y);

        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                hashSet.Add(item);
            }
        }

        public static List<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}