using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Header Table in a resource fork.
/// </summary>
public readonly struct FontHeaderTable
{
    /// <summary>
    /// Size of a Font Header Table in bytes.
    /// </summary>
    public const int Size = 54;

    /// <summary>
    /// Gets the version of the Font Header Table.
    /// </summary>
    public uint Version { get; }

    /// <summary>
    /// Gets the font revision.
    /// </summary>
    public uint FontRevision { get; }

    /// <summary>
    /// Gets the checksum adjustment.
    /// </summary>
    public uint CheckSumAdjustment { get; }

    /// <summary>
    /// Gets the magic number.
    /// </summary>
    public uint MagicNumber { get; }

    /// <summary>
    /// Gets the flags.
    /// </summary>
    public ushort Flags { get; }

    /// <summary>
    /// Gets the units per em.
    /// </summary>
    public ushort UnitsPerEm { get; }

    /// <summary>
    /// Gets the creation date.
    /// </summary>
    public DateTime CreationDate { get; }

    /// <summary>
    /// Gets the modification date.
    /// </summary>
    public DateTime ModificationDate { get; }

    /// <summary>
    /// Gets the minimum x-coordinate of the font's bounding box.
    /// </summary>
    public short XMin { get; }

    /// <summary>
    /// Gets the minimum y-coordinate of the font's bounding box.
    /// </summary>
    public short YMin { get; }

    /// <summary>
    /// Gets the maximum x-coordinate of the font's bounding box.
    /// </summary>
    public short XMax { get; }

    /// <summary>
    /// Gets the maximum y-coordinate of the font's bounding box.
    /// </summary>
    public short YMax { get; }

    /// <summary>
    /// Gets the style.
    /// </summary>
    public ushort Style { get; }

    /// <summary>
    /// Gets the smallest readable size.
    /// </summary>
    public ushort SmallestReadableSize { get; }

    /// <summary>
    /// Gets the direction.
    /// </summary>
    public short FontDirection { get; }

    /// <summary>
    /// Gets the location table format.
    /// </summary>
    public short LocationTableFormat { get; }

    /// <summary>
    /// Gets the glyph data format.
    /// </summary>
    public short GlyphDataFormat { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontHeaderTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Font Header Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public FontHeaderTable(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length {data.Length} does not equal the required size of {Size} bytes for a Font Header Table.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-79 to 4-82
        int offset = 0;

        //  Version. The version number of the table, as a fixed-point value. This
        // value is $00010000 if the version number is 1.0.
        Version = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Font revision. A fixed-point value set by the font designer. 
        FontRevision = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Checksum adjustment. The checksum of the font, as an unsigned long integer.
        CheckSumAdjustment = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        MagicNumber = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Flags = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Units per em. This unsigned integer value represents a power of 2 that
        // ranges from 64 to 16,384. Apple’s TrueType fonts use the value 2048.
        UnitsPerEm = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Creation date. The date this font was created. This is a long date-time
        // value of data type LongDateTime, which is a 64-bit, signed representation
        // of the number of seconds since Jan. 1, 1904.
        CreationDate = SpanUtilities.ReadMacOSTimestampLong(data.Slice(offset, 8));
        offset += 8;

        // Modification date. The date this font was last modified. This is a
        // long date-time value of data type LongDateTime, which is a 64-bit,
        // signed representation of the number of seconds since Jan. 1, 1904. 
        ModificationDate = SpanUtilities.ReadMacOSTimestampLong(data.Slice(offset, 8));
        offset += 8;

        // Not documented.
        XMin = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Not documented.
        YMin = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Not documented.
        XMax = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Not documented.
        YMax = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Not documented.
        Style = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Smallest readable size. The smallest readable size for the font, in
        // pixels per em. The RealFont function, which is described in the
        // section “RealFont” beginning on page 4-52, returns FALSE for a
        // TrueType font if the requested size is smaller than this value. 
        SmallestReadableSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Not documented.
        FontDirection = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Location table format. The format of the location table (tag name:
        // 'loca'), as an signed integer value. The table has two formats: if
        // the value is 0, the table uses the short offset format; if the value
        // is 1, the table uses the long offset format. The location table is
        // described in the section “The Location Table” on page 4-84.
        LocationTableFormat = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Not documented.
        GlyphDataFormat = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Offset should not exceed data length.");
    }
}
