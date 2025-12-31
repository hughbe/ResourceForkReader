using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer File Atom Record ('infa') in a resource fork.
/// </summary>
public readonly struct InstallerFileAtomRecord
{
    /// <summary>
    /// The minimum size of an Installer File Atom Record in bytes.
    /// </summary>
    public const int MinSize = 13;

    /// <summary>
    /// Gets the format version.
    /// </summary>
    public ushort FormatVersion { get; }

    /// <summary>
    /// Gets the installer file atom flags.
    /// </summary>
    public InstallerFileAtomFlags Flags { get; }

    /// <summary>
    /// Gets the resource ID of the target file spec.
    /// </summary>
    public short TargetFileSpecResourceID { get; }

    /// <summary>
    /// Gets the resource ID of the source file spec.
    /// </summary>
    public short SourceFileSpecResourceID { get; }

    /// <summary>
    /// Gets the file size.
    /// </summary>
    public uint FileSize { get; }

    /// <summary>
    /// Gets the file description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerFileAtomRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 13 bytes of Installer File Atom Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 13 bytes long.</exception>
    public InstallerFileAtomRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in http://www.bitsavers.org/pdf/apple/mac/blue/Apple_Blue_Book_vol2_1989.pdf
        // page 19 to 22
        int offset = 0;

        FormatVersion = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Flags = (InstallerFileAtomFlags)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TargetFileSpecResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        SourceFileSpecResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        FileSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;
        
        Description = SpanUtilities.ReadPascalString(data[offset..], out var descriptionBytesRead);
        offset += descriptionBytesRead;

        if (offset % 2 != 0)
        {
            offset += 1; // Padding byte to align to even size
        }

        Debug.Assert(offset == data.Length, "Did not consume all data for InstallerFileAtomRecord.");
    }
}
