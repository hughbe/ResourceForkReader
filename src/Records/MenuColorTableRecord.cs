using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Menu Color Table Record ('mctb') in a resource fork.
/// </summary>
public readonly struct MenuColorTableRecord
{
    /// <summary>
    /// The minimum size of a Menu Color Table Record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the list of menu color table entries.
    /// </summary>
    public ushort NumberOfEntries { get; }

    /// <summary>
    /// Gets the list of menu color table entries.
    /// </summary>
    public List<MenuColorTableEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuColorTableRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 2 bytes of Menu Color Table Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 2 bytes long.</exception>
    public MenuColorTableRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Menu Color Table Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 3-155 to 3-157
        int offset = 0;

        // A count of the number of menu color entry description
        NumberOfEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // a variable number of menu color entries
        var entries = new List<MenuColorTableEntry>(NumberOfEntries);
        for (int i = 0; i < NumberOfEntries; i++)
        {
            entries.Add(new MenuColorTableEntry(data.Slice(offset, MenuColorTableEntry.Size)));
            offset += MenuColorTableEntry.Size;
        }

        Entries = entries;

        // Seen additional data in some resources, ignore it.

        Debug.Assert(offset <= data.Length, "Did not consume all data for Menu Color Table Record.");
    }
}
