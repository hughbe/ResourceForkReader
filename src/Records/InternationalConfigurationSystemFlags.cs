namespace ResourceForkReader.Records;

/// <summary>
/// System flags for the International Configuration Record ('itlc').
/// </summary>
[Flags]
public enum InternationalConfigurationSystemFlags : ushort
{
    /// <summary>
    /// No flags are set.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Text is displayed right to left.
    /// </summary>
    RightToLeft = 1 << 15
}
