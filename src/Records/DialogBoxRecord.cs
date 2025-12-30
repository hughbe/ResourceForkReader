using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Dialog Box Resource ('DLOG').
/// </summary>
public readonly struct DialogBoxRecord
{
    /// <summary>
    /// The minimum size of a DialogBoxRecord in bytes.
    /// </summary>
    public const int MinSize = 21;

    /// <summary>
    /// Gets the bounds of the dialog box.
    /// </summary>
    public RECT Bounds { get; }

    /// <summary>
    /// Gets the resource ID of the window definition associated with this dialog box.
    /// </summary>
    public ushort WindowDefinitionID { get; }

    /// <summary>
    /// Gets a value indicating whether the dialog box is visible.
    /// </summary>
    public bool Visible { get; }

    /// <summary>
    /// Gets the reserved byte 1.
    /// </summary>
    public byte Reserved1 { get; }

    /// <summary>
    /// Gets a value indicating whether the dialog box has a close box.
    /// </summary>
    public bool CloseBox { get; }

    /// <summary>
    /// Gets the reserved byte 2.
    /// </summary>
    public byte Reserved2 { get; }

    /// <summary>
    /// Gets the reference constant for the dialog box.
    /// </summary>
    public uint ReferenceConstant { get; }

    /// <summary>
    /// Gets the resource ID of the item list ('DITL') associated with this dialog box.
    /// </summary>
    public short ItemListResourceID { get; }

    /// <summary>
    /// Gets the title of the dialog box window.
    /// </summary>
    public string WindowTitle { get; }

    /// <summary>
    /// Gets the positioning option for the dialog box.
    /// </summary>
    public DialogBoxPosition Position { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogBoxRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the dialog box data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to contain a valid DialogBoxRecord.</exception>
    public DialogBoxRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException("Data is too short to contain a DialogBoxRecord.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 6-148 to 6-150
        int offset = 0;

        // Rectangle. This determines the dialog box’s dimensions and,
        // possibly, its position. (The last element in the dialog
        // resource usually specifies a position for the dialog box.)
        Bounds = new RECT(data.Slice(offset, 8));
        offset += 8;

        // Window definition ID.
        // ■ If the integer 0 appears here (as specified in the Rez
        // input file by the dBoxProc window definition ID), the Dialog
        // Manager displays a modal dialog box. 
        // ■ If the integer 4 appears here (as specified in the Rez
        // input file by the noGrowDocProc window definition ID),
        // the Dialog Manager displays a modeless dialog box.
        // ■ If the integer 5 appears here (as specified in the Rez
        // input file by the movableDBoxProc window definition ID),
        // the Dialog Manager displays a movable modal dialog box. 
        WindowDefinitionID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Visibility. If this is set to a value of 1 (as specified by
        // the visible constant in the Rez input file), the Dialog
        // Manager displays this dialog box as soon as you call the
        // GetNewDialog function. If this is set to a value of 0 (as
        // specified by the invisible constant in the Rez input file),
        // the Dialog Manager does not display this dialog box until
        // you call the Window Manager procedure ShowWindow.
        Visible = data[offset] != 0;
        offset += 1;

        Reserved1 = data[offset];
        offset += 1;

        // Close box specification. This specifies whether to draw a
        // close box. Normally, this is set to a value of 1 (as
        // specified by the goAway constant in the Rez input file) only
        // for a modeless dialog box to specify a close box in its
        // title bar. Otherwise, this is set to a value of 0 (as
        // specified by the noGoAway constant in the Rez input file).
        CloseBox = data[offset] != 0;
        offset += 1;

        Reserved2 = data[offset];
        offset += 1;

        // Reference constant. This contains any value that an
        // application stores here. For example, an application can
        // store a number that represents a dialog box type, or it
        // can store a handle to a record that maintains state
        // information about the dialog box or other window types.
        // An application can use the Window Manager procedure
        // SetWRefCon at any time to change this value in the
        // dialog record for a dialog box, and you can use the
        // GetWRefCon function to determine its current value.
        ReferenceConstant = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        //  Item list resource ID. The ID of the item list resource
        // that specifies the items—such as buttons and static
        // text—to display in the dialog box.
        ItemListResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Window title. This is a Pascal string displayed in the
        // dialog box’s title bar only when the dialog box is modeless.
        WindowTitle = SpanUtilities.ReadPascalString(data[offset..]);
        offset += 1 + WindowTitle.Length;

        if (offset + 2 < data.Length)
        {
            // Alignment byte. This is an extra byte added if necessary to
            // make the previous Pascal string end on a word boundary
            if (offset % 2 != 0)
            {
                offset += 1;
            }

            // Dialog box position. This specifies the position of the
            // dialog box on the screen. (If your application positions
            // dialog boxes on its own, don’t use these constants,
            // because your code may conflict with the Dialog Manager.)
            // ■ If 0x0000 appears here (as specified by the noAutoCenter
            // constant in the Rez input file), the Dialog Manager positions
            // this dialog box according to the global coordinates specified
            // in the rectangle element of this resource.
            // ■ If 0xB00A appears here (as specified by the alertPositionParentWindow
            // constant in the Rez input file), the Dialog Manager positions
            // the dialog box over the frontmost window so that the window’s
            // title bar appears. This is illustrated in Figure 6-33 on page 6-63.
            // ■ If 0x300A appears here (as specified by the alertPositionMainScreen constant
            // in the Rez input file), the Dialog Manager centers the dialog box near the top of the
            // main screen. This is illustrated in Figure 6-34 on page 6-63.
            // ■ If 0x700A appears here (as specified in the Rez input file by the
            // alertPositionParentWindowScreen constant), the Dialog Manager
            // positions the dialog box on the screen where the user is currently working.
            // This is illustrated in Figure 6-35 on page 6-64.
            Position = (DialogBoxPosition)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        Debug.Assert(offset <= data.Length, "Did not consume all data for DialogBoxRecord.");
    }

    /// <summary>
    /// The dialog box positioning options.
    /// </summary>
    public enum DialogBoxPosition : ushort
    {
        /// <summary>
        /// No auto-centering.
        /// </summary>
        NoAutoCenter = 0x0000,

        /// <summary>
        /// Centers on the parent window.
        /// </summary>
        ParentWindow = 0xB00A,

        /// <summary>
        /// Centers on the main screen.
        /// </summary>
        MainScreen = 0x300A,

        /// <summary>
        /// Centers on the screen containing the parent window.
        /// </summary>
        ParentWindowScreen = 0x700A
    }
}
