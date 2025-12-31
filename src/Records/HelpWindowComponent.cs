using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Help Window Component within a Help Window Record.
/// </summary>
public readonly struct HelpWindowComponent
{
    /// <summary>
    /// Gets the size of this Help Window Component in bytes.
    /// </summary>
    public const int MinSize = 8;

    /// <summary>
    /// Gets the size of this Help Window Component in bytes.
    /// </summary>
    public short ResourceID { get; }

    /// <summary>
    /// Gets the resource type of this Help Window Component.
    /// </summary>
    public string ResourceType { get; }

    /// <summary>
    /// Gets the length of the comparison string or window kind.
    /// </summary>
    public short LengthOfComparisonStringOrWindowKind { get; }

    /// <summary>
    /// Gets the window title string or comparison string.
    /// </summary>
    public string WindowTitleString { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HelpWindowComponent"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the help window component data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid or insufficient.</exception>
    public HelpWindowComponent(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes.", nameof(data));
        }

        int offset = 0;

        // Resource ID. The ID of the associated resource (either 'hrct'
        // or 'hdlg') that specifies the help messages for the window.
        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // ype of associated resource. A resource type; either 'hrct' or 'hdlg'.
        ResourceType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        // Length of comparison string—or a windowKind value. If the
        // integer in this element is positive, this is the number of
        // characters used for matching this component to a window’s
        // title. If the integer in this element is negative, this is
        // a value used for matching this component to a window by the
        // windowKind value in the window’s window record.
        LengthOfComparisonStringOrWindowKind = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Window title string. If the previous element is a positive integer,
        // this element consists of characters that the Help Manager uses
        // to match this component to a window by the window’s title.
        // If the previous element is a negative integer, this is an
        // empty string.
        if (LengthOfComparisonStringOrWindowKind > 0)
        {
            if (offset + 1 + LengthOfComparisonStringOrWindowKind > data.Length)
            {
                throw new ArgumentException("Data is too short for the specified string length.", nameof(data));
            }

            WindowTitleString = SpanUtilities.ReadPascalString(data.Slice(offset, 1 + LengthOfComparisonStringOrWindowKind), out var windowTitleStringBytesRead);
            offset += windowTitleStringBytesRead;
        }
        else
        {
            WindowTitleString = string.Empty;
        }

        // Alignment bytes. Zero or one bytes used to make the window
        // title string end on a word boundary
        if (offset % 2 != 0)
        {
            offset += 1; // Skip alignment byte
        }

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Offset should not exceed data length.");
    }
}
