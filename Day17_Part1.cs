using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var comp = new AocComputer(lines.Take(3).Select(l => int.Parse(Regex.Match(l, @"Register [ABC]: (\d+)").Groups[1].Value)).ToArray(),
    Regex.Match(lines[4], @"Program: ((\d+),?)+").Groups[2].Captures.Select(c => int.Parse(c.Value)).ToArray());
Console.WriteLine(comp.Execute());

class AocComputer
{
    private int a, b, c, ip;
    private int[] program;
    private List<int> output = new List<int>();

    public AocComputer(int[] regs, int[] prog)
    {
        this.a = regs[0];
        this.b = regs[1];
        this.c = regs[2];
        program = prog;
    }

    public string Execute()
    {
        while (ip < program.Length)
        {
           switch(program[ip])
            {
                case 0:
                    a = a / (int)Math.Pow(2, GetComboOperand(program[ip + 1]));
                    ip += 2;
                    break;
                case 1:
                    b = b ^ program[ip + 1];
                    ip += 2;
                    break;
                case 2:
                    b = GetComboOperand(program[ip + 1]) % 8;
                    ip += 2;
                    break;
                case 3:
                    if (a == 0)
                        ip += 2;
                    else
                        ip = program[ip + 1];
                    break;
                case 4:
                    b = b ^ c;
                    ip += 2;
                    break;
                case 5:
                    output.Add(GetComboOperand(program[ip + 1]) % 8);
                    ip += 2;
                    break;
                case 7:
                    c = a / (int)Math.Pow(2, GetComboOperand(program[ip + 1]));
                    ip += 2;
                    break;
            }
        }
        return output.Count == 0 ? "" : string.Join(',', output);
    }

    private int GetComboOperand(int operand)
    {
        if (operand < 4)
            return operand;
        else if (operand == 4)
            return a;
        else if (operand == 5)
            return b;
        else
            return c;
    }
}
