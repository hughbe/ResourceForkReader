using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Menu Color Table Entry.
/// </summary>
public readonly struct MenuColorTableEntry
{
    /// <summary>
    /// The size of a Menu Color Table Entry in bytes.
    /// </summary>
    public const int Size = 28;

    /// <summary>
    /// Gets the menu ID.
    /// </summary>
    public ushort MenuID { get; }

    /// <summary>
    /// Gets the item ID.
    /// </summary>
    public ushort ItemID { get; }

    /// <summary>
    /// Gets the first RGB color.
    /// </summary>
    public ColorRGB RGB1 { get; }

    /// <summary>
    /// Gets the second RGB color.
    /// </summary>
    public ColorRGB RGB2 { get; }

    /// <summary>
    /// Gets the third RGB color.
    /// </summary>
    public ColorRGB RGB3 { get; }

    /// <summary>
    /// Gets the fourth RGB color.
    /// </summary>
    public ColorRGB RGB4 { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuColorTableEntry"/> struct.
    /// </summary>
    /// <param name="data">A span containing the menu color table entry data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid.</exception>
    public MenuColorTableEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes to be a valid Menu Color Table Entry.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 3-155 to 3-157
        int offset = 0;

        // A menu ID to indicate that this entry is either a menu item entry
        // or menu title entry, 0 to indicate that this entry is a menu bar
        // entry, or –99 to indicate that this is the last entry in this
        // resource.
        MenuID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // An item number to indicate that this entry is a menu item entry,
        // or 0 to indicate that this is either a menu title or menu bar entry.
        // Together, the menu ID and menu item determine how the type of menu
        // color entry is described. See Table 3-7 on page 3-100 for a
        // complete description of how the menu ID and menu item specifications
        // define the type of menu color entry.
        ItemID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // RGB1: for a menu bar entry, the default color for menu titles;
        // for a menu title entry, the title color of a specific menu;
        // for a menu item entry, the mark color for a specific item.
        RGB1 = new ColorRGB(data.Slice(offset, ColorRGB.Size));
        offset += ColorRGB.Size;

        RGB2 = new ColorRGB(data.Slice(offset, ColorRGB.Size));
        offset += ColorRGB.Size;

        //  RGB3: for a menu bar entry, the default color of items in a
        // displayed menu; for a menu title entry, the default color for
        // items in a specific menu; for a menu item entry, the color for
        // the keyboard equivalent of a specific item.
        RGB3 = new ColorRGB(data.Slice(offset, ColorRGB.Size));
        offset += ColorRGB.Size;

        // RGB4: for a menu bar entry, the default color of the menu bar;
        // for a menu title entry, the background color of a specific menu;
        // for a menu item entry, the background color of a specific menu. 
        RGB4 = new ColorRGB(data.Slice(offset, ColorRGB.Size));
        offset += ColorRGB.Size;

        Debug.Assert(offset == data.Length, "Did not consume all data for Menu Color Table Entry.");
    }
}
