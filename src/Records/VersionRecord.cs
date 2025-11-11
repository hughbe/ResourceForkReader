using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a version resource ('VERS') containing major and minor version information.
/// </summary>
public struct VersionRecord
{
    /// <summary>
    /// The size of a version record in bytes.
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Gets the major version string (2 characters).
    /// </summary>
    public string Major { get; }
    
    /// <summary>
    /// Gets the minor version string (2 characters).
    /// </summary>
    public string Minor { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 4 bytes of version data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short.</exception>
    public VersionRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < Size)
        {
            throw new ArgumentException($"Data must be at least {Size} bytes long.", nameof(data));
        }

        int offset = 0;
        Major = SpanUtilities.ReadString(data, offset, 2);
        offset += 2;

        Minor = SpanUtilities.ReadString(data, offset, 2);
        offset += 2;

        Debug.Assert(offset == Size);
    }
}
