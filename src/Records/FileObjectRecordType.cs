namespace ResourceForkReader.Records;

/// <summary>
/// Represents the type of a file object ('FOBJ') in a resource fork.
/// </summary>
public enum FileObjectRecordType : short
{
    /// <summary>
    /// Folder file object.
    /// </summary>
    Folder = 8,

    /// <summary>
    /// Disk file object.
    /// </summary>
    Disk = 4
}
