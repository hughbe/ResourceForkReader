using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Family Glyph Width Table in a resource fork.
/// </summary>
public readonly struct FontFamilyGlyphWidthTable
{
    /// <summary>
    /// The minimum size of a Font Family Glyph Width Table in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of widths in the glyph width table minus one.
    /// </summary>
    public ushort NumberOfWidths { get; }

    /// <summary>
    /// Gets the list of font family glyph width table entries.
    /// </summary>
    public List<FontFamilyGlyphWidthTableEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontFamilyGlyphWidthTable"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the font family glyph width table data.</param>
    /// <param name="firstCharacter">The first character code in the font.</param>
    /// <param name="lastCharacter">The last character code in the font.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid or insufficient.</exception>
    public FontFamilyGlyphWidthTable(ReadOnlySpan<byte> data, ushort firstCharacter, ushort lastCharacter, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-48 and
        // and https://adobe-type-tools.github.io/font-tech-notes/pdfs/0091.Mac_Fond.pdf
        int offset = 0;

        NumberOfWidths = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var entries = new List<FontFamilyGlyphWidthTableEntry>(NumberOfWidths + 1);
        for (int i = 0; i < NumberOfWidths + 1; i++)
        {
            var entry = new FontFamilyGlyphWidthTableEntry(data[offset..], firstCharacter, lastCharacter, out int entryBytesRead);
            offset += entryBytesRead;
            entries.Add(entry);
        }

        Entries = entries;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Read beyond the end of the data for FontFamilyGlyphWidthTable");
    }
}
