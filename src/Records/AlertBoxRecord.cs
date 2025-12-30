using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Alert Box ('ALRT') in a resource fork.
/// </summary>
public struct AlertBoxRecord
{
    /// <summary>
    /// The minimum size of a valid AlertBoxRecord in bytes.
    /// </summary>
    public const int MinSize = 12;

    /// <summary>
    /// Gets the bounds of the alert box.
    /// </summary>
    public RECT Bounds { get; }

    /// <summary>
    /// Gets the item list resource ID.
    /// </summary>
    public short ItemListResourceID { get; }

    /// <summary>
    /// Gets the fourth-stage alert information.
    /// </summary>
    public byte FourthStageAlertInfo { get; }

    /// <summary>
    /// Gets the third-stage alert information.
    /// </summary>
    public byte ThirdStageAlertInfo { get; }

    /// <summary>
    /// Gets the second-stage alert information.
    /// </summary>
    public byte SecondStageAlertInfo { get; }

    /// <summary>
    /// Gets the first-stage alert information.
    /// </summary>
    public byte FirstStageAlertInfo { get; }

    /// <summary>
    /// Gets the alert box position.
    /// </summary>
    public ushort AlertBoxPosition { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlertBoxRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the alert box data.</param>
    /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
    public AlertBoxRecord(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 6-150 to 6-151
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        int offset = 0;

        // Rectangle. This determines the alert box’s dimensions and, possibly,
        // its position. (The last element in the alert resource usually
        // specifies a position for the alert box.)
        Bounds = new RECT(data.Slice(offset, RECT.Size));
        offset += RECT.Size;

        // Item list resource ID. The ID of the item list resource that
        // specifies the items—such as buttons and static text—to display
        // in the alert box.
        ItemListResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Fourth-stage alert information. This specifies the response when the user repeats the
        // action that invokes this alert four or more consecutive times. The Dialog Manager
        // responds in the manner specified in the 4 bits that make up this element.
        // ■ If the first bit is set, the Dialog Manager draws a bold outline around the second
        // item in the item list resource (typically, the Cancel button) and—if your application
        // does not specify an event filter function—returns 2 when the user presses the
        // Return or Enter key at the fourth consecutive occurrence of the alert. If the first bit is
        // not set, the Dialog Manager draws a bold outline around the first item in the item
        // list resource (typically, the OK button) and—if your application does not specify an
        // event filter function—returns 1 when the user presses the Return or Enter key
        // ■ If the second bit is set, the Dialog Manager displays the alert box at this stage. If the
        // second bit is not set, the Dialog Manager doesn’t display the alert box at this stage.
        // ■ If neither of the next 2 bits is set, the Dialog Manager plays no alert sound at this
        // stage. If bit 3 is set and bit 4 is not set, the Dialog Manager plays the first alert
        // sound—by default, the system alert sound. If bit 3 is not set and bit 4 is set, the
        // Dialog Manager plays the second alert sound; by default, it plays the system alert
        // sound twice. If both bit 3 and bit 4 are set, the Dialog Manager plays the third alert
        // sound; by default, it plays the system alert sound three times. By defining your own
        // alert sound (described on page 6-144) and calling the ErrorSound procedure
        // (described on page 6-104) to make it the current sound procedure, you can specify
        // your own alert sounds.
        // ■ Third-stage alert information. This specifies the response when the user repeats the
        // action that invokes this alert three consecutive times. The Dialog Manager interprets
        // these 4 bits in the manner described for the fourth-stage alert.
        // ■ Second-stage alert information. This specifies the response when the user repeats the
        // action that invokes this alert two consecutive times. The Dialog Manager interprets
        // these 4 bits in the manner described for the fourth-stage alert.
        // ■ First-stage alert information. This specifies the response for the first time that the user
        // performs the action that invokes this alert. The Dialog Manager interprets these 4 bits
        // in the manner described for the fourth-stage alert.
        FourthStageAlertInfo = (byte)(data[offset] & 0x0F);
        ThirdStageAlertInfo = (byte)((data[offset] >> 4) & 0x0F);
        offset += 1;

        SecondStageAlertInfo = (byte)(data[offset] & 0x0F);
        FirstStageAlertInfo = (byte)((data[offset] >> 4) & 0x0F);
        offset += 1;

        // Alert box position. This specifies the position of the alert box on the screen. (If your
        // application positions alert boxes on its own, don’t use these constants, because your
        // code may conflict with the Dialog Manager.)
        // ■ If 0x0000 appears here (as specified by the noAutoCenter constant in the Rez
        // input file), the Dialog Manager positions this alert box according to the global
        // coordinates specified in the rectangle element of this resource.
        // ■ If 0xB00A appears here (as specified by the alertPositionParentWindow
        // constant in the Rez input file), the Dialog Manager positions the alert box over the
        // frontmost window so that the window’s title bar appears. This is illustrated in
        // Figure 6-33 on page 6-63.
        // ■ If 0x300A appears here (as specified by the alertPositionMainScreen constant
        // in the Rez input file), the Dialog Manager centers the alert box near the top of the
        // main screen. This is illustrated in Figure 6-34 on page 6-63.
        // ■ If 0x700A appears here (as specified in the Rez input file by the
        // alertPositionParentWindowScreen constant), the Dialog Manager
        // positions the alert box on the screen where the user is currently working.
        // This is illustrated in Figure 6-35 on page 6-64.
        if (data.Length > offset)
        {
            AlertBoxPosition = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        Debug.Assert(offset == data.Length, "Did not consume all data for AlertBoxRecord.");
    }
}
