namespace AdventOfCode.Solutions.Year2025.Day06;

class Solution : SolutionBase
{
    public Solution() : base(06, 2025, "", useDebugInput: false) { }

    protected override string? SolvePartOne()
    {
        var input = Input.SplitByNewline();
        var inputMap = input.Select(line => line.SplitByWhitespace().ToArray()).ToArray();
        var operators = inputMap.Last();
        inputMap = inputMap[..^1];

        var mathProblems = new List<MathProblem>();

        for (var i = 0; i < operators.Length; i++)
        {
            var mathProblem = new MathProblem();
            var op = operators[i];
            for (var j = 0; j < inputMap.Length; j++)
            {
                var row = inputMap[j];
                if (decimal.TryParse(row[i], out var number))
                {
                    mathProblem.Numbers.Add(number);
                }
            }
            mathProblem.Operator = op;
            mathProblems.Add(mathProblem);
        }
        return mathProblems.Sum(p => p.Result).ToString();
    }

    protected override string? SolvePartTwo()
    {
        var problems = new List<MathProblem>();
        var inputLines = Input.SplitByNewline();
        var operatorsLine = inputLines.Last();
        inputLines = inputLines[..^1];

        var currentOperator = string.Empty;

        var lastOperator = string.Empty;
        var problemNumbers = new List<decimal>();

        for (var i = 0; i < operatorsLine.Length; i++)
        {
            currentOperator = operatorsLine[i].ToString();
            var numberString = string.Empty;

            if (!string.IsNullOrWhiteSpace(currentOperator))
            {
                if (problemNumbers.Count > 0)
                {
                    var problem = new MathProblem
                    {
                        Operator = lastOperator,
                        Numbers = [.. problemNumbers]
                    };
                    problems.Add(problem);
                    problemNumbers.Clear();
                    numberString = string.Empty;
                }
                lastOperator = currentOperator;
            }

            for (var j = 0; j < inputLines.Length; j++)
            {
                numberString += inputLines[j][i];
            }
            if (decimal.TryParse(numberString?.Trim(), out var number))
            {
                problemNumbers.Add(number);
            }
        }

        if (problemNumbers.Count > 0)
        {
            var problem = new MathProblem
            {
                Operator = lastOperator,
                Numbers = [.. problemNumbers]
            };
            problems.Add(problem);
        }

        return problems.Sum(p => p.Result).ToString();
    }
}

class MathProblem
{
    public List<decimal> Numbers { get; set; } = [];
    public string Operator { get; set; } = string.Empty;
    public decimal Result
    {
        get
        {
            var result = Numbers.First();
            foreach (var number in Numbers.Skip(1))
            {
                switch (Operator)
                {
                    case "+":
                        result += number;
                        break;
                    case "-":
                        result -= number;
                        break;
                    case "*":
                        result *= number;
                        break;
                    case "/":
                        result /= number;
                        break;
                }
            }
            return result;
        }
    }
}
