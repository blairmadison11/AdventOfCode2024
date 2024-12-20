var lines = File.ReadAllLines("input.txt");
TreeNode.Root = new TreeNode(lines[0].Split(", "));
Console.WriteLine(lines.Skip(2).Select(l => TreeNode.Root.GetMatches(l)).Aggregate((a, c) => a + c));

class TreeNode
{
    public static TreeNode Root;
    private static Dictionary<string, ulong> memo = new Dictionary<string, ulong>();

    private Dictionary<char, TreeNode> children = new Dictionary<char, TreeNode>();
    private bool isLeaf = false;
    
    public TreeNode(string[] atoms)
    {
        if (atoms.Any(a => a.Length == 0))
            isLeaf = true;
        foreach (var f in atoms.Where(a => a.Length > 0).Select(a => a[0]).Distinct())
            children.Add(f, new TreeNode(atoms.Where(a => a.StartsWith(f)).Select(a => a.Substring(1)).ToArray()));
    }

    public ulong GetMatches(string str)
    {
        ulong matches = 0;
        if (str != "")
        {
            if (isLeaf)
            {
                if (memo.ContainsKey(str))
                {
                    matches = memo[str];
                }
                else
                {
                    matches = Root.GetMatches(str);
                    memo[str] = matches;
                }
            }
            if (children.ContainsKey(str[0]))
            {
                matches += children[str[0]].GetMatches(str.Substring(1));
            }
        }
        else if (isLeaf)
        {
            matches = 1;
        }
        return matches;
    }
}
