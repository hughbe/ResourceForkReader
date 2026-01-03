using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Menu ('MENU') in a resource fork.
/// </summary>
public readonly struct MenuRecord
{
    /// <summary>
    /// Gets the menu ID.
    /// </summary>
    public ushort ID { get; }

    /// <summary>
    /// Gets the menu width.
    /// </summary>
    public ushort Width { get; }

    /// <summary>
    /// Gets the menu height.
    /// </summary>
    public ushort Height { get; }

    /// <summary>
    /// Gets the resource ID of the menu definition procedure.
    /// </summary>
    public short DefinitionProcedureResourceID { get; }

    /// <summary>
    /// Gets the placeholder value.
    /// </summary>
    public ushort Placeholder1 { get; }

    /// <summary>
    /// Gets the enabled state bitmask.
    /// </summary>
    public uint EnabledStateBitmask { get; }

    /// <summary>
    /// Gets the length of the menu title.
    /// </summary>
    public byte TitleLength { get; }

    /// <summary>
    /// Gets the menu title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the list of menu items.
    /// </summary>
    public List<StandardMenuItem> Items { get; }

    /// <summary>
    /// Gets the placeholder value.
    /// </summary>
    public byte Placeholder2 { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the menu data.</param>
    /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
    public MenuRecord(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 3-151 to 3-155
        int offset = 0;

        // Menu ID. Each menu in your application should have a unique
        // menu ID. Note that the menu ID does not have to match the
        // resource ID, although by convention most applications assign
        // the same number for a menu’s resource ID and menu ID. A
        // negative menu ID indicates a menu belonging to a desk
        // accessory (except for submenus of a desk accessory). A menu
        // ID from 1 through 235 indicates a menu (or submenu) of an
        // application; a menu ID from 236 through 255 indicates a
        // submenu of a desk accessory. Apple reserves the menu ID of 0. 
        ID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Placeholder (two integers containing 0) for the menu’s width
        // and height. After reading in the resource data, the Menu
        // Manager requests the menu’s menu definition procedure to
        // calculate the width and height of the menu and to store
        // these values in the menuWidth and menuHeight fields of
        // the menu record.
        Width = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Height = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Resource ID of the menu’s menu definition procedure.
        // If the integer 0 appears here (as specified by the
        // textMenuProc constant in the Rez input file), the Menu
        // Manager uses the standard menu definition procedure to
        // manage the menu. If you provide your own menu definition
        // procedure, its resource ID should appear in these bytes.
        // After reading in the menu’s resource data, the Menu Manager
        // reads in the menu definition procedure, if necessary.
        // The Menu Manager stores a handle to the menu’s menu definition
        // procedure in the menuProc field of the menu record. 
        DefinitionProcedureResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Placeholder (an integer containing 0).
        Placeholder1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The initial enabled state of the menu and first 31 menu items.
        // This is a 32-bit value, where bits 1–31 indicate if the
        // corresponding menu item is disabled or enabled, and bit
        // 0 indicates whether the menu is enabled or disabled. The
        // Menu Manager automatically enables menu items greater than
        // 31 when a menu is created.
        EnabledStateBitmask = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The length (in bytes) of the menu title.
        TitleLength = data[offset];
        offset += 1;

        // The title of the menu. 
        Title = Encoding.ASCII.GetString(data.Slice(offset, TitleLength));
        offset += TitleLength;

        // Variable-length data that describes the menu items. If you
        // provide your own menu definition procedure, you can define
        // and provide this variable-length data according to the needs
        // of your procedure. The Menu Manager simply reads in the data
        // for each menu item and stores it as variable data at the end
        // of the menu record. The menu definition procedure is responsible
        // for interpreting the contents of the data. For example, the
        // standard menu definition procedure interprets this data
        // according to the description given in the following paragraphs.
        var menuItems = new List<StandardMenuItem>();
        while (data[offset] != 0)
        {
            var menuItem = new StandardMenuItem(data[offset..], out var itemBytesRead);
            offset += itemBytesRead;
            menuItems.Add(menuItem);
        }

        Items = menuItems;

        // Placeholder (a byte containing 0) to indicate the end of the menu item definitions.
        Placeholder2 = data[offset];
        offset += 1;

        // Sometimes has a padding byte for even length.
#if DEBUG
        if (offset < data.Length && offset % 2 != 0)
        {
            offset += 1;
        }

        // Seen additional data in some resources, ignore it.

        Debug.Assert(offset <= data.Length, "Did not consume all data for MenuRecord.");
#endif
    }

    /// <summary>
    /// Represents a standard menu item.
    /// </summary>
    public readonly struct StandardMenuItem
    {
        /// <summary>
        /// The minimum size of a StandardMenuItem in bytes.
        /// </summary>
        public const int MinSize = 1;

        /// <summary>
        /// Gets the length of the menu item text.
        /// </summary>
        public byte TextLength { get; }

        /// <summary>
        /// Gets the menu item text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the icon number, script code, or 0.
        /// </summary>
        public byte IconNumber { get; }

        /// <summary>
        /// Gets the keyboard equivalent.
        /// </summary>
        public byte KeyboardEquivalent { get; }

        /// <summary>
        /// Gets the marking character or submenu ID.
        /// </summary>
        public byte MarkingCharacterOrSubmenuID { get; }

        /// <summary>
        /// Gets the font style.
        /// </summary>
        public byte FontStyle { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardMenuItem"/> struct by parsing binary data.
        /// </summary>
        /// <param name="data">A span containing the menu item data.</param>
        /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
        /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
        public StandardMenuItem(ReadOnlySpan<byte> data, out int bytesRead)
        {
            if (data.Length < MinSize)
            {
                throw new ArgumentException("Insufficient data for StandardMenuItem.", nameof(data));
            }

            int offset = 0;

            // The length (in bytes) of the menu item text.
            TextLength = data[offset];
            offset += 1;

            if (data.Length - offset < TextLength)
            {
                throw new ArgumentException("Insufficient data for StandardMenuItem text.", nameof(data));
            }

            // Text of the menu item
            Text = Encoding.ASCII.GetString(data.Slice(offset, TextLength));
            offset += TextLength;

            // Icon number, script code, or 0 (as specified by the noicon
            // constant in a Rez input file) if the menu item doesn’t
            // contain an icon and uses the system script. The icon number
            // is a number from 1 through 255 (or from 1 through 254 for
            // small or reduced icons). The Menu Manager adds 256 to the
            // icon number to generate the resource ID of the menu item’s
            // icon. If a menu item has an icon, you should also provide a
            // 'cicn' or an 'ICON' resource with the resource ID equal to
            // the icon number plus 256. If you want the Menu Manager to
            // reduce an 'ICON' resource to the size of a small icon, also
            // provide the value $1D in the keyboard equivalent field.
            // If you provide an 'SICN' resource, provide $1E in the
            // keyboard equivalent field. Otherwise, the Menu Manager
            // looks first for a 'cicn' resource with the calculated
            // resource ID and uses that icon. If you want the Menu
            // Manager to draw the item’s text in a script other than
            // the system script, specify the script code here and
            // also provide $1C in the keyboard equivalent field.
            // If the script system for the specified script is
            // installed, the Menu Manager draws the item’s text using
            // that script. An item that is drawn in a script other
            // than the system script cannot also have an icon. 
            IconNumber = data[offset];
            offset += 1;

            // Keyboard equivalent (specified as a 1-byte character),
            // the value $1B (as specified by the constant hierarchicalMenu
            // in a Rez input file) if the item has a submenu, the value
            // $1C if the item uses a script other than the system script,
            // or 0 (as specified by the nokey constant in a Rez input file)
            // if the item has neither a keyboard equivalent nor a submenu
            // and uses the system script. A menu item can have a keyboard
            // equivalent, a submenu, a small icon, a reduced icon, or a
            // script code, but not more than one of these characteristics.
            // For items containing icons, you can provide $1D in this field
            // if you want the Menu Manager to reduce an 'ICON' resource to
            // the size of a small icon. Provide $1E if you want the Menu
            // Manager to use an 'SICN' resource for the item’s icon. The
            // values $01 through $1A as well as $1F and $20 are reserved
            // for use by Apple; your application should not use any of
            // these reserved values in this field.
            KeyboardEquivalent = data[offset];
            offset += 1;

            // Marking character, the menu ID of the item’s submenu, or
            // 0 (as specified by the nomark constant in a Rez input
            // file) if the item has neither a mark nor a submenu. A
            // menu item can have a mark or a submenu, but not both.
            // Submenus of an application should have menu IDs from
            // 1 through 235; submenus of a desk accessory should have
            // menu IDs from 236 through 255. 
            MarkingCharacterOrSubmenuID = data[offset];
            offset += 1;

            // Font style of the menu item. The constants bold, italic,
            // plain, outline, and shadow can be used in a Rez input
            // file to define their corresponding styles.
            FontStyle = data[offset];
            offset += 1;

            bytesRead = offset;
            Debug.Assert(offset <= data.Length, "Did not consume all data for StandardMenuItem.");
        }
    }
}
