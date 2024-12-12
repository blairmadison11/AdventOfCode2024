var nums = File.ReadAllText("input.txt").TrimEnd().Select(x => x - '0').ToArray();
var drive = new Drive();
bool free = false;
foreach (var num in nums)
{
    drive.AddSegment(num, free);
    free = !free;
}
drive.Defrag();
Console.WriteLine(drive.Checksum);

class Drive
{
    private int curPos = 0, nextFileId = 0;
    private List<Segment> fileSegs = new List<Segment>();
    private SortedList<int, Segment> freeSegs = new SortedList<int, Segment>();

    public ulong Checksum => fileSegs.Aggregate((ulong)0, (a, c) => a + c.Checksum);

    public void AddSegment(int count, bool free)
    {
        if (free)
        {
            freeSegs.Add(curPos, new Segment(curPos, count, -1));
        }
        else
        {
            fileSegs.Add(new Segment(curPos, count, nextFileId++));
        }
        curPos += count;
    }

    public void Defrag()
    {
        for (int i = fileSegs.Count - 1; i >= 0; --i)
        {
            var fileSeg = fileSegs[i];
            foreach (var freeSeg in freeSegs.Values)
            {
                if (freeSeg.Position > fileSeg.Position) break;
                var diff = freeSeg.Size - fileSeg.Size;
                if (diff >= 0)
                {
                    fileSeg.Move(freeSeg.Position);
                    freeSegs.Remove(freeSeg.Position);
                    if (diff > 0)
                    {
                        var pos = freeSeg.Position + fileSeg.Size;
                        freeSegs.Add(pos, new Segment(pos, diff, -1));
                    }
                    break;
                }
            }
        }
    }
}
class Segment
{
    private int pos, length, fileId;

    public int Position => pos;
    public int Size => length;
    public ulong Checksum => Enumerable.Range(pos, length).Select(x => (ulong)x * (ulong)fileId).Aggregate((ulong)0, (a, c) => a + c);

    public Segment(int pos, int length, int fileId)
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
