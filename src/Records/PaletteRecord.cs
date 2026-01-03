using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Palette Record ('pltt') in a resource fork.
/// </summary>
public readonly struct PaletteRecord
{
    /// <summary>
    /// Gets the minimum size of a Palette Record in bytes.
    /// </summary>
    public const int MinSize = 16;

    /// <summary>
    /// Gets the number of palette entries.
    /// </summary>
    public ushort NumberOfEntries { get; }

    /// <summary>
    /// Gets the reserved fields.
    /// </summary>
    public ushort Reserved1 { get; }

    /// <summary>
    /// Gets the reserved fields.
    /// </summary>
    public ushort Reserved2 { get; }

    /// <summary>
    /// Gets the reserved fields.
    /// </summary>
    public ushort Reserved3 { get; }

    /// <summary>
    /// Gets the reserved fields.
    /// </summary>
    public ushort Reserved4 { get; }
    
    /// <summary>
    /// Gets the reserved fields.
    /// </summary>
    public ushort Reserved5 { get; }

    /// <summary>
    /// Gets the reserved fields.
    /// </summary>
    public ushort Reserved6 { get; }

    /// <summary>
    /// Gets the reserved fields.
    /// </summary>
    public ushort Reserved7 { get; }

    /// <summary>
    /// Gets the list of palette entries.
    /// </summary>
    public List<PaletteEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaletteRecord"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the palette record data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid.</exception>
    public PaletteRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException("Data is too short to be a valid Palette Record.", nameof(data));
        }

        // Structure documented in https://dev.os9.ca/techpubs/mac/ACI/ACI-28.html
        // and https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/ResourceFile.cc#L2736-L2751
        int offset = 0;

        NumberOfEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved2 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved3 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved4 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved5 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved6 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved7 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var entries = new List<PaletteEntry>(NumberOfEntries);
        for (int i = 0; i < NumberOfEntries; i++)
        {
            if (offset + PaletteEntry.Size > data.Length)
            {
                throw new ArgumentException("Data is too short to contain all palette entries.", nameof(data));
            }

            entries.Add(new PaletteEntry(data.Slice(offset, PaletteEntry.Size)));
            offset += PaletteEntry.Size;
        }

        Entries = entries;

        Debug.Assert(offset == data.Length, "Did not consume all data for Palette Record.");
    }
}

