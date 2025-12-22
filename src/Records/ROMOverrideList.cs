using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

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

    /// <summary>
    /// Gets the number of ROM override entries in the list.
    /// </summary>
    public struct ROMOverrideListEntry
    {
        /// <summary>
        /// The size of a ROM override list entry in bytes.
        /// </summary>
        public const int Size = 6;

        /// <summary>
        /// Gets the resource type of the ROM override.
        /// </summary>
        public string ResourceType { get; }

        /// <summary>
        /// Gets the resource ID of the ROM override.
        /// </summary>
        public ushort ResourceID { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ROMOverrideListEntry"/> struct by parsing binary data.
        /// </summary>
        /// <param name="data">A 6-byte span containing the ROM override list entry data.</param>
        /// <exception cref="ArgumentException">Thrown when data is not exactly 6 bytes.</exception>
        public ROMOverrideListEntry(ReadOnlySpan<byte> data)
        {
            if (data.Length != Size)
            {
                throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
            }

            int offset = 0;

            ResourceType = Encoding.ASCII.GetString(data[offset..(offset + 4)]);
            offset += 4;

            ResourceID = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
            offset += 2;

            Debug.Assert(offset == data.Length, "Did not consume all bytes for ROM override list entry.");
        }
    }
}
