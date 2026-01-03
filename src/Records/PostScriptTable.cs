using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a PostScript Table in a resource fork.
/// </summary>
public readonly struct PostScriptTable
{
    /// <summary>
    /// Minimum size of a PostScript Table in bytes.
    /// </summary>
    public const int MinSize = 28;

    /// <summary>
    /// Gets the format of this table.
    /// </summary>
    public ushort Format { get; }

    /// <summary>
    /// Gets the italic angle in degrees.
    /// </summary>
    public ushort ItalicAngle { get; }

    /// <summary>
    /// Gets the underline position.
    /// </summary>
    public ushort UnderlinePosition { get; }

    /// <summary>
    /// Gets the underline thickness.
    /// </summary>
    public ushort UnderlineThickness { get; }

    /// <summary>
    /// Gets whether the font is monospaced.
    /// </summary>
    public uint IsFixedPitch { get; }

    /// <summary>
    /// Gets the minimum memory usage when a TrueType font is downloaded as a Type 42 font.
    /// </summary>
    public uint MinMemType42 { get; }

    /// <summary>
    /// Gets the maximum memory usage when a TrueType font is downloaded as a Type 42 font.
    /// </summary>
    public uint MaxMemType42 { get; }

    /// <summary>
    /// Gets the minimum memory usage when a TrueType font is downloaded as a Type 1 font.
    /// </summary>
    public uint MinMemType1 { get; }

    /// <summary>
    /// Gets the maximum memory usage when a TrueType font is downloaded as a Type 1 font.
    /// </summary>
    public uint MaxMemType1 { get; }

    /// <summary>
    /// Gets the format-specific data.
    /// </summary>
    public byte[] FormatData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostScriptTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the PostScript Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public PostScriptTable(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than the minimum required size of {MinSize} bytes for a PostScript Table.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-89
        // and https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6post.html
        int offset = 0;

        // Format of this table
        Format = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Italic angle in degrees
        ItalicAngle = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Underline position
        UnderlinePosition = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Underline thickness
        UnderlineThickness = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Font is monospaced; set to 1 if the font is monospaced and 0 otherwise
        // (N.B., to maintain compatibility with older versions of the TrueType
        // spec, accept any non-zero value as meaning that the font is monospaced)
        IsFixedPitch = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Minimum memory usage when a TrueType font is downloaded as a Type 42 font
        MinMemType42 = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Maximum memory usage when a TrueType font is downloaded as a Type 42 font
        MaxMemType42 = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Minimum memory usage when a TrueType font is downloaded as a Type 1 font
        MinMemType1 = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Maximum memory usage when a TrueType font is downloaded as a Type 1 font
        MaxMemType1 = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        FormatData = data.Slice(offset).ToArray();
        offset += FormatData.Length;

        Debug.Assert(offset == data.Length, "Offset should equal data length after reading PostScript Table.");
    }
}
