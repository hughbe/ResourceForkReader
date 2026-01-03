using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Control Value Table ('cvt ') in a resource fork.
/// </summary>
public readonly struct ControlValueTable
{
    /// <summary>
    /// Gets the array of control values.
    /// </summary>
    public uint[] Values { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlValueTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Control Value Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data length is not a multiple of 4 bytes.</exception>
    public ControlValueTable(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-77
        int offset = 0;

        var count = data.Length / 4;
        var values = new uint[count];
        for (int i = 0; i < count; i++)
        {
            values[i] = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
            offset += 4;
        }

        Values = values;

        // Does not have to be exactly multiple of 4 bytes.
        Debug.Assert(offset <= data.Length, "Did not consume all data for Control Value Table.");
    }
}
