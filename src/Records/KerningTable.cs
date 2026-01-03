using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Kerning Table in a resource fork.
/// </summary>
public readonly struct KerningTable
{
    /// <summary>
    /// Gets the raw kerning table data.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KerningTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Kerning Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public KerningTable(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-84
        int offset = 0;

        RawData = data.ToArray();
        offset += RawData.Length;

        Debug.Assert(offset == data.Length, "Offset should equal data length after reading Kerning Table.");
    }
}
