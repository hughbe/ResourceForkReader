using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a style run within a 'styl' style resource.
/// </summary>
public readonly struct StyleRun
{
    /// <summary>
    /// The size of a StyleRun structure in bytes.
    /// </summary>
    public const int Size = 20;

    /// <summary>
    /// The offset of the style run within the text.
    /// </summary>
    public uint Offset { get; }

    /// <summary>
    /// The line height for the style run.
    /// </summary>
    public ushort LineHeight { get; }

    /// <summary>
    /// The font ascent for the style run.
    /// </summary>
    public ushort FontAscent { get; }

    /// <summary>
    /// The font resource ID for the style run.
    /// </summary>
    public short FontResourceID { get; }

    /// <summary>
    /// The flags for the style run.
    /// </summary>
    public ushort Flags { get; }

    /// <summary>
    /// The point size for the style run.
    /// </summary>
    public ushort PointSize { get; }

    /// <summary>
    /// The color for the style run.
    /// </summary>
    public ColorRGB Color { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleRun"/> struct from the given span.
    /// </summary>
    /// <param name="data">The span containing the StyleRun data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is not the correct size to contain a StyleRun.</exception>
    public StyleRun(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data is not the correct size to contain a StyleRun. Expected size is {Size} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 2-74 to 2-76
        int offset = 0;

        Offset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        LineHeight = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        FontAscent = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        FontResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Flags = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        PointSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Color = new ColorRGB(data.Slice(offset, ColorRGB.Size));
        offset += ColorRGB.Size;

        Debug.Assert(offset == data.Length, "All bytes in the StyleRun structure should have been read.");
    }
}
