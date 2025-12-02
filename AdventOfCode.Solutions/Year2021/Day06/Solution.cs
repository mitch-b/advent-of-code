namespace AdventOfCode.Solutions.Year2021.Day06;

class Solution : SolutionBase
{
    public Solution() : base(06, 2021, "") { }

    protected override string? SolvePartOne()
    {
        var numberOfDays = 80;
        var fishSpawnWindow = GenerateFishSpawnClassification(7);
        AdvanceFishSpawnDays(fishSpawnWindow, numberOfDays);
        return fishSpawnWindow.Sum(n => (long)n).ToString();
    }

    protected override string? SolvePartTwo()
    {
        var numberOfDays = 256;
        var fishSpawnWindow = GenerateFishSpawnClassification(7);
        AdvanceFishSpawnDays(fishSpawnWindow, numberOfDays);
        return fishSpawnWindow.Sum(n => (long)n).ToString();
    }

    private long[] GenerateFishSpawnClassification(int spawnAge)
    {
        var fishSpawnWindow = new long[spawnAge + 2];
        var lanternfish = this.Input.Split(',')
            .Select(f => int.Parse(f));
        foreach (var fishAge in lanternfish)
        {
            fishSpawnWindow[fishAge]++;
        }
        return fishSpawnWindow;
    }

    private void AdvanceFishSpawnDays(long[] fishSpawnWindow, int days)
    {
        var spawnAge = fishSpawnWindow.Length - 2;
        for (var i = 0; i < days; i++)
        {
            var newFishCount = fishSpawnWindow[spawnAge];
            // shift # of fish left each day in array
            for (var day = spawnAge + 1; day >= 0; day--)
            {
                var priorWindowCount = newFishCount;
                newFishCount = fishSpawnWindow[day];
                fishSpawnWindow[day] = priorWindowCount;
            }
            // set all newly spawned fish around the spawnAge
            fishSpawnWindow[spawnAge + 1] = newFishCount;
            fishSpawnWindow[spawnAge - 1] += newFishCount;

            if (this.Debug)
            {
                Console.WriteLine($"After {(i + 1).ToString("D2")} days: {fishSpawnWindow[0]},{fishSpawnWindow[1]},{fishSpawnWindow[2]},{fishSpawnWindow[3]},{fishSpawnWindow[4]},{fishSpawnWindow[5]},{fishSpawnWindow[6]},{fishSpawnWindow[7]},{fishSpawnWindow[8]}");
            }
        }
        return;
    }
}
