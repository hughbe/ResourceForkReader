using System.Buffers.Binary;
using System.Diagnostics;
using System.Numerics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Family Record ('FOND') in a resource fork.
/// </summary>
public readonly partial struct FontFamilyRecord
{
    /// <summary>
    /// The minimum size of a Font Family Record.
    /// </summary>
    public const int MinSize = 52;

    /// <summary>
    /// Gets the font family flags.
    /// </summary>
    public FontFamilyFlags Flags { get; }

    /// <summary>
    /// Gets the font family ID.
    /// </summary>
    public ushort ID { get; }

    /// <summary>
    /// Gets the font family first character.
    /// </summary>
    public ushort FirstCharacter { get; }

    /// <summary>
    /// Gets the font family last character.
    /// </summary>
    public ushort LastCharacter { get; }

    /// <summary>
    /// Gets the font family maximum ascent.
    /// </summary>
    public ushort MaximumAscent { get; }

    /// <summary>
    /// Gets the font family maximum descent.
    /// </summary>
    public ushort MaximumDescent { get; }

    /// <summary>
    /// Gets the font family maximum leading.
    /// </summary>
    public ushort MaximumLeading { get; }

    /// <summary>
    /// Gets the font family maximum glyph width.
    /// </summary>
    public ushort MaximumGlyphWidth { get; }

    /// <summary>
    /// Gets the offset to family glyph-width table.
    /// </summary>
    public uint GlyphWidthTableOffset { get; }

    /// <summary>
    /// Gets the offset to family kerning table.
    /// </summary>
    public uint KerningTableOffset { get; }

    /// <summary>
    /// Gets the offset to family style-mapping table.
    /// </summary>
    public uint StyleMappingTableOffset { get; }

    /// <summary>
    /// Gets the font family style properties.
    /// </summary>
    public StyleProperties ExtraWidths { get; }

    /// <summary>
    /// Gets the international information field 1.
    /// </summary>
    public ushort InternationalInfo1 { get; }

    /// <summary>
    /// Gets the international information field 2.
    /// </summary>
    public ushort InternationalInfo2 { get; }

    /// <summary>
    /// Gets the version.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// Gets the font association table.
    /// </summary>
    public FontAssociationTable FontAssociationTable { get; }

    /// <summary>
    /// Gets the font family glyph width table, if present.
    /// </summary>
    public FontFamilyGlyphWidthTable? GlyphWidthTable { get; }

    /// <summary>
    /// Gets the font family kerning table, if present.
    /// </summary>
    public FontFamilyKerningTable? KerningTable { get; }

    /// <summary>
    /// Gets the font family style mapping table, if present.
    /// </summary>
    public FontFamilyStyleMappingTable? StyleMappingTable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontFamilyRecord"/> struct by reading from the specified data span.
    /// </summary>
    /// <param name="data">The span containing the Font Family Record data.</param>
    /// <exception cref="ArgumentException">Thrown when the data span is too short to contain a valid Font Family Record.</exception>
    public FontFamilyRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long to read a FontFamilyRecord.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-90 to 4-94
        int offset = 0;

        // Font family flags. An integer value, the bits of which specify
        // general characteristics of the font family. This value is
        // represented by the ffFlags field in the FamRec data type.
        // The bits in the ffFlags field have the following meanings: 
        // Bit Meaning
        // 0 This bit is reserved by Apple and should be cleared to 0.
        // 1 This bit is set to 1 if the resource contains a glyph-width table.
        // 2–11 These bits are reserved by Apple and should be cleared to 0.
        // 12 This bit is set to 1 if the font family ignores the value of the FractEnable
        // global variable when deciding whether to use fixed-point values for stylistic
        // variations; the value of bit 13 is then the deciding factor. The value of the
        // FractEnable global variable is set by the SetFractEnable procedure.
        // 13 This bit is set to 1 if the font family should use integer extra width for stylistic
        // variations. If not set, the font family should compute the fixed-point extra
        // width from the family style-mapping table, but only if the FractEnable
        // global variable has a value of TRUE.
        // 14 This bit is set to 1 if the family fractional-width table is not used, and is
        // cleared to 0 if the table is used.
        // 15 This bit is set to 1 if the font family describes fixed-width fonts, and is cleared
        // to 0 if the font describes proportional fonts.
        Flags = (FontFamilyFlags)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Font family ID. An integer value that specifies the 'FOND' resource
        // ID number for this font family. This value is represented by the
        // ffFamID field in the FamRec data type.
        ID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Font family first character. An integer value that specifies the
        // ASCII character code of the first glyph in the font family. This
        // value is represented by the ffFirstChar field in the FamRec data
        // type.
        FirstCharacter = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        //  Font family last character. An integer value that specifies the
        // ASCII character code of the last glyph in the font family. This
        // value is represented by the ffLastChar field in the FamRec data
        // type.
        LastCharacter = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Font family maximum ascent. The maximum ascent measurement for a
        // one-point font of the font family. This value is in a 16-bit
        // fixed-point format with an integer part in the high-order 4 bits
        // and a fractional part in the low-order 12 bits. This value is
        // represented by the ffAscent field in the FamRec data type.
        MaximumAscent = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Font family maximum descent. The maximum descent measurement for a
        // one-point font of the font family. This value is in a 16-bit
        // fixed-point format with an integer part in the high-order 4 bits
        // and a fractional part in the low-order 12 bits. This value is
        // represented by the ffDescent field in the FamRec data type.
        MaximumDescent = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Font family maximum leading. The maximum leading for a 1-point
        // font of the font family. This value is in a 16-bit fixed-point
        // format with an integer part in the high-order 4 bits and a
        // fractional part in the low-order 12 bits. This value is
        // represented by the ffLeading field in the FamRec data type.
        MaximumLeading = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Font family maximum glyph width. The maximum glyph width of any
        // glyph in a one-point font of the font family. This value is in
        // a 16-bit fixed-point format with an integer part in the high-order
        // 4 bits and a fractional part in the low-order 12 bits. This value
        // is represented by the ffWidMax field in the FamRec data type.
        MaximumGlyphWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Offset to family glyph-width table. The offset to the family
        // glyph-width table from the beginning of the font family resource
        // to the beginning of the table, in bytes. The family glyph-width
        // table is described in the section “The Family Glyph-Width Table,”
        // beginning on page 4-98. This value is represented by the ffTabOff
        // field in the FamRec data type.
        GlyphWidthTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Offset to family kerning table. The offset to the beginning of
        // the kerning table from the beginning of the 'FOND' resource, in
        // bytes. The kerning table is described in the section “The Font
        // Family Kerning Table,” beginning on page 4-106. This value is
        // represented by the ffKernOff field in the FamRec data type.
        KerningTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Offset to family style-mapping table. The offset to the style-mapping
        // table from the beginning of the font family resource to the beginning
        // of the table, in bytes. The style-mapping table is described in
        // the section “The Style-Mapping Table,” beginning on page 4-99.
        // This value is represented by the ffStyleOff field in the FamRec
        // data type.
        StyleMappingTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Style properties. An array of 9 integers, each indicating the
        // extra width, in pixels, that would be added to the glyphs of a
        // 1-point font in this font family after a stylistic variation has
        // been applied. This value is represented by the ffProperty field in
        // the FamRec data type, which is an array with nine values. The Font
        // Manager multiplies these values by the requested point size to get
        // the correct width. Each value is in a 16-bit fixed-point format
        // with an integer part in the high-order 4 bits and a fractional
        // part in the low-order 12 bits. If the font with a given stylistic
        // variation already exists as an intrinsic font, the Font Manager
        // ignores the value in the ffProperty field for that style. The
        // values in this array are used as follows:
        // Property index Meaning
        // 1 Extra width for plain text. Should be set to 0.
        // 2 Extra width for bold text.
        // 3 Extra width for italic text.
        // 4 Extra width for underline text.
        // 5 Extra width for outline text.
        // 6 Extra width for shadow text.
        // 7 Extra width for condensed text.
        // 8 Extra width for extended text.
        // 9 Not used. Should be set to 0.
        ExtraWidths = new StyleProperties(data.Slice(offset, StyleProperties.Size));
        offset += StyleProperties.Size;

        // International information. An array of 2 integers reserved for
        // internal use by script management software. This value is
        // represented by the ffIntl field in the FamRec data type.
        InternationalInfo1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        InternationalInfo2 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Version. An integer value that specifies the version number of
        // the font family resource, which indicates whether certain tables
        // are available. This value is represented by the ffVersion field
        // in the FamRec data type. Because this field has been used
        // inconsistently in the system software, it is better to analyze
        // the data in the resource itself instead of relying on the version
        // number. The possible values are as follows: 
        // Value Meaning
        // $0000 Created by the Macintosh system software. The font family
        // resource will not have the glyph-width tables and the fields
        // will contain 0.
        // $0001 Original format as designed by the font developer. This
        // font family record probably has the width tables and most of
        // the fields are filled.
        // $0002 This record may contain the offset and bounding-box tables.
        // $0003 This record definitely contains the offset and bounding-box
        // tables.
        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        FontAssociationTable = new FontAssociationTable(data[offset..], out int bytesReadTable);
        offset += bytesReadTable;

        if (GlyphWidthTableOffset != 0)
        {
            if (GlyphWidthTableOffset >= data.Length)
            {
                throw new ArgumentException("GlyphWidthTableOffset is out of bounds of the data span.", nameof(data));
            }

            GlyphWidthTable = new FontFamilyGlyphWidthTable(data[(int)GlyphWidthTableOffset..], FirstCharacter, LastCharacter, out _);
        }

        if (KerningTableOffset != 0)
        {
            if (KerningTableOffset >= data.Length)
            {
                throw new ArgumentException("KerningTableOffset is out of bounds of the data span.", nameof(data));
            }

            KerningTable = new FontFamilyKerningTable(data[(int)KerningTableOffset..], out _);
        }

        if (StyleMappingTableOffset != 0)
        {
            if (StyleMappingTableOffset >= data.Length)
            {
                throw new ArgumentException("StyleMappingTableOffset is out of bounds of the data span.", nameof(data));
            }

            StyleMappingTable = new FontFamilyStyleMappingTable(data[(int)StyleMappingTableOffset..], out _);
        }

        Debug.Assert(offset <= data.Length, "Read beyond the end of the data for FontFamilyRecord");
    }

    /// <summary>
    /// Gets the font family flags.
    /// </summary>
    [Flags]
    public enum FontFamilyFlags : ushort
    {
        /// <summary>
        /// No flags set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the resource contains a glyph-width table.
        /// </summary>
        HasGlyphWidthTable = 1 << 1,

        /// <summary>
        /// Indicates that the font family ignores the value of the FractEnable global variable.
        /// </summary>
        IgnoreFractEnable = 1 << 12,

        /// <summary>
        /// Indicates that the font family should use integer extra width for stylistic variations.
        /// </summary>
        UseIntegerExtraWidth = 1 << 13,

        /// <summary>
        /// Indicates that the family fractional-width table is not used.
        /// </summary>
        NoFractionalWidthTable = 1 << 14,

        /// <summary>
        /// Indicates that the font family describes fixed-width fonts.
        /// </summary>
        FixedWidthFonts = 1 << 15
    }

    /// <summary>
    /// Gets the font family style properties.
    /// </summary>
    public struct StyleProperties
    {
        /// <summary>
        /// The size of the StyleProperties structure in bytes.
        /// </summary>
        public const int Size = 18;

        /// <summary>
        /// Gets the extra width for plain text.
        /// </summary>
        public ushort PlainTextExtraWidth { get; }

        /// <summary>
        /// Gets the extra width for bold text.
        /// </summary>
        public ushort BoldTextExtraWidth { get; }

        /// <summary>
        /// Gets the extra width for italic text.
        /// </summary>
        public ushort ItalicTextExtraWidth { get; }

        /// <summary>
        /// Gets the extra width for underline text.
        /// </summary>
        public ushort UnderlineTextExtraWidth { get; }

        /// <summary>
        /// Gets the extra width for outline text.
        /// </summary>
        public ushort OutlineTextExtraWidth { get; }

        /// <summary>
        /// Gets the extra width for shadow text.
        /// </summary>
        public ushort ShadowTextExtraWidth { get; }

        /// <summary>
        /// Gets the extra width for condensed text.
        /// </summary>
        public ushort CondensedTextExtraWidth { get; }

        /// <summary>
        /// Gets the extra width for extended text.
        /// </summary>
        public ushort ExtendedTextExtraWidth { get; }

        /// <summary>
        /// Gets the unused field.
        /// </summary>
        public ushort Unused { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleProperties"/> struct by reading from the specified data span.
        /// </summary>
        /// <param name="data">The span containing the StyleProperties data.</param>
        /// <exception cref="ArgumentException">Thrown when the data span is not the correct size to contain StyleProperties.</exception>
        public StyleProperties(ReadOnlySpan<byte> data)
        {
            if (data.Length != Size)
            {
                throw new ArgumentException($"Data must be exactly {Size} bytes long to read StyleProperties.", nameof(data));
            }

            int offset = 0;

            PlainTextExtraWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            BoldTextExtraWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            ItalicTextExtraWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            UnderlineTextExtraWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            OutlineTextExtraWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            ShadowTextExtraWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            CondensedTextExtraWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            ExtendedTextExtraWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            Unused = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            Debug.Assert(offset == data.Length, "Did not consume all data for StyleProperties");
        }
    }
}
