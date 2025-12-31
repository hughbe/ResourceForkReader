using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a file comment ('FCMT') in a resource fork.
/// </summary>
public struct FileCommentRecord
{
    /// <summary>
    /// The minimum size of a valid FileCommentRecord in bytes.
    /// </summary>
    public const int MinSize = 1;

    /// <summary>
    /// Gets the comment string.
    /// </summary>
    public string Comment { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileCommentRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">The binary data of the file comment record.</param>
    public FileCommentRecord(ReadOnlySpan<byte> data)
    {
        int offset = 0;

        Comment = SpanUtilities.ReadPascalString(data, out var commentBytesRead);
        offset += commentBytesRead;

        Debug.Assert(offset == data.Length, "Did not consume all data for FileCommentRecord.");
    }
}
