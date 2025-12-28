using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Family Kerning Table in a resource fork.
/// </summary>
public readonly struct FontFamilyKerningTable
{
    /// <summary>
    /// The minimum size of a Font Family Kerning Table in bytes.
    /// </summary>
    public const int MinSize = 2;
    
    /// <summary>
    /// Gets the number of kernings in the kerning table minus one.
    /// </summary>
    public ushort NumberOfKernings { get; }

    /// <summary>
    /// Gets the list of font family kerning table entries.
    /// </summary>
    public List<FontFamilyKerningTableEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontFamilyKerningTable"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the font family kerning table data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid or insufficient.</exception>
    public FontFamilyKerningTable(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-49 to 4-50
        // and https://adobe-type-tools.github.io/font-tech-notes/pdfs/0091.Mac_Fond.pdf
        int offset = 0;

        NumberOfKernings = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var entries = new List<FontFamilyKerningTableEntry>(NumberOfKernings + 1);
        for (int i = 0; i < NumberOfKernings + 1; i++)
        {
            var entry = new FontFamilyKerningTableEntry(data[offset..], out int entryBytesRead);
            offset += entryBytesRead;
            entries.Add(entry);
        }

        Entries = entries;
        
        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Read beyond the end of the data for FontFamilyKerningTable");
    }
}
