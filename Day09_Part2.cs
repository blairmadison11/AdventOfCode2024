var nums = File.ReadAllText("input.txt").TrimEnd().Select(x => x - '0').ToArray();
var drive = new Drive();
bool free = false;
foreach (var num in nums)
{
    drive.AddSector(num, free);
    free = !free;
}
drive.Defrag();
Console.WriteLine(drive.Checksum);

class Drive
{
    private int curPos = 0, nextFileId = 0;
    private List<Sector> fileSectors = new List<Sector>();
    private SortedList<int, Sector> freeSectors = new SortedList<int, Sector>();

    public ulong Checksum => fileSectors.Aggregate((ulong)0, (a, c) => a + c.Checksum);

    public void AddSector(int count, bool free)
    {
        if (free)
        {
            freeSectors.Add(curPos, new Sector(curPos, count, -1));
        }
        else
        {
            fileSectors.Add(new Sector(curPos, count, nextFileId++));
        }
        curPos += count;
    }

    public void Defrag()
    {
        for (int i = fileSectors.Count - 1; i >= 0; --i)
        {
            var fileSec = fileSectors[i];   
            foreach (var freeSec in freeSectors.Values)
            {
                if (freeSec.Position > fileSec.Position) break;
                var diff = freeSec.Size - fileSec.Size;
                if (diff >= 0)
                {
                    fileSec.Move(freeSec.Position);
                    freeSectors.Remove(freeSec.Position);
                    if (diff > 0)
                    {
                        var pos = freeSec.Position + fileSec.Size;
                        freeSectors.Add(pos, new Sector(pos, diff, -1));
                    }
                    break;
                }
            }
        }
    }
}
class Sector
{
    private int pos, length, fileId;

    public int Position => pos;
    public int Size => length;
    public ulong Checksum => Enumerable.Range(pos, length).Select(x => (ulong)x * (ulong)fileId).Aggregate((ulong)0, (a, c) => a + c);

    public Sector(int pos, int length, int fileId)
    {
        this.pos = pos;
        this.length = length;
        this.fileId = fileId;
    }

    public void Move(int pos)
    {
        this.pos = pos;
    }
}
