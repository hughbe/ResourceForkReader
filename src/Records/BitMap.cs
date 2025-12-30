using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Bit Map structure.
/// </summary>
public readonly struct BitMap
{
    /// <summary>
    /// The size of a Bit Map in bytes.
    /// </summary>
    public const int Size = 14;

    /// <summary>
    /// Gets the base address of the bit map.
    /// </summary>
    public uint BaseAddress { get; }

    /// <summary>
    /// Gets the row bytes.
    /// </summary>
    public ushort RowBytes { get; }

    /// <summary>
    /// Gets the boundary rectangle.
    /// </summary>
    public RECT Bounds { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitMap"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 14 bytes of Bit Map data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is not exactly 14 bytes long.</exception>
    public BitMap(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/ImagingWithQuickDraw.pdf
        // 2-29 to 2-30
        int offset = 0;

        // A pointer to the beginning of the bit image.
        BaseAddress = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The offset in bytes from one row of the image to the next.
        // The value of the rowBytes field must be less than $4000.
        RowBytes = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (RowBytes >= 0x4000)
        {
            throw new ArgumentException("Invalid BitMap data: RowBytes must be less than $4000.", nameof(data));
        }

        // The bitmap’s boundary rectangle; by default, the entire main screen.
        Bounds = new RECT(data.Slice(offset, RECT.Size));
        offset += RECT.Size;
        
        Debug.Assert(offset == data.Length, "Did not consume all data for BitMap.");
    }

    /// <summary>
    /// Gets the size of the bitmap data in bytes.
    /// </summary>
    public int DataSize
    {
        get
        {
            int rowBytes = RowBytes & 0x3FFF; // Mask off the high bit which is used for something else
            return rowBytes * Bounds.Height;
        }
    }
}
