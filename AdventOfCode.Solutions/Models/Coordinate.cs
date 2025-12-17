using System.Numerics;

namespace AdventOfCode.Solutions.Models;

public struct Coordinate<T>(T x, T y, T? z = default) : IEquatable<Coordinate<T>>
    where T : struct, INumber<T>
{
    public T X { get; private set; } = x;
    public T Y { get; private set; } = y;
    public T Z { get; private set; } = z ?? T.Zero;

    public static implicit operator (T, T)(Coordinate<T> c) => (c.X, c.Y);
    public static implicit operator Coordinate<T>((T X, T Y) c) => new(c.X, c.Y);
    public static implicit operator (T, T, T)(Coordinate<T> c) => (c.X, c.Y, c.Z);
    public static implicit operator Coordinate<T>((T X, T Y, T Z) c) => new(c.X, c.Y, c.Z);

    public readonly bool InBounds<TElement>(IEnumerable<IEnumerable<TElement>> puzzle) =>
        X >= T.Zero && Y >= T.Zero &&
        int.CreateTruncating(Y) < puzzle.Count() &&
        int.CreateTruncating(X) < puzzle.ElementAt(int.CreateTruncating(Y)).Count();

    public readonly void Deconstruct(out T x, out T y)
    {
        x = this.X;
        y = this.Y;
    }

    public readonly void Deconstruct(out T x, out T y, out T z)
    {
        x = this.X;
        y = this.Y;
        z = this.Z;
    }

    public static Coordinate<T> operator +(Coordinate<T> a, Coordinate<T> b) =>
        new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Coordinate<T> operator -(Coordinate<T> a, Coordinate<T> b) =>
        new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public override readonly string ToString() => $"({X},{Y},{Z})";

    public override bool Equals(object? obj) => obj is Coordinate<T> other && this.Equals(other);

    public readonly bool Equals(Coordinate<T> c) => X == c.X && Y == c.Y && Z == c.Z;

    public override readonly int GetHashCode() => (X, Y, Z).GetHashCode();

    public static bool operator ==(Coordinate<T> a, Coordinate<T> b) => a.Equals(b);

    public static bool operator !=(Coordinate<T> a, Coordinate<T> b) => !(a == b);
}

// maintain backward compatibility...
public struct Coordinate(int x, int y, int z = 0) : IEquatable<Coordinate>
{
    private Coordinate<int> _inner = new(x, y, z);

    public int X { readonly get => _inner.X; private set => _inner = new(value, _inner.Y, _inner.Z); }
    public int Y { readonly get => _inner.Y; private set => _inner = new(_inner.X, value, _inner.Z); }
    public int Z { readonly get => _inner.Z; private set => _inner = new(_inner.X, _inner.Y, value); }

    public static implicit operator (int, int)(Coordinate c) => (c.X, c.Y);
    public static implicit operator Coordinate((int X, int Y) c) => new(c.X, c.Y);
    public static implicit operator (int, int, int)(Coordinate c) => (c.X, c.Y, c.Z);
    public static implicit operator Coordinate((int X, int Y, int Z) c) => new(c.X, c.Y, c.Z);
    public static implicit operator Coordinate<int>(Coordinate c) => new(c.X, c.Y, c.Z);
    public static implicit operator Coordinate(Coordinate<int> c) => new(c.X, c.Y, c.Z);

    public readonly bool InBounds<T>(IEnumerable<IEnumerable<T>> puzzle) =>
        X >= 0 && Y >= 0 && Y < puzzle.Count() && X < puzzle.ElementAt(Y).Count();

    public readonly void Deconstruct(out int x, out int y)
    {
        x = this.X;
        y = this.Y;
    }

    public readonly void Deconstruct(out int x, out int y, out int z)
    {
        x = this.X;
        y = this.Y;
        z = this.Z;
    }

    public static Coordinate operator +(Coordinate a, Coordinate b) =>
        new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Coordinate operator -(Coordinate a, Coordinate b) =>
        new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public override readonly string ToString() => $"({X},{Y},{Z})";

    public override bool Equals(object? obj) => obj is Coordinate other && this.Equals(other);

    public readonly bool Equals(Coordinate c) => X == c.X && Y == c.Y && Z == c.Z;

    public override readonly int GetHashCode() => (X, Y, Z).GetHashCode();

    public static bool operator ==(Coordinate a, Coordinate b) => a.Equals(b);

    public static bool operator !=(Coordinate a, Coordinate b) => !(a == b);
}