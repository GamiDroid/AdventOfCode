
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Challenge(2015, 16)]
internal class Day16_AuntSue
{
    public AuntSue[] _auntSues = Array.Empty<AuntSue>();

    [Setup]
    public void Setup()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var text = File.ReadAllText(filePath);
        _auntSues = GetAuntSues(text);
    }

    private static AuntSue[] GetAuntSues(string text)
    {
        var auntSues = new List<AuntSue>();

        MatchCollection matches = Regex.Matches(text, "Sue (?<id>\\d+): (?<info>.*)");

        foreach (Match match in matches.Cast<Match>())
        {
            var groups = match.Groups;

            var id = groups["id"].ToInt32();
            var info = groups["info"].Value;

            var auntSue = new AuntSue(id);

            var propertyMatches = Regex.Matches(info, "(?<property>\\w+): (?<value>\\d+)");

            foreach (Match propertyMatch in propertyMatches.Cast<Match>())
            {
                var grps = propertyMatch.Groups;

                var propertyName = grps["property"].Value;
                var value = grps["value"].ToInt32();

                switch (propertyName)
                {
                    case "children": auntSue.Children = value; break;
                    case "cats": auntSue.Cats = value; break;
                    case "samoyeds": auntSue.Samoyeds = value; break;
                    case "pomeranians": auntSue.Pomeranians = value; break;
                    case "akitas": auntSue.Akitas = value; break;
                    case "vizslas": auntSue.Vizslas = value; break;
                    case "goldfish": auntSue.Goldfish = value; break;
                    case "trees": auntSue.Trees = value; break;
                    case "cars": auntSue.Cars = value; break;
                    case "perfumes": auntSue.Perfumes = value; break;
                    default: break;
                }
            }

            auntSues.Add(auntSue);
        }

        return auntSues.ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        var tickerTape = new AuntSue()
        {
            Children = 3,
            Cats = 7,
            Samoyeds = 2,
            Pomeranians = 3,
            Akitas = 0,
            Vizslas = 0,
            Goldfish = 5,
            Trees = 3,
            Cars = 2,
            Perfumes = 1,
        };

        int bestScore = 0;
        AuntSue bestAuntSue = new();
        foreach (var auntSue in _auntSues)
        {
            var score = tickerTape.Compare(auntSue);
            if (score > bestScore)
            {
                bestScore = score;
                bestAuntSue = auntSue;
            }
        }

        Console.WriteLine("Best Aunt Sue: {0}", bestAuntSue);
    }


    [Part(2)]
    public void Part02()
    {
        var tickerTape = new AuntSue()
        {
            Children = 3,
            Cats = 7,
            Samoyeds = 2,
            Pomeranians = 3,
            Akitas = 0,
            Vizslas = 0,
            Goldfish = 5,
            Trees = 3,
            Cars = 2,
            Perfumes = 1,
        };

        int bestScore = 0;
        AuntSue bestAuntSue = new();
        foreach (var auntSue in _auntSues)
        {
            var score = tickerTape.BetterCompare(auntSue);
            if (score > bestScore)
            {
                bestScore = score;
                bestAuntSue = auntSue;
            }
        }

        Console.WriteLine("Best Aunt Sue: {0}", bestAuntSue);
    }


    public record struct AuntSue
    {
        public AuntSue(int identifier)
        {
            Identifier = identifier;
        }

        public int Identifier { get; set; }
        public int Children { get; set; } = -1;
        public int Cats { get; set; } = -1;
        public int Samoyeds { get; set; } = -1;
        public int Pomeranians { get; set; } = -1;
        public int Akitas { get; set; } = -1;
        public int Vizslas { get; set; } = -1;
        public int Goldfish { get; set; } = -1;
        public int Trees { get; set; } = -1;
        public int Cars { get; set; } = -1;
        public int Perfumes { get; set; } = -1;

        public int Compare(AuntSue other)
        {
            int score = 0;

            score += (Children == other.Children) ? 1 : 0;
            score += (Cats == other.Cats) ? 1 : 0;
            score += (Samoyeds == other.Samoyeds) ? 1 : 0;
            score += (Pomeranians == other.Pomeranians) ? 1 : 0;
            score += (Akitas == other.Akitas) ? 1 : 0;
            score += (Vizslas == other.Vizslas) ? 1 : 0;
            score += (Goldfish == other.Goldfish) ? 1 : 0;
            score += (Trees == other.Trees) ? 1 : 0;
            score += (Cars == other.Cars) ? 1 : 0;
            score += (Perfumes == other.Perfumes) ? 1 : 0;

            return score;
        }

        public int BetterCompare(AuntSue other)
        {
            int score = 0;

            score += (Children == other.Children) ? 1 : 0;
            score += (Cats < other.Cats && other.Cats != -1) ? 1 : 0;
            score += (Samoyeds == other.Samoyeds) ? 1 : 0;
            score += (Pomeranians > other.Pomeranians && other.Pomeranians != -1) ? 1 : 0;
            score += (Akitas == other.Akitas) ? 1 : 0;
            score += (Vizslas == other.Vizslas) ? 1 : 0;
            score += (Goldfish > other.Goldfish && other.Goldfish != -1) ? 1 : 0;
            score += (Trees < other.Trees && other.Trees != -1) ? 1 : 0;
            score += (Cars == other.Cars) ? 1 : 0;
            score += (Perfumes == other.Perfumes) ? 1 : 0;

            return score;
        }
    }
}
