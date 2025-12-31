using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Resource Atom Record ('inra') in a resource fork.
/// </summary>
public readonly struct InstallerResourceAtomRecord
{
    /// <summary>
    /// The minimum size of an Installer Resource Atom Record in bytes.
    /// </summary>
    public const int MinSize = 22;

    /// <summary>
    /// Gets the format version.
    /// </summary>
    public ushort FormatVersion { get; }

    /// <summary>
    /// Gets the installer file atom flags.
    /// </summary>
    public InstallerResourceAtomFlags Flags { get; }

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
    public string ResourceType { get; }

    /// <summary>
    /// Gets the resource ID of the source resource.
    /// </summary>
    public short SourceResourceID { get; }

    /// <summary>
    /// Gets the resource ID of the target resource.
    /// </summary>
    public short TargetResourceID { get; }

    /// <summary>
    /// Gets the size of the resource.
    /// </summary>
    public uint ResourceSize { get; }

    /// <summary>
    /// Gets the file description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the name of the resource.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerResourceAtomRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 22 bytes of Installer Resource Atom Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 22 bytes long.</exception>
    public InstallerResourceAtomRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in http://www.bitsavers.org/pdf/apple/mac/blue/Apple_Blue_Book_vol2_1989.pdf
        // page 22 to 25
        // In https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L623-L653
        // ResourceSize is incorrectly read as a UInt16 instead of UInt32.
        int offset = 0;

        FormatVersion = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Flags = (InstallerResourceAtomFlags)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TargetFileSpecResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        SourceFileSpecResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ResourceType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        SourceResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TargetResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));;
        offset += 2;

        ResourceSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Description = SpanUtilities.ReadPascalString(data[offset..], out var descriptionBytesRead);
        offset += descriptionBytesRead;

        if (offset % 2 != 0)
        {
            offset += 1; // Padding byte to align to even size
        }

        Name = SpanUtilities.ReadPascalString(data[offset..], out var nameBytesRead);
        offset += nameBytesRead;

        if (offset % 2 != 0)
        {
            offset += 1; // Padding byte to align to even size
        }

        Debug.Assert(offset == data.Length, "Did not consume all data for InstallerResourceAtomRecord.");
    }
}
