using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Text Style Record ('TxSt') in a resource fork.
/// </summary>
public readonly struct TextStyleRecord
{
    /// <summary>
    /// Minimum size of a Text Style Record in bytes.
    /// </summary>
    public const int MinSize = 11;

    /// <summary>
    /// Gets the font style.
    /// </summary>
    public byte FontStyle { get; }

    /// <summary>
    /// Gets the font size.
    /// </summary>
    public ushort FontSize { get; }

    /// <summary>
    /// Gets the font name.
    /// </summary>
    public string FontName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextStyleRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Text Style Record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public TextStyleRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1184-L1190
        int offset = 0;

        FontStyle = data[offset];
        offset += 2; // Skip 1 byte padding

        FontSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // In the template but not seen in practice.
        // TextColor = new ColorRGB(data.Slice(offset, ColorRGB.Size));
        // offset += ColorRGB.Size;

        FontName = SpanUtilities.ReadPascalString(data[offset..], out int bytesRead);
        offset += bytesRead;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for TextStyleRecord.");
    }
}
