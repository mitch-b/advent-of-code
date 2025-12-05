namespace AdventOfCode.Solutions.Models;

public class ComparableRange<T>(T start, T end) where T : IComparable<T>
{
    public T Start { get; set; } = start;
    public T End { get; set; } = end;

    public bool Contains(T value) => Start.CompareTo(value) <= 0 && End.CompareTo(value) >= 0;

    public T GetSize()
    {
        dynamic end = End;
        dynamic start = Start;
        return end - start + 1; // +1 to count the boundary of range
    }
}