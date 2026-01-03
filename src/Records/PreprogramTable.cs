using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Preprogram Table in a resource fork.
/// </summary>
public readonly struct PreprogramTable
{
    /// <summary>
    /// Gets the raw preprogram table data.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PreprogramTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Preprogram Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public PreprogramTable(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-89
        // and https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6prep.html
        int offset = 0;

        RawData = data.ToArray();
        offset += RawData.Length;

        Debug.Assert(offset == data.Length, "Offset should equal data length after reading Preprogram Table.");
    }
}
