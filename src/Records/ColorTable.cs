using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Color Table.
/// </summary>
public readonly struct ColorTable
{
    /// <summary>
    /// Gets the minimum size of a Color Table in bytes.
    /// </summary>
    public const int MinSize = 8;
    
    /// <summary>
    /// Gets the seed value.
    /// </summary>
    public uint Seed { get; }

    /// <summary>
    /// Gets the flags field.
    /// </summary>
    public ColorTableFlags Flags { get; }

    /// <summary>
    /// Gets the number of entries in the color lookup table, minus one.
    /// </summary>
    public ushort NumberOfEntries { get; }

    /// <summary>
    /// Gets the list of color table entries.
    /// </summary>
    public List<ColorTableEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorLookupTableRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the color lookup table data.</param>
    /// <param name="bytesRead">The number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to be a valid Color Lookup Table Record.</exception>
    public ColorTable(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException("Data is too short to be a valid Color Lookup Table Record.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/ImagingWithQuickDraw.pdf
        // 4-104 to 4-105
        int offset = 0;

        // Seed. This contains the resource ID for this resource.
        Seed = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Flags. A value of $0000 identifies this as a color table for a pixel map. A value of $8000 identifies this as a color table for an indexed device.
        Flags = (ColorTableFlags)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Size. One less than the number of color specification entries in the rest of this resource.
        NumberOfEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (NumberOfEntries == ushort.MaxValue)
        {
            // Special case indicating no entries.
            Entries = [];
        }
        else
        {
            // An array of color specification entries. Each entry contains a pixel value and a color specified by the values for the red, green, and blue components of the entry.
            var entries = new List<ColorTableEntry>(NumberOfEntries + 1);
            for (int i = 0; i < NumberOfEntries + 1; i++)
            {
                if (data.Length < offset + ColorTableEntry.Size)
                {
                    throw new ArgumentException("Data is too short to contain all color lookup table entries.", nameof(data));
                }

                entries.Add(new ColorTableEntry(data.Slice(offset, ColorTableEntry.Size)));
                offset += ColorTableEntry.Size;
            }

            Entries = entries;
        }

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all data for Color Lookup Table Record.");
    }
}
