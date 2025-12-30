using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Long Date Format Extended Record in an International Resource.
/// </summary>
public readonly struct LongDateFormatExtendedRecord
{
    /// <summary>
    /// The minimum size of a Long Date Format Extended Record in bytes.
    /// </summary>
    public const int MinSize = 46;

    /// <summary>
    /// Gets the version.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// Gets the format code.
    /// </summary>
    public ushort FormatCode { get; }

    /// <summary>
    /// Gets the calendar code.
    /// </summary>
    public CalendarCode CalendarCode { get; }

    /// <summary>
    /// Gets the offset to the extra day names table.
    /// </summary>
    public uint ExtraDayNamesTableOffset { get; }

    /// <summary>
    /// Gets the length of the extra day names table.
    /// </summary>
    public uint ExtraDayNamesTableLength { get; }

    /// <summary>
    /// Gets the offset to the extra month table.
    /// </summary>
    public uint ExtraMonthNamesTableOffset { get; }

    /// <summary>
    /// Gets the length of the extra month table.
    /// </summary>
    public uint ExtraMonthNamesTableLength { get; }

    /// <summary>
    /// Gets the offset to the abbreviated day names.
    /// </summary>
    public uint AbbreviatedDayNamesOffset { get; }

    /// <summary>
    /// Gets the length of the abbreviated day names.
    /// </summary>
    public uint AbbreviatedDayNamesLength { get; }

    /// <summary>
    /// Gets the offset to the abbreviated month.
    /// </summary>
    public uint AbbreviatedMonthNamesOffset { get; }

    /// <summary>
    /// Gets the length of the abbreviated month.
    /// </summary>
    public uint AbbreviatedMonthNamesLength { get; }

    /// <summary>
    /// Gets the offset to the extra separators.
    /// </summary>
    public uint ExtraSeparatorsOffset { get; }

    /// <summary>
    /// Gets the length of the extra separators.
    /// </summary>
    public uint ExtraSeparatorsLength { get; }

    /// <summary>
    /// Gets the extra day names.
    /// </summary>
    public List<string> ExtraDayNames { get; }

    /// <summary>
    /// Gets the extra month names.
    /// </summary>
    public List<string> ExtraMonthNames { get; }

    /// <summary>
    /// Gets the abbreviated day names.
    /// </summary>
    public List<string> AbbreviatedDayNames { get; }

    /// <summary>
    /// Gets the abbreviated month names.
    /// </summary>
    public List<string> AbbreviatedMonthNames { get; }

    /// <summary>
    /// Gets the extra separators.
    /// </summary>
    public List<string> ExtraSeparators { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LongDateFormatExtendedRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Long Date Format Extended Record data.</param>
    /// <param name="recordStartOffset">The offset of the start of the record in the original resource data.</param>
    /// <param name="bytesRead">>Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when data is less than 46 bytes long.</exception>
    public LongDateFormatExtendedRecord(ReadOnlySpan<byte> data, int recordStartOffset, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Long Date Format Extended Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-31 to B-34
        int offset = 0;

        // The version number of this extension. Unlike the intl1Vers field
        // in the unextended 'itl1' resource, this field contains nothing but
        // the version number.
        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // A number that identifies the format of this resource. The current
        // extended long-date-format resource format has a format code of 0.
        FormatCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Multiple calendars may be available on some systems, and it is
        // necessary to identify the particular calendar for use with this
        // long-date-format resource.
        CalendarCode = (CalendarCode)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The offset from the beginning of the long-date-format resource to
        // the extra days table.
        ExtraDayNamesTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The length in bytes of the extra days table.
        ExtraDayNamesTableLength = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The offset from the beginning of the long-date-format resource to
        // the extra months table.
        ExtraMonthNamesTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset  += 4;

        // The length in bytes of the extra months table.
        ExtraMonthNamesTableLength = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The offset from the beginning of the long-date-format resource to
        // the abbreviated day table.
        AbbreviatedDayNamesOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The length in bytes of the abbreviated day table.
        AbbreviatedDayNamesLength = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The offset from the beginning of the long-date-format resource to
        // the abbreviated month table.
        AbbreviatedMonthNamesOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The length in bytes of the abbreviated month table.
        AbbreviatedMonthNamesLength = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The offset from the beginning of the long-date-format resource to
        // the extra separators table.
        ExtraSeparatorsOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The length in bytes of the extra separators table.
        ExtraSeparatorsLength = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The tables that make up the rest of the 'itl1' resource extension.
        // Each table in the 'itl1' resource extension is an array consisting
        // of an integer count followed by a list of Pascal strings specifying
        // names of days, names of months, or separators.
        List<string> ReadTable(ReadOnlySpan<byte> data, uint tableOffset, uint tableLength)
        {
            if (tableLength == 0)
            {
                return [];
            }

            if (tableOffset < recordStartOffset || tableOffset + tableLength > recordStartOffset + data.Length)
            {
                throw new ArgumentException("Table offset and length are out of bounds of the Long Date Format Extended Record data.", nameof(data));
            }

            var actualOffset = (int)(tableOffset - recordStartOffset);
            var tableData = data.Slice(actualOffset, (int)tableLength);

            int tableDataOffset = 0;
            ushort count = BinaryPrimitives.ReadUInt16BigEndian(tableData.Slice(tableDataOffset, 2));
            tableDataOffset += 2;

            var names = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                string name = SpanUtilities.ReadPascalString(tableData.Slice(tableDataOffset));
                names.Add(name);
                tableDataOffset += 1 + name.Length;
            }

            return names;
        }

        // Extra days table. A list of names. This format is for those
        // calendars with more than 7 day names.
        ExtraDayNames = ReadTable(data, ExtraDayNamesTableOffset, ExtraDayNamesTableLength);

        // Extra months table. A list of names. This format is for those
        // calendars with more than 12 months.
        ExtraMonthNames = ReadTable(data, ExtraMonthNamesTableOffset, ExtraMonthNamesTableLength);

        // Abbreviated days table. A table of abbreviations. If the header
        // specifies an offset to and length of the abbreviated days table,
        // the Text Utilities routines that create date strings use this
        // table instead of truncating day names to the number of characters
        // specified in the abbrevLen field of the standard 'itl1' resource.
        AbbreviatedDayNames = ReadTable(data, AbbreviatedDayNamesOffset, AbbreviatedDayNamesLength);

        // Abbreviated months table. A table of abbreviations. If the header
        // specifies an offset to and length of the abbreviated months table,
        // the Text Utilities routines that create date strings use this
        // table instead of truncating month names to the number of characters
        // specified in the abbrevLen field of the standard 'itl1' resource.
        AbbreviatedMonthNames = ReadTable(data, AbbreviatedMonthNamesOffset, AbbreviatedMonthNamesLength);

        // Extra separators table. A list of additional date separators.
        // When parsing date strings, the Text Utilities StringToDate and
        // StringToTime functions permit the separators in this list to be
        // used in addition to the date separators specified elsewhere in the
        // numeric-format and long-date-format resources.
        ExtraSeparators = ReadTable(data, ExtraSeparatorsOffset, ExtraSeparatorsLength);

        bytesRead = offset;
        Debug.Assert(bytesRead <= data.Length, "Read more bytes than available in Long Date Format Extended Record data.");
    }
}
