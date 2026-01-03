using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an ISO9660 Volume Descriptor Record ('NRVD') in a resource fork.
/// </summary>
public readonly struct ISO9660VolumeDescriptorRecord
{
    /// <summary>
    /// Gets the size of the ISO9660 Volume Descriptor Record in bytes.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Gets the volume flags of the ISO9660 Volume Descriptor Record.
    /// </summary>
    public ushort VolumeFlags { get; }

    /// <summary>
    /// Gets the escape sequences of the ISO9660 Volume Descriptor Record.
    /// </summary>
    public uint EscapeSequences { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ISO9660VolumeDescriptorRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the ISO9660 Volume Descriptor Record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the required size.</exception>
    public ISO9660VolumeDescriptorRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < Size)
        {
            throw new ArgumentException($"Data length must be at least {Size} bytes.", nameof(data));
        }

        // Structure partially documented in https://vintageapple.org/develop/pdf/develop-03_9007_July_1990.pdf
        // 275
        int offset = 0;

        VolumeFlags = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        EscapeSequences = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset <= data.Length, "Did not consume all data for ISO9660 Volume Descriptor Record.");
    }
}
