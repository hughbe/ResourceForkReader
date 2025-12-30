namespace ResourceForkReader.Records;

/// <summary>
/// Time Format Flags in a Numeric Format Record ('INTL' and 'intl0').
/// </summary>
[Flags]
public enum TimeFormatFlags : byte
{
    /// <summary>
    /// No flags set.
    /// </summary>
    None = 0,

    /// <summary>
    /// Use leading zero for seconds.
    /// </summary>
    LeadingZeroSeconds = 32,

    /// <summary>
    /// Use leading zero for minutes.
    /// </summary>
    LeadingZeroMinutes = 64,

    /// <summary>
    /// Use leading zero for hours.
    /// </summary>
    LeadingZeroHours = 128
}
