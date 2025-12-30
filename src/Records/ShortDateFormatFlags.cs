namespace ResourceForkReader.Records;

/// <summary>
/// Short Date Format Flags in a Numeric Format Record ('INTL' and 'intl0').
/// </summary>
public enum ShortDateFormatFlags : byte
{
    /// <summary>
    /// No flags set.
    /// </summary>
    None,

    /// <summary>
    /// Leading zero in day representation.
    /// </summary>
    LeadingZeroInDay = 32,

    /// <summary>
    /// Leading zero in month representation.
    /// </summary>
    LeadingZeroInMonth = 64,

    /// <summary>
    /// Show century in year representation.
    /// </summary>
    ShowCentury = 128
}
