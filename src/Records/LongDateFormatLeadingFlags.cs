namespace ResourceForkReader.Records;

/// <summary>
/// Flags indicating whether to include or suppress leading components in a long date format.
/// </summary>
[Flags]
public enum LongDateFormatLeadingFlags : byte
{
    /// <summary>
    /// Include the component.
    /// </summary>
    Include = 0x00,
    
    /// <summary>
    /// Suppress the component.
    /// </summary>
    Suppress = 0xFF,
}
