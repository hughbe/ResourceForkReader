namespace ResourceForkReader.Records;

/// <summary>
/// Flags for Installer File Atom Records ('infa').
/// </summary>
[Flags]
public enum InstallerFileAtomFlags : ushort
{
    /// <summary>
    /// No flags set.
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates that the file should be deleted when the package is removed.
    /// </summary>
    DeleteOnRemove = 1 << 0,

    /// <summary>
    /// Indicates that the file should be deleted upon installation.
    /// </summary>
    DeleteOnInstall = 1 << 1,

    /// <summary>
    /// Indicates that the file is a copy-only file.
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
    /// Unused flag 7.
    /// </summary>
    Unused7 = 1 << 9,

    /// <summary>
    /// Unused flag 8.
    /// </summary>
    Unused8 = 1 << 10,

    /// <summary>
    /// Indicates that the file should be left alone if a newer version exists.
    /// </summary>
    LeaveAloneIfNewer = 1 << 11,

    /// <summary>
    /// Indicates that existing files should be kept.
    /// </summary>
    KeepExisting = 1 << 12,

    /// <summary>
    /// Indicates that only updates should be applied.
    /// </summary>
    UpdateOnly = 1 << 13,

    /// <summary>
    /// Indicates that the file is a resource fork.
    /// </summary>
    ResourceFork = 1 << 14,

    /// <summary>
    /// Indicates that the file is a data fork.
    /// </summary>
    DataFork = 1 << 15,
}
