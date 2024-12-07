using System.Text.RegularExpressions;

var equations = File.ReadAllLines("input.txt").Select(l =>
{
    var m = Regex.Match(l, @"(\d+):\s+((\d+)\s*)+");
    return (ulong.Parse(m.Groups[1].Value), m.Groups[3].Captures.Select(c => ulong.Parse(c.Value)).ToArray());
});

ulong sum = 0;
foreach (var equation in equations)
{
    var opTree = new BinaryOp(equation.Item2);
    if (opTree.GetValues().Contains(equation.Item1)) sum += equation.Item1;
}
Console.WriteLine(sum);

interface OpTree
{
    public HashSet<ulong> GetValues();
}
class BinaryOp : OpTree
{
    private static readonly List<Func<ulong, ulong, ulong>> operations = new List<Func<ulong, ulong, ulong>>() { (a, b) => a + b, (a, b) => a * b, (a, b) => ulong.Parse(a.ToString() + b.ToString()) };

    private OpTree left;
    private ulong right;

    public BinaryOp(ulong[] operands)
    {
        right = operands[^1];
        if (operands.Length == 2)
        {
            left = new ConstOp(operands[0]);
        }
        else
        {
            left = new BinaryOp(operands.SkipLast(1).ToArray());
        }
    }

    public HashSet<ulong> GetValues()
    {
        var vals = new HashSet<ulong>();
        foreach (var lval in left.GetValues())
        {
            foreach (var op in operations)
            {
                vals.Add(op(lval, right));
            }
        }
        return vals;
    }
}

class ConstOp : OpTree
{
    private ulong value;

    public ConstOp(ulong value)
    {
        this.value = value;
    }

    public HashSet<ulong> GetValues() => new HashSet<ulong>() { value };
}
