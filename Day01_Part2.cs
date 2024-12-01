using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var list1 = lines.Select(l => int.Parse(Regex.Match(l, @"^\d+").Value)).Order().ToArray();
var list2 = lines.Select(l => int.Parse(Regex.Match(l, @"\d+$").Value)).Order().ToArray();
var sum = 0;
for (int i = 0; i < list1.Length; ++i)
{
    sum += list1[i] * list2.Aggregate(0, (a, c) => list1[i] == c ? a + 1 : a);
}
Console.WriteLine(sum);
