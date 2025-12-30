using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a ResEdit Picker Record ('PICK') in a resource fork.
/// </summary>
public readonly struct ResEditPickerRecord
{
    /// <summary>
    /// The minimum size of a ResEdit Picker Record in bytes.
    /// </summary>
    public const int MinSize = 17;

    /// <summary>
    /// Gets the type of picker.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets a value indicating whether to use color.
    /// </summary>
    public byte UseColor { get; }

    /// <summary>
    /// Gets the picker type.
    /// </summary>
    public byte PickerType { get; }

    /// <summary>
    /// Gets the view by option.
    /// </summary>
    public byte ViewBy { get; }

    /// <summary>
    /// Gets the reserved byte.
    /// </summary>
    public byte Reserved { get; }

    /// <summary>
    /// Gets the vertical cell size.
    /// </summary>
    public ushort VerticalCellSize { get; }

    /// <summary>
    /// Gets the horizontal cell size.
    /// </summary>
    public ushort HorizontalCellSize { get; }

    /// <summary>
    /// Gets the LDEF type.
    /// </summary>
    public string LDEFType { get; }

    /// <summary>
    /// Gets the option string.
    /// </summary>
    public string OptionString { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResEditPickerRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 17 bytes of ResEdit Picker Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 17 bytes long.</exception>
    public ResEditPickerRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid ResEdit Picker Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        int offset = 0;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        UseColor = data[offset];
        offset += 1;

        PickerType = data[offset];
        offset += 1;

        ViewBy = data[offset];
        offset += 1;

        Reserved = data[offset];
        offset += 1;

        VerticalCellSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        HorizontalCellSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        LDEFType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        OptionString = SpanUtilities.ReadPascalString(data[offset..]);
        offset += 1 + OptionString.Length;

        Debug.Assert(offset == data.Length, "Did not consume all data for ResEdit Picker Record.");
    }
}
