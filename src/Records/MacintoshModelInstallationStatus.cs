namespace ResourceForkReader.Records;

/// <summary>
/// Represents the installation status of a Macintosh model.
/// </summary>
public enum MacintoshModelInstallationStatus : uint
{
    /// <summary>
    /// Not installed.
    /// </summary>
    NotInstalled = 0,

    /// <summary>
    /// Minimal installation.
    /// </summary>
    MinimalInstallation = 1,

    /// <summary>
    /// Full installation.
    /// </summary>
    FullInstallation = 2
}
