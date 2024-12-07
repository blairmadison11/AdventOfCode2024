var lines = File.ReadAllLines("input.txt");
var nodeGrid = new Node[lines.Length, lines[0].Length];
Node? cur = null;
for (int i = 0; i < lines.Length; ++i)
{
    for (int j = 0; j < lines[i].Length; ++j)
    {
        Node node = new Node();
        nodeGrid[i, j] = node;
        if (i > 0) node.AddAdjacent(Cardinal.North, nodeGrid[i - 1, j]);
        if (j > 0) node.AddAdjacent(Cardinal.West, nodeGrid[i, j - 1]);
        if (lines[i][j] == '#')
        {
            node.Type = NodeType.Obstacle;
        }
        else if (lines[i][j] != '.')
        {
            node.SetStartDir(lines[i][j]);
            cur = node;
        }
    }
}

var start = cur;
var visitedNodes = new HashSet<Node>();
while (cur != null)
{
    visitedNodes.Add(cur);
    cur = cur.GetNext();
}
Console.WriteLine(visitedNodes.Count);

// *** Classes ***
enum Cardinal { None, North, South, East, West };
enum NodeType { Path, Obstacle };
class Node
{
    static private readonly Dictionary<Cardinal, Cardinal> Opposite = new Dictionary<Cardinal, Cardinal>() { { Cardinal.North, Cardinal.South },
        { Cardinal.South, Cardinal.North }, { Cardinal.East, Cardinal.West }, { Cardinal.West, Cardinal.East } };
    static private readonly Dictionary<Cardinal, Cardinal> RightTurn = new Dictionary<Cardinal, Cardinal>() { { Cardinal.North, Cardinal.East },
        { Cardinal.East, Cardinal.South }, { Cardinal.South, Cardinal.West }, { Cardinal.West, Cardinal.North } };
    static private readonly Dictionary<char, Cardinal> CharDir = new Dictionary<char, Cardinal> { { '^', Cardinal.North },
        { '>', Cardinal.East }, { 'v', Cardinal.South}, { '<', Cardinal.West } };

    public NodeType Type = NodeType.Path;

    private Cardinal dir = Cardinal.None;
    private Dictionary<Cardinal, Node> adjacent = new Dictionary<Cardinal, Node>();

    public bool IsObstacle => Type == NodeType.Obstacle;

    public void AddAdjacent(Cardinal direction, Node node)
    {
        adjacent[direction] = node;
        node.adjacent[Opposite[direction]] = this;
    }

    public void SetStartDir(char c)
    {
        dir = CharDir[c];
    }

    public Node? GetNext()
    {
        Node? next = this;
        while (next == this)
        {
            if (!adjacent.ContainsKey(dir))
            {
                next = null;
            }
            else if (adjacent[dir].IsObstacle)
            {
                dir = RightTurn[dir];
            }
            else
            {
                next = adjacent[dir];
                next.dir = dir;
            }
        }
        return next;
    }
}
