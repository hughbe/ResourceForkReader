using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Family Kerning Table Pair in a resource fork.
/// </summary>
public readonly struct FontFamilyKerningTablePair
{
    /// <summary>
    /// The size of a Font Family Kerning Table Pair in bytes.
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Gets the code of the first character in the kerning pair.
    /// </summary>
    public byte First { get; }

    /// <summary>
    /// Gets the code of the second character in the kerning pair.
    /// </summary>
    public byte Second { get; }

    /// <summary>
    /// Gets the kerning value in 1pt fixed format.
    /// </summary>
    public ushort Width { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontFamilyKerningTablePair"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the font family kerning table pair data.</param>
    /// <exception cref="ArgumentException"></exception>
    public FontFamilyKerningTablePair(ReadOnlySpan<byte> data)
    {
        if (data.Length < Size)
        {
            throw new ArgumentException($"Data must be at least {Size} bytes long to read a FontFamilyKerningTablePair.", nameof(data));
        }

        int offset = 0;

        First = data[offset];
        offset += 1;

        Second = data[offset];
        offset += 1;

        Width = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not read the expected number of bytes for FontFamilyKerningTablePair");
    }
}
