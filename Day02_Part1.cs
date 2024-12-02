var total = 0;
foreach (string line in File.ReadAllLines("input.txt"))
{
    var nums = line.Split(' ').Select(x => int.Parse(x)).ToArray();
    var safe = true;
    var increasing = nums[1] > nums[0];
    for (int i = 0; i < nums.Length - 1; ++i)
    {
        var diff = increasing ? nums[i + 1] - nums[i] : nums[i] - nums[i + 1];
        if (diff < 1 || diff > 3)
        {
            safe = false;
            break;
        }
    }
    total += safe ? 1 : 0;
}
Console.WriteLine(total);
