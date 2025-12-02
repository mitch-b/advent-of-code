namespace AdventOfCode.Solutions.Year2023.Day05;

class Solution : SolutionBase
{
    public Solution() : base(05, 2023, "") { }

    protected override string SolvePartOne()
    {
        var seedInput = Input.SplitByNewline().First();
        // Parse input
        List<long> initialSeeds = ParseSeeds(Input);
        Dictionary<string, List<ConversionMap>> conversionMaps = ParseConversionMaps(Input.SplitByNewline().Skip(1).JoinAsStrings("\n"));

        // // Find the lowest location number
        long lowestLocationNumber = FindLowestLocationNumber(initialSeeds, conversionMaps);

        // // Output the result
        Console.WriteLine("Lowest Location Number: " + lowestLocationNumber);

        return lowestLocationNumber.ToString();
    }

    protected override string SolvePartTwo()
    {
        var seedInput = Input.SplitByNewline().First();
        // Parse input
        List<long> initialSeeds = ParseSeeds2(Input);
        Dictionary<string, List<ConversionMap>> conversionMaps = ParseConversionMaps(Input.SplitByNewline().Skip(1).JoinAsStrings("\n"));

        // // Find the lowest location number
        long lowestLocationNumber = FindLowestLocationNumber(initialSeeds, conversionMaps);

        // // Output the result
        Console.WriteLine("Lowest Location Number: " + lowestLocationNumber);

        return lowestLocationNumber.ToString();
    }

    private List<long> ParseSeeds(string input)
    {
        // Extract and return the initial seed numbers from the input
        // You may need to adjust this based on the actual input format
        // For example, you can use regular expressions or string manipulation
        return input.Split(["seeds: ", "\n"], StringSplitOptions.RemoveEmptyEntries)
                    .First()
                    .Split(' ')
                    .Select(long.Parse)
                    .ToList();
    }

    private List<long> ParseSeeds2(string input)
    {
        // Extract and return the initial seed numbers from the input
        // You may need to adjust this based on the actual input format
        // For example, you can use regular expressions or string manipulation
        var seedInput = input.Split(["seeds: ", "\n"], StringSplitOptions.RemoveEmptyEntries)
                    .First()
                    .Split(' ')
                    .Select(long.Parse)
                    .ToList();
        var seeds = new List<long>();
        for (var i = 0; i < seedInput.Count - 1; i += 2)
        {
            for (long j = seedInput[i]; j < seedInput[i + 1]; j++)
            {
                seeds.Add(j);
            }
        }
        return seeds;
    }

    private Dictionary<string, List<ConversionMap>> ParseConversionMaps(string input)
    {
        // Extract and return the conversion maps from the input
        // You may need to adjust this based on the actual input format
        // For example, you can use regular expressions or string manipulation
        var conversionMaps = new Dictionary<string, List<ConversionMap>>();
        string[] sections = input.Split(["\n"], StringSplitOptions.RemoveEmptyEntries);

        string? currentMapType = null;
        foreach (string section in sections)
        {
            if (section.EndsWith(" map:"))
            {
                currentMapType = section.Replace(" map:", "");
                conversionMaps[currentMapType] = new List<ConversionMap>();
            }
            else if (currentMapType != null)
            {
                // Parse each line of the map
                var values = section.Split(' ').Select(long.Parse).ToArray();
                conversionMaps[currentMapType].Add(new ConversionMap(values[0], values[1], values[2]));
            }
        }

        return conversionMaps;
    }

    private long FindLowestLocationNumber(List<long> initialSeeds, Dictionary<string, List<ConversionMap>> conversionMaps)
    {
        long lowestLocationNumber = long.MaxValue;

        foreach (long seed in initialSeeds)
        {
            long locationNumber = ConvertCategory(seed, "seed", "location", conversionMaps);
            lowestLocationNumber = Math.Min(lowestLocationNumber, locationNumber);
        }

        return lowestLocationNumber;
    }

    private long ConvertCategory(long number, string sourceCategory, string destinationCategory, Dictionary<string, List<ConversionMap>> conversionMaps)
    {
        // Perform the conversion using the specified maps
        long result = number;

        var mapName = conversionMaps.Keys.FirstOrDefault(k => k.StartsWith($"{sourceCategory}-to-"));

        foreach (var map in conversionMaps[mapName!])
        {
            if (number >= map.SourceStart && number < map.SourceStart + map.RangeLength)
            {
                result = map.DestinationStart + (number - map.SourceStart);
                break;
            }
        }

        var mappedCategory = mapName!.Replace($"{sourceCategory}-to-", "");

        if (mappedCategory != destinationCategory)
        {
            // Recursively convert to the destination category
            result = ConvertCategory(result, mappedCategory, destinationCategory, conversionMaps);
        }

        return result;
    }

    private long GetConvertedValueFromMaps(long number, List<ConversionMap> maps)
    {
        // Perform the conversion using the specified maps
        long result = number;
        foreach (var map in maps)
        {
            if (number >= map.SourceStart && number < map.SourceStart + map.RangeLength)
            {
                result = map.DestinationStart + (number - map.SourceStart);
                break;
            }
        }
        return result;
    }
}

class ConversionMap
{
    public long DestinationStart { get; }
    public long SourceStart { get; }
    public long RangeLength { get; }

    public ConversionMap(long destinationStart, long sourceStart, long rangeLength)
    {
        DestinationStart = destinationStart;
        SourceStart = sourceStart;
        RangeLength = rangeLength;
    }
}