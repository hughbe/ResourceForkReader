namespace ResourceForkReader.Records;

/// <summary>
/// Defines flags for component registration.
/// </summary>
[Flags]
public enum ComponentRegisterFlags : uint
{
    /// <summary>
    /// No special registration flags.
    /// </summary>
    None = 0x00000000,

    /// <summary>
    /// Indicates that the Component Manager should automatically obtain the version number of the component when it is registered.
    /// </summary>
    DoAutoVersion = 0x00000001,

    /// <summary>
    /// Indicates that the component wants to receive an unregister notification when it is unregistered.
    /// </summary>
    WantsUnregister = 0x00000002,

    /// <summary>
    /// Indicates that the automatic versioning should include the component's flags.
    /// </summary>
    AutoVersionIncludeFlags = 0x00000004,
}
