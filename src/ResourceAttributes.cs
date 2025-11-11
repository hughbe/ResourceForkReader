namespace ResourceForkReader;

public enum ResourceAttributes : byte
{
    None = 0x00,
    Reserved1 = 0x01,
    Changed = 0x02,
    Preload = 0x04,
    Protected = 0x08,
    Locked = 0x10,
    Purgeable = 0x20,
    SystemHeap = 0x40,
    Reserved2 = 0x80
}