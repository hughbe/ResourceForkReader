using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Key Caps Shape Point Entry in a Key Caps Record ('KCAP') in a resource fork.
/// </summary>
public readonly struct KeyCapsShapePointEntry
{
    /// <summary>
    /// The size of a Key Caps Shape Point Entry in bytes.
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Gets the X coordinate.
    /// </summary>
    public ushort X { get; }

    /// <summary>
    /// Gets the Y coordinate.
    /// </summary>
    public ushort Y { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyCapsShapePointEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 4 bytes of Key Caps Shape Point Entry data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 4 bytes long.</exception>
    public KeyCapsShapePointEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // C-28 to C-34
        int offset = 0;

        // Shape array. A (zero-based) count of entries followed by one or more
        // entries. Each entry is a point, representing the relative pixel offset
        // from the origin of the key, that define a particular key shape.
        // The shape array is a single point for rectangular keys. More complex
        // keys, like the Return key, need two points in their shape array.
        X = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Y = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for Key Caps Shape Point Entry.");
    }
}
