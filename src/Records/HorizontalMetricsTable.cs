using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Horizontal Metrics Table in a resource fork.
/// </summary>
public readonly struct HorizontalMetricsTable
{
    /// <summary>
    /// Minimum size of a Horizontal Metrics Table in bytes.
    /// </summary>
    public const int MinSize = 6;

    /// <summary>
    /// Gets the advance width.
    /// </summary>
    public ushort AdvanceWidth { get; }

    /// <summary>
    /// Gets the left side bearings.
    /// </summary>
    public short[] LeftSideBearings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HorizontalMetricsTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Horizontal Metrics Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public HorizontalMetricsTable(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than the minimum required size of {MinSize} bytes for a Horizontal Metrics Table.", nameof(data));
        }
        if (data.Length % 2 != 0)
        {
            throw new ArgumentException("Data length must be even to accommodate Left Side Bearings in a Horizontal Metrics Table.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-83
        int offset = 0;

        AdvanceWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var count = (data.Length - offset) / 2;
        var leftSideBearings = new short[count];
        for (int i = 0; i < count; i++)
        {
            leftSideBearings[i] = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        LeftSideBearings = leftSideBearings;

        Debug.Assert(offset == data.Length, "Offset should equal data length after reading Horizontal Metrics Table.");
    }
}
