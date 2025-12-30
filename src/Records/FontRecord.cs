using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Record ('NFNT' or 'FONT') in a resource fork.
/// </summary>
public readonly struct FontRecord
{
    /// <summary>
    /// The minimum size of a Font Record in bytes.
    /// </summary>
    public const int MinSize = 26;

    /// <summary>
    /// Gets the font type.
    /// </summary>
    public FontType Type { get; }

    /// <summary>
    /// Gets the first character code.
    /// </summary>
    public char FirstCharacterCode { get; }

    /// <summary>
    /// Gets the last character code.
    /// </summary>
    public char LastCharacterCode { get; }

    /// <summary>
    /// Gets the maximum width.
    /// </summary>
    public ushort MaximumWidth { get; }

    /// <summary>
    /// Gets the maximum kerning.
    /// </summary>
    public short MaximumKerning { get; }

    /// <summary>
    /// Gets the negated descent value.
    /// </summary>
    public short NegatedDescentValue { get; }

    /// <summary>
    /// Gets the font rectangle width.
    /// </summary>
    public ushort RectangleWidth { get; }

    /// <summary>
    /// Gets the font rectangle height.
    /// </summary>
    public ushort RectangleHeight { get; }

    /// <summary>
    /// Gets the offset to the width/offset table.
    /// </summary>
    public ushort WidthOffsetTableOffset { get; }

    /// <summary>
    /// Gets the maximum ascent.
    /// </summary>
    public short MaximumAscent { get; }

    /// <summary>
    /// Gets the maximum descent.
    /// </summary>
    public short MaximumDescent { get; }
    
    /// <summary>
    /// Gets the leading.
    /// </summary>
    public short Leading { get; }

    /// <summary>
    /// Gets the bit image row width.
    /// </summary>
    public ushort BitImageRowWidth { get; }

    /// <summary>
    /// Gets the bit image table.
    /// </summary>
    public byte[] BitImageTable { get; }

    /// <summary>
    /// Gets the bitmap location table.
    /// </summary>
    public ushort[] BitmapLocationTable { get; }

    /// <summary>
    /// Gets the width/offset table.
    /// </summary>
    public (byte Offset, byte Width)[] WidthOffsetTable { get; }

    /// <summary>
    /// Gets the glyph-width table, if present.
    /// </summary>
    public ushort[]? GlyphWidthTable { get; }

    /// <summary>
    /// Gets the image height table, if present.
    /// </summary>
    public (byte Offset, byte NumberOfRows)[]? ImageHeightTable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the Font Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is less than the minimum required size.</exception>
    public FontRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-67 to 4-71
        int offset = 0;

        //  Font type. An integer value that is used to specify the general
        // characteristics of the font, such as whether it is fixed-width
        // or proportional, whether the optional image-height and glyph-width
        // tables are attached to the font, and information about the font
        // depth and colors. This value is represented by the fontType field
        // in the FontRec data type. For the meaning of the bits in this
        // field, see “The Font Type Element” on page 4-70.
        Type = (FontType)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // First character code. An integer value that specifies the ASCII
        // character code of the first glyph in the font. This value is
        // represented by the firstChar field in the FontRec data type.
        FirstCharacterCode = (char)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Last character code. An integer value that specifies the ASCII
        // character code of the last glyph in the font. This value is
        // represented by the lastChar field in the FontRec data type.
        LastCharacterCode = (char)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Maximum width. An integer value that specifies the maximum width
        // of the widest glyph in the font, in pixels. This value is
        // represented by the widMax field in the FontRec data type.
        MaximumWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Maximum kerning. An integer value that specifies the distance
        // from the font rectangle’s glyph origin to the left edge of the
        // font rectangle, in pixels. If a glyph in the font kerns to the
        // left, the amount is represented as a negative number. If the glyph
        // origin lies on the left edge of the font rectangle, the value of
        // the kernMax field is 0. This value is represented by the kernMax
        // field in the FontRec data type. 
        MaximumKerning = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Negated descent value. If this font has very large tables and
        // this value is positive, this value is the high word of the offset
        // to the width/offset table. For more information, see “The Offset
        // to the Width/Offset Table” on page 4-71. If this value is negative,
        // it is the negative of the descent and is not used by the Font
        // Manager. This value is represented by the nDescent field in the
        // FontRec data type. 
        NegatedDescentValue = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Font rectangle width. An integer value that specifies the width,
        // in pixels, of the image created if all the glyphs in the font were
        // superimposed at their glyph origins. This value is represented by
        // the fRectWidth field in the FontRec data type.
        RectangleWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Font rectangle height. An integer value that specifies the height,
        // in pixels, of the image created if all the glyphs in the font were
        // superimposed at their glyph origins. This value equals the sum of
        // the maximum ascent and maximum descent measurements for the font.
        // This value is represented by the fRectHeight field in the FontRec
        // data type.
        RectangleHeight = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Offset to width/offset table. An integer value that specifies the
        // offset to the offset/width table from this point in the font record,
        // in words. If this font has very large tables, this value is only
        // the low word of the offset and the negated descent value is the
        // high word, as explained in the section “The Offset to the Width
        // /Offset Table” on page 4-71. This value is represented by the
        // owTLoc field in the FontRec data type.
        var originalOffset = offset;
        WidthOffsetTableOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Maximum ascent. An integer value that specifies the maximum ascent
        // measurement for the entire font, in pixels. The ascent is the
        // distance from the glyph origin to the top of the font rectangle.
        // This value is represented by the ascent field in the FontRec data
        // type. 
        MaximumAscent = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Maximum descent. An integer value that specifies the maximum
        // descent measurement for the entire font, in pixels. The descent
        // is the distance from the glyph origin to the bottom of the font
        // rectangle. This value is represented by the descent field in the
        // FontRec data type.
        MaximumDescent = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Leading. An integer value that specifies the leading measurement
        // for the entire font, in pixels. Leading is the distance from the
        // descent line of one line of single-spaced text to the ascent line
        // of the next line of text. This value is represented by the leading
        // field in the FontRec data type. 
        Leading = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Bit image row width. An integer value that specifies the width of
        // the bit image, in words. This is the width of each glyph’s bit
        // image as a number of words. This value is represented by the
        // rowWords field in the FontRec data type. 
        BitImageRowWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Bit image table. The bit image of the glyphs in the font. The glyph
        // images of every defined glyph in the font are placed sequentially
        // in order of increasing ASCII code. The bit image is one pixel image
        // with no undefined stretches that has a height given by the value
        // of the font rectangle element and a width given by the value of
        // the bit image row width element. The image is padded at the end
        // with extra pixels to make its length a multiple of 16.
        var bitmapSize = BitImageRowWidth * 2 * RectangleHeight;
        BitImageTable = data.Slice(offset, bitmapSize).ToArray();
        offset += bitmapSize;

        // Skip padding to ensure the offset is aligned to a 16-bit (word) boundary.
        if (offset % 2 != 0)
        {
            offset += 1;
        }

        // Bitmap location table. For every glyph in the font, this table
        // contains a word that specifies the bit offset to the location of
        // the bitmap for that glyph in the bit image table.
        // If a glyph is missing from the font, its entry contains the same
        // value for its location as the entry for the next glyph. The
        // missing glyph is the last glyph of the bit image for that font.
        // The last word of the table contains the offset to one bit beyond
        // the end of the bit image. You can determine the image width of
        // each glyph from the bitmap location table by subtracting the
        // bit offset to that glyph from the bit offset to the next glyph in
        // the table.
        var numberOfGlyphs = LastCharacterCode - FirstCharacterCode + 2;

        // Extra word for end offset
        var bitmapLocationTable = new ushort[numberOfGlyphs + 1];
        for (int i = 0; i < bitmapLocationTable.Length; i++)
        {
            bitmapLocationTable[i] = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        BitmapLocationTable = bitmapLocationTable;

        // Width/offset table. For every glyph in the font, this table
        // contains a word with the glyph offset in the high-order byte
        // and the glyph’s width, in integer form, in the low-order byte.
        // The value of the offset, when added to the maximum kerning
        // value for the font, determines the horizontal distance from
        // the glyph origin to the left edge of the bit image of the glyph,
        // in pixels. If this sum is negative, the glyph origin is to the
        // right of the glyph image’s left edge, meaning the glyph kerns
        // to the left.
        // If the sum is positive, the origin is to the left of the image’s 
        // left edge. If the sum equals zero, the glyph origin corresponds
        // with the left edge of the bit image. Missing glyphs are
        // represented by a word value of –1. The last word of this table
        // is also –1, representing the end.
        
        // Move to the start of the width/offset table.
        if (NegatedDescentValue > 0)
        {
            // Offset is combined high and low words
            int highWord = (NegatedDescentValue & 0xFFFF) << 16;
            int lowWord = WidthOffsetTableOffset;
            offset = originalOffset + (highWord | lowWord) * 2;
        }
        else
        {
            // Offset is just the low word
            offset = originalOffset + WidthOffsetTableOffset * 2;
        }

        var widthOffsetTable = new (byte Offset, byte Width)[numberOfGlyphs];
        for (int i = 0; i < widthOffsetTable.Length; i++)
        {
            widthOffsetTable[i] = (
                Offset: data[offset],
                Width: data[offset + 1]);
            offset += 2;
        }

        WidthOffsetTable = widthOffsetTable;

        // Glyph-width table. For every glyph in the font, this table
        // contains a word that specifies the glyph’s fixed-point glyph
        // width at the given point size and font style, in pixels.
        // The Font Manager gives precedence to the values in this table
        // over those in the font family glyph-width table. There is an
        // unsigned integer in the high-order byte and a fractional part
        // in the low-order byte. This table is optional.
        if (Type.HasFlag(FontType.ContainsGlyphWidthTable))
        {
            var glyphWidthTable = new ushort[numberOfGlyphs];
            for (int i = 0; i < glyphWidthTable.Length; i++)
            {
                glyphWidthTable[i] = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
                offset += 2;
            }

            GlyphWidthTable = glyphWidthTable;
        }

        // Image height table. For every glyph in the font, this table
        // contains a word that specifies the image height of the glyph,
        // in pixels. The image height is the height of the glyph image
        // and is less than or equal to the font height. QuickDraw uses
        // the image height for improved character plotting, because it
        // only draws the visible part of the glyph.
        // The high-order byte of the word is the offset from the top of
        // the font rectangle of the first non-blank (or nonwhite) row in
        // the glyph, and the low-order byte is the number of rows that
        // must be drawn. The Font Manager creates this table.
        if (Type.HasFlag(FontType.ContainsImageHeightTable))
        {
            var imageHeightTable = new (byte Offset, byte NumberOfRows)[numberOfGlyphs];
            for (int i = 0; i < numberOfGlyphs ; i++)
            {
                imageHeightTable[i] = (
                    Offset: data[offset],
                    NumberOfRows: data[offset + 1]);
                offset += 2;
            }

            ImageHeightTable = imageHeightTable;
        }

        Debug.Assert(offset <= data.Length, "Parsed beyond the end of the FontRec data.");
    }
}
