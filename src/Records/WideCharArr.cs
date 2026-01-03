using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// An array of wide characters used in various records in a resource fork.
/// </summary>
public readonly struct WideCharArr
{
    /// <summary>
    /// The size of the WideCharArr character array in bytes.
    /// </summary>
    public const int Size = 22;

    /// <summary>
    /// The number of items in the table minus 1.
    /// </summary>
    public ushort NumberOfCharacters { get; }

    /// <summary>
    /// The character array.
    /// </summary>
    public char[] Characters { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WideCharArr"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing WideCharArr data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is not exactly Size bytes long.</exception>
    public WideCharArr(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data is not the correct size to be a valid WideCharArr. Size must be exactly {Size} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-57
        int offset = 0;

        // The number of items in the table minus 1.
        NumberOfCharacters = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Up to ten wide characters. If the number part is only a single
        // 1-byte character, that character is in the low-order byte of the word.
        var characters = new char[10];
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i] = (char)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        Characters = characters;

        Debug.Assert(offset == data.Length, "Did not consume all data for WideCharArr.");
    }
}
