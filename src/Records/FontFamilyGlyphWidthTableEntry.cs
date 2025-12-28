using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Family Glyph Width Table Entry in a resource fork.
/// </summary>
public struct FontFamilyGlyphWidthTableEntry
{
    /// <summary>
    /// The minimum size of a Font Family Glyph Width Table Entry in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the style identifier for this glyph width table entry.
    /// </summary>
    public ushort Style { get; }

    /// <summary>
    /// Gets the list of glyph widths for this entry.
    /// </summary>
    public List<ushort> Widths { get; }

    /// <summary>
    /// Gets the width to use for missing characters.
    /// </summary>
    public ushort MissingCharacterWidth { get; }

    /// <summary>
    /// Gets the unused field.
    /// </summary>
    public ushort Unused { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontFamilyGlyphWidthTableEntry"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the font family glyph width table entry data.</param>
    /// <param name="firstCharacter">The first character code in the font.</param>
    /// <param name="lastCharacter">The last character code in the font.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid or insufficient.</exception>
    public FontFamilyGlyphWidthTableEntry(ReadOnlySpan<byte> data, ushort firstCharacter, ushort lastCharacter, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long to read a FontFamilyGlyphWidthTableEntry.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-48
        // and https://adobe-type-tools.github.io/font-tech-notes/pdfs/0091.Mac_Fond.pdf
        int offset = 0;

        Style = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        int numberOfWidths = lastCharacter - firstCharacter + 1;
        var entries = new List<ushort>(numberOfWidths);
        for (int i = 0; i < numberOfWidths; i++)
        {
            var width = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
            entries.Add(width);
        }

        Widths = entries;

        MissingCharacterWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Unused = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Read beyond the end of the data for FontFamilyGlyphWidthTableEntry");
    }
}