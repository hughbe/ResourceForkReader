namespace ResourceForkReader.Records;

/// <summary>
/// Calendar codes used in Long Date Format Extended Records.
/// </summary>
public enum CalendarCode : ushort
{
    /// <summary>
    /// Gregorian calendar.
    /// </summary>
    Gregorian = 0,

    /// <summary>
    /// Arabic Civil calendar.
    /// </summary>
    ArabicCivil = 1,

    /// <summary>
    /// Arabic Lunar calendar.
    /// </summary>
    ArabicLunar = 2,

    /// <summary>
    /// Japanese calendar.
    /// </summary>
    Japanese = 3,

    /// <summary>
    /// Jewish calendar.
    /// </summary>
    Jewish = 4,

    /// <summary>
    /// Coptic calendar.
    /// </summary>
    Coptic = 5,

    /// <summary>
    /// Persian calendar.
    /// </summary>
    Persian = 6
}
