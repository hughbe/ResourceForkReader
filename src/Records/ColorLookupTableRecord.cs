using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Color Lookup Table Resource ('clut').
/// </summary>
public readonly struct ColorLookupTableRecord
{
    /// <summary>
    /// Gets the minimum size of a Color Lookup Table Record in bytes.
    /// </summary>
    public const int MinSize = 8;

    /// <summary>
    /// Gets the first reserved field.
    /// </summary>
    public ushort Reserved1 { get; }

    /// <summary>
    /// Gets the second reserved field.
    /// </summary>
    public ushort Reserved2 { get; }

    /// <summary>
    /// Gets the third reserved field.
    /// </summary>
    public ushort Reserved3 { get; }

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
    public ColorLookupTableRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException("Data is too short to be a valid Color Lookup Table Record.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/ResourceFile.cc#L2761-L2785
        int offset = 0;

        Reserved1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved2 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved3 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        NumberOfEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (NumberOfEntries == ushort.MaxValue)
        {
            // Special case indicating no entries.
            Entries = [];
            Debug.Assert(offset == data.Length, "Did not consume all data for Color Lookup Table Record.");
            return;
        }

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

        Debug.Assert(offset == data.Length, "Did not consume all data for Color Lookup Table Record.");
    }
}
