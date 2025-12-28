using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Color Table Entry in a Color Lookup Table Record ('clut').
/// </summary>
public struct ColorTableEntry
{
    /// <summary>
    /// Gets the size of a Color Table Entry in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the color table entry number.
    /// </summary>
    public ushort Number { get; }

    /// <summary>
    /// Gets the red component of the color.
    /// </summary>
    public ushort Red { get; }

    /// <summary>
    /// Gets the green component of the color.
    /// </summary>
    public ushort Green { get; }

    /// <summary>
    /// Gets the blue component of the color.
    /// </summary>
    public ushort Blue { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorTableEntry"/> struct.
    /// </summary>
    /// <param name="data">A span containing the color table entry data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid.</exception>
    public ColorTableEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes to be a valid Color Table Entry.", nameof(data));
        }

        int offset = 0;

        Number = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Red = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Green = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Blue = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == Size, "Did not consume all data for Color Table Entry.");
    }
}
