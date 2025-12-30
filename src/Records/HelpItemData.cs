using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

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
    public short ResourceID { get; }

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
        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
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
