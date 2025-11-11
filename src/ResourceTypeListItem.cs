using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader;

public struct ResourceTypeListItem
{
    public const int Size = 8;

    public string Type { get; }

    public ushort ResourceCount { get; }

    public ushort ResourceListOffset { get; }

    public ResourceTypeListItem(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }


        int offset = 0;
        Type = SpanUtilities.ReadString(data, offset, 4);
        offset += 4;

        ResourceCount = SpanUtilities.ReadUInt16BE(data, 4);
        offset += 2;

        ResourceListOffset = SpanUtilities.ReadUInt16BE(data, 6);
        offset += 2;

        Debug.Assert(offset == Size);
    }
}
