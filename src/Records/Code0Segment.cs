using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents the special CODE 0 segment resource containing jump table and application globals information.
/// </summary>
public class Code0Segment
{
    /// <summary>
    /// The minimum size of the CODE 0 segment header in bytes.
    /// </summary>
    public const int MinSize = 16;

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

        Span<byte> data = stackalloc byte[MinSize];
        if (stream.Read(data) != MinSize)
        {
            throw new ArgumentException($"Stream is too small to contain a valid CODE 0 segment. Minimum size is {MinSize} bytes.", nameof(stream));
        }

        int offset = 0;

        // Above A5 size. The size (in bytes) from the location pointed to by A5 to the upper end of the application space.
        AboveA5Size = BinaryPrimitives.ReadUInt32BigEndian(data[offset..]);
        offset += 4;

        // Below A5 size. The size (in bytes) of the application's global variables plus the QuickDraw global variables.
        BelowA5Size = BinaryPrimitives.ReadUInt32BigEndian(data[offset..]);
        offset += 4;

        // Jump table size. The size of the jump table. The jump table contains one 8-byte entry for each externally referenced routine.
        JumpTableSize = BinaryPrimitives.ReadUInt32BigEndian(data[offset..]);
        offset += 4;

        // The offset (in bytes) of the jump table from the location pointed to by A5. This offset is stored in the global variable CurJTOffset.
        JumpTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data[offset..]);
        offset += 4;

        Debug.Assert(offset == 16);

        if (JumpTableOffset >= stream.Length)
        {
            throw new ArgumentException("Jump table offset is out of bounds.", nameof(stream));
        }
        if (JumpTableSize % 8 != 0)
        {
            throw new ArgumentException("Jump table size must be a multiple of 8 bytes.", nameof(stream));
        }
        if (stream.Position + JumpTableSize * 8 > stream.Length)
        {
            throw new ArgumentException("Stream is too small to contain the jump table.", nameof(stream));
        }

        Span<byte> entryData = stackalloc byte[8];
        var entries = new List<ulong>((int)JumpTableSize);
        for (int i = 0; i < JumpTableSize; i += 8)
        {
            // Each jump table entry is 8 bytes.
            if (stream.Read(entryData) != 8)
            {
                throw new ArgumentException("Unable to read complete jump table entry.", nameof(stream));
            }

            var entry = BinaryPrimitives.ReadUInt64BigEndian(entryData);
            entries.Add(entry);
        }

        JumpTableEntries = entries;

        Debug.Assert(stream.Position == stream.Length, "Did not consume all data for Code0Segment.");
    }
}
