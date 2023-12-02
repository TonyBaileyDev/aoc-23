using System.Text.RegularExpressions;

const int qR = 12;
const int qB = 14;
const int qG = 13;

var games = ReadFile();

var sum = 0;
foreach(var game in games)
{
    if (IsPossible(game, qR, qB, qG))
    {
        sum += game.Name;
    }
}

Console.WriteLine($"AOC-23 2.1={sum}");

var powerSum = 0;
foreach(var game in games)
{
    var (red, blue, green) = Most(game);
    var power = 1;
    if (red != 0) {
        power *= red;
    }
    if (blue != 0) {
        power *= blue;
    }
    if (green != 0) {
        power *= green;
    }
    powerSum += power;
}

Console.WriteLine($"AOC-23 2.2={powerSum}");

(int red, int blue, int green) Most(Game game) 
{
    var fR = 0;
    var fB = 0;
    var fG = 0;
    foreach (var round in game.Rounds)
    {
        if (round[Game.Red] > fR)
        {
            fR = round[Game.Red];
        }

        if (round[Game.Blue] > fB)
        {
            fB = round[Game.Blue];
        }

        if (round[Game.Green] > fG)
        {
            fG = round[Game.Green];
        }
    }

    return (fR, fB, fG);
}

bool IsPossible(Game game, int red, int blue, int green)
{
    foreach (var round in game.Rounds) 
    {
        if (round[Game.Red] > red) {
            return false;
        }

        if (round[Game.Blue] > blue) {
            return false;
        }

        if (round[Game.Green] > green) {
            return false;
        }
    }

    return true;
}

List<Game> ReadFile()
{
    var games = new List<Game>();
    string? line;
    var sr = new StreamReader("input.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        games.Add(FromLine(line));
        line = sr.ReadLine();
    }
    sr.Close();

    return games;
}

Game FromLine(string line)
{
    var name = line.Substring(4, line.IndexOf(":") - 4);
    var game = new Game(int.Parse(name));

    line = line[line.IndexOf(":")..];
    var rounds = line.Split(';');

    foreach (var round in rounds)
    {
        var red = GetCount(round, Game.Red);
        var blue = GetCount(round, Game.Blue);
        var green = GetCount(round, Game.Green);

        game.AddRound(red, blue, green);
    }

    return game;
}

int GetCount(string round, string colour) {
    var regex = new Regex(@"(?<count>[0-9]*) " + colour);
    var match = regex.Match(round);
    if (match.Success)
    {
        return int.Parse(match.Groups["count"].Value);
    }

    return 0;
}

class Game {
    public const string Red = "red";
    public const string Blue = "blue";
    public const string Green = "green";

    public List<Dictionary<string, int>> Rounds { get; }

    public int Name { get; }

    public Game(int name) {
        Name = name;
        Rounds = new List<Dictionary<string, int>>();
    }

    public Dictionary<string, int> AddRound(int red, int blue, int green) {
        var round = new Dictionary<string, int>()
        {
            { Red, red },
            { Blue, blue },
            { Green, green }
        };

        Rounds.Add(round);
        return round;
    }

    public override string ToString()
    {
        var s = $"{Name}: ";
        foreach(var round in Rounds)
        {
            s += $"{round[Red]} {Red}, {round[Blue]} {Blue}, {round[Green]} {Green};";
        }

        return s;
    }
}