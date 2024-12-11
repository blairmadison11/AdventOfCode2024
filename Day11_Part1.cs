var stones = new StoneCollection(File.ReadAllText("input.txt").Split().Select(ulong.Parse).ToArray());
Console.WriteLine(stones.GetCount(25));

class StoneCollection
{
    private Dictionary<(ulong, uint), ulong> dict = new Dictionary<(ulong, uint), ulong>();
    private ulong[] nums;

    public StoneCollection(ulong[] nums)
    {
        this.nums = nums;
    }

    public ulong GetCount(uint iters)
    {
        return nums.Select(x => GetCount(x, iters)).Aggregate((ulong)0, (a, c) => a + c);
    }

    private ulong[] GetNums(ulong x)
    {
        if (x == 0)
        {
            return new ulong[] { 1 };
        }
        else
        {
            var str = x.ToString();
            if (str.Length % 2 == 0)
            {
                var mid = str.Length / 2;
                return new ulong[] { ulong.Parse(str.Substring(0, mid)), ulong.Parse(str.Substring(mid, mid)) };
            }
            else
            {
                return new ulong[] { x * 2024 };
            }
        }
    }

    public ulong GetCount(ulong x, uint iters)
    {
        if (iters == 0) return 1;
        if (dict.ContainsKey((x, iters))) return dict[(x, iters)];
        var count = GetNums(x).Select(x => GetCount(x, iters - 1)).Aggregate((ulong)0, (a, c) => a + c);
        dict[(x, iters)] = count;
        return count;
    }
}
