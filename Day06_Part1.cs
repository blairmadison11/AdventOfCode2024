var lines = File.ReadAllLines("input.txt");
var nodeGrid = new Node[lines.Length, lines[0].Length];
Node? cur  = null;
for (int i = 0; i < lines.Length; ++i)
{
    for (int j = 0; j < lines[i].Length; ++j)
    {
        Node node = new Node(j, i);
        nodeGrid[i, j] = node;
        if (i > 0) node.AddAdjacent(Direction.North, nodeGrid[i - 1, j]);
        if (j > 0) node.AddAdjacent(Direction.West, nodeGrid[i, j - 1]);
        if (lines[i][j] == '.')
        {
            node.Type = NodeType.Path;
        }
        else if (lines[i][j] == '#')
        {
            node.Type |= NodeType.Obstacle;
        }
        else
        {
            node.Type = NodeType.Path;
            node.SetDirection(lines[i][j]);
            cur = node;
        }
    }
}

var visitedNodes = new HashSet<Node>();
while (cur != null)
{
    visitedNodes.Add(cur);
    cur = cur.GetNext();
}
Console.WriteLine(visitedNodes.Count);

// *** Classes ***
enum Direction { None, North, South, East, West };
enum NodeType { Path, Obstacle };
class Node
{
    static private Dictionary<Direction, Direction> OppDir = new Dictionary<Direction, Direction>() { { Direction.North, Direction.South },
        { Direction.South, Direction.North }, { Direction.East, Direction.West }, { Direction.West, Direction.East } };
    static private Dictionary<Direction, Direction> RightTurnDir = new Dictionary<Direction, Direction>() { { Direction.North, Direction.East },
        { Direction.East, Direction.South }, { Direction.South, Direction.West }, { Direction.West, Direction.North } };
    static private Dictionary<char, Direction> CharDir = new Dictionary<char, Direction> { { '^', Direction.North },
        { '>', Direction.East }, { 'v', Direction.South}, { '<', Direction.West } };

    public NodeType Type;
    private static Direction dir = Direction.None;

    private int x, y;
    private Dictionary<Direction, Node> adjacent = new Dictionary<Direction, Node>();
    private bool visited = false, obstacle = false;

    public bool IsObstacle => Type == NodeType.Obstacle;

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void AddAdjacent(Direction direction, Node node)
    {
        adjacent[direction] = node;
        node.adjacent[OppDir[direction]] = this;
    }

    public void SetDirection(char c)
    {
        dir = CharDir[c];
    }

    public Node? GetNext()
    {
        Node? next = null;
        var done = false;
        while (!done)
        {
            if (!adjacent.ContainsKey(dir))
            {
                done = true;
            }
            else if (adjacent[dir].IsObstacle)
            {
                dir = RightTurnDir[dir];
            }
            else
            {
                done = true;
                next = adjacent[dir];
            }
        }
        return next;
    }
}
