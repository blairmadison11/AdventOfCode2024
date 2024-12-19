var chars = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();
var grid = new Node[chars.Length, chars[0].Length];
Node start = null;
for (ushort i = 0; i < chars.Length; ++i)
{
    for (ushort j = 0; j < chars[i].Length; ++j)
    {
        if (chars[i][j] != '#')
        {
            var n = new Node();
            if (i > 0 && grid[i - 1, j] != null)
                n.AddNeighbor(0, grid[i - 1, j]);
            if (j > 0 && grid[i, j - 1] != null)
                n.AddNeighbor(3, grid[i, j - 1]);
            if (chars[i][j] == 'S')
                start = n;
            else if (chars[i][j] == 'E')
                n.IsEnd = true;
            grid[i, j] = n;
        }
    }
}
var pq = new PriorityQueue<Path, int>();
pq.Enqueue(new Path(start), 0);
var goodNodes = new HashSet<Node>();
int minScore = int.MaxValue;
while (pq.Count > 0)
{
    var pc = pq.Dequeue();
    if (pc.IsComplete)
    {
        if (pc.Points < minScore)
        {
            minScore = pc.Points;
            goodNodes.UnionWith(pc.Nodes);
        }
        else if (pc.Points == minScore)
        {
            goodNodes.UnionWith(pc.Nodes);
        }
    }
    else
    {
        foreach (var pn in pc.NextPaths)
        {
            pq.Enqueue(pn, pn.Points);
        }
    }
}
Console.WriteLine(goodNodes.Count + 1);

// *** Classes ***
class Node
{
    public bool IsEnd = false;
    private Dictionary<ushort, Node> neighbors = new Dictionary<ushort, Node>();

    public void AddNeighbor(ushort x, Node n)
    {
        neighbors[x] = n;
        n.neighbors[(ushort)((x + 2) % 4)] = this;
    }

    public (Node N, ushort D)[] GetNext(ushort dir)
    {
        var turns = new List<(Node, ushort)>();
        ushort left = (ushort)((dir + 3) % 4), right = (ushort)((dir + 1) % 4);
        if (neighbors.ContainsKey(dir))
        {
            turns.Add((neighbors[dir], dir));
        }
        if (neighbors.ContainsKey(left))
        {
            turns.Add((neighbors[left], left));
        }
        if (neighbors.ContainsKey(right))
        {
            turns.Add((neighbors[right], right));
        }
        return turns.ToArray();
    }
}

class Path
{
    private static Dictionary<(Node, ushort), int> minScores = new Dictionary<(Node, ushort), int>();

    private HashSet<Node> visited;
    private Node curNode;
    private ushort curDir; // 0 = north, 1, = east, 2 = south, 3 = west
    private int points;

    public Path(Node node)
    {
        visited = new HashSet<Node>();
        curNode = node;
        curDir = 1;
        points = 0;
    }

    public Path(HashSet<Node> visited, Node node, ushort dir, int points)
    {
        this.visited = visited;
        this.curNode = node;
        this.curDir = dir;
        this.points = points;
    }

    public HashSet<Node> Nodes => visited;
    public int Points => points;
    public bool IsComplete => curNode.IsEnd;

    public Path[] NextPaths
    {
        get
        {
            var nps = curNode.GetNext(curDir).Select(x => new Path(new HashSet<Node>(visited) { curNode }, x.N, x.D, x.D == curDir ? points + 1 : points + 1001));
            var rv = new List<Path>();
            foreach (var np in nps)
            {
                if (!minScores.ContainsKey((np.curNode, np.curDir)) || np.Points <= minScores[(np.curNode, np.curDir)])
                {
                    minScores[(np.curNode, np.curDir)] = np.Points;
                    rv.Add(np);
                }
            }
            return rv.ToArray();
        }
    }
}
