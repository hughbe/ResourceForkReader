using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Machine Resource ('mach').
/// </summary>
public readonly struct MachineRecord
{
    /// <summary>
    /// The size of a MachineRecord in bytes.
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Gets the soft mask.
    /// </summary>
    public ushort SoftMask { get; }

    /// <summary>
    /// Gets the hard mask.
    /// </summary>
    public ushort HardMask { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MachineRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the machine record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 4 bytes long.</exception>
    public MachineRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 
        int offset = 0;

        // Soft mask. See Table 8-5 for a description of this mask.
        SoftMask = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Hard mask. See Table 8-5 for a description of this mask.
        HardMask = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == Size, "Did not consume all bytes for MachineRecord.");
    }
}
