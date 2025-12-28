using System.Buffers.Binary;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using ResourceForkReader.Utilities;

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
    public List<Item> Items { get; }

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

        var items = new List<Item>(ItemCount + 1);
        for (int i = 0; i < ItemCount + 1; i++)
        {
            // The format of each item depends on its type.
            items.Add(new Item(data[offset..], out int itemBytesRead));
            offset += itemBytesRead;

            if (offset % 2 != 0)
            {
                // Each item starts on a word boundary.
                offset += 1;
            }
        }

        Items = items;

        Debug.Assert(offset == data.Length, "Did not consume all data for ItemListRecord.");
    }

    /// <summary>
    /// Represents a single item in an item list.
    /// </summary>
    public struct Item
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
        public ItemType Type { get; }

        /// <summary>
        /// Gets the item data.
        /// </summary>
        public object Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> struct by parsing binary data.
        /// </summary>
        /// <param name="data">A span containing the item data.</param>
        /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
        /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
        public Item(ReadOnlySpan<byte> data, out int bytesRead)
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
            Type = (ItemType)(enabledAndType & 0x7F);

            int dataBytesRead;
            Data = Type switch
            {
                ItemType.UserItem => new UserItemData(data[offset..], out dataBytesRead),
                ItemType.Help => new HelpItemData(data[offset..], out dataBytesRead),
                ItemType.Button or ItemType.CheckBox or ItemType.RadioButton or ItemType.StaticText or ItemType.EditText => new TextItemData(data[offset..], out dataBytesRead),
                ItemType.IconButton or ItemType.Icon or ItemType.Picture => new IconItemData(data[offset..], out dataBytesRead),
                _ => throw new ArgumentException($"Unknown item type: {Type}.", nameof(data)),
            };
            offset += dataBytesRead;

            bytesRead = offset;
            Debug.Assert(offset <= data.Length, "Did not consume all data for Item.");
        }
    }

    /// <summary>
    /// Represents user item data in an item list.
    /// </summary>
    public readonly struct UserItemData
    {
        /// <summary>
        /// The size of the UserItemData structure in bytes.
        /// </summary>
        public const int Size = 1;

        /// <summary>
        /// Gets length of reserved data.
        /// </summary>
        public byte ReservedLength { get; }

        /// <summary>
        /// Gets the reserved data.
        /// </summary>
        public byte[] ReservedData { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserItemData"/> struct by parsing binary data.
        /// </summary>
        /// <param name="data">A span containing the user item data.</param>
        /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
        /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
        public UserItemData(ReadOnlySpan<byte> data, out int bytesRead)
        {
            if (data.Length < Size)
            {
                throw new ArgumentException($"Data must be at least {Size} bytes long.", nameof(data));
            }

            int offset = 0;

            ReservedLength = data[offset];
            offset += 1;

            if (data.Length - offset < ReservedLength)
            {
                throw new ArgumentException("Data is too small to contain the specified reserved data.", nameof(data));
            }

            // Advance by the reserved size.
            ReservedData = data.Slice(offset, ReservedLength).ToArray();
            offset += ReservedLength;

            bytesRead = offset;
            Debug.Assert(offset <= data.Length, "Did not consume all data for UserItemData.");
        }
    }

    /// <summary>
    /// Represents help item data in an item list.
    /// </summary>
    public readonly struct HelpItemData
    {
        /// <summary>
        /// The minimum size of a valid HelpItemData in bytes.
        /// </summary>
        public const int MinSize = 5;
        
        /// <summary>
        /// The size of the HelpItemData structure in bytes.
        /// </summary>
        public byte Size { get; }

        /// <summary>
        /// Gets the type of help item.
        /// </summary>
        public HelpItemType Type { get; }

        /// <summary>
        /// Gets the resource ID of the help item.
        /// </summary>
        public ushort ResourceID { get; }

        /// <summary>
        /// Gets the item number of the help item.
        /// </summary>
        public ushort? ItemNumber { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpItemData"/> struct by parsing binary data.
        /// </summary>
        /// <param name="data">A span containing the help item data.</param>
        /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
        /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
        public HelpItemData(ReadOnlySpan<byte> data, out int bytesRead)
        {
            if (data.Length < MinSize)
            {
                throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
            }

            int offset = 0;

            // Size. This specifies the number of bytes contained in the
            // rest of this element.
            // This is set to 4 for an item identified by either the
            // HMScanhdlg or HMScanhrct identifier, or it’s set to 6
            // for an item identified by the HMScanAppendhdlg identifier.
            Size = data[offset];
            offset += 1;

            if (Size != 4 && Size != 6)
            {
                throw new ArgumentException($"Invalid HelpItemData size: {Size}.", nameof(data));
            }

            if (data.Length - offset + 1 < Size)
            {
                throw new ArgumentException($"Data must be at least {Size} bytes long.", nameof(data));
            }

            // HelpItem type. This specifies the type of help item defined
            // in the resource.
            // ■ For an item identified by the HMScanhdlg identifier, this
            // element contains the value 1.
            // ■ For an item identified by the HMScanhrct identifier, this
            // element contains the value 2.
            // ■ For an item identified by the HMScanAppendhdlg identifier,
            // this element contains the value 8.
            Type = (HelpItemType)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            // Resource ID. This is the resource ID of the resource containing
            // the help messages for this alert box or dialog box.
            // ■ For an item identified by either the HMScanhdlg or HMScanAppendhdlg
            // identifier, this is the ID of an 'hdlg' resource.
            // ■ For an item identified by the HMScanhrct identifier,
            // this is the ID of an 'hrct' resource.
            ResourceID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            // Item number. This is available only for an item identified
            // by the HMScanAppendhdlg identifier. This is the item
            // number within the alert box or dialog box after which the
            // help messages specified in the 'hdlg' resource should be
            // displayed. These help messages relate to the items that
            // are appended to the alert box or dialog box. (The item
            // list resource does not contain these 2 bytes for items
            // identified by either the HMScanhdlg or HMScanhrct
            // identifier.)
            if (Size >= 6)
            {
                ItemNumber = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
                offset += 2;
            }
            else
            {
                ItemNumber = null;
            }

            bytesRead = offset;
            Debug.Assert(offset <= data.Length, "Did not consume all data for HelpItemData.");
        }

        /// <summary>
        /// The type of the help item.
        /// </summary>        
        public enum HelpItemType : ushort
        {
            /// <summary>
            /// HMScanhdlg help item.
            /// </summary>
            HMScanhdlg = 1,

            /// <summary>
            /// HMScanhrct help item.
            /// </summary>
            HMScanhrct = 2,

            /// <summary>
            /// HMScanAppendhdlg help item.
            /// </summary>
            HMScanAppendhdlg = 8,
        }
    }

    /// <summary>
    /// Represents text item data in an item list.
    /// </summary>
    public readonly struct TextItemData
    {
        /// <summary>
        /// Gets the text of the item.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextItemData"/> struct by parsing binary data.
        /// </summary>
        /// <param name="data">A span containing the text item data.</param>
        /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
        /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
        public TextItemData(ReadOnlySpan<byte> data, out int bytesRead)
        {
            int offset = 0;

            // Text. This specifies the text that appears in the item. This element consists of a length
            // byte and as many as 255 additional bytes for the text. (“Titles for Buttons, Checkboxes,
            // and Radio Buttons” beginning on page 6-37 and “Text Strings for Static Text and
            // Editable Text Items” beginning on page 6-40 contain recommendations about appropriate text in items.)
            // ■ For a button, checkbox, or radio button, this is the title for that control.
            // ■ For a static text item, this is the text of the item.
            // ■ For an editable text item, this can be an empty string (in which case the editable text
            // item contains no text), or it can be a string that appears as the default string in the
            // editable text item.
            Text = SpanUtilities.ReadPascalString(data[offset..]);
            offset += 1 + Text.Length;

            bytesRead = offset;
            Debug.Assert(offset <= data.Length, "Did not consume all data for TextItemData.");
        }
    }

    /// <summary>
    /// Represents icon item data in an item list.
    /// </summary>
    public readonly struct IconItemData
    {
        /// <summary>
        /// The size of the IconItemData structure in bytes.
        /// </summary>
        public const int Size = 3;

        /// <summary>
        /// Gets the reserved field.
        /// </summary>
        public byte Reserved { get; }

        /// <summary>
        /// Gets the icon resource ID.
        /// </summary>
        public ushort IconResourceID { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconItemData"/> struct by parsing binary data.
        /// </summary>
        /// <param name="data">A span containing the icon item data.</param>
        /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
        /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
        public IconItemData(ReadOnlySpan<byte> data, out int bytesRead)
        {
            if (data.Length < Size)
            {
                throw new ArgumentException($"Data must be at least {Size} bytes long.", nameof(data));
            }

            int offset = 0;

            Reserved = data[offset];
            offset += 1;

            // Resource ID.
            // ■ For a control item, this is the resource ID of a 'CTRL' resource.
            // ■ For an icon item, this is the resource ID of an 'ICON' resource
            // and, optionally, a 'cicn' resource
            // ■ For a picture item, this is the resource ID of a 'PICT'
            // resource. 
            IconResourceID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            bytesRead = offset;
            Debug.Assert(offset <= data.Length, "Did not consume all data for IconItemData.");
        }
    }

    /// <summary>
    /// The type of item in an item list.
    /// </summary>
    public enum ItemType : byte
    {
        /// <summary>
        /// User item.
        /// </summary>
        UserItem = 0,

        /// <summary>
        /// Help item.
        /// </summary>
        Help = 1,

        /// <summary>
        /// Button item.
        /// </summary>
        Button = 4,

        /// <summary>
        /// Checkbox item.
        /// </summary>
        CheckBox = 5,

        /// <summary>
        /// Radio button item.
        /// </summary>
        RadioButton = 6,

        /// <summary>
        /// Icon button item.
        /// </summary>
        IconButton = 7,

        /// <summary>
        /// Static text item.
        /// </summary>
        StaticText = 8,

        /// <summary>
        /// Editable text item.
        /// </summary>
        EditText = 16,

        /// <summary>
        /// Icon item.
        /// </summary>
        Icon = 32,

        /// <summary>
        /// Picture item.
        /// </summary>
        Picture = 64
    }
}
