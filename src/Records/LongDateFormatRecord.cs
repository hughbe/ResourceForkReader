using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Long Date Format Record ('INTL' or 'itl1') in an International Resource.
/// </summary>
public readonly struct LongDateFormatRecord
{
    /// <summary>
    /// The minimum size of a Long Date Format Record in bytes.
    /// </summary>
    public const int MinSize = 332;

    /// <summary>
    /// Gets the day names.
    /// </summary>
    public string[] DayNames { get; }

    /// <summary>
    /// Gets the month names.
    /// </summary>
    public string[] MonthNames { get; }

    /// <summary>
    /// Gets the suppression flags.
    /// </summary>
    public LongDateSuppressionFlags SuppressionFlags { get; }

    /// <summary>
    /// Gets the order of components in the long date format.
    /// </summary>
    public LongDateFormatOrder[] Order { get; }

    /// <summary>
    /// Gets the leading flags.
    /// </summary>
    public LongDateFormatLeadingFlags LeadingFlags { get; }

    /// <summary>
    /// Gets the abbreviation length.
    /// </summary>
    public byte AbbreviationLength { get; }

    /// <summary>
    /// Gets the separator string 0.
    /// </summary>
    public string SeparatorString0 { get; }

    /// <summary>
    /// Gets the separator string 1.
    /// </summary>
    public string SeparatorString1 { get; }

    /// <summary>
    /// Gets the separator string 2.
    /// </summary>
    public string SeparatorString2 { get; }

    /// <summary>
    /// Gets the separator string 3.
    /// </summary>
    public string SeparatorString3 { get; }

    /// <summary>
    /// Gets the separator string 4.
    /// </summary>
    public string SeparatorString4 { get; }

    /// <summary>
    /// Gets the region code.
    /// </summary>
    public byte RegionCode { get; }

    /// <summary>
    /// Gets the version.
    /// </summary>
    public byte Version { get; }

    /// <summary>
    /// Gets the localization routine.
    /// </summary>
    public ushort LocalizationRoutine { get; }

    /// <summary>
    /// Gets the extended record, if present.
    /// </summary>
    public LongDateFormatExtendedRecord? ExtendedRecord { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LongDateFormatRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Long Date Format Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is less than the minimum size.</exception>
    public LongDateFormatRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Long Date Format Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-28 to B-34
        int offset = 0;
        
        // An array of 7 day names (ordered for days corresponding to Sunday
        // through Saturday). Each day name may consist of a maximum of 15
        // characters.
        var dayNames = new string[7];
        for (int i = 0; i < 7; i++)
        {
            dayNames[i] = SpanUtilities.ReadPascalString(data.Slice(offset, 16));
            offset += 16;
        }

        DayNames = dayNames;

        // An array of 12 month names (ordered for months corresponding to
        // January through December). Each month name may consist of a
        // maximum of 15 characters.
        var monthNames = new string[12];
        for (int i = 0; i < 12; i++)
        {
            monthNames[i] = SpanUtilities.ReadPascalString(data.Slice(offset, 16));
            offset += 16;
        }

        MonthNames = monthNames;

        // A byte that lets you omit any element in the long date. To include
        // the day name in the long date, you set the field to 0. To suppress
        // the day name, set the field to 255 ($FF). If the value does not
        // equal 0 or $FF, this field is treated as bit flags.
        SuppressionFlags = (LongDateSuppressionFlags)data[offset];
        offset += 1;

        // The byte that indicates the order of long date elements. If the
        // byte value of the field is neither 0 (which specifies an order of
        // day/ month/year) nor $FF (which specifies an order of month/day/
        // year), then its value is divided into 4 fields of 2 bits each.
        // The least significant bit field (bits 0 and 1) corresponds to
        // the first element in the long date format, whereas the most
        // significant bit field (bits 6 and 7) specifies the last (fourth)
        // element in the format.
        var orderByte = data[offset];
        offset += 1;

        var order = new LongDateFormatOrder[4];
        for (int i = 0; i < 4; i++)
        {
            var componentValue = (orderByte >> (i * 2)) & 0x03;
            order[i] = (LongDateFormatOrder)componentValue;
        }

        Order = order;
        
        // If 255 ($FF), specifies a leading zero in a day number. If 0, no
        // leading zero is included in the day number.
        LeadingFlags = (LongDateFormatLeadingFlags)data[offset];
        offset += 1;

        // The number of characters to which month and day names should be
        // abbreviated when abbreviation is desired.
        AbbreviationLength = data[offset];
        offset += 1;

        // String that precedes (in memory) the first element in a long date.
        // See Table B-5 and Figure B-3.
        SeparatorString0 = Encoding.ASCII.GetString(data.Slice(offset, 4)).TrimEnd('\0');
        offset += 4;

        // String that separates the first and second elements in a long date.
        // See Table B-5 and Figure B-3. This string is suppressed if the
        // first element in the long date is suppressed.
        SeparatorString1 = Encoding.ASCII.GetString(data.Slice(offset, 4)).TrimEnd('\0');
        offset += 4;

        // String that separates the second and third elements in a long date.
        // See Table B-5 and Figure B-3. This string is suppressed if the
        // second element in the long date is suppressed.
        SeparatorString2 = Encoding.ASCII.GetString(data.Slice(offset, 4)).TrimEnd('\0');
        offset += 4;

        // String that separates the third and fourth elements in a long date.
        // See Table B-5 and Figure B-3. This string is suppressed if the
        // third element in the long date is suppressed.
        SeparatorString3 = Encoding.ASCII.GetString(data.Slice(offset, 4)).TrimEnd('\0');
        offset += 4;

        // String that follows the fourth element in a long date.
        // See Table B-5 and Figure B-3. This string is suppressed if the
        // fourth element in the long date is suppressed.
        SeparatorString4 = Encoding.ASCII.GetString(data.Slice(offset, 4)).TrimEnd('\0');
        offset += 4;

        // Region code and version number. The code number of the region that
        // this resource applies to is in the high-order byte, and the version
        // number of this long-date-format resource is in the low-order byte.
        RegionCode = data[offset];
        offset += 1;

        Version = data[offset];;
        offset += 1;

        // Originally designed to contain a routine that localizes sorting
        // order; now unused for that purpose. If an extended long-date-format
        // resource is available (see the next section), this field contains
        // the hexadecimal value $A89F as the first word.
        LocalizationRoutine = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));;
        offset += 2;

        if (LocalizationRoutine == 0xA89F)
        {
            ExtendedRecord = new LongDateFormatExtendedRecord(data.Slice(offset), offset, out var bytesRead);
            offset += bytesRead;
        }
        else
        {
            ExtendedRecord = null;
        }

        Debug.Assert(offset == data.Length, "Did not consume all data for Long Date Format Record.");
    }
}
