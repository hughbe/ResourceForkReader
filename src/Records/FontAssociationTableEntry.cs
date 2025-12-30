using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Gets the size of a Font Association Table Entry.
/// </summary>
public struct FontAssociationTableEntry
{
    /// <summary>
    /// The size of a Font Association Table Entry.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Gets the point size of the font association.
    /// </summary>
    public ushort PointSize { get; }

    /// <summary>
    /// Gets the style of the font association.
    /// </summary>
    public ushort Style { get; }

    /// <summary>
    /// Gets the resource ID of the font association.
    /// </summary>
    public short ResourceID { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontAssociationTableEntry"/> struct by reading from the specified data span.
    /// </summary>
    /// <param name="data">The span containing the Font Association Table Entry data.</param>
    /// <exception cref="ArgumentException">Thrown when the data span is not the correct size to contain a Font Association Table Entry.</exception>
    public FontAssociationTableEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long to read a FontAssociationTableEntry.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-47 to 4-48
        // and https://adobe-type-tools.github.io/font-tech-notes/pdfs/0091.Mac_Fond.pdf
        int offset = 0;

        PointSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Style = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for FontAssociationTableEntry");
    }
}
