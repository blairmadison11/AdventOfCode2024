var nodes = File.ReadAllLines("input.txt").Select((l, y) => l.ToCharArray().Select((c, x) => c == '.' ? new Node(x, y) : new Node(x, y, c)).ToArray()).ToArray();
int dimX = nodes[0].Length, dimY = nodes.Length;
var PrintGrid = () =>
{
    foreach (var row in nodes)
    {
        foreach (var col in row)
        {
            Console.Write(col);
        }
        Console.WriteLine();
    }
};

foreach (var freq in Node.Frequencies)
{
    var ats = Node.Antennas(freq);
    var pairs = ats.SelectMany(a => ats, (a, b) => new { a, b }).Where(x => x.a != x.b);
    foreach (var pair in pairs)
    {
        var xdiff = pair.a.X - pair.b.X;
        var ydiff = pair.a.Y - pair.b.Y;
        var nPos = (x: pair.b.X + xdiff, y: pair.b.Y + ydiff);
        while (nPos.x >= 0 && nPos.x < dimX && nPos.y >= 0 && nPos.y < dimY)
        {
            nodes[nPos.y][nPos.x].AddAntinode();
            nPos = (x: nPos.x + xdiff, y: nPos.y + ydiff);
        }
    }
}
PrintGrid();
Console.WriteLine("Total antinodes: {0}", Node.TotalAntinodes);

class Node
{
    private static HashSet<Node> anNodes = new HashSet<Node>();
    private static Dictionary<char, List<Node>> antennas = new Dictionary<char, List<Node>>();
    private int x, y;
    private char frequency;
    private bool isAntenna = false, isAntinode = false;

    public static char[] Frequencies => antennas.Keys.ToArray();
    public static List<Node> Antennas(char freq) => antennas[freq];
    public static int TotalAntinodes => anNodes.Count;
    
    public int X => x;
    public int Y => y;

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Node(int x, int y, char frequency) : this(x, y)
    {
        this.frequency = frequency;
        this.isAntenna = true;
        if (antennas.ContainsKey(frequency))
        {
            antennas[frequency].Add(this);
        }
        else
        {
            antennas.Add(frequency, new List<Node>() { this });
        }
    }

    public void AddAntinode()
    {
        isAntinode = true;
        anNodes.Add(this);
    }

    public override string? ToString() => isAntenna ? frequency.ToString() : (isAntinode ? "#" : ".");
}
