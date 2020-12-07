using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day06 : ASolution
    {
        private readonly IEnumerable<Group> groups;
        public Day06() : base(06, 2020, "")
        {
            groups = Input.SplitByEmptyLine().Select(answers => new Group(answers));
        }

        protected override string SolvePartOne()
        {
            return groups.Select(g => g.UniqueAnsweredQuestions.Count).Sum().ToString();
        }

        protected override string SolvePartTwo()
        {
            return groups.Select(g => g.AnsweredByAll.Count).Sum().ToString();
        }
    }

    class Group
    {
        public HashSet<char> UniqueAnsweredQuestions { get; private set; }
        public HashSet<char> AnsweredByAll { get; private set; }
        public List<string> Answers { get; set; } = new List<string>();

        public Group(IEnumerable<string> individualAnswers)
        {
            UniqueAnsweredQuestions = string.Join("", individualAnswers).ToHashSet();
            Answers.AddRange(individualAnswers);

            AnsweredByAll = new HashSet<char>(UniqueAnsweredQuestions);
            foreach (var answer in Answers)
            {
                AnsweredByAll = AnsweredByAll.Intersect(answer.ToHashSet()).ToHashSet();
            }
        }
    }
}
