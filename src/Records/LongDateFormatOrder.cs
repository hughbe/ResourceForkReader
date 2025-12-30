namespace ResourceForkReader.Records;

/// <summary>
/// Specifies the order of components in a long date format.
/// </summary>
public enum LongDateFormatOrder : byte
{
    /// <summary>
    /// Day of the month
    /// </summary>
    DayOfMonth = 0x00,

    /// <summary>
    /// Day of the week.
    /// </summary>
    DayOfWeek = 0x01,

    /// <summary>
    /// Month.
    /// </summary>
    Month = 0x02,

    /// <summary>
    /// Year.
    /// </summary>
    Year = 0x03,
}