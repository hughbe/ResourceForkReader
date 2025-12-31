using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Package Record ('inpr') in a resource fork.
/// </summary>
public readonly struct InstallerPackageRecord
{
    /// <summary>
    /// The minimum size of an Installer Package Record in bytes.
    /// </summary>
    public const int MinSize = 12;

    /// <summary>
    /// Gets the format version.
    /// </summary>
    public ushort FormatVersion { get; }

    /// <summary>
    /// Gets the installer package flags.
    /// </summary>
    public InstallerPackageFlags Flags { get; }

    /// <summary>
    /// Gets the resource ID of the installer comment.
    /// </summary>
    public short InstallerCommentResourceID { get; }

    /// <summary>
    /// Gets the size of the package.
    /// </summary>
    public uint PackageSize { get; }

    /// <summary>
    /// Gets the package name.
    /// </summary>
    public string PackageName { get; }

    /// <summary>
    /// Gets the number of parts.
    /// </summary>
    public ushort NumberOfParts { get; }

    /// <summary>
    /// Gets the list of installer package parts.
    /// </summary>
    public List<InstallerPackagePart> Parts { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerPackageRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 12 bytes of Installer Package Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 12 bytes long.</exception>
    public InstallerPackageRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in http://www.bitsavers.org/pdf/apple/mac/blue/Apple_Blue_Book_vol2_1989.pdf
        // page 15 to 17
        // and https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L602-L622
        int offset = 0;

        FormatVersion = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Flags = (InstallerPackageFlags)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        InstallerCommentResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        PackageSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        PackageName = SpanUtilities.ReadPascalString(data[offset..], out var packageNameBytesRead);
        offset += packageNameBytesRead;

        if (offset % 2 != 0)
        {
            offset += 1; // Padding byte to align to even size
        }

        NumberOfParts = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var parts = new List<InstallerPackagePart>(NumberOfParts);
        for (int i = 0; i < NumberOfParts; i++)
        {
            parts.Add(new InstallerPackagePart(data.Slice(offset, InstallerPackagePart.Size)));
            offset += InstallerPackagePart.Size;
        }

        Parts = parts;

        Debug.Assert(offset == data.Length, "Did not consume all data for Installer Package Record.");
    }
}
