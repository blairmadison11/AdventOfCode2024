using System.Text.RegularExpressions;

Console.WriteLine(Regex.Matches(File.ReadAllText("input.txt"), @"mul\((\d\d?\d?),(\d\d?\d?)\)").Select(x => int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value)).Sum());
