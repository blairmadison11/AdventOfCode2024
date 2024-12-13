using System.Text.RegularExpressions;

var machines = File.ReadAllText("input.txt").Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None).Select(x => {
    var m = Regex.Match(x, @"(Button [AB]: X\+(\d+), Y\+(\d+)[\r\n]+)+Prize: X=(\d+), Y=(\d+)");
    var a = (int.Parse(m.Groups[2].Captures[0].Value), int.Parse(m.Groups[3].Captures[0].Value));
    var b = (int.Parse(m.Groups[2].Captures[1].Value), int.Parse(m.Groups[3].Captures[1].Value));
    var p = (int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value));
    return new ClawMachine(a, b, p);
}).ToArray();
Console.WriteLine(machines.Select(m => m.GetMinTokens()).Sum());

class ClawMachine
{
    private static readonly int aCost = 3, bCost = 1;
    
    private (int x, int y) a, b, prize;

    public ClawMachine((int, int) a, (int, int) b, (int, int) prize)
    {
        this.a = a;
        this.b = b;
        this.prize = prize;
    }

    public int GetMinTokens()
    {
        var wins = new List<(int a, int b)>();
        var maxA = (prize.x / a.x) + 1;
        for (int apc = 0; apc < maxA; ++apc)
        {
            if ((prize.x - (a.x * apc)) % b.x == 0)
            {
                var bpc = (prize.x - (a.x * apc)) / b.x;
                if ((a.y * apc) + (b.y * bpc) == prize.y)
                {
                    wins.Add((apc, bpc));
                }
            }
        }
        if (wins.Count == 0) return 0;
        return wins.Min(w => (w.a * 3) + w.b);
    }
}
