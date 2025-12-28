using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// The 'mcky' resource type for mouse tracking resources.
/// </summary>
public readonly struct MouseTrackingRecord
{
    /// <summary>
    /// The size of a mouse tracking record in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the threshold value for the 1st mouse button.
    /// </summary>
    public byte Threshold1 { get; }

    /// <summary>
    /// Gets the threshold value for the 2nd mouse button.
    /// </summary>
    public byte Threshold2 { get; }

    /// <summary>
    /// Gets the threshold value for the 3rd mouse button.
    /// </summary>
    public byte Threshold3 { get; }

    /// <summary>
    /// Gets the threshold value for the 4th mouse button.
    /// </summary>
    public byte Threshold4 { get; }

    /// <summary>
    /// Gets the threshold value for the 5th mouse button.
    /// </summary>
    public byte Threshold5 { get; }

    /// <summary>
    /// Gets the threshold value for the 6th mouse button.
    /// </summary>
    public byte Threshold6 { get; }

    /// <summary>
    /// Gets the threshold value for the 7th mouse button.
    /// </summary>
    public byte Threshold7 { get; }

    /// <summary>
    /// Gets the threshold value for the 8th mouse button.
    /// </summary>
    public byte Threshold8 { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MouseTrackingRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A 8-byte span containing the mouse tracking record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 8 bytes.</exception>
    public MouseTrackingRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L891-L900
        int offset = 0;

        Threshold1 = data[offset];
        offset++;

        Threshold2 = data[offset];
        offset++;

        Threshold3 = data[offset];
        offset++;

        Threshold4 = data[offset];
        offset++;

        Threshold5 = data[offset];
        offset++;

        Threshold6 = data[offset];
        offset++;

        Threshold7 = data[offset];
        offset++;

        Threshold8 = data[offset];
        offset++;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for MouseTracking record.");
    }
}
