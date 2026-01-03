namespace ResourceForkReader.Records;

/// <summary>
/// Flags2 for the LayoutRecord.
/// </summary>
[Flags]
public enum LayoutRecordFlags2 : byte
{
    /// <summary>
    /// Unused flag 7.
    /// </summary>
    Unused7 = 1 << 7,

    /// <summary>
    /// Unused flag 6.
    /// </summary>
    Unused6 = 1 << 6,

    /// <summary>
    /// Unused flag 5.
    /// </summary>
    Unused5 = 1 << 5,

    /// <summary>
    /// Unused flag 4.
    /// </summary>
    Unused4 = 1 << 4,

    /// <summary>
    /// Indicates whether to use physical icons.
    /// </summary>
    UsePhysicalIcon = 1 << 3,

    /// <summary>
    /// Indicates whether the title is clickable.
    /// </summary>
    TitleClickable = 1 << 2,

    /// <summary>
    /// Indicates whether to copy on inherit.
    /// </summary>
    CopyInherit = 1 << 1,

    /// <summary>
    /// Indicates whether to new fold inherit.
    /// </summary>
    NewFoldInherit = 1 << 0,
}
