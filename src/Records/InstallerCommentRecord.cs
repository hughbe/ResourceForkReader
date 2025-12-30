using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Comment Record ('icmt') in a resource fork.
/// </summary>
public readonly struct InstallerCommentRecord
{
    /// <summary>
    /// The minimum size of an Installer Comment Record in bytes.
    /// </summary>
    public const int MinSize = 11;

    /// <summary>
    /// Gets the creation date.
    /// </summary>
    public DateTime ReleaseDate { get; }

    /// <summary>
    /// Gets the version number.
    /// </summary>
    public uint Version { get; }

    /// <summary>
    /// Gets the icon resource ID.
    /// </summary>
    public short IconResourceID { get; }

    /// <summary>
    /// Gets the comment string.
    /// </summary>
    public string Comment { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerCommentRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 11 bytes of Installer Comment Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 11 bytes long.</exception>
    public InstallerCommentRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in http://www.bitsavers.org/pdf/apple/mac/blue/Apple_Blue_Book_vol2_1989.pdf
        // page 17 to 18
        // and https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L528-L533
        int offset = 0;

        ReleaseDate = SpanUtilities.ReadMacOSTimestamp(data.Slice(offset, 4));
        offset += 4;

        Version = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        IconResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Comment = SpanUtilities.ReadPascalString(data[offset..]);
        offset += 1 + Comment.Length;

        if (offset % 2 != 0)
        {
            offset += 1; // Padding byte to align to even size
        }

        Debug.Assert(offset == data.Length, "Did not consume all data for InstallerCommentRecord.");
    }
}
