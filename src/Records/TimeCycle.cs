namespace ResourceForkReader.Records;

/// <summary>
/// Time Cycle options in a Numeric Format Record ('INTL' and 'intl0').
/// </summary>
public enum TimeCycle : byte
{
    /// <summary>
    /// Use 24-hour format (midnight = 0:00).
    /// </summary>
    TwentyFourHour = 0,

    /// <summary>
    /// Use A.M./P.M. format (midnight and noon = 0:00)
    /// </summary>
    TwelveHourMidnightZero = 1,

    /// <summary>
    /// Use A.M./P.M. format (midnight and noon = 12:00)
    /// </summary>
    TwelveHourMidnightTwelve = 255
}
