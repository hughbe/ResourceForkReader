using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a ROM override list resource.
/// </summary>
public readonly struct ROMOverrideList
{
    /// <summary>
    /// The minimum size of a ROM override list in bytes.
    /// </summary>
    public const int MinSize = 4;

    /// <summary>
    /// Gets the version number of the ROM override list.
    /// </summary>
    public ushort VersionNumber { get; }

    /// <summary>
    /// Gets the number of ROM override entries in the list.
    /// </summary>
    public ushort NumberOfResources { get; }

    /// <summary>
    /// Gets the list of ROM override entries.
    /// </summary>
    public List<ROMOverrideListEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ROMOverrideList"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the ROM override list data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to contain the ROM override list.</exception>
    public ROMOverrideList(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 1-135 to 1-136
        int offset = 0;

        VersionNumber = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        NumberOfResources = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        var entries = new List<ROMOverrideListEntry>(NumberOfResources + 1);
        for (int i = 0; i < NumberOfResources + 1; i++)
        {
            if (offset + ROMOverrideListEntry.Size > data.Length)
            {
                throw new ArgumentException("Data is too short to contain all ROM override list entries.", nameof(data));
            }

            var entry = new ROMOverrideListEntry(data[offset..(offset + ROMOverrideListEntry.Size)]);
            entries.Add(entry);
            offset += ROMOverrideListEntry.Size;   
        }

        Entries = entries;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for ROM override list.");
    }
}
