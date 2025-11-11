using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

public class Code0Segment
{
    public uint AboveA5Size { get; }

    public uint BelowA5Size { get; }

    public uint JumpTableSize { get; }

    public uint JumpTableOffset { get; }

    public List<ulong> JumpTableEntries { get; }

    public Code0Segment(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        Span<byte> data = stackalloc byte[16];
        stream.ReadExactly(data);

        int offset = 0;

        // Above A5 size. The size (in bytes) from the location pointed to by A5 to the upper end of the application space.
        AboveA5Size = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        // Below A5 size. The size (in bytes) of the application's global variables plus the QuickDraw global variables.
        BelowA5Size = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        // Jump table size. The size of the jump table. The jump table contains one 8-byte entry for each externally referenced routine.
        JumpTableSize = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        // The offset (in bytes) of the jump table from the location pointed to by A5. This offset is stored in the global variable CurJTOffset.
        JumpTableOffset = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        Debug.Assert(offset == 16);

        if (stream.Position + JumpTableSize > stream.Length)
        {
            throw new ArgumentException("Stream is too small to contain the jump table.", nameof(stream));
        }

        Span<byte> entryData = stackalloc byte[8];
        var entries = new List<ulong>((int)JumpTableSize);
        for (int i = 0; i < JumpTableSize; i += 8)
        {
            // Each jump table entry is 8 bytes.
            stream.ReadExactly(entryData);

            ulong entry = SpanUtilities.ReadUInt64BE(entryData, 0);
            entries.Add(entry);
        }

        JumpTableEntries = entries;
    }
}
