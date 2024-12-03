var lists = File.ReadAllLines("input.txt").Select(l => l.Split(' ').Select(int.Parse).ToArray()).ToArray();
var isOrdered = (int[] nums) =>
{
    var pairs = nums.Zip(nums.Skip(1));
    return pairs.All(x => x.First > x.Second) ||
        pairs.All(x => x.First < x.Second);
};
var isSmallDiff = (int[] nums) => nums.Zip(nums.Skip(1)).All(x => Math.Abs(x.First - x.Second) is >= 1 and <= 3);
var isSafe = (int[] nums) => isOrdered(nums) && isSmallDiff(nums);
Console.WriteLine(lists.Aggregate(0, (a, c) => a + (isSafe(c) || Enumerable.Range(0, c.Length).Any(x => isSafe(c.Where((e, i) => i != x).ToArray())) ? 1 : 0)));
