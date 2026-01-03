using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Help Rectangles Record ('hrct') in a resource fork.
/// </summary>
public readonly struct HelpRectanglesRecord
{
    /// <summary>
    /// Gets the minimum size of a Help Rectangles Record in bytes.
    /// </summary>
    public const int MinSize = 12;

    /// <summary>
    /// Gets the version of the Help Rectangles Record.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// Gets the options flags of the Help Rectangles Record.
    /// </summary>
    public uint Options { get; }

    /// <summary>
    /// Gets the resource ID of the balloon definition function.
    /// </summary>
    public short BalloonDefinitionFunctionResourceID { get; }

    /// <summary>
    /// Gets the variation code of the Help Rectangles Record.
    /// </summary>
    public ushort VariationCode { get; }

    /// <summary>
    /// Gets the count of hot rectangle components.
    /// </summary>
    public ushort HotRectangleComponentCount { get; }

    /// <summary>
    /// Gets the remaining data containing the hot-rectangle components.
    /// </summary>
    public byte[] RemainingData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HelpRectanglesRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Help Rectangles Record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public HelpRectanglesRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length must be at least {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 3-148 to 3-153
        int offset = 0;

        // Help Manager version. The version of the Help Manager to use. This is
        // usually specified in a Rez input file with the HelpMgrVersion constant.
        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Options. The sum of the values of available options, described in “Specifying Options
        // in Help Resources” beginning on page 3-25.
        Options = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Balloon definition function. The resource ID of the window definition function used
        // for drawing the help balloon. The standard balloon definition function is of type
        // 'WDEF' with resource ID 126; this can be specified by 0 in the Rez input file.
        BalloonDefinitionFunctionResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Variation code. A number signifying the preferred position of the help balloon relative
        // to the hot rectangle. The balloon definition function draws the frame of the help
        // balloon based on the variation code specified here. The eight variation codes and how
        // they affect the standard balloon definition function are illustrated in Figure 3-4 on
        // page 3-10.
        VariationCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Hot-rectangle component count. The number of hot-rectangle components defined in 
        // the rest of this resource.
        HotRectangleComponentCount = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        RemainingData = data[offset..].ToArray();
        offset += RemainingData.Length;

        Debug.Assert(offset == data.Length, "Did not consume all data for Help Rectangles Record.");
    }
}
