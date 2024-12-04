var chars = File.ReadAllLines("input.txt").Select(x => x.ToCharArray()).ToArray();
var getC = ((int, int) p) => chars[p.Item1][p.Item2];
var count = 0;
for (int i = 0; i < chars.Length; ++i)
{
    for (int j = 0; j < chars[i].Length; ++j)
    {
        if (chars[i][j] == 'A')
        {
            var pl = new List<(int, int)>() { (i - 1, j - 1), (i + 1, j + 1), (i - 1, j + 1), (i + 1, j - 1) };
            if (pl.All(p => p.Item1 >= 0 && p.Item2 >= 0 && p.Item1 < chars.Length && p.Item2 < chars[p.Item1].Length) &&
                ((getC(pl[0]) == 'M' && getC(pl[1]) == 'S') || (getC(pl[0]) == 'S' && getC(pl[1]) == 'M')) &&
                ((getC(pl[2]) == 'M' && getC(pl[3]) == 'S') || (getC(pl[2]) == 'S' && getC(pl[3]) == 'M')))
            {
                ++count;
            }
        }
    }
}
Console.WriteLine(count);
