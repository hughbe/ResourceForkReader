namespace ResourceForkReader.Records;

/// <summary>
/// Gets the installer package flags.
/// </summary>
[Flags]
public enum InstallerPackageFlags : ushort
{
    /// <summary>
    /// No flags set.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Indicates that the package shows on the custom install screen.
    /// </summary>
    ShowsOnCustom = 1 << 0,

    /// <summary>
    /// Indicates that the package is removable.
    /// </summary>
    Removable = 1 << 1,
}