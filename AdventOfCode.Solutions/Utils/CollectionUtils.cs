using AdventOfCode.Solutions.Models;

namespace AdventOfCode.Solutions.Utils;

public static class CollectionUtils
{
    public static T GetXY<T>(this IEnumerable<IEnumerable<T>> matrix, int x, int y) =>
        matrix.ElementAt(y).ElementAt(x);

    public static T GetXY<T>(this IEnumerable<IEnumerable<T>> matrix, Coordinate coordinate) =>
        matrix.GetXY(coordinate.X, coordinate.Y);

    public static void SetXY<T>(this T[][] matrix, int x, int y, T value)
    {
        matrix[y][x] = value;
    }

    public static Coordinate[] GetSurroundingCoordinates<T>(this IEnumerable<IEnumerable<T>> matrix, int x, int y)
    {
        var directions = new (int x, int y)[]
        {
            (-1, -1), (0, -1), (1, -1),
            (-1, 0),  /* :) */ (1, 0),
            (-1, 1),  (0, 1),  (1, 1)
        };

        var surroundingCoordinates = new List<Coordinate>();

        foreach (var (dx, dy) in directions)
        {
            var newX = x + dx;
            var newY = y + dy;

            if (newX >= 0 && newY >= 0 &&
                newY < matrix.Count() &&
                newX < matrix.ElementAt(newY).Count())
            {
                surroundingCoordinates.Add(new Coordinate(newX, newY));
            }
        }

        return [.. surroundingCoordinates];
    }

    public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> input)
        => input.Aggregate(input.First(), (intersector, next) => intersector.Intersect(next));

    public static string JoinAsStrings<T>(this IEnumerable<T> items, string delimiter = "") =>
        string.Join(delimiter, items);

    public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values) => values.Count() == 1
        ? new[] { values }
        : values.SelectMany(v =>
            Permutations(values.Where(x => x?.Equals(v) == false)), (v, p) => p.Prepend(v));
}
