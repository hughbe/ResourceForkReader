using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Macintosh Model in a resource fork.
/// </summary>
public readonly struct MacintoshModel
{
    /// <summary>
    /// Size of a Macintosh Model in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the model type.
    /// </summary>
    public string ModelType { get; }

    /// <summary>
    /// Gets the installation status.
    /// </summary>
    public MacintoshModelInstallationStatus InstallationStatus { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacintoshModel"/> struct.
    /// </summary>
    /// <param name="data">The data for the Macintosh Model.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not equal to the expected size.</exception>
    public MacintoshModel(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1078-L1087
        int offset = 0;

        ModelType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        InstallationStatus = (MacintoshModelInstallationStatus)BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == Size, "Did not consume all bytes for MacintoshModel.");
    }
}
