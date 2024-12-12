var nodes = File.ReadAllLines("sample.txt").Select((l, y) => l.ToCharArray().Select((c, x) => c == '.' ? new Node(x, y) : new Node(x, y, c)).ToArray()).ToArray();
var GridString = () => string.Join('\n', nodes.Select(r => string.Join("", r.Select(c => c.ToString()))));
int dimX = nodes[0].Length, dimY = nodes.Length;
foreach (var freq in Node.Frequencies)
{
    var ats = Node.Antennas(freq);
    var pairs = ats.SelectMany(a => ats, (a, b) => new { a, b }).Where(x => x.a != x.b);
    foreach (var pair in pairs)
    {
        var x = pair.b.X - (pair.a.X - pair.b.X);
        var y = pair.b.Y - (pair.a.Y - pair.b.Y);
        if (x >= 0 && x < dimX && y >= 0 && y < dimY)
        {
            nodes[y][x].AddAntinode();
        }
    }
}
Console.WriteLine(GridString());
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
