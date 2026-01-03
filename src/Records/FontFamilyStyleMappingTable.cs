using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Family Style Mapping Table in a resource fork.
/// </summary>
public readonly struct FontFamilyStyleMappingTable
{
    /// <summary>
    /// The minimum size of a Font Family Style Mapping Table in bytes.
    /// </summary>
    public const int MinSize = 58;

    /// <summary>
    /// Gets the font class identifier.
    /// </summary>
    public ushort FontClass { get; }

    /// <summary>
    /// Gets the offset to the encoding table.
    /// </summary>
    public uint EncodingTableOffset { get; }

    /// <summary>
    /// Gets the reserved field.
    /// </summary>
    public uint Reserved { get; }

    /// <summary>
    /// Gets the style index table.
    /// </summary>
    public byte[] StyleIndexTable { get; }

    /// <summary>
    /// Gets the font family name table.
    /// </summary>
    public FontFamilyNameTable NameTable { get; }

    /// <summary>
    /// Gets the font family glyph name encoding table.
    /// </summary>
    public FontFamilyGlyphNameEncodingTable GlyphNameEncodingTable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontFamilyStyleMappingTable"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the font family style mapping table data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException"></exception>
    public FontFamilyStyleMappingTable(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long from the specified offset to read a FontFamilyStyleMappingTable.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-99 to 4-106
        int offset = 0;

        // Font class. An integer value that specifies a collection of flags
        // that alert the printer driver to what type of PostScript font this
        // font family is. This value is represented by the fontClass field
        // of the StyleTable data type. For more information about how these
        // flags are used, see the LaserWriter Reference book.
        // The default font class definition is 0, which has settings that
        // indicate the printer driver should derive the bold, italic,
        // condense, and extend styles from the plain font. Intrinsic fonts
        // are assigned classes (bits 2 through 8) that prevent these derivations
        // from occurring. The meanings of the 16 bits of the fontClass word are
        // as follows:
        // Bit Meaning
        // 0 This bit is set to 1 if the font name needs coordinating.
        // 1 This bit is set to 1 if the Macintosh vector reencoding scheme
        // is required. Some glyphs in the Apple character set, such as the
        // Apple glyph, do not occur in the standard Adobe character set.
        // This glyph must be mapped in from a font that has it, such as the
        // Symbol font, to a font that does not, like Helvetica.
        // 2 This bit is set to 1 if the font family creates the outline
        // style by changing PaintType, a PostScript variable, to 2.
        // 3 This bit is set to 1 if the font family disallows simulating
        // the outline style by smearing the glyph and whiting out the middle.
        // 4 This bit is set to 1 if the font family does not allow simulation
        // of the bold style by smearing the glyphs.
        // 5 This bit is set to 1 if the font family simulates the bold style
        // by increasing point size.
        // 6 This bit is set to 1 if the font family disallows simulating
        // the italic style.
        // 7 This bit is set to 1 if the font family disallows automatic
        // simulation of the condense style.
        // 8 This bit is set to 1 if the font family disallows automatic
        // simulation of the extend style.
        // 9 This bit is set to 1 if the font family requires reencoding
        // other than Macintosh vector encoding, in which case the
        // glyph-encoding table is present.
        // 10 This bit is set to 1 if the font family should have no
        // additional intercharacter spacing other than the space character.
        // 11–15 Reserved. Should be set to 0.
        FontClass = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Offset. A long integer value that specifies the offset from the
        // start of this table to the glyph-encoding subtable component.
        // This value is represented by the offset field of the StyleTable
        // data type.
        EncodingTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Reserved. A long integer element reserved for use by Apple Computer, Inc.
        Reserved = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Index to font name suffix subtable. This is an array of 48 integer
        // index values, each of which is a location in the naming table. The
        // value of the first element is an index into the naming table for
        // the string name for style code 0; the value of the forty-eighth
        // element is an index into the naming table for the string name for
        // style code 47. This array is represented by the indexes field of
        // the StyleTable data type.
        StyleIndexTable = data.Slice(offset, 48).ToArray();
        offset += 48;

        // Font family name table.
        NameTable = new FontFamilyNameTable(data[offset..], out int bytesReadTable);
        offset += bytesReadTable;
        
        // Font family glyph name encoding table.
        if (EncodingTableOffset != 0)
        {
            GlyphNameEncodingTable = new FontFamilyGlyphNameEncodingTable(data[(int)EncodingTableOffset..], out bytesReadTable);
            offset += bytesReadTable;
        }

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Read beyond the end of the data for FontFamilyStyleMappingTable");
    }
}
