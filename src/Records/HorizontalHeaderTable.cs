using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Horizontal Header Table in a resource fork.
/// </summary>
public readonly struct HorizontalHeaderTable
{
    /// <summary>
    /// Gets the raw data of the Horizontal Header Table.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HorizontalHeaderTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Horizontal Header Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the data length is less than the minimum required size.</exception>
    public HorizontalHeaderTable(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-83
        int offset = 0;

        RawData = data.ToArray();
        offset += data.Length;

        Debug.Assert(offset == data.Length, "Offset should equal data length after reading Horizontal Header Table.");
    }
}
