namespace ResourceForkReader.Records;

/// <summary>
/// Flags for the Scripting Addition Size Record.
/// </summary>
[Flags]
public enum ScriptingAdditionSizeRecordFlags : ushort
{
    /// <summary>
    /// No flags are set.
    /// </summary>
    None = 0x00,

    /// <summary>
    /// Indicates that the scripting addition should not open a resource file.
    /// </summary>
    DontOpenResourceFile = 0x01,

    /// <summary>
    /// Indicates that the scripting addition should not accept remote events.
    /// </summary>
    DontAcceptRemoteEvents = 0x02,
}
