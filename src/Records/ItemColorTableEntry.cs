using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an entry in an Item Color Table Resource ('ictb').
/// </summary>
public readonly struct ItemColorTableEntry
{
    /// <summary>
    /// Gets the size of an Item Color Table Entry in bytes.
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Gets the item data.
    /// </summary>
    public ushort Data { get; }

    /// <summary>
    /// Gets the item offset.
    /// </summary>
    public ushort Offset { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemColorTableEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the item color table entry data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is not the correct size for an Item Color Table Entry.</exception>
    public ItemColorTableEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long to be a valid Item Color Table Entry.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 6-159 to 6-164
        int offset = 0;

        // Item data. This contains information about how this item is
        // described in the rest of this resource.
        // For a control, this is the length (in bytes) of its control color table
        // For a static text item or an editable text item, the bits of this
        // element determine which elements of the text style table to use
        // and are interpreted as follows:
        // Bit Meaning
        // 0 Change the font family.
        // 1 Change the typeface.
        // 2 Change the font size.
        // 3 Change the font foreground color.
        // 4 Add the font size.
        // 13 Change the font background color.
        // 14 Change the font mode.
        // 15 The font element is an offset to the name.
        Data = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Item offset. The number of bytes from the beginning of the
        // resource to either the control color table or the text style
        // table that describes this item.
        Offset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for Item Color Table Entry.");
    }
}
