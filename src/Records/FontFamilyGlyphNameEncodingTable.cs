using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Family Glyph Name Encoding Table in a resource fork.
/// </summary>
public readonly struct FontFamilyGlyphNameEncodingTable
{
    /// <summary>
    /// The minimum size of a Font Family Glyph Name Encoding Table in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of entries in the glyph name encoding table.
    /// </summary>
    public ushort NumberOfEntries { get; }

    /// <summary>
    /// Gets the list of glyph name encoding entries.
    /// </summary>
    public List<string> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontFamilyGlyphNameEncodingTable"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the font family glyph name encoding table data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown when the data is invalid or insufficient.</exception>
    public FontFamilyGlyphNameEncodingTable(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long to read a FontFamilyGlyphNameEncodingTable.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-105 to 4-106
        // and https://adobe-type-tools.github.io/font-tech-notes/pdfs/0091.Mac_Fond.pdf
        int offset = 0;

        NumberOfEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var entries = new List<string>(NumberOfEntries);
        for (int i = 0; i < NumberOfEntries; i++)
        {
            string entry = SpanUtilities.ReadPascalString(data[offset..], out var entryBytesRead);
            entries.Add(entry);
            offset += entryBytesRead;
        }

        Entries = entries;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Read beyond the end of the data for FontFamilyGlyphNameEncodingTable");
    }
}
