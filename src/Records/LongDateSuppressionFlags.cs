namespace ResourceForkReader.Records;

/// <summary>
/// Flags for suppressing elements in a long date format.
/// </summary>
public enum LongDateSuppressionFlags : byte
{
    /// <summary>
    /// No suppression; include all elements.
    /// </summary>
    None = 0x00,

    /// <summary>
    /// Suppress day of month.
    /// </summary>
    SuppressDayName = 0x01,
    
    /// <summary>
    /// Suppress day name.
    /// </summary>
    SuppressWeek = 0x02,

    /// <summary>
    /// Suppress month.
    /// </summary>
    SuppressMonth = 0x04,

    /// <summary>
    /// Suppress year.
    /// </summary>
    SuppressYear = 0x08,

    /// <summary>
    /// Suppress all elements.
    /// </summary>
    SuppressAll = 0xFF
}
