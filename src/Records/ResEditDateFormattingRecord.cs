using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Res Edit long date format extended record ('ITL1') in a resource fork.
/// </summary>
public readonly struct ResEditDateFormattingRecord
{
    /// <summary>
    /// The size of an International Date Formatting Information Record in bytes.
    /// </summary>
    public const int Size = 2;

    /// <summary>
    /// Gets a value indicating whether to use short dates before the system date.
    /// </summary>
    public ushort UseShortDatesBeforeSystem { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResEditDateFormattingRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing International Date Formatting Information Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is not exactly 2 bytes long.</exception>
    public ResEditDateFormattingRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        UseShortDatesBeforeSystem = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for International Date Formatting Information Record.");
    }
}
