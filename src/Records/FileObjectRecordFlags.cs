namespace ResourceForkReader.Records;

/// <summary>
/// Represents flags for a file object record ('FOBJ') in a resource fork.
/// </summary>
public enum FileObjectRecordFlags : ushort
{
    /// <summary>
    /// The file object is locked.
    /// </summary>
    Locked = 0x8000
}
