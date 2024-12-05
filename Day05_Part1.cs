var lines = File.ReadAllLines("input.txt");
var rulebook = new Dictionary<int, List<(int First, int Last)>>();
var updates = new List<Dictionary<int, int>>();
int i = 0;
for (; lines[i] != ""; ++i)
{
    var rule = lines[i].Split('|').Select(int.Parse).ToArray();
    var ruleTup = (rule[0], rule[1]);
    foreach (var num in rule)
    {
        if (!rulebook.ContainsKey(num))
        {
            rulebook.Add(num, new List<(int First, int Last)>() { ruleTup });
        }
        else
        {
            rulebook[num].Add(ruleTup);
        }
    }
}
for (++i; i < lines.Length; ++i)
{
    updates.Add(lines[i].Split(',').Select((x, i) => new { Val = int.Parse(x), Index = i }).ToDictionary(e => e.Val, e => e.Index));
}
var sum = 0;
foreach (var update in updates)
{
    var fail = false;
    var nums = update.Keys.ToArray();
    for (i = 0; !fail && i < nums.Length; ++i)
    {
        for (int j = i; !fail && j < nums.Length; ++j)
        {
            var rules1 = rulebook[nums[i]];
            var rules2 = rulebook[nums[j]];
            if (rules1 != null && rules2 != null)
            {
                var rulesInt = rules1.Intersect(rules2);
                if (rulesInt.Count() == 1)
                {
                    var rule = rulesInt.First();
                    fail = update[rule.First] > update[rule.Last];
                }
            }
        }
    }
    if (!fail)
    {
        sum += update.First(x => x.Value == (update.Count / 2)).Key;
    }
}
Console.WriteLine(sum);
