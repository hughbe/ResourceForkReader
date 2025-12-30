using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer File Spec Record ('infs') in a resource fork.
/// </summary>
public readonly struct InstallerFileSpecRecord
{
    /// <summary>
    /// The minimum size of an Installer File Spec Record in bytes.
    /// </summary>
    public const int MinSize = 16;

    /// <summary>
    /// Gets the file type.
    /// </summary>
    public string FileType { get; }

    /// <summary>
    /// Gets the file creator.
    /// </summary>
    public string FileCreator { get; }

    /// <summary>
    /// Gets the file name.
    /// </summary>
    public DateTime CreationDate { get; }

    /// <summary>
    /// Gets the file flags.
    /// </summary>
    public InstallerFileSpecRecordFlags Flags { get; }

    /// <summary>
    /// Gets the file name.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerFileSpecRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Installer File Spec Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 14 bytes long.</exception>
    public InstallerFileSpecRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Installer File Spec Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L585-L601
        int offset = 0;

        FileType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        FileCreator = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        CreationDate = SpanUtilities.ReadMacOSTimestamp(data.Slice(offset, 4));
        offset += 4;

        Flags = (InstallerFileSpecRecordFlags)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        FileName = SpanUtilities.ReadPascalString(data[offset..]);
        offset += 1 + FileName.Length;

        if (offset < data.Length && offset % 2 != 0)
        {
            offset += 1; // Padding byte to align to even size
        }

        Debug.Assert(offset == data.Length, "Did not consume all data for InstallerFileSpecRecord.");
    }

    /// <summary>
    /// Gets the file specification flags.
    /// </summary>
    [Flags]
    public enum InstallerFileSpecRecordFlags : ushort
    {
        /// <summary>
        /// No flags set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Search for file.
        /// </summary>
        SearchForFile = 1 << 0,

        /// <summary>
        /// Type and creator must match.
        /// </summary>
        TypeAndCreatorMustMatch = 1 << 1,
    }
}
