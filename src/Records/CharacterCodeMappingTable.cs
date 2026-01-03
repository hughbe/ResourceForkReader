using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Character Code Mapping Table in a resource fork.
/// </summary>
public readonly struct CharacterCodeMappingTable
{
    /// <summary>
    /// Gets the raw character code mapping table data.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterCodeMappingTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Character Code Mapping Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public CharacterCodeMappingTable(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-76
        // and https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6cmap.html
        int offset = 0;

        RawData = data.ToArray();
        offset += RawData.Length;

        Debug.Assert(offset == data.Length, "Offset should equal data length after reading Character Code Mapping Table.");
    }
}
