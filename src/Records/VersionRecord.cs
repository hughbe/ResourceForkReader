using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a version resource ('VERS') containing major and minor version information.
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
    /// Gets the development stage byte.
    /// </summary>
    public byte DevelopmentStage { get; }

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

        int offset = 0;

        Major = data[offset];
        offset += 1;

        Minor = data[offset];
        offset += 1;

        DevelopmentStage = data[offset];
        offset += 1;

        PreReleaseVersionLevel = data[offset];
        offset += 1;

        RegionCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        VersionNumber = SpanUtilities.ReadPascalString(data[offset..], out int versionNumberLength);
        offset += versionNumberLength;

        VersionMessage = SpanUtilities.ReadPascalString(data[offset..], out int versionMessageLength);
        offset += versionMessageLength;

        Debug.Assert(offset <= data.Length);
    }
}
