using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents the special CODE 0 segment resource containing jump table and application globals information.
/// </summary>
public class Code0Segment
{
    /// <summary>
    /// Gets the size (in bytes) from the location pointed to by A5 to the upper end of the application space.
    /// </summary>
    public uint AboveA5Size { get; }

    /// <summary>
    /// Gets the size (in bytes) of the application's global variables plus the QuickDraw global variables.
    /// </summary>
    public uint BelowA5Size { get; }

    /// <summary>
    /// Gets the size of the jump table. The jump table contains one 8-byte entry for each externally referenced routine.
    /// </summary>
    public uint JumpTableSize { get; }

    /// <summary>
    /// Gets the offset (in bytes) of the jump table from the location pointed to by A5.
    /// </summary>
    public uint JumpTableOffset { get; }

    /// <summary>
    /// Gets the list of 8-byte jump table entries.
    /// </summary>
    public List<ulong> JumpTableEntries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Code0Segment"/> class by reading from a stream.
    /// </summary>
    /// <param name="stream">A stream containing the CODE 0 segment data.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the stream is too small to contain the jump table.</exception>
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
