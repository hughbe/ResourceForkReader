using System.Buffers.Binary;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// The Whitespace Table in a Tokens Record ('itl4') in a resource fork.
/// </summary>
public readonly struct WhitespaceTable
{
    /// <summary>
    /// The minimum size of a Whitespace Table in bytes.
    /// </summary>
    public const int MinSize = 4;

    /// <summary>
    /// Gets the length of the whitespace table in bytes.
    /// </summary>
    public ushort Length { get; }

    /// <summary>
    /// Gets the number of entries in the table.
    /// </summary>
    public ushort NumberOfEntries { get; }

    /// <summary>
    /// Gets the offsets of the whitespace strings.
    /// </summary>
    public List<ushort> Offsets { get; }

    /// <summary>
    /// Gets the whitespace strings.
    /// </summary>
    public List<string> Strings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WhitespaceTable"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Whitespace Table data.</param>
    /// <exception cref="ArgumentException">>>Thrown when data is less than MinSize bytes long.</exception>
    public WhitespaceTable(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Whitespace Table. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-58
        int offset = 0;

        // Length of whitespace table.
        Length = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Number of entries.
        NumberOfEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Offset from beginning to first entry.
        // Offset from beginning to second entry.
        // ...
        var offsets = new List<ushort>(NumberOfEntries);
        for (int i = 0; i < NumberOfEntries; i++)
        {
            offsets.Add(BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2)));
            offset += 2;
        }

        Offsets = offsets;

        // First entry (Pascal string)
        // Second entry (Pascal string)
        // ...
        var strings = new List<string>(offsets.Count);
        for (int i = 0; i < offsets.Count; i++)
        {
            int stringOffset = Offsets[i];
            if (stringOffset >= Length)
            {
                throw new ArgumentException("Invalid Whitespace Table: string offset is beyond the length of the table.", nameof(data));
            }

            strings.Add(SpanUtilities.ReadPascalString(data[stringOffset..]));
        }

        Strings = strings;
    }
}
