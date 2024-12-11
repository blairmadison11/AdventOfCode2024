var nums = File.ReadAllText("input.txt").TrimEnd().Select(x => x - '0').ToArray();
var drive = new Drive();
bool free = false;
foreach (var num in nums)
{
    drive.AddBlocks(num, free);
    free = !free;
}
drive.Compact();
Console.WriteLine(drive.Checksum);

class Drive
{
    private int curPos = 0, nextFileId = 0;
    private List<Block> blocks = new List<Block>();

    public ulong Checksum => blocks.Aggregate((ulong)0, (a, c) => a + c.Checksum);

    public void AddBlocks(int count, bool free)
    {
        if (free)
        {
            blocks.AddRange(Enumerable.Range(curPos, count).Select(x => new Block(x)).ToArray());
        }
        else
        {
            blocks.AddRange(Enumerable.Range(curPos, count).Select(x => new Block(x, nextFileId)).ToArray());
            ++nextFileId;
        }
        curPos += count;
    }

    public void Compact()
    {
        int freePos = blocks.FindIndex(x => x.IsFree), filePos = blocks.FindLastIndex(x => !x.IsFree);
        while (freePos < filePos)
        {
            blocks[freePos].SetFileID(blocks[filePos].FileID);
            blocks[filePos].Free();
            freePos = blocks.FindIndex(freePos + 1, x => x.IsFree);
            filePos = blocks.FindLastIndex(filePos - 1, x => !x.IsFree);
        }
    }
}
class Block
{
    private int pos, fileId;
    private bool free = false;

    public bool IsFree => free;
    public int FileID => fileId;
    public ulong Checksum => free ? 0 : (ulong)pos * (ulong)fileId;

    public Block(int pos)
    {
        this.pos = pos;
        free = true;
    }

    public Block(int pos, int fileId)
    {
        this.pos = pos;
        this.fileId = fileId;
    }
    
    public void Free()
    {
        this.free = true;
    }

    public void SetFileID(int id)
    {
        fileId = id;
        free = false;
    }
}
