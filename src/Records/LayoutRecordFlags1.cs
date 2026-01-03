namespace ResourceForkReader.Records;

/// <summary>
/// Flags for the LayoutRecord.
/// </summary>
[Flags]
public enum LayoutRecordFlags1 : byte
{
    /// <summary>
    /// Indicates whether zoom rectangles are used.
    /// </summary>
    UseZoomRectangles = 1 << 7,

    /// <summary>
    /// Indicates whether to skip trash warning.
    /// </summary>
    SkipTrashWarning = 1 << 6,

    /// <summary>
    /// Indicates whether grid drags are always enabled.
    /// </summary>
    AlwaysGridDrags = 1 << 5,

    /// <summary>
    /// Unused flag 4.
    /// </summary>
    Unused4 = 1 << 4,

    /// <summary>
    /// Unused flag 3.
    /// </summary>
    Unused3 = 1 << 3,

    /// <summary>
    /// Unused flag 2.
    /// </summary>
    Unused2 = 1 << 2,

    /// <summary>
    /// Unused flag 1.
    /// </summary>
    Unused1 = 1 << 1,

    /// <summary>
    /// Unused flag 0.
    /// </summary>
    Unused0 = 1 << 0,
}
