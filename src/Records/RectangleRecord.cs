using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// The 'RECT' resource type for rectangle positions list.
/// </summary>
public readonly struct RectangleRecord
{
    /// <summary>
    /// The size of a rectangle record in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the rectangle structure.
    /// </summary>
    public RECT Rectangle { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RectangleRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the rectangle record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 8 bytes.</exception>
    public RectangleRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be at least {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 8-88
        int offset = 0;

        Rectangle = new RECT(data);
        offset += RECT.Size;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for RectangleRecord.");
    }
}
