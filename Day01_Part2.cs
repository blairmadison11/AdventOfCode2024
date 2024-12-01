using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var list1 = lines.Select(l => int.Parse(Regex.Match(l, @"^\d+").Value)).ToArray();
var dict2 = new Dictionary<int, int>();
foreach (string line in lines)
{
    var num = int.Parse(Regex.Match(line, @"\d+$").Value);
    dict2[num] = dict2.GetValueOrDefault(num) + 1;
}
Console.WriteLine(list1.Aggregate(0, (a, c) => a + (c * dict2.GetValueOrDefault(c))));
