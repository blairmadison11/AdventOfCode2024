using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var list1 = lines.Select(l => int.Parse(Regex.Match(l, @"^\d+").Value)).Order().ToArray();
var list2 = lines.Select(l => int.Parse(Regex.Match(l, @"\d+$").Value)).Order().ToArray();
Console.WriteLine(list1.Zip(list2).Aggregate(0, (a, c) => a + Math.Abs(c.First - c.Second)));
