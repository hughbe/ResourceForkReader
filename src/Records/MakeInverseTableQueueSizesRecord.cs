using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Make Inverse Table Queue Sizes Record ('mitq') in a resource fork.
/// </summary>
public readonly struct MakeInverseTableQueueSizesRecord
{
    /// <summary>
    /// The size of a Make Inverse Table Queue Sizes Record in bytes.
    /// </summary>
    public const int Size = 12;
    
    /// <summary>
    /// Gets the queue size for 3-bit inverse tables.
    /// </summary>
    public uint QueueSize3Bit { get; }

    /// <summary>
    /// Gets the queue size for 4-bit inverse tables.
    /// </summary>
    public uint QueueSize4Bit { get; }

    /// <summary>
    /// Gets the queue size for 5-bit inverse tables.
    /// </summary>
    public uint QueueSize5Bit { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MakeInverseTableQueueSizesRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 12 bytes of Make Inverse Table Queue Sizes Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 12 bytes long.</exception>
    public MakeInverseTableQueueSizesRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L922-L926
        int offset = 0;

        QueueSize3Bit = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        QueueSize4Bit = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        QueueSize5Bit = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for Make Inverse Table Queue Sizes Record.");
    }
}
