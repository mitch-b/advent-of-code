using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2023.Day02;

class Solution : SolutionBase
{
    public Solution() : base(02, 2023, "") { }

    private readonly Regex _gameRegex = new Regex(@"Game (\d+): (.*)");

    protected override string? SolvePartOne()
    {
        var gameLimits = new Dictionary<string, int>
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };
        var games = Input.SplitByNewline().Select(game => _gameRegex.Match(game)).Select(match => new
        {
            Id = int.Parse(match.Groups[1].Value),
            Game = match.Groups[2].Value
        });
        var possibleGames = new List<int>();
        foreach (var game in games)
        {
            var gameIsImpossible = false;
            var handfuls = game.Game.Split(';');
            foreach (var handful in handfuls)
            {
                if (gameIsImpossible)
                {
                    break;
                }

                var cubes = handful.Split(", ").Select(cube => cube.Trim()).ToList();
                foreach (var cubeInfo in cubes)
                {
                    var color = cubeInfo.Split(' ')[1];
                    var amount = int.Parse(cubeInfo.Split(' ')[0]);
                    if (amount <= gameLimits[color] && !possibleGames.Contains(game.Id))
                    {
                        possibleGames.Add(game.Id);
                    }
                    else if (amount > gameLimits[color])
                    {
                        possibleGames.Remove(game.Id);
                        gameIsImpossible = true;
                        break;
                    }
                }
            }
            gameIsImpossible = false;
        }
        return possibleGames.Sum().ToString();
    }

    protected override string? SolvePartTwo()
    {
        var games = Input.SplitByNewline().Select(game => _gameRegex.Match(game)).Select(match => new
        {
            Id = int.Parse(match.Groups[1].Value),
            Game = match.Groups[2].Value
        });
        var cubePower = new List<int>();
        foreach (var game in games)
        {
            var maxCubesDrawn = new Dictionary<string, int>
            {
                { "red", 1 },
                { "green", 1 },
                { "blue", 1 }
            };
            var handfuls = game.Game.Split(';');
            foreach (var handful in handfuls)
            {
                var cubes = handful.Split(", ").Select(cube => cube.Trim()).ToList();
                foreach (var cubeInfo in cubes)
                {
                    var color = cubeInfo.Split(' ')[1];
                    var amount = int.Parse(cubeInfo.Split(' ')[0]);
                    if (amount >= maxCubesDrawn[color])
                    {
                        maxCubesDrawn[color] = amount;
                    }
                }
            }
            cubePower.Add(maxCubesDrawn["red"] * maxCubesDrawn["green"] * maxCubesDrawn["blue"]);
        }
        return cubePower.Sum().ToString();
    }
}
