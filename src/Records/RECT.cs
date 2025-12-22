using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a rectangle structure in a resource fork.
/// </summary>
public struct RECT
{
    /// <summary>
    /// The size of a RECT structure in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the top coordinate.
    /// </summary>
    public ushort Top { get; }

    /// <summary>
    /// Gets the bottom coordinate.
    /// </summary>
    public ushort Left { get; }

    /// <summary>
    /// Gets the bottom coordinate.
    /// </summary>
    public ushort Bottom { get; }

    /// <summary>
    /// Gets the right coordinate.
    /// </summary>
    public ushort Right { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RECT"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 8 bytes of RECT data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 8 bytes long.</exception>
    public RECT(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        Top = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Left = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Bottom = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Right = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not parse the entire RECT data.");
    }
}
