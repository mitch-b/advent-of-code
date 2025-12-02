namespace AdventOfCode.Solutions.Year2024.Day09;

class Solution : SolutionBase
{
    public Solution() : base(09, 2024, "") { }

    protected override string SolvePartOne()
    {
        var disk = new Disk(Input);
        disk.Defragment();
        return disk.Checksum().ToString();
    }

    protected override string SolvePartTwo()
    {
        var disk = new Disk(Input);
        disk.DefragmentFullFileId();
        return disk.Checksum().ToString();
    }
}

class Disk
{
    private List<(int position, int size)> Gaps { get; set; } = [];
    private List<(int position, int size)> Files { get; set; } = [];
    public long?[] FileIds { get; private set; }
    public Disk(string data)
    {
        var blockSize = data.Select(c => c).Sum(c => long.Parse(c.ToString()));
        FileIds = new long?[blockSize];
        bool isFileLength = false;
        bool isFreeSpace = false;
        var position = 0;
        var fileId = 0;
        for (int i = 0; i < data.Length; i++)
        {
            var item = int.Parse(data[i].ToString());
            isFileLength = i % 2 == 0;
            isFreeSpace = i % 2 != 0;
            if (isFileLength)
            {
                Files.Add((position, item));
                for (int x = 0; x < item; x++)
                {
                    FileIds[position++] = fileId;
                }
                fileId++;
            }
            else if (isFreeSpace)
            {
                Gaps.Add((position, item));
                for (int x = 0; x < item; x++)
                {
                    FileIds[position++] = null;
                }
            }
        }
    }

    public void Defragment()
    {
        for (int i = FileIds.Length - 1; i >= 0; i--)
        {
            if (FileIds[i] is null)
            {
                continue;
            }
            for (int x = 0; x < i; x++)
            {
                if (FileIds[x] is null)
                {
                    FileIds[x] = FileIds[i];
                    FileIds[i] = null;
                    break;
                }
            }
        }
    }

    public void DefragmentFullFileId()
    {
        // foreach file starting at end
        List<(int position, int size)> reversedFiles = [.. Files];
        reversedFiles.Reverse();
        foreach (var file in reversedFiles)
        {
            // find first gap
            for (var i = 0; i < Gaps.Count; i++)
            {
                var gap = Gaps[i];
                if (gap.size >= file.size)
                {
                    var counter = 0;
                    for (int x = gap.position; x < gap.position + file.size; x++)
                    {
                        FileIds[x] = FileIds[file.position + counter];
                        FileIds[file.position + counter++] = null;
                    }
                    Gaps.RemoveAt(i);
                    if (gap.size > file.size)
                    {
                        Gaps.Insert(i, (gap.position + file.size, gap.size - file.size));
                    }
                    break;
                }
            }
        }
    }

    public long Checksum()
    {
        long checksum = 0;
        var fileId = 0;
        for (int i = 0; i < FileIds.Length; i++)
        {
            if (FileIds[i] is not null)
            {
                checksum += fileId * FileIds[i]!.Value;
            }
            fileId++;
        }
        return checksum;
    }
}
