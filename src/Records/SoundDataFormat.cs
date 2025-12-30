using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a sound data format within a sound resource.
/// </summary>
public readonly struct SoundDataFormat
{
    /// <summary>
    /// The size of the SoundDataFormat structure in bytes.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Gets the sound data format ID.
    /// </summary>
    public ushort ID { get; }
    
    /// <summary>
    /// Gets the initialization options.
    /// </summary>
    public uint InitializationOptions { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundDataFormat"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 6 bytes of SoundDataFormat data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is not exactly 6 bytes long.</exception>
    public SoundDataFormat(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length must be exactly {Size} bytes to be a valid SoundDataFormat.", nameof(data));
        }

        int offset = 0;

        ID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        InitializationOptions = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == data.Length, "Parsed beyond the end of the SoundDataFormat data.");
    }
}
