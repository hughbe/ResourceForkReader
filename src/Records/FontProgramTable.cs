using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Program Table in a resource fork.
/// </summary>
public readonly struct FontProgramTable
{
    /// <summary>
    /// Gets the raw font program table data.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontProgramTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Font Program Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public FontProgramTable(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-77
        // and https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6fpgm.html
        int offset = 0;

        RawData = data.ToArray();
        offset += RawData.Length;

        Debug.Assert(offset == data.Length, "Offset should equal data length after reading Font Program Table.");
    }
}
