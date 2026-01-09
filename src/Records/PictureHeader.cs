using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents the header of a picture resource ('PICT').
/// </summary>
public readonly struct PictureHeader
{
    /// <summary>
    /// The size of the PictureHeader structure in bytes.
    /// </summary>
    public const int Size = 10;

    /// <summary>
    /// Gets the size of the picture data in bytes.
    /// </summary>
    public ushort DataSize { get; }

    /// <summary>
    /// Gets the bounding rectangle of the picture.
    /// </summary>
    public RECT Bounds { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PictureHeader"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the picture header data.</param>
    /// <exception cref="ArgumentException">Thrown if the provided data is not the correct size.</exception>
    public PictureHeader(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/ImagingWithQuickDraw.pdf.
        // 7-68
        int offset = 0;

        DataSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Bounds = new RECT(data.Slice(offset, RECT.Size));
        offset += RECT.Size;

        Debug.Assert(offset == data.Length, "Did not consume all bytes in PictureHeader.");
    }
}
