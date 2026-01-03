using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a version resource ('vers') containing version information.
/// </summary>
public struct VersionRecord
{
    /// <summary>
    /// The minimum size of a version record in bytes.
    /// </summary>
    public const int MinSize = 6;

    /// <summary>
    /// Gets the major version byte.
    /// </summary>
    public byte Major { get; }

    /// <summary>
    /// Gets the minor version byte.
    /// </summary>
    public byte Minor { get; }

    /// <summary>
    /// Gets the development stage.
    /// </summary>
    public DevelopmentStage DevelopmentStage { get; }

    /// <summary>
    /// Gets the pre-release version level byte.
    /// </summary>
    public byte PreReleaseVersionLevel { get; }

    /// <summary>
    /// Gets the region code.
    /// </summary>
    public ushort RegionCode { get; }

    /// <summary>
    /// Gets the version number as a string.
    /// </summary>
    public string VersionNumber { get; }

    /// <summary>
    /// Gets the version message.
    /// </summary>
    public string VersionMessage { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 4 bytes of version data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short.</exception>
    public VersionRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 7-69 to 7-70
        int offset = 0;

        // Major revision level in binary-coded decimal format.
        Major = data[offset];
        offset += 1;

        // Minor revision level in binary-coded decimal format.
        Minor = data[offset];
        offset += 1;

        // Development stage. The values that can appear in this field, as well
        // as the constants that can be used to specify them in a Rez input file,
        // are the following:
        // Value Constant Description
        // 0x20 development Prealpha file
        // 0x40 alpha Alpha file
        // 0x60 beta Beta file
        // 0x80 release Released file
        DevelopmentStage = (DevelopmentStage)data[offset];
        offset += 1;

        // Prerelease revision level. This number specifies the version if the
        // software is still prerelease.
        PreReleaseVersionLevel = data[offset];
        offset += 1;

        // Region code. This identifies the script system for which this version of
        // the software is intended. See the chapter “Script Manager” in Inside
        // Macintosh: Text for information about the values represented by the various region codes that can be specified here.
        RegionCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Version number. This Pascal string identifies the version number of
        // the software. When the user opens the Views control panel, clicks the
        // Show version box, and then chooses any command from the View menu
        // other than by Icon or by Small Icon, the Finder window containing
        // this application displays this string.
        VersionNumber = SpanUtilities.ReadPascalString(data[offset..], out int versionNumberLength);
        offset += versionNumberLength;

        // Version message. This Pascal string identifies the version number and
        // either a company copyright for a file or a product name for a superset
        // of files. When the user selects this file and chooses the Get Info
        // command, the Finder displays this string in the information window
        // as follows:
        // ■ For a version resource with a resource ID number of 1, this string
        // is displayed in the version field of the information window.
        // ■ For a version resource with a resource ID number of 2, this string
        // is displayed beneath the file’s name next to the file’s icon at the
        // top of the information window.
        VersionMessage = SpanUtilities.ReadPascalString(data[offset..], out int versionMessageLength);
        offset += versionMessageLength;

        Debug.Assert(offset <= data.Length);
    }
}
