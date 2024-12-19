using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var rgx = string.Format("^({0})+$", lines[0].Replace(", ", "|"));
Console.WriteLine(lines.Skip(2).Aggregate(0, (a, c) => a + (Regex.IsMatch(c, rgx) ? 1 : 0)));
