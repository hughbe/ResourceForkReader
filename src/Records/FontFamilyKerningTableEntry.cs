using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Family Kerning Table Entry in a resource fork.
/// </summary>
public readonly struct FontFamilyKerningTableEntry
{
    /// <summary>
    /// The minimum size of a Font Family Kerning Table Entry in bytes.
    /// </summary>
    public const int MinSize = 4;

    /// <summary>
    /// Gets the style identifier for this kerning table entry.
    /// </summary>
    public ushort Style { get; }

    /// <summary>
    /// Gets the number of kerning pairs in this kerning table entry.
    /// </summary>
    public ushort NumberOfPairs { get; }

    /// <summary>
    /// Gets the list of font family kerning table pairs.
    /// </summary>
    public List<FontFamilyKerningTablePair> Pairs { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontFamilyKerningTableEntry"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the font family kerning table entry data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid or insufficient.</exception>
    public FontFamilyKerningTableEntry(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long to read a FontFamilyKerningTableEntry.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-49 to 4-50
        // and https://adobe-type-tools.github.io/font-tech-notes/pdfs/0091.Mac_Fond.pdf
        int offset = 0;

        Style = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        NumberOfPairs = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var pairs = new List<FontFamilyKerningTablePair>(NumberOfPairs);
        for (int i = 0; i < NumberOfPairs; i++)
        {
            var pair = new FontFamilyKerningTablePair(data.Slice(offset, FontFamilyKerningTablePair.Size));
            offset += FontFamilyKerningTablePair.Size;
            pairs.Add(pair);
        }

        Pairs = pairs;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Read beyond the end of the data for FontFamilyKerningTableEntry");
    }
}