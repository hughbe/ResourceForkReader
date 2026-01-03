using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a version resource ('VERS') containing major and minor version information.
/// </summary>
public struct VersionStringRecord
{
    /// <summary>
    /// The size of a version string record in bytes.
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Gets the version string.
    /// </summary>
    public string Major { get; }

    /// <summary>
    /// Gets the minor version string.
    /// </summary>
    public string Minor { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionStringRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 4 bytes of version data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short.</exception>
    public VersionStringRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure not documented; inferred from examples.
        int offset = 0;

        Major = Encoding.ASCII.GetString(data.Slice(offset, 2));
        offset += 2;

        Minor = Encoding.ASCII.GetString(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for VersionStringRecord.");
    }
}
