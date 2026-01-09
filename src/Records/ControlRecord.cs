using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Control Resource ('CTRL').
/// </summary>
public readonly struct ControlRecord
{
    /// <summary>
    /// The minimum size of a ControlRecord in bytes.
    /// </summary>
    public const int MinSize = 22;

    /// <summary>
    /// Gets the bounds of the control.
    /// </summary>
    public RECT Bounds { get; }

    /// <summary>
    /// Gets the initial setting of the control.
    /// </summary>
    public ushort InitialSetting { get; }

    /// <summary>
    /// Gets a value indicating whether the control is visible.
    /// </summary>
    public bool Visible { get; }

    /// <summary>
    /// Gets the fill byte of the control.
    /// </summary>
    public byte Fill { get; }

    /// <summary>
    /// Gets the maximum setting of the control.
    /// </summary>
    public ushort MaximumSetting { get; }

    /// <summary>
    /// Gets the minimum setting of the control.
    /// </summary>
    public ushort MinimumSetting { get; }

    /// <summary>
    /// Gets the resource ID of the control definition associated with this control.
    /// </summary>
    public ushort DefinitionID { get; }

    /// <summary>
    /// Gets the reference constant for the control.
    /// </summary>
    public uint ReferenceValue { get; }

    /// <summary>
    /// Gets the title of the control.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the control record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to contain a valid ControlRecord.</exception>
    public ControlRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 5-118 to 5-121
        int offset = 0;

        // The rectangle, specified in coordinates local to the window, that
        // encloses the control; this rectangle encloses the control and
        // thus determines its size and location.
        Bounds = new RECT(data.Slice(offset, RECT.Size));
        offset += RECT.Size;

        // The initial setting for the control.
        // ■ For controls—such as buttons—that don’t retain a setting,
        // this value should be 0.
        // ■ For controls—such as checkboxes or radio buttons—that
        // retain an on-or-off setting, a value of 0 in this element
        // indicates that the control is initially off; a value of 1
        // indicates that the control is initially on.
        // ■ For controls—such as scroll bars and dials—that can take
        // a range of settings, whatever initial value is appropriate
        // within that range is specified in this element.
        // ■ For pop-up menus, a combination of values instructs the
        // Control Manager where and how to draw the control title.
        // Appropriate values, along with the constants used to specify
        // them in a Rez input file, are listed here:
        // CONST popupTitleBold = $00000100; {boldface font style}
        // popupTitleItalic = $00000200; {italic font style}
        // popupTitleUnderline = $00000400; {underline font }
        // { style}
        // popupTitleOutline = $00000800; {outline font style}
        // popupTitleShadow = $00001000; {shadow font style}
        // popupTitleCondense = $00002000; {condensed text}
        // popupTitleExtend = $00004000; {extended text}
        // popupTitleNoStyle = $00008000; {monostyle text}
        // popupTitleLeftJust = $00000000; {place title left }
        // { of pop-up box}
        // popupTitleCenterJust = $00000001; {center title over }
        // { pop-up box}
        // popupTitleRightJust = $000000FF; {place title right }
        // { of pop-up box}
        InitialSetting = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The visibility of the control. If this element contains the
        // value TRUE, GetNewControl draws the control immediately,
        // without using the application’s standard updating mechanism
        // for windows. If this element contains the value FALSE, the
        // application must use the ShowControl procedure (described on
        // page 5-86) when it’s prepared to display the control.
        Visible = data[offset] != 0;
        offset += 1;

        //  Fill. This should be set to 0.
        Fill = data[offset];
        offset += 1;

        // The maximum setting for the control.
        // ■ For controls—such as buttons—that don’t retain a setting,
        // this value should be 1.
        // ■ For controls—such as checkboxes or radio buttons—that retain
        // an on-or-off setting, this element should contain the value 1
        // (meaning “on”).
        // ■ For controls—such as scroll bars and dials—that can take a
        // range of settings, this element can contain whatever maximum
        // value is appropriate; when the application makes the maximum
        // setting of a scroll bar equal to its minimum setting, the control
        // definition function automatically makes the scroll bar inactive,
        // and when the application makes the maximum setting exceed the
        // minimum, the control definition function makes the scroll bar
        // active again.
        // ■ For pop-up menus, this element contains the width, in pixels,
        // of the control title.
        MaximumSetting = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The minimum setting for the control.
        // ■ For controls—such as buttons—that don’t retain a setting,
        // this value should be 0.
        // ■ For controls—such as checkboxes or radio buttons—that
        // retain an on-or-off setting, the value 0 (meaning “off”) should
        // be set in this element.
        // ■ For controls—such as scroll bars and dials—that can take a
        // range of settings, this element contains whatever minimum
        // value is appropriate.
        // ■ For pop-up menus, this element contains the resource ID of
        // the 'MENU' resource that describes the menu items.
        MinimumSetting = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The control definition ID, which the Control Manager uses to
        // determine the control definition function for this control.
        // “Defining Your Own Control Definition Function” beginning on
        // page 5-109 describes how to create control definition functions
        // and their corresponding control definition IDs. The following
        // list shows the control definition ID numbers—and the constants
        // that represent them in Rez input files—for the standard
        // controls.
        // CONST
        // pushButProc = 0; {button}
        // checkBoxProc = 1; {checkbox}
        // radioButProc = 2; {radio button}
        // useWFont = 8; {when added to above, shows }
        // { title in the window font}
        // scrollBarProc = 16; {scroll bar}
        // popupMenuProc = 1008; {pop-up menu}
        // popupFixedWidth = $0001; {add to popupMenuProc to }
        // { use fixed-width control}
        // popupUseAddResMenu = $0004; {add to popupMenuProc to }
        // { specify a value of type }
        // { ResType in the contrlRfCon }
        // { field of the control }
        // { record; Menu Manager }
        // { adds resources of this }
        // { type to the menu}
        // popupUseWFont = $0008; {if added to popupMenuProc, }
        // { shows title in window font}
        DefinitionID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The control’s reference value, which is set and used only by
        // the application (except when the application adds the popupUseAddResMenu
        // variation code to the popupMenuProc control definition ID, as
        // described in “Creating a Pop-Up Menu” beginning on page 5-25)
        ReferenceValue = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // For controls—such as buttons, checkboxes, radio buttons, and
        // pop-up menus—that need a title, the string for that title;
        // for controls that don’t use titles, an empty string.
        Title = SpanUtilities.ReadPascalString(data[offset..], out var titleBytesRead);
        offset += titleBytesRead;

        Debug.Assert(offset <= data.Length, "Did not consume all bytes for ControlRecord.");
    }
}
