using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Pixel Map structure.
/// </summary>
public readonly struct PixelMap
{
    /// <summary>
    /// The size of a Pixel Map in bytes.
    /// </summary>
    public const int Size = 50;

    /// <summary>
    /// Gets the base address of the pixel map.
    /// </summary>
    public uint BaseAddress { get; }

    /// <summary>
    /// Gets the flags and row bytes.
    /// </summary>
    public ushort FlagsRowBytes { get; }

    /// <summary>
    /// Gets the bounds of the pixel map.
    /// </summary>
    public RECT Bounds { get; }

    /// <summary>
    /// Gets the version.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// Gets the pack type.
    /// </summary>
    public ushort PackType { get; }

    /// <summary>
    /// Gets the pack size.
    /// </summary>
    public uint PackSize { get; }

    /// <summary>
    /// Gets the horizontal resolution.
    /// </summary>
    public uint HorizontalResolution { get; }

    /// <summary>
    /// Gets the vertical resolution.
    /// </summary>
    public uint VerticalResolution { get; }

    /// <summary>
    /// Gets the pixel type.
    /// </summary>
    public ushort PixelType { get; }

    /// <summary>
    /// Gets the pixel size.
    /// </summary>
    public ushort PixelSize { get; }

    /// <summary>
    /// Gets the component count.
    /// </summary>
    public ushort ComponentCount { get; }

    /// <summary>
    /// Gets the component size.
    /// </summary>
    public ushort ComponentSize { get; }

    /// <summary>
    /// Gets the plane offset.
    /// </summary>
    public uint PlaneOffset { get; }

    /// <summary>
    /// Gets the color table offset.
    /// </summary>
    public uint ColorTableOffset { get; }

    /// <summary>
    /// Gets the reserved field.
    /// </summary>
    public uint Reserved { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PixelMap"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the pixel map data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid.</exception>
    public PixelMap(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes to be a valid Pixel Map.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/ImagingWithQuickDraw.pdf
        // 4-9 to 4-12
        int offset = 0;

        // Pointer to the image data
        BaseAddress = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Flags, and bytes in a row
        FlagsRowBytes = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Boundary rectangle
        Bounds = new RECT(data.Slice(offset, RECT.Size));
        offset += RECT.Size;

        // Pixel map version number
        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Packing format
        PackType = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Size of data in packed state
        PackSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Horizontal resolution in dots per inch
        HorizontalResolution = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Vertical resolution in dots per inch
        VerticalResolution = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Format of pixel image
        PixelType = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Physical bits per pixel
        PixelSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (PixelSize != 1 && PixelSize != 2 && PixelSize != 4 && PixelSize != 8)
        {
            throw new ArgumentException($"Invalid Pixel Size {PixelSize} in Pixel Map. Must be 1, 2, 4, or 8.", nameof(data));
        }

        // Number of components in each pixel
        ComponentCount = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;
        
        // Number of bits in each component
        ComponentSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Offset to next plane
        PlaneOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Handle to a color table for this image
        ColorTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Reserved
        Reserved = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == Size, "Did not read the expected number of bytes for Pixel Map.");
    }

    /// <summary>
    /// Gets the size of the bitmap data in bytes.
    /// </summary>
    public int DataSize
    {
        get
        {
            int rowBytes = FlagsRowBytes & 0x3FFF; // Mask off the high bit which is used for something else
            return rowBytes * Bounds.Height;
        }
    }
}