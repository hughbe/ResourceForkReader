using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Help Window Record ('hwin') in a resource fork.
/// </summary>
public readonly struct HelpWindowRecord
{
    /// <summary>
    /// The minimum size of a Help Window Record in bytes.
    /// </summary>
    public const int MinSize = 8;

    /// <summary>
    /// Gets the Help Manager version.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// Gets the Help Manager options.
    /// </summary>
    public HelpManagerOptions Options { get; }

    /// <summary>
    /// Gets the number of window components defined in this help window record.
    /// </summary>
    public ushort WindowComponentCount { get; }

    /// <summary>
    /// Gets the list of window components defined in this help window record.
    /// </summary>
    public List<HelpWindowComponent> WindowComponents { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HelpWindowRecord"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the help window record data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid or insufficient.</exception>
    public HelpWindowRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 3-153 to 3-156
        int offset = 0;

        // Help Manager version. The version of the Help Manager to use.
        // This is usually specified in a Rez input file with the HelpMgrVersion constant.
        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;
    
        // Options. The sum of the values of available options, described in
        // “Specifying Options in Help Resources” beginning on page 3-25.
        Options = (HelpManagerOptions)BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Window component count. The number of window components defined in the
        // rest of this resource. The Help Manager determines the end of the 'hwin'
        // resource by using this component count information.
        WindowComponentCount = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var windowComponents = new List<HelpWindowComponent>(WindowComponentCount);
        for (int i = 0; i < WindowComponentCount; i++)
        {
            var component = new HelpWindowComponent(data[offset..], out int componentBytesRead);
            offset += componentBytesRead;
            windowComponents.Add(component);
        }

        WindowComponents = windowComponents;

        Debug.Assert(offset == data.Length, "Did not consume all data in Help Window Record.");
    }

    /// <summary>
    /// Help Manager options for a Help Window Record.
    /// </summary>
    [Flags]
    public enum HelpManagerOptions : uint
    {
        /// <summary>
        /// Use defaults
        /// </summary>
        Default = 0,

        /// <summary>
        /// Use subrange resource IDs for owned resources.
        /// </summary>
        UseSubrangeResourceIDs = 0x00000001,

        /// <summary>
        /// Ignore coords of window origin and treat upper-left corner of window as 0,0.
        /// </summary>
        AbsoluteCoordinates = 0x00000002,

        /// <summary>
        /// Don't create window; save bits; no update event.
        /// </summary>
        SaveBitsNoWindow = 0x00000004,

        /// <summary>
        /// Save bits behind window and generate update event.
        /// </summary>
        SaveBitsWindow = 0x00000008,

        /// <summary>
        /// Match window by string anywhere in title string.
        /// </summary>
        MatchInTitle = 0x00000010,
    }

    /// <summary>
    /// Represents a Help Window Component within a Help Window Record.
    /// </summary>
    public struct HelpWindowComponent
    {
        /// <summary>
        /// Gets the size of this Help Window Component in bytes.
        /// </summary>
        public const int MinSize = 8;

        /// <summary>
        /// Gets the size of this Help Window Component in bytes.
        /// </summary>
        public ushort ResourceID { get; }

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
            ResourceID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
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

                WindowTitleString = SpanUtilities.ReadPascalString(data.Slice(offset, 1 + LengthOfComparisonStringOrWindowKind));
                offset += 1 + WindowTitleString.Length;
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
}
