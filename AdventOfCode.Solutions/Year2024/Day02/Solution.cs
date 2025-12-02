namespace AdventOfCode.Solutions.Year2024.Day02;

class Solution : SolutionBase
{
    private readonly List<int[]> reports;
    public Solution() : base(02, 2024, "")
    {
        this.reports = Input.SplitByNewline()
            .Select(x => x.Split(' ').Select(int.Parse).ToArray())
            .ToList();
    }

    protected override string? SolvePartOne()
    {
        var goodReports = GetGoodReports(this.reports, useDampener: false);
        return goodReports.Count.ToString();
    }

    protected override string? SolvePartTwo()
    {
        var goodReports = GetGoodReports(this.reports, useDampener: true);
        return goodReports.Count.ToString();
    }

    private List<int[]> GetGoodReports(List<int[]> reports, bool useDampener)
    {
        var goodReports = new List<int[]>();
        foreach (var report in reports)
        {
            var result = GetReportStatus(report);
            if (result.IsGood)
            {
                goodReports.Add(report);
            }
            else if (useDampener)
            {
                foreach (var badIndex in result.BadIndexes)
                {
                    var newReport = report.ToList();
                    newReport.RemoveAt(badIndex);
                    result = GetReportStatus([.. newReport]);
                    if (result.IsGood)
                    {
                        goodReports.Add(report);
                        break;
                    }
                }
            }
        }
        return goodReports;
    }

    public ReportStatus GetReportStatus(int[] report)
    {
        var increasing = report[0] < report[1];
        var decreasing = report[0] > report[1];
        var previous = report[0];
        for (var i = 1; i < report.Length; i++)
        {
            var next = report[i];
            var diff = Math.Abs(previous - next);
            if (
                diff == 0 ||
                diff > 3 ||
                (increasing && (previous > next)) ||
                (decreasing && (previous < next))
            )
            {
                int[] indexes = i < 3 ? [0, 1, 2] : [i - 1, i];
                return new ReportStatus(false, indexes);
            }
            previous = next;
        }
        return new ReportStatus(true, []);
    }

    public record ReportStatus(bool IsGood, int[] BadIndexes);
}
