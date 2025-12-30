namespace ResourceForkReader.Records;

/// <summary>
/// Date order options in a Numeric Format Record ('INTL' and 'intl0').
/// </summary>
public enum DateOrder : byte
{
    /// <summary>
    /// Month-Day-Year (e.g., 12/31/2024).
    /// </summary>
    MonthDayYear = 0,

    /// <summary>
    /// Day-Month-Year (e.g., 31/12/2024).
    /// </summary>
    DayMonthYear = 1,

    /// <summary>
    /// Year-Month-Day (e.g., 2024/12/31).
    /// </summary>
    YearMonthDay = 2,

    /// <summary>
    /// Month-Year-Day (e.g., 12/2024/31).
    /// </summary>
    MonthYearDay = 3,

    /// <summary>
    /// Day-Year-Month (e.g., 31/2024/12).
    /// </summary>
    DayYearMonth = 4,

    /// <summary>
    /// Year-Day-Month (e.g., 2024/31/12).
    /// </summary>
    YearDayMonth = 5
}
