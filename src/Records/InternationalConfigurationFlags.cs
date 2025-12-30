namespace ResourceForkReader.Records;

/// <summary>
/// Flags for the International Configuration Record ('itlc').
/// </summary>
[Flags]
public enum InternationalConfigurationFlags : byte
{
    /// <summary>
    /// No flags are set.
    /// </summary>
    None = 0,

    /// <summary>
    /// Always show the keyboard icon.
    /// </summary>
    AlwaysShowKeyboardIcon = 1 << 0,

    /// <summary>
    /// Use dual caret for mixed-direction text.
    /// </summary>
    UseDualCaretForMixedDirectionText = 1 << 1,
    
    /// <summary>
    /// Unused flag.
    /// </summary>
    Unused1 = 1 << 2,

    /// <summary>
    /// Unused flag.
    /// </summary>
    Unused2 = 1 << 3,

    /// <summary>
    /// Unused flag.
    /// </summary>
    Unused3 = 1 << 4,

    /// <summary>
    /// Unused flag.
    /// </summary>
    Unused4 = 1 << 5,

    /// <summary>
    /// Unused flag.
    /// </summary>
    Unused5 = 1 << 6,

    /// <summary>
    /// Unused flag.
    /// </summary>
    Unused6 = 1 << 7,
}
