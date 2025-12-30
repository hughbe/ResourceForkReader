using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a color icon resource ('cicn').
/// </summary>
public readonly struct ColorIconRecord
{
    /// <summary>
    /// The minimum size of a Color Icon Record in bytes.
    /// </summary>
    public const int MinSize = 82;

    /// <summary>
    /// Gets the icon pixel map data.
    /// </summary>
    public PixelMap PixelMap { get; }

    /// <summary>
    /// Gets the icon mask bitmap data.
    /// </summary>
    public BitMap MaskBitmap { get; }

    /// <summary>
    /// Gets the icon bitmap data.
    /// </summary>
    public BitMap IconBitmap { get; }

    /// <summary>
    /// Gets the icon data.
    /// </summary>
    public uint IconData { get; }

    /// <summary>
    /// Gets the bitmap image data for the bitmap to be used on 1-bit screens.
    /// </summary>
    public byte[] MaskBitmapData { get; }

    /// <summary>
    /// Gets the bitmap image data for the bitmap to be used on 1-bit screens.
    /// </summary>
    public byte[] IconBitmapData { get; }

    /// <summary>
    /// Gets the color table.
    /// </summary>
    public ColorTable ColorTable { get; }

    /// <summary>
    /// Gets the pixel map image data.
    /// </summary>
    public byte[] PixelMapData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorIconRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 128 bytes of icon data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 128 bytes long.</exception>
    public ColorIconRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Color Icon Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure partially documented in https://developer.apple.com/library/archive/documentation/mac/pdf/ImagingWithQuickDraw.pdf
        // 4-105 to 4-106
        int offset = 0;

        // A pixel map. This pixel map describes the image when drawing the
        // icon on a color screen.
        PixelMap = new PixelMap(data.Slice(offset, PixelMap.Size));
        offset += PixelMap.Size;

        // A bitmap for the icon’s mask. 
        MaskBitmap = new BitMap(data.Slice(offset, BitMap.Size));
        offset += BitMap.Size;

        // A bitmap for the icon. This contains the image to use when drawing
        // the icon to a 1-bit screen.
        IconBitmap = new BitMap(data.Slice(offset, BitMap.Size));
        offset += BitMap.Size;

        // Icon data.
        IconData = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The bitmap image data for the icon’s mask.
        int maskBitmapSize = MaskBitmap.DataSize;
        MaskBitmapData = data.Slice(offset, maskBitmapSize).ToArray();
        offset += maskBitmapSize;

        // The bitmap image data for the bitmap to be used on 1-bit screens. It may be NIL.
        int iconBitmapSize = IconBitmap.DataSize;
        IconBitmapData = data.Slice(offset, iconBitmapSize).ToArray();
        offset += iconBitmapSize;

        // A color table containing the color information for the icon’s pixel map.
        ColorTable = new ColorTable(data[offset..], out int bytesRead);
        offset += bytesRead;

        // The image data for the pixel map. 
        int pixelMapDataSize = PixelMap.DataSize;
        PixelMapData = data.Slice(offset, pixelMapDataSize).ToArray();
        offset += pixelMapDataSize;

        Debug.Assert(offset == data.Length, "Did not consume all data for Color Icon Record.");
    }
}