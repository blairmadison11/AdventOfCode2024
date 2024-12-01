using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var gl = (string rgx) => lines.Select(l => int.Parse(Regex.Match(l, rgx).Value)).Order();
Console.WriteLine(gl(@"^\d+").Zip(gl(@"\d+$")).Aggregate(0, (a, c) => a + Math.Abs(c.First - c.Second)));
