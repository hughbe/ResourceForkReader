using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Gets the minimum size of a Font Association Table.
/// </summary>
public partial struct FontAssociationTable
{
    /// <summary>
    /// The minimum size of a Font Association Table.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of entries in the Font Association Table minus one.
    /// </summary>
    public ushort NumberOfEntries { get; }

    /// <summary>
    /// Gets the entries in the Font Association Table.
    /// </summary>
    public List<FontAssociationTableEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontAssociationTable"/> struct by reading from the specified data span.
    /// </summary>
    /// <param name="data">The span containing the Font Association Table data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when the data span is too short to contain a valid Font Association Table.</exception>
    public FontAssociationTable(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long to read a FontAssociationTable.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-47 to 4-48
        // and https://adobe-type-tools.github.io/font-tech-notes/pdfs/0091.Mac_Fond.pdf
        int offset = 0;

        NumberOfEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var entries = new List<FontAssociationTableEntry>(NumberOfEntries + 1);
        for (int i = 0; i < NumberOfEntries + 1; i++)
        {
            if (data.Length < offset + FontAssociationTableEntry.Size)
            {
                throw new ArgumentException("Data is too short to contain all Font Association Table entries.", nameof(data));
            }

            var entry = new FontAssociationTableEntry(data.Slice(offset, FontAssociationTableEntry.Size));
            entries.Add(entry);
            offset += FontAssociationTableEntry.Size;
        }

        Entries = entries;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Read beyond the end of the data for FontAssociationTable");
    }
}
