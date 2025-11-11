using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader;

public struct ResourceForkHeader
{
    public const int Size = 16;

    public uint DataOffset { get; }

    public uint MapOffset { get; }

    public uint DataLength { get; }

    public uint MapLength { get; }

    public ResourceForkHeader(ReadOnlySpan<byte> data)
    {
        int offset = 0;

        // Offset from beginning of resource fork to resource data.
        DataOffset = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        // Offset from beginning of resource fork to resource map.
        MapOffset = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        // Length of resource data.
        DataLength = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        // Length of resource map.
        MapLength = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        Debug.Assert(offset == Size);
    }
}
