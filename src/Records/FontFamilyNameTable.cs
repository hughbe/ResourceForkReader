using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Family Name Table in a resource fork.
/// </summary>
public readonly struct FontFamilyNameTable
{
    /// <summary>
    /// The minimum size of a Font Family Style Mapping Table in bytes.
    /// </summary>
    public const int MinSize = 3;

    /// <summary>
    /// Gets the number of entries in the name table.
    /// </summary>
    public ushort NumberOfSuffixes { get; }

    /// <summary>
    /// Gets the base font name.
    /// </summary>
    public string BaseFontName { get; }

    /// <summary>
    /// Gets the list of suffixes in the name table.
    /// </summary>
    public List<string> Suffixes { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontFamilyNameTable"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the font family name table data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid or insufficient.</exception>
    public FontFamilyNameTable(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long to read a FontFamilyNameTable.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-49 and 4-102 to 4-105.
        int offset = 0;

        // String count. An integer value that specifies the number of strings
        // in the array of suffixes. This value is represented by the stringCount
        // field of the NameTable data type.
        NumberOfSuffixes = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Base font name. The font family name in a 256 byte long Pascal
        // string. This value is represented by the baseFontName field of the
        // NameTable data type.
        BaseFontName = SpanUtilities.ReadPascalString(data[offset..], out var baseFontNameBytesRead);
        offset += baseFontNameBytesRead;

        if (offset == data.Length)
        {
            // Seen examples where there are no suffixes.
            Suffixes = [];
        }
        else
        {
            // Strings. A variable length array of Pascal strings, each of which
            // contains the suffixes or numbers specifying which suffixes to put
            // together to produce the real PostScript name. This array is
            // represented by the strings field of the NameTable data type.
            // This section describes the format of these strings and provides
            // an example of using this subtable.
            var suffixes = new List<string>(NumberOfSuffixes);
            for (int i = 0; i < NumberOfSuffixes; i++)
            {
                var suffix = SpanUtilities.ReadPascalString(data[offset..], out var suffixBytesRead);
                offset += suffixBytesRead;
                suffixes.Add(suffix);
            }

            Suffixes = suffixes;
        }

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Read beyond the end of the data for FontFamilyNameTable");
    }
}
