using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Maximum Profile Table in a resource fork.
/// </summary>
public readonly struct MaximumProfileTable
{
    /// <summary>
    /// Minimum size of a Maximum Profile Table in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the raw data of the Maximum Profile Table.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MaximumProfileTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Maximum Profile Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the data length is less than the minimum required size.</exception>
    public MaximumProfileTable(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than the minimum required size of {MinSize} bytes for a Maximum Profile Table.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-84
        int offset = 0;

        Data = data.ToArray();
        offset += data.Length;

        Debug.Assert(offset == data.Length, "Offset should equal data length after reading Maximum Profile Table.");
    }
}
