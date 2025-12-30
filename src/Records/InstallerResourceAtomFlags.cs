namespace ResourceForkReader.Records;

/// <summary>
/// Flags for Installer Resource Atom Records.
/// </summary>
[Flags]
public enum InstallerResourceAtomFlags : ushort
{
    /// <summary>
    /// No flags are set.
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates that the resource should be deleted when the package is removed.
    /// </summary>
    DeleteOnRemove = 1 << 0,

    /// <summary>
    /// Indicates that the resource should be deleted upon installation.
    /// </summary>
    DeleteOnInstall = 1 << 1,

    /// <summary>
    /// Indicates that the resource should be copied rather than moved.
    /// </summary>
    Copy = 1 << 2,

    /// <summary>
    /// Unused flag 1.
    /// </summary>
    Unused1 = 1 << 3,

    /// <summary>
    /// Unused flag 2.
    /// </summary>
    Unused2 = 1 << 4,

    /// <summary>
    /// Unused flag 3.
    /// </summary>
    Unused3 = 1 << 5,

    /// <summary>
    /// Unused flag 4.
    /// </summary>
    Unused4 = 1 << 6,

    /// <summary>
    /// Unused flag 5.
    /// </summary>
    Unused5 = 1 << 7,

    /// <summary>
    /// Unused flag 6.
    /// </summary>
    Unused6 = 1 << 8,

    /// <summary>
    /// Indicates that the target resource must exist for the operation to proceed.
    /// </summary>
    TargetRequired = 1 << 9,

    /// <summary>
    /// Indicates that existing resources should be kept and not overwritten.
    /// </summary>
    KeepExisting = 1 << 10,

    /// <summary>
    /// Indicates that only updates should be applied to the resource.
    /// </summary>
    UpdateOnly = 1 << 11,

    /// <summary>
    /// Indicates that the operation should proceed even if the resource is protected.
    /// </summary>
    EvenIfProtected = 1 << 12,

    /// <summary>
    /// Indicates that the target resource need not exist for the operation to proceed.
    /// </summary>
    NeedNotExist = 1 << 13,

    /// <summary>
    /// Indicates that the resource should be found by its ID.
    /// </summary>
    FindByID = 1 << 14,

    /// <summary>
    /// Indicates that the resource should be found by its name.
    /// </summary>
    NameMustMatch = 1 << 15,
}
