using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// The 'MACS' resource type for system version resources.
/// </summary>
public readonly struct SystemVersionRecord
{
    /// <summary>
    /// The minimum size of a video card record in bytes.
    /// </summary>
    public const int MinSize = 1;

    /// <summary>
    /// Gets the version of the system.
    /// </summary>
    public string? Version { get; }

    /// <summary>
    /// Gets the raw data of the system version record.
    /// </summary>
    public byte[]? RawData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SystemVersionRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the system version record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to contain the system version record.</exception>
    public SystemVersionRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/resource_dasm.cc#L2240
        int offset = 0;

        byte versionLength = data[offset];
        if (versionLength + 1 != data.Length)
        {
            Version = null;
            RawData = data.ToArray();
            offset += data.Length;
        }
        else
        {
            Version = SpanUtilities.ReadPascalString(data, out var versionBytesRead);
            RawData = null;
            offset += versionBytesRead;
        }

        Debug.Assert(offset == data.Length, "Did not consume all bytes for SystemVersionRecord.");
    }
}
