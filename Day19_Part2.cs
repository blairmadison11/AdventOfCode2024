var lines = File.ReadAllLines("input.txt");
var atoms = lines[0].Split(", ");
TreeNode.Root = new TreeNode(atoms);
ulong total = 0;
foreach (var cmb in lines.Skip(2))
{
    total += TreeNode.Root.GetMatches(cmb);
}
Console.WriteLine(total);

class TreeNode
{
    public static TreeNode Root;
    private static Dictionary<string, ulong> memo = new Dictionary<string, ulong>();

    private Dictionary<char, TreeNode> children = new Dictionary<char, TreeNode>();
    private bool isLeaf = false;
    
    public TreeNode(string[] atoms)
    {
        if (atoms.Length > 0)
        {
            if (atoms.Any(a => a.Length == 0))
                isLeaf = true;
            var firsts = new HashSet<char>(atoms.Where(a => a.Length > 0).Select(a => a[0]));
            foreach (var f in firsts)
            {
                children.Add(f, new TreeNode(atoms.Where(a => a.StartsWith(f)).Select(a => a.Substring(1)).ToArray()));
            }
        }
    }

    public ulong GetMatches(string str)
    {
        ulong matches = 0;
        if (str == "")
        {
            if (isLeaf)
            {
                matches = 1;
            }
        }
        else
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
        return matches;
    }
}
