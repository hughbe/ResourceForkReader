using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Numeric Format Record ('INTL' and 'intl0') in a resource fork.
/// </summary>
public readonly struct NumericFormatRecord
{
    /// <summary>
    /// The size of a Numeric Format Record in bytes.
    /// </summary>
    public const int Size = 32;

    /// <summary>
    /// Gets the decimal separator character.
    /// </summary>
    public byte DecimalSeparator { get; }

    /// <summary>
    /// Gets the thousands separator character.
    /// </summary>
    public byte ThousandsSeparator { get; }

    /// <summary>
    /// Gets the list separator character.
    /// </summary>
    public byte ListSeparator { get; }

    /// <summary>
    /// Gets the first byte of the currency symbol.
    /// </summary>
    public byte CurrencySymbol1 { get; }

    /// <summary>
    /// Gets the second byte of the currency symbol.
    /// </summary>
    public byte CurrencySymbol2 { get; }

    /// <summary>
    /// Gets the third byte of the currency symbol.
    /// </summary>
    public byte CurrencySymbol3 { get; }

    /// <summary>
    /// Gets the currency format flags.
    /// </summary>
    public CurrencyFormatFlags CurrencyFormatFlags { get; }

    /// <summary>
    /// Gets the date order.
    /// </summary>
    public DateOrder DateOrder { get; }

    /// <summary>
    /// Gets the short date format flags.
    /// </summary>
    public ShortDateFormatFlags ShortDateFormatFlags { get; }

    /// <summary>
    /// Gets the date separator character.
    /// </summary>
    public byte DateSeparator { get; }

    /// <summary>
    /// Gets the time cycle.
    /// </summary>
    public TimeCycle TimeCycle { get; }

    /// <summary>
    /// Gets the time format flags.
    /// </summary>
    public TimeFormatFlags TimeFormatFlags { get; }

    /// <summary>
    /// Gets the AM string.
    /// </summary>
    public string AMString { get; }

    /// <summary>
    /// Gets the PM string.
    /// </summary>
    public string PMString { get; }

    /// <summary>
    /// Gets the time separator character.
    /// </summary>
    public byte TimeSeparator { get; }

    /// <summary>
    /// Gets the 24-hour morning string.
    /// </summary>
    public string TwentyFourHourMorningString { get; }

    /// <summary>
    /// Gets the 24-hour evening string.
    /// </summary>
    public string TwentyFourHourEveningString { get; }

    /// <summary>
    /// Gets the unit of measure.
    /// </summary>
    public UnitOfMeasure UnitOfMeasure { get; }

    /// <summary>
    /// Gets the region code.
    /// </summary>
    public byte RegionCode { get; }

    /// <summary>
    /// Gets the version number.
    /// </summary>
    public byte Version { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericFormatRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Numeric Format Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is less than 38 bytes long.</exception>
    public NumericFormatRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data is not the correct size to be a valid Numeric Format Record. Size must be {Size} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-22 to B-27
        int offset = 0;

        // Part of the number format definition. The1-byte character that
        // appears before the decimal representation of a fraction with a
        // denominator of 10. In the United States, this format is a period.
        // In several European countries, it is a comma.
        DecimalSeparator = data[offset];
        offset += 1;

        // Part of the number format definition. The 1-byte character that
        // separates every three digits to the left of the decimal point.
        // In the United States, this format is a comma. In several European
        // countries, it is a period.
        ThousandsSeparator = data[offset];
        offset += 1;

        // Part of the number format definition. The 1-byte character that
        // separates numbers, as when a list of numbers is entered by the
        // user; it must be different from the decimal point character. If
        // it’s the same as the thousands separator, the user must not
        // include the latter in entered numbers. In the United States,
        // this format is a semicolon. In the United Kingdom, it is a comma.
        ListSeparator = data[offset];
        offset += 1;

        // Part of the currency format definition. The initial byte used to
        // indicate currency. One character is sufficient for the United States
        // ($) and United Kingdom (£).
        CurrencySymbol1 = data[offset];
        offset += 1;

        // Part of the currency format definition. The second byte used to
        // indicate currency. Two characters are required for France (Fr).
        CurrencySymbol2 = data[offset];
        offset += 1;

        // Part of the currency format definition. The third byte used to
        // indicate currency. Three characters are required for Italy (Li.)
        // and Germany (DM.).
        CurrencySymbol3 = data[offset];
        offset += 1;

        // Part of the currency format definition. The four least significant
        // bits are unused. The four most significant bits are Boolean values.
        // Bit 7 determines whether there is a leading integer zero; for example,
        // a 1 in this field specifies a format like 0.23, whereas a 0 specifies
        // .23. Bit 6 determines whether there are trailing decimal zeros;
        // for example, a 1 in this field specifies a format like 325.00,
        // whereas a 0 specifies 325. Bit 5 determines whether to use a minus
        // sign or parentheses to denote a negative currency amount; for
        // example, a 1 in this field specifies a format like –0.45, whereas 
        // a 0 specifies (0.45). Bit 4 determines whether the currency symbol
        // trails or leads; for example, a value of 1 in this field specifies
        // a format like the $3.00 used in the United States, whereas a value
        // of 0 specifies the 3 DM. used in Germany.
        CurrencyFormatFlags = (CurrencyFormatFlags)data[offset];
        offset += 1;

        // Part of the short date format definition. Defines the order of
        // the elements (month, day, and year) of the short date format.
        // The order varies from region to region—for example, 12/29/72
        // is a common order in the United States, whereas 29.12.72 is
        // common in Europe.
        DateOrder = (DateOrder)data[offset];
        offset += 1;

        // Part of the short date format definition. The five least
        // significant bits are unused. The three most significant bit
        // fields are Boolean values that determine whether to show the
        // century, and whether to show leading zeros in month and day
        // numbers. For example, if the first bit is set to 1 it specifies
        // a date format like 10/21/1917, and set to 0 specifies the format
        // 10/21/17. The second bit set to 1 specifies a format like 
        // 05/23/84, and set to 0 specifies the format 5/23/84. The third
        // bit set to 1 specifies a format like 12/03/46, and set to 0
        // specifies the format 12/3/46.
        ShortDateFormatFlags = (ShortDateFormatFlags)data[offset];
        offset += 1;

        // Part of the short date format definition. The 1-byte character
        // that separates the different parts of the date. For example,
        // in the United States this character is a slash (12/3/46), in
        // Italy it is a hyphen (3-12-46), and in Japan it is a decimal
        // point (46.12.3).
        DateSeparator = data[offset];
        offset += 1;

        // Part of the time format definition. Indicates the time cycle—that
        // is, whether to use 12 or 24 hours as the basis of time, and
        // whether to consider midnight and noon to be 12:00 or 0:00.
        TimeCycle = (TimeCycle)data[offset];
        offset += 1;

        // Part of the time format definition. Indicates whether to show
        // leading zeros in time representation. Bit 5 determines whether
        // there are leading zeros in seconds; for example, a value of 1
        // in this field specifies a format like 11:15:05, whereas a 0
        // specifies the format 11:15:5. Bit 6 determines whether there
        // are leading zeros in minutes; for example, a value of 1 in
        // this field specifies a format like 10:05, whereas a 0 specifies
        // the format 10:5. Bit 7 determines whether there are leading
        // zeros in hours; for example, a value of 1 in this field specifies
        // a format like 09:15, whereas a 0 specifies the format 9:15.
        TimeFormatFlags = (TimeFormatFlags)data[offset];
        offset += 1;

        // Part of the time format definition. A string of up to 4 bytes to
        // follow the time to indicate morning (for example, “ AM”).
        // Typically, the string includes a leading space.
        AMString = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        // Part of the time format definition. A string of up to 4 bytes
        // to follow the time to indicate evening (for example, “ PM”).
        // Typically, the string includes a leading space.
        PMString = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        // Part of the time format definition. The 1-byte character that
        // is the time separator (for example, the colon).
        TimeSeparator = data[offset];
        offset += 1;

        // Part of the time format definition. A trailing string of up to
        // 4 bytes for the morning part of the 24-hour cycle. For example,
        // the German string “uhr” can be stored here.
        TwentyFourHourMorningString = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        // Part of the time format definition. A trailing string of up to 4
        // bytes for the evening part of the 24-hour cycle. Typically, this
        // string duplicates the string contained in time1Suff through
        // time4Suff. For example, the German string “uhr” can be stored
        // here.
        TwentyFourHourEveningString = Encoding.ASCII.GetString(data.Slice(offset, 4));;
        offset += 4;

        // The unit-of-measure definition. Indicates whether to use the
        // metric system. If 255, the metric system is used; if 0, metric
        // is not used.
        UnitOfMeasure = (UnitOfMeasure)data[offset];
        offset += 1;

        // Region code and version number. The code number of the region
        // that this resource applies to is in the high-order byte, and
        // the version number of this numeric-format resource is in the
        // low-order byte. 
        RegionCode = data[offset];
        offset += 1;

        Version = data[offset];;
        offset += 1;

        Debug.Assert(offset == Size, "Did not read all data for Numeric Format Record.");
    }
}
