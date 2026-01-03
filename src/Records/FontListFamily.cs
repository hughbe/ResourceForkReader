using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font List Family in a resource fork.
/// </summary>
public readonly struct FontListFamily
{
    /// <summary>
    /// Minimum size of a Font List Family in bytes.
    /// </summary>
    public const int MinSize = 3;

    /// <summary>
    /// Gets the name of the font family.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the number of fonts in the family.
    /// </summary>
    public ushort NumberOfFonts { get; }

    /// <summary>
    /// Gets the list of fonts in the family.
    /// </summary>
    public List<FontListFamilyFont> Fonts { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontListFamily"/> struct.
    /// </summary>
    /// <param name="data">The data for the Font List Family.</param>
    /// <param name="bytesRead">The number of bytes read from the data.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public FontListFamily(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1078-L1087
        int offset = 0;

        Name = SpanUtilities.ReadPascalString(data[offset..], out int nameBytesRead);
        offset += nameBytesRead;

        if (offset % 2 != 0)
        {
            // Align to even byte boundary
            offset += 1;
        }

        NumberOfFonts = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var fonts = new List<FontListFamilyFont>(NumberOfFonts);
        for (int i = 0; i < NumberOfFonts; i++)
        {
            fonts.Add(new FontListFamilyFont(data.Slice(offset, FontListFamilyFont.Size)));
            offset += FontListFamilyFont.Size;
        }

        Fonts = fonts;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all bytes for FontListFamily.");
    }
}
    