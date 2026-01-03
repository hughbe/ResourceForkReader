using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Item Color Table Resource ('ictb').
/// </summary>
public readonly struct ItemColorTableRecord
{
    /// <summary>
    /// Gets the item color table entries.
    /// </summary>
    public List<ItemColorTableEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemColorTableRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the item color table record data.</param>
    public ItemColorTableRecord(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 6-159 to 6-164
        int offset = 0;

        var entries = new List<ItemColorTableEntry>();
        while (offset + ItemColorTableEntry.Size <= data.Length)
        {
            var entry = new ItemColorTableEntry(data.Slice(offset, ItemColorTableEntry.Size));
            entries.Add(entry);
            offset += ItemColorTableEntry.Size;
        }

        Entries = entries;

        Debug.Assert(offset <= data.Length, "Did not consume all data for Item Color Table Record.");
    }   
}
