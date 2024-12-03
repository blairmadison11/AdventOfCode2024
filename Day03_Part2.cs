using System.Text.RegularExpressions;

var matches = Regex.Matches(File.ReadAllText("input.txt"), @"mul\((\d\d?\d?),(\d\d?\d?)\)|do\(\)|don't\(\)");
var enabled = true;
var sum = 0;
foreach (Match match in matches)
{
    if (match.Value.StartsWith("do("))
    {
        enabled = true;
    }
    else if (match.Value.StartsWith("don't("))
    {
        enabled = false;
    }
    else if (enabled)
    {
        sum += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
    }
}
Console.WriteLine(sum);
