using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font List Record ('resf') in a resource fork.
/// </summary>
public readonly struct FontListRecord
{
    /// <summary>
    /// Minimum size of a Font List Record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of font families.
    /// </summary>
    public ushort NumberOfFontFamilies { get; }

    /// <summary>
    /// Gets the list of font families.
    /// </summary>
    public List<FontListFamily> FontFamilies { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontListRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Font List Record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public FontListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1078-L1087
        int offset = 0;

        NumberOfFontFamilies = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var fontFamilies = new List<FontListFamily>(NumberOfFontFamilies);
        for (int i = 0; i < NumberOfFontFamilies; i++)
        {
            fontFamilies.Add(new FontListFamily(data[offset..], out int bytesRead));
            offset += bytesRead;
        }

        FontFamilies = fontFamilies;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for FontListRecord.");
    }
}
