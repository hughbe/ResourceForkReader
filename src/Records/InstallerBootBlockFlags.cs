namespace ResourceForkReader.Records;

/// <summary>
/// Flags for the Installer Boot Block Record ('inbb').
/// </summary>
[Flags]
public enum InstallerBootBlockFlags : ushort
{
    /// <summary>
    /// No flags are set.
    /// </summary>
    None = 0,

    /// <summary>
    /// Unused flag 1.
    /// </summary>
    Unused1 = 1 << 0,

    /// <summary>
    /// Unused flag 2.
    /// </summary>
    Unused2 = 1 << 1,

    /// <summary>
    /// Unused flag 3.
    /// </summary>
    Unused3 = 1 << 2,

    /// <summary>
    /// Unused flag 4.
    /// </summary>
    Unused4 = 1 << 3,

    /// <summary>
    /// Unused flag 5.
    /// </summary>
    Unused5 = 1 << 4,

    /// <summary>
    /// Unused flag 6.
    /// </summary>
    Unused6 = 1 << 5,

    /// <summary>
    /// Unused flag 7.
    /// </summary>
    Unused7 = 1 << 6,

    /// <summary>
    /// Unused flag 8.
    /// </summary>
    Unused8 = 1 << 7,

    /// <summary>
    /// Unused flag 9.
    /// </summary>
    Unused9 = 1 << 8,

    /// <summary>
    /// Unused flag 10.
    /// </summary>
    Unused10 = 1 << 9,

    /// <summary>
    /// Unused flag 11.
    /// </summary>
    Unused11 = 1 << 10,

    /// <summary>
    /// Unused flag 12.
    /// </summary>
    Unused12 = 1 << 11,

    /// <summary>
    /// Unused flag 13.
    /// </summary>
    Unused13 = 1 << 12,

    /// <summary>
    /// Unused flag 14.
    /// </summary>
    Unused14 = 1 << 13,

    /// <summary>
    /// Indicates that the item changes on install.
    /// </summary>
    ChangeOnInstall = 1 << 14,

    /// <summary>
    /// Indicates that the item changes on remove.
    /// </summary>
    ChangeOnRemove = 1 << 15
}
