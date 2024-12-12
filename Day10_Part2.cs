var nums = File.ReadLines("input.txt").Select(r => r.ToCharArray().Select(c => c - '0').ToArray()).ToArray();
var nodes = new Node[nums.Length, nums[0].Length];
var trailheads = new List<Node>();
for (int i = 0; i < nums.Length; ++i)
{
    for (int j = 0; j < nums[i].Length; ++j)
    {
        var node = new Node(nums[i][j]);
        nodes[i, j] = node;
        if (i > 0) node.AddNeighbor(Cardinal.North, nodes[i - 1, j]);
        if (j > 0) node.AddNeighbor(Cardinal.West, nodes[i, j - 1]);
        if (nums[i][j] == 0) trailheads.Add(node);
    }
}
Console.WriteLine(trailheads.Select(t => t.GetScore()).Sum());

enum Cardinal { None, North, South, East, West }
class Node
{
    private static readonly Dictionary<Cardinal, Cardinal> Opposite = new Dictionary<Cardinal, Cardinal>() { { Cardinal.North, Cardinal.South },
        { Cardinal.South, Cardinal.North }, { Cardinal.East, Cardinal.West }, { Cardinal.West, Cardinal.East } };

    private Dictionary<Cardinal, Node> neighbors = new Dictionary<Cardinal, Node>();
    private int elevation;

    public int Elevation => elevation;

    public Node(int elevation)
    {
        this.elevation = elevation;
    }

    public void AddNeighbor(Cardinal c, Node n)
    {
        neighbors[c] = n;
        n.neighbors[Opposite[c]] = this;
    }

    public int GetScore()
    {
        if (elevation == 9)
        {
            return 1;
        }
        else
        {
            var ns = neighbors.Values.Where(n => n.Elevation == elevation + 1).ToArray();
            if (ns.Length > 0)
            {
                return ns.Select(n => n.GetScore()).Sum();
            }
            else
            {
                return 0;
            }
        }
    }
}
