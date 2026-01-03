using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Sound Command in a sound resource ('snd ').
/// </summary>
public readonly struct SoundCommand
{
    /// <summary>
    /// The size of a Sound Command in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the command type.
    /// </summary>
    public ushort CommandType { get; }

    /// <summary>
    /// Gets the first parameter.
    /// </summary>
    public ushort Param1 { get; }

    /// <summary>
    /// Gets the second parameter.
    /// </summary>
    public uint Param2 { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundCommand"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 8 bytes of Sound Command data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is not exactly 8 bytes long.</exception>
    public SoundCommand(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long to be a valid Sound Command.", nameof(data));
        }

        int offset = 0;

        CommandType = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Param1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Param2 = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == data.Length, "Did not parse exactly 8 bytes for Sound Command.");
    }
}
