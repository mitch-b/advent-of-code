/**
 * This utility class is largely based on:
 * https://github.com/jeroenheijmans/advent-of-code-2018/blob/master/AdventOfCode2018/Util.cs
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                return [.. str
                    .Split(delimiter)
                    .Where(n => int.TryParse(n, out int v))
                    .Select(n => Convert.ToInt32(n))];
            }

        }

        public static T[,] ToRectArray<T>(this string multiLineStr)
        {
            var rows = multiLineStr.SplitByNewline();
            var rectArray = new T[rows.Length, rows[0].Length];
            for (var i = 0; i < rows.Length; i++)
            {
                for (var j = 0; j < rows[0].Length; j++)
                {
                    rectArray[i, j] = (T)Convert.ChangeType(rows[i][j].ToString(), typeof(T));
                }
            }
            return rectArray;
        }

        public static long[] ToLongArray(this string str, string delimiter = "")
        {
            if (delimiter == "")
            {
                var result = new List<long>();
                foreach (char c in str)
                {
                    if (long.TryParse(c.ToString(), out long n))
                    {
                        result.Add(n);
                    }
                }

                return [.. result];
            }
            else
            {
                return [.. str
                    .Split(delimiter)
                    .Where(n => long.TryParse(n, out long v))
                    .Select(n => Convert.ToInt64(n))];
            }

        }

        public static int[] ToIntArray(this string[] array)
        {
            return string.Join(",", array).ToIntArray(",");
        }

        public static void WriteLine(object str)
        {
            Console.WriteLine(str);
            Trace.WriteLine(str);
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

        public static double FindGCD(double a, double b)
        {
            if (a == 0 || b == 0)
            {
                return Math.Max(a, b);
            }

            return (a % b == 0) ? b : FindGCD(b, a % b);
        }

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
            return (values.Count() == 1) ? [values] : values.SelectMany(v => Permutations(values.Where(x => !EqualityComparer<T>.Default.Equals(x, v))), (v, p) => p.Prepend(v));
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values, int subcount)
        {
            foreach (var combination in Combinations(values, subcount))
            {
                var perms = Permutations(combination);
                foreach (int i in Enumerable.Range(0, perms.Count()))
                {
                    yield return perms.ElementAt(i);
                }
            }
        }

        private static IEnumerable<int[]> Combinations(int subcount, int length)
        {
            int[] res = new int[subcount];
            Stack<int> stack = new Stack<int>(subcount);
            stack.Push(0);
            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();
                while (value < length)
                {
                    res[index++] = value++;
                    stack.Push(value);
                    if (index != subcount)
                    {
                        continue;
                    }

                    yield return (int[])res.Clone();
                    break;
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> values, int subcount)
        {
            if (values.Count() < subcount)
            {
                throw new ArgumentException("Array Length can't be less than sub-array length");
            }

            if (subcount < 1)
            {
                throw new ArgumentException("Subarrays must be at least length 1 long");
            }

            T[] res = new T[subcount];
            foreach (var combination in Combinations(subcount, values.Count()))
            {
                foreach (int i in Enumerable.Range(0, subcount))
                {
                    res[i] = values.ElementAt(combination[i]);
                }

                yield return res;
            }
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> array, int size)
        {
            for (var i = 0; i < (float)array.Count() / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }

        /// <summary>
        /// Rotates an IEnumarable by the requested amount
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="rotations">Number of steps to take, positive numbers move indices up (item at end moves to start), negative numbers move them down (first item moves to end of array)</param>
        /// <returns></returns>
        public static IEnumerable<T> Rotate<T>(this IEnumerable<T> array, int rotations)
        {
            for (int i = 0; i < array.Count(); i++)
            {
                yield return i + rotations >= 0 ? array.ElementAt((i + rotations) % array.Count()) : array.ElementAt((i + rotations) + array.Count());
            }
        }

        // https://stackoverflow.com/questions/49190830/is-it-possible-for-string-split-to-return-tuple
        public static void Deconstruct<T>(this IList<T> list, out T? first, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default; // or throw
            rest = [.. list.Skip(1)];
        }

        public static void Deconstruct<T>(this IList<T> list, out T? first, out T? second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default; // or throw
            second = list.Count > 1 ? list[1] : default; // or throw
            rest = [.. list.Skip(2)];
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
            return [.. listToClone.Select(item => (T)item.Clone())];
        }

        public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> input)
            => input.Aggregate(input.First(), (intersector, next) => intersector.Intersect(next));

        //https://stackoverflow.com/questions/2641326/finding-all-positions-of-substring-in-a-larger-string-in-c-sharp
        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException("the string to find may not be empty", nameof(value));
            }

            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                {
                    break;
                }

                yield return index;
            }
        }
    }
}
