using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Tokens Record ('itl4') in a resource fork.
/// </summary>
public readonly struct TokensRecord
{
    /// <summary>
    /// The minimum size of a Tokens Record in bytes.
    /// </summary>
    public const int MinSize = 66;

    /// <summary>
    /// Gets the flags.
    /// </summary>
    public ushort Flags { get; }

    /// <summary>
    /// Gets the resource type.
    /// </summary>
    public string ResourceType { get; }

    /// <summary>
    /// Gets the resource ID.
    /// </summary>
    public short ResourceID { get; }

    /// <summary>
    /// Gets the version number.
    /// </summary>
    public ushort VersionNumber { get; }

    /// <summary>
    /// Gets the format code.
    /// </summary>
    public ushort FormatCode { get; }

    /// <summary>
    /// Gets the reserved1 field.
    /// </summary>
    public ushort Reserved1 { get; }

    /// <summary>
    /// Gets the reserved2 field.
    /// </summary>
    public uint Reserved2 { get; }

    /// <summary>
    /// Gets the number of tables.
    /// </summary>
    public ushort NumberOfTables { get; }

    /// <summary>
    /// Gets the token table offset.
    /// </summary>
    public uint TokenTableOffset { get; }

    /// <summary>
    /// Gets the string copy routine offset.
    /// </summary>
    public uint StringCopyRoutineOffset { get; }

    /// <summary>
    /// Gets the extension fetch routine offset.
    /// </summary>
    public uint ExtensionFetchRoutineOffset { get; }

    /// <summary>
    /// Gets the untoken table offset.
    /// </summary>
    public uint UntokenTableOffset { get; }

    /// <summary>
    /// Gets the number parts table offset.
    /// </summary>
    public uint NumberPartsTableOffset { get; }

    /// <summary>
    /// Gets the whitespace table offset.
    /// </summary>
    public uint WhitespaceTableOffset { get; }

    /// <summary>
    /// Gets the reserved offset 7.
    /// </summary>
    public uint ReservedOffset7 { get; }

    /// <summary>
    /// Gets the reserved offset 8.
    /// </summary>
    public uint ReservedOffset8 { get; }

    /// <summary>
    /// Gets the reserved length 1.
    /// </summary>
    public ushort ReservedLength1 { get; }

    /// <summary>
    /// Gets the reserved length 2.
    /// </summary>
    public ushort ReservedLength2 { get; }

    /// <summary>
    /// Gets the reserved length 3.
    /// </summary>
    public ushort ReservedLength3 { get; }

    /// <summary>
    /// Gets the untoken table length.
    /// </summary>
    public ushort UntokenTableLength { get; }

    /// <summary>
    /// Gets the number parts table length.
    /// </summary>
    public ushort NumberPartsTableLength { get; }

    /// <summary>
    /// Gets the whitespace table length.
    /// </summary>
    public ushort WhitespaceTableLength { get; }

    /// <summary>
    /// Gets the reserved length 7.
    /// </summary>
    public ushort ReservedLength7 { get; }

    /// <summary>
    /// Gets the reserved length 8.
    /// </summary>
    public ushort ReservedLength8 { get; }

    /// <summary>
    /// Gets the token table.
    /// </summary>
    public byte[] TokenTable { get; }

    /// <summary>
    /// Gets the untoken table, if present.
    /// </summary>
    public UntokenTable? UntokenTable { get; }

    /// <summary>
    /// Gets the number parts table, if present.
    /// </summary>
    public NumberPartsTable? NumberPartsTable { get; }

    /// <summary>
    /// Gets the whitespace table, if present.
    /// </summary>
    public WhitespaceTable? WhitespaceTable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokensRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Tokens Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than MinSize bytes long.</exception>
    public TokensRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException("Data is too short to be a valid Tokens Record.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-50 to B-53
        int offset = 0;

        // (reserved)
        Flags = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // 'itl4' (the resource type of the tokens resource).
        ResourceType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        if (ResourceType != "itl4")
        {
            throw new ArgumentException("Invalid Tokens Record: unexpected resource type.", nameof(data));
        }

        // The resource ID number of this tokens resource.
        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The version number of this tokens resource.
        VersionNumber = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The format code, a number that identifies the format of this
        // tokens resource.
        FormatCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // (reserved)
        Reserved1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // (reserved)
        Reserved2 = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The number of tables in this tokens resource.
        NumberOfTables = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The offset in bytes from the beginning of the resource to the
        // token table, an array that maps each byte to a token type.
        TokenTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The offset in bytes from the beginning of the resource to the
        // token-string copy routine, a code segment that creates strings
        // that correspond to the text that generated each token.
        StringCopyRoutineOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The offset in bytes from the beginning of the resource to the
        // extension-fetching routine, a code segment that fetches the second
        // byte of a 2-byte character for the IntlTokenize function.
        ExtensionFetchRoutineOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The offset in bytes from the beginning of the resource to the
        // untoken table, an array that maps token types back to the
        // canonical strings that represent them.
        UntokenTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The offset in bytes from the beginning of the resource to the
        // number parts table, an array of characters that correspond to
        // each part of a number format (used primarily by the
        // FormatRecToString and StringToExtended functions).
        NumberPartsTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The offset in bytes from the beginning of the resource to the
        // whitespace table, a list of all the characters that should be
        // treated as whitespace—for example, blank and tab for the Roman
        // script system.
        WhitespaceTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // (reserved)
        ReservedOffset7 = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // (reserved)
        ReservedOffset8 = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // (reserved)
        ReservedLength1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;
        
        // (reserved)
        ReservedLength2 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // (reserved)
        ReservedLength3 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The length in bytes of the untoken table.
        UntokenTableLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The length in bytes of the number parts table.
        NumberPartsTableLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The length in bytes of the whitespace table.
        WhitespaceTableLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // (reserved)
        ReservedLength7 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // (reserved)
        ReservedLength8 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The 'itl4' resource includes the token table, an array of type
        // mapCharTable. The token table, also called the character-mapping
        // table, maps each possible byte value in a 1-byte character set
        // into a token type.
        if (TokenTableOffset + 256 > data.Length)
        {
            throw new ArgumentException("Invalid Tokens Record: token table extends beyond the end of the data.", nameof(data));
        }

        TokenTable = data.Slice((int)TokenTableOffset, 256).ToArray();

        // The untoken table provides a Pascal string for any type of fixed token.
        // A fixed token is a token whose representation is unvarying, like
        // punctuation. Alphabetic and numeric tokens are not fixed;
        // specifying the token does not specify the string it represents.
        if (UntokenTableLength != 0)
        {
            if (UntokenTableOffset + UntokenTableLength > data.Length)
            {
                throw new ArgumentException("Invalid Tokens Record: untoken table extends beyond the end of the data.", nameof(data));
            }

            // UntokenTable length is ignored as seen examples where it is
            // too short.
            UntokenTable = new UntokenTable(data[(int)UntokenTableOffset..]);
        }
        else
        {
            UntokenTable = null;
        }

        // The number parts table contains standard representations for the
        // components of numbers and numeric strings. The Text Utilities
        // number-formatting routines StringToExtended and ExtendedToString
        // use the number parts table, along with a number-format string
        // created by the StringToFormatRec and FormatRecToString routines,
        // to create number strings in localized formats.
        if (NumberPartsTableLength != 0)
        {
            if (NumberPartsTableOffset + NumberPartsTableLength > data.Length)
            {
                throw new ArgumentException("Invalid Tokens Record: number parts table extends beyond the end of the data.", nameof(data));
            }

            NumberPartsTable = new NumberPartsTable(data.Slice((int)NumberPartsTableOffset, NumberPartsTableLength));
        }
        else
        {
            NumberPartsTable = null;
        }

        // The whitespace table contains characters that may be used to
        // indicate white space, such as blanks, tabs, and carriage returns.
        // Figure B-13 shows the format of the whitespace table. Each entry
        // pointed to by the table is a Pascal string specifying a single
        // whitespace character (which may be 1 or 2 bytes long). The strings
        // immediately follow the offset fields.
        if (WhitespaceTableLength != 0)
        {
            if (WhitespaceTableOffset + WhitespaceTableLength > data.Length)
            {
                throw new ArgumentException("Invalid Tokens Record: whitespace table extends beyond the end of the data.", nameof(data));
            }

            var whitespaceTableData = data.Slice((int)WhitespaceTableOffset, WhitespaceTableLength);
            WhitespaceTable = new WhitespaceTable(whitespaceTableData);
        }
        else
        {
            WhitespaceTable = null;
        }

        Debug.Assert(offset <= data.Length, "Did not consume all data for TokensRecord.");
    }
}
