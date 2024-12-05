var lines = File.ReadAllLines("input.txt");
var rulebook = new Rulebook();
Page.Rules = rulebook;
var updates = new List<Update>();
int i = 0;
for (; lines[i] != ""; ++i)
{
    rulebook.AddRule(lines[i].Split('|').Select(int.Parse).ToArray());
}
for (++i; i < lines.Length; ++i)
{
    updates.Add(new Update(lines[i].Split(',').Select(int.Parse).ToArray()));
}
var sum = 0;
foreach (var update in updates)
{
    if (!update.IsOrdered)
    {
        update.Sort();
        sum += update.MiddlePage;
    }
}
Console.WriteLine(sum);


/// *******
/// Classes
/// *******
class Rule
{
    public int First { get; }
    public int Last { get; }

    public Rule(int first, int last)
    {
        First = first;
        Last = last;
    }
}

class Rulebook
{
    private Dictionary<int, List<Rule>> myRules = new Dictionary<int, List<Rule>>();
    
    public Rule GetRule(int num1, int num2)
    {
        var rules1 = myRules[num1];
        var rules2 = myRules[num2];
        if (rules1 != null && rules2 != null)
        {
            var rulesInt = rules1.Intersect(rules2);
            if (rulesInt.Count() == 1)
            {
                var rule = rulesInt.First();
                return rule;
            }
        }
        return null;
    }

    public void AddRule(int[] nums)
    {
        var rule = new Rule(nums[0], nums[1]);
        foreach (int num in nums)
        {
            if (!myRules.ContainsKey(num))
            {
                myRules.Add(num, new List<Rule>() { rule });
            }
            else
            {
                myRules[num].Add(rule);
            }
        }
    }
}

class Page : IComparable<Page>
{
    public int Number { get; }
    public static Rulebook Rules;

    public Page(int number)
    {
        this.Number = number;
    }

    public int CompareTo(Page? other)
    {
        Rule rule = Rules.GetRule(this.Number, other.Number);
        if (rule == null) return 0;
        if (this.Number == rule.First) return -1;
        return 1;
    }
}

class Update
{
    private List<Page> myPages;
    public int MiddlePage => myPages[myPages.Count / 2].Number;
    public bool IsOrdered
    {
        get
        {
            for (int i = 0; i < myPages.Count - 1; ++i)
            {
                for (int j = i + 1; j < myPages.Count; ++j)
                {
                    if (myPages[i].CompareTo(myPages[j]) > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public Update(int[] nums)
    {
        myPages = nums.Select(i => new Page(i)).ToList();
    }

    public void Sort()
    {
        myPages.Sort();
    }
}
