using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents the target volume requirements in an Installer Default Map Record ('IDFM') in a resource fork.
/// </summary>
public readonly struct InstallerDefaultMapTargetVolumeRequirements
{
    /// <summary>
    /// The size of Installer Default Map Target Volume Requirements in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the minimal target size in bytes.
    /// </summary>
    public uint MinimalTargetSize { get; }
    
    /// <summary>
    /// Gets the maximal target size in bytes.
    /// </summary>
    public uint MaximalTargetSize { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerDefaultMapTargetVolumeRequirements"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">>A span containing exactly 8 bytes of Installer Default Map Target Volume Requirements data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is not exactly 8 bytes long.</exception>
    public InstallerDefaultMapTargetVolumeRequirements(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        MinimalTargetSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        MaximalTargetSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == data.Length, "Did not consume all data for Installer Default Map Target Volume Requirements.");
    }
}
