using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Horizontal Device Metrics Table ('hdmx') in a resource fork.
/// </summary>
public readonly struct HorizontalDeviceMetricsTable
{
    /// <summary>
    /// Gets the array of widths.
    /// </summary>
    public ushort[] Widths { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HorizontalDeviceMetricsTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Horizontal Device Metrics Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data length is not a multiple of 4 bytes.</exception>
    public HorizontalDeviceMetricsTable(ReadOnlySpan<byte> data)
    {
        if (data.Length % 2 != 0)
        {
            throw new ArgumentException("Data length must be a multiple of 2 bytes for Horizontal Device Metrics Table.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-78
        int offset = 0;

        var count = data.Length / 2;
        var values = new ushort[count];
        for (int i = 0; i < count; i++)
        {
            values[i] = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        Widths = values;

        Debug.Assert(offset <= data.Length, "Did not consume all data for Horizontal Device Metrics Table.");
    }
}
