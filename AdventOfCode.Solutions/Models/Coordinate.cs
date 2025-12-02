namespace AdventOfCode.Solutions.Models;

public struct Coordinate(int x, int y, int z = 0) : IEquatable<Coordinate>
{
    public int X { get; private set; } = x;
    public int Y { get; private set; } = y;
    public int Z { get; private set; } = z;

    public static implicit operator (int, int)(Coordinate c) => (c.X, c.Y);
    public static implicit operator Coordinate((int X, int Y) c) => new(c.X, c.Y);
    public static implicit operator (int, int, int)(Coordinate c) => (c.X, c.Y, c.Z);
    public static implicit operator Coordinate((int X, int Y, int Z) c) => new(c.X, c.Y, c.Z);

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