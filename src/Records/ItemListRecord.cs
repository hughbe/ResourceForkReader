using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an dialog or alert box item list ('DITL') in a resource fork.
/// </summary>
public readonly struct ItemListRecord
{
    /// <summary>
    /// Gets the number of items in the item list minus 1.
    /// </summary>
    public ushort ItemCount { get; }

    /// <summary>
    /// Gets the list of items in the item list.
    /// </summary>
    public List<ItemListItem> Items { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the item list data.</param>
    /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
    public ItemListRecord(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 6-152 to 6-156
        if (data.Length < 2)
        {
            throw new ArgumentException("Data must be at least 2 bytes long.", nameof(data));
        }

        int offset = 0;

        // Item count minus 1. This value is 1 less than the total number
        // of items defined in this resource.
        ItemCount = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (ItemCount == 0xFFFF)
        {
            // An ItemCount of 0xFFFF indicates there are no items.
            Items = [];
        }
        else
        {
            var items = new List<ItemListItem>(ItemCount + 1);
            for (int i = 0; i < ItemCount + 1; i++)
            {
                // The format of each item depends on its type.
                items.Add(new ItemListItem(data[offset..], out int itemBytesRead));
                offset += itemBytesRead;

                if (offset % 2 != 0)
                {
                    // Each item starts on a word boundary.
                    offset += 1;
                }
            }

            Items = items;
        }

        Debug.Assert(offset == data.Length, "Did not consume all data for ItemListRecord.");
    }
}
