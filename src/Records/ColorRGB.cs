using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an RGB color structure.
/// </summary>
public readonly struct ColorRGB
{
    /// <summary>
    /// The size of a ColorRGB structure in bytes.
    /// </summary>
    public const int Size = 6;

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
    /// Initializes a new instance of the <see cref="ColorRGB"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">The binary data representing the ColorRGB structure.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 6 bytes long.</exception>
    public ColorRGB(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        Red = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Green = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Blue = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for ColorRGB.");
    }
}
