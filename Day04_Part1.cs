var chars = File.ReadAllLines("input.txt").Select(x => x.ToCharArray()).ToArray();
var dirs = new List<(int, int)>() { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 0), (0, 1), (1, -1), (1, 0), (1, 1) };
var word = "XMAS";
var count = 0;
for (int i = 0; i < chars.Length; ++i)
{
    for (int j = 0; j < chars[i].Length; ++j)
    {
        foreach (var dir in dirs)
        {
            var pos = (i, j);
            var wordi = 0;
            var fail = false;
            while (!fail && wordi < word.Length)
            {
                if (pos.i >= 0 && pos.j >= 0 && pos.i < chars.Length && pos.j < chars[pos.i].Length && chars[pos.i][pos.j] == word[wordi])
                {
                    pos = (pos.i + dir.Item1, pos.j + dir.Item2);
                    ++wordi;
                }
                else
                {
                    fail = true;
                }
            }
            if (!fail)
            {
                ++count;
            }
        }
    }
}
Console.WriteLine(count);
