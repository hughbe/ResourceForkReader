using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader;

public struct ResourceListEntry
{
    public const int Size = 12;

    public ushort ID { get; }

    public ushort NameOffset { get; }

    public ResourceAttributes Attributes { get; }

    public uint DataOffset { get; }

    public uint ReservedHandle { get; }

    public ResourceListEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        // Resource ID
        ID = SpanUtilities.ReadUInt16BE(data, offset);
        offset += 2;

        // Offset from beginning of resource name list to resource name
        NameOffset = SpanUtilities.ReadUInt16BE(data, offset);
        offset += 2;

        // 1 byte: Resource attributes and
        // 3 bytes: Offset from beginning of resource data to data for this resource
        var attributesAndOffset = SpanUtilities.ReadUInt32BE(data, offset);
        Attributes = (ResourceAttributes)(attributesAndOffset >> 24);
        DataOffset = attributesAndOffset & 0x00FFFFFF;
        offset += 4;

        // Reserved for handle to resource
        ReservedHandle = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        Debug.Assert(offset == Size);
    }
}
