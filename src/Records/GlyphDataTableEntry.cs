using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Glyph Data Table Entry in a resource fork.
/// </summary>
public readonly struct GlpyhDataTableEntry
{
    /// <summary>
    /// Minimum size of a Glyph Data Table Entry in bytes.
    /// </summary>
    public const int MinSize = 10;

    /// <summary>
    /// Gets the number of contours in the glyph.
    /// </summary>
    public short NumberOfContours { get; }

    /// <summary>
    /// Gets the minimum x-coordinate of the glyph's bounding box.
    /// </summary>
    public short XMin { get; }

    /// <summary>
    /// Gets the minimum y-coordinate of the glyph's bounding box.
    /// </summary>
    public short YMin { get; }

    /// <summary>
    /// Gets the maximum x-coordinate of the glyph's bounding box.
    /// </summary>
    public short XMax { get; }

    /// <summary>
    /// Gets the maximum y-coordinate of the glyph's bounding box.
    /// </summary>
    public short YMax { get; }

    /// <summary>
    /// Gets the glyph definition data.
    /// </summary>
    public byte[] GlyphData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GlpyhDataTableEntry"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Glyph Data Table Entry data.</param>
    /// <param name="bytesRead">The number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public GlpyhDataTableEntry(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than the minimum required size of {MinSize} bytes for a Glyph Data Table Entry.", nameof(data));
        }
    
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-77 to 4-78
        int offset = 0;

        // Number of contours. If this integer value is positive, it specifies
        // the number of closed curves defined in the outline data for the glyph.
        // If it is –1, it indicates that the glyph is composed of other simple
        // glyphs (see the explanation of component glyphs in the section “The
        // Maximum Profile Table” beginning on page 4-84).
        NumberOfContours = BinaryPrimitives.ReadInt16BigEndian(data[offset..]);
        offset += 2;

        // xMin. The left edge of the glyph’s bounding box, specified in units per em.
        XMin = BinaryPrimitives.ReadInt16BigEndian(data[offset..]);
        offset += 2;

        // yMin. The top edge of the glyph’s bounding box, specified in units per em. 
        YMin = BinaryPrimitives.ReadInt16BigEndian(data[offset..]);
        offset += 2;

        // xMax. The right edge of the glyph’s bounding box, specified in units per em.
        XMax = BinaryPrimitives.ReadInt16BigEndian(data[offset..]);
        offset += 2;

        // yMax. The bottom edge of the glyph’s bounding box, specified in units per em.
        YMax = BinaryPrimitives.ReadInt16BigEndian(data[offset..]);
        offset += 2;

        // Glyph definition data. The data that defines the appearance of the glyph,
        // as described in the TrueType Font Format Specification.
        GlyphData = [];
        offset += GlyphData.Length;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Offset should not exceed data length.");
    }
}
