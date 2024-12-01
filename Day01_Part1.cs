using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var list1 = lines.Select(l => int.Parse(Regex.Match(l, @"^\d+").Value)).Order().ToArray();
var list2 = lines.Select(l => int.Parse(Regex.Match(l, @"\d+$").Value)).Order().ToArray();
var sum = 0;
for (int i = 0; i < list1.Length; ++i)
{
    sum += Math.Abs(list1[i] - list2[i]);
}
Console.WriteLine(sum);
