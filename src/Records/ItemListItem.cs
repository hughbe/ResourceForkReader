using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a single item in an item list.
/// </summary>
public struct ItemListItem
{
    /// <summary>
    /// The minimum size of a valid item in bytes.
    /// </summary>
    public const int MinSize = 13;

    /// <summary>
    /// Gets the reserved field.
    /// </summary>
    public uint Reserved { get; }

    /// <summary>
    /// Gets the display rectangle.
    /// </summary>
    public RECT DisplayRectangle { get; }

    /// <summary>
    /// Gets a value indicating whether the item is enabled.
    /// </summary>
    public bool Enabled { get; }

    /// <summary>
    /// Gets the type of the item.
    /// </summary>
    public ItemListItemType Type { get; }

    /// <summary>
    /// Gets the item data.
    /// </summary>
    public object Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemListItem"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the item data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
    public ItemListItem(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException("Data must be at least 4 bytes long.", nameof(data));
        }

        int offset = 0;

        // Reserved. The Dialog Manager uses the element for storage.
        Reserved = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Display rectangle. This determines the size and location
        // of the item in the alert box or dialog box. The display
        // rectangle is specified in coordinates local to the alert
        // box or dialog box; these coordinates specify the upper-left
        // and lower-right corners of the item.
        DisplayRectangle = new RECT(data.Slice(offset, RECT.Size));
        offset += RECT.Size;

        // Enable flag (1 bit). This specifies whether the item is enabled or disabled.
        // If this bit is set, the item is enabled and the Dialog Manager
        // reports to your application whenever mouse-down events occur
        // inside this item.
        // Item type (7 bits).
        // ■ If this bit string is set to 4 (as specified in the Rez input file by the Button constant),
        // then the item is a button.
        // ■ If this bit string is set to 5 (as specified in the Rez input file by the CheckBox
        // constant), then the item is a checkbox.
        // ■ If this bit string is set to 6 (as specified in the Rez input file by the RadioButton
        // constant), then the item is a radio button.
        // ■ If this bit string is set to 8 (as specified in the Rez input file by the StaticText
        // constant), then the item is static text.
        // ■ If this bit string is set to 16 (as specified in the Rez input file by the EditText
        // constant), then the item is editable text.
        var enabledAndType = data[offset];
        offset += 1;

        Enabled = (enabledAndType & 0x80) != 0;
        Type = (ItemListItemType)(enabledAndType & 0x7F);

        int dataBytesRead;
        Data = Type switch
        {
            ItemListItemType.UserItem => new UserItemData(data[offset..], out dataBytesRead),
            ItemListItemType.Help => new HelpItemData(data[offset..], out dataBytesRead),
            ItemListItemType.Button or ItemListItemType.CheckBox or ItemListItemType.RadioButton or ItemListItemType.StaticText or ItemListItemType.EditText => new TextItemData(data[offset..], out dataBytesRead),
            ItemListItemType.IconButton or ItemListItemType.Icon or ItemListItemType.Picture => new IconItemData(data[offset..], out dataBytesRead),
            _ => throw new ArgumentException($"Unknown item type: {Type}.", nameof(data)),
        };
        offset += dataBytesRead;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all data for Item.");
    }
}
