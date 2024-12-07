var lines = File.ReadAllLines("input.txt");
var nodeGrid = new Node[lines.Length, lines[0].Length];
Node? cur  = null;
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

var count = 0;
visitedNodes.Remove(start);
foreach (var node in visitedNodes)
{
    node.Type = NodeType.Obstacle;
    if (start.IsLoop) ++count;
    node.Type = NodeType.Path;
}

Console.WriteLine(count);

// *** Classes ***
enum Cardinal { None, North, South, East, West };
enum NodeType { Path, Obstacle };
class Node
{
    static private Dictionary<Cardinal, Cardinal> OppDir = new Dictionary<Cardinal, Cardinal>() { { Cardinal.North, Cardinal.South },
        { Cardinal.South, Cardinal.North }, { Cardinal.East, Cardinal.West }, { Cardinal.West, Cardinal.East } };
    static private Dictionary<Cardinal, Cardinal> RightTurnDir = new Dictionary<Cardinal, Cardinal>() { { Cardinal.North, Cardinal.East },
        { Cardinal.East, Cardinal.South }, { Cardinal.South, Cardinal.West }, { Cardinal.West, Cardinal.North } };
    static private Dictionary<char, Cardinal> CharDir = new Dictionary<char, Cardinal> { { '^', Cardinal.North },
        { '>', Cardinal.East }, { 'v', Cardinal.South}, { '<', Cardinal.West } };

    public NodeType Type = NodeType.Path;
    public Cardinal Direction = Cardinal.None;

    private Cardinal startDir = Cardinal.None;
    private Dictionary<Cardinal, Node> adjacent = new Dictionary<Cardinal, Node>();
    private bool visited = false, obstacle = false;

    public bool IsObstacle => Type == NodeType.Obstacle;
    public bool IsLoop
    { 
        get
        {
            if (startDir != Cardinal.None) Direction = startDir;
            var isLoop = false;
            var prev = new HashSet<(Node, Cardinal)>();
            var done = false;
            var cur = this;
            while (cur != null)
            {
                prev.Add((cur, cur.Direction));
                cur = cur.GetNext();
                if (cur != null && prev.Contains((cur, cur.Direction)))
                {
                    isLoop = true;
                    cur = null;
                }
            }
            return isLoop;
        }
    }

    public void AddAdjacent(Cardinal direction, Node node)
    {
        adjacent[direction] = node;
        node.adjacent[OppDir[direction]] = this;
    }

    public void SetStartDir(char c)
    {
        Direction = CharDir[c];
        startDir = Direction;
    }

    public Node? GetNext()
    {
        Node? next = null;
        var done = false;
        while (!done)
        {
            if (!adjacent.ContainsKey(Direction))
            {
                done = true;
            }
            else if (adjacent[Direction].IsObstacle)
            {
                Direction = RightTurnDir[Direction];
            }
            else
            {
                done = true;
                next = adjacent[Direction];
                next.Direction = Direction;
            }
        }
        return next;
    }
}
