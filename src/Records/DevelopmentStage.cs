namespace ResourceForkReader.Records;

/// <summary>
/// Specifies the development stage of a version resource.
/// </summary>
public enum DevelopmentStage : byte
{
    /// <summary>
    /// Prealpha file.
    /// </summary>
    Development = 0x20,

    /// <summary>
    /// Alpha file.
    /// </summary>
    Alpha = 0x40,

    /// <summary>
    /// Beta file.
    /// </summary>
    Beta = 0x60,

    /// <summary>
    /// Released file.
    /// </summary>
    Release = 0x80
}
