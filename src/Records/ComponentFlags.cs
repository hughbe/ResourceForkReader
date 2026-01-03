namespace ResourceForkReader.Records;

/// <summary>
/// Flags for a Component.
/// </summary>
public enum ComponentFlags : uint
{
    /// <summary>
    /// No flags are set.
    /// </summary>
    None = 0x00000000,

    /// <summary>
    /// The component wants to receive register messages.
    /// </summary>
    WantsRegisterMessage = 0x80000000,

    /// <summary>
    /// The component supports fast dispatch.
    /// </summary>
    FastDispatch = 0x40000000
}