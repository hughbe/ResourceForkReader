using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Untoken Table in a Tokens Record ('itl4') in a resource fork.
/// </summary>
public readonly struct UntokenTable
{
    /// <summary>
    /// The minimum size of an Untoken Table in bytes.
    /// </summary>
    public const int MinSize = 4;

    /// <summary>
    /// Gets the length of the untoken table in bytes.
    /// </summary>
    public ushort Length { get; }

    /// <summary>
    /// Gets the last token index.
    /// </summary>
    public ushort LastToken { get; }

    /// <summary>
    /// Gets the offsets of the tokens.
    /// </summary>
    public ushort[] Offsets { get; }

    /// <summary>
    /// Gets the tokens.
    /// </summary>
    public string[] Tokens { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UntokenTable"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Untoken Table data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than MinSize bytes long.</exception>
    public UntokenTable(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Untoken Table. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-54 to B-55
        int offset = 0;

        // The length in bytes of the untoken table.
        Length = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The highest token code used in this table (for range-checking).
        LastToken = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // An array of byte offsets from the beginning of the untoken table
        // to Pascal strings—one for each possible token type—that give the
        // canonical format for each fixed token type. The entries in the
        // array correspond, in order, to token code values from 0 to lastToken.
        // For example, the offset to the Pascal string for tokenColonEqual
        // (token code = 39) is found at offset 39 in the array.
        var offsets = new ushort[LastToken + 1];
        for (int i = 0; i < offsets.Length; i++)
        {
            offsets[i] = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        Offsets = offsets;

        // The string data directly follows the index array. It is a simple
        // concatenation of Pascal strings; for example, the Pascal string
        // for the token type tokenColonEqual may consist of a length byte
        // (of value 2) followed by the characters “:=”.
        var tokens = new string[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
        {
            int tokenOffset = offsets[i];
            if (tokenOffset >= Length)
            {
                throw new ArgumentException("Invalid Untoken Table: token offset is out of bounds.", nameof(data));
            }

            tokens[i] = SpanUtilities.ReadPascalString(data[tokenOffset..], out _);
        }

        Tokens = tokens;

        Debug.Assert(offset <= data.Length, "Did not consume all data for UntokenTable.");
    }
}
