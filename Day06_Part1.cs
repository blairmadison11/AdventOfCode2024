var dirs = new Dictionary<char, int>() { { '^', 0 }, { '>', 1 }, { 'v', 2 }, { '<', 3 } };
var dirVecs = new (int, int)[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
var map = File.ReadAllLines("input.txt").Select(x => x.ToCharArray()).ToArray();
var y = Array.FindIndex(map, r => Array.FindIndex(r, c => dirs.ContainsKey(c)) != -1);
var x = Array.FindIndex(map[y], c => dirs.ContainsKey(c));
var pos = (x, y);
var next = pos;
var dir = dirs[map[y][x]];
var done = false;
var count = 0;
while (!done)
{
    next = (pos.x + dirVecs[dir].Item1, pos.y + dirVecs[dir].Item2);
    if (next.y < 0 || next.y >= map.Length || next.x < 0 || next.x >= map[next.y].Length)
    {
        if (map[pos.y][pos.x] != 'X')
        {
            map[pos.y][pos.x] = 'X';
            ++count;
        }
        done = true;
    }
    else if (map[next.y][next.x] == '#')
    {
        dir = (dir + 1) % 4;
    }
    else
    {
        if (map[pos.y][pos.x] != 'X')
        {
            map[pos.y][pos.x] = 'X';
            ++count;
        }
        pos = next;
    }
}
Console.WriteLine(count);
