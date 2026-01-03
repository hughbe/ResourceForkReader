namespace ResourceForkReader.Records;

/// <summary>
/// Flags for the Scripting Size Record.
/// </summary>
[Flags]
public enum ScriptingSizeRecordFlags : uint
{
    /// <summary>
    /// No flags are set.
    /// </summary>
    None = 0x00,

    /// <summary>
    /// Indicates that extension terms should be read.
    /// </summary>
    ReadExtensionTerms = 0x01
}
