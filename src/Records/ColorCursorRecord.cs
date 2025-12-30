using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a color cursor resource ('crsr').
/// </summary>
public struct ColorCursorRecord
{
    /// <summary>
    /// The minimum size of a Color Cursor Record in bytes.
    /// </summary>
    public const int MinSize = 140;

    /// <summary>
    /// Gets the cursor type.
    /// </summary>
    public CursorType Type { get; }

    /// <summary>
    /// Gets the offset to the pixel map data.
    /// </summary>
    public uint PixelMapOffset { get; }

    /// <summary>
    /// Gets the offset to the pixel data.
    /// </summary>
    public uint PixelDataOffset { get; }

    /// <summary>
    /// Gets the expanded cursor data.
    /// </summary>
    public uint ExpandedCursorData { get; }

    /// <summary>
    /// Gets the expanded data depth.
    /// </summary>
    public ushort ExpandedDataDepth { get; }

    /// <summary>
    /// Gets the reserved field.
    /// </summary>
    public uint Reserved { get; }

    /// <summary>
    /// Gets the cursor data.
    /// </summary>
    public byte[] CursorData { get; }

    /// <summary>
    /// Gets the cursor mask.
    /// </summary>
    public byte[] CursorMask { get; }

    /// <summary>
    /// Gets the hot spot Y coordinate.
    /// </summary>
    public ushort HotSpotY { get; }

    /// <summary>
    /// Gets the hot spot X coordinate.
    /// </summary>
    public ushort HotSpotX { get; }

    /// <summary>
    /// Gets the color table offset.
    /// </summary>
    public uint ColorTableOffset { get; }

    /// <summary>
    /// Gets the pixel map.
    /// </summary>
    public PixelMap PixelMap { get; }

    /// <summary>
    /// Gets the bounds of the cursor.
    /// </summary>
    public RECT Bounds { get; }

    /// <summary>
    /// Gets the pixel size.
    /// </summary>
    public byte[] PixelSize { get; }

    /// <summary>
    /// Gets the color table.
    /// </summary>
    public ColorTable ColorTable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorCursorRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 140 bytes of Color Cursor Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 140 bytes long.</exception>
    public ColorCursorRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long to be a valid Color Cursor Record.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Imaging_With_QuickDraw/Cursor_Utilities.pdf
        // 8-34 to 8-36
        int offset = 0;

        // Type of cursor. A value of $8001 identifies this as a color cursor.
        // A value of $8000 identifies this as a black-and-white cursor. 
        Type = (CursorType)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Offset to PixMap record. This offset is from the beginning of the resource data
        PixelMapOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Offset to pixel data. This offset is from the beginning of the resource data. 
        PixelDataOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        //  Expanded cursor data. This expanded pixel image is used internally
        // by Color QuickDraw. 
        ExpandedCursorData = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Expanded data depth. This is the pixel depth of the expanded cursor image.
        ExpandedDataDepth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Reserved. The Resource Manager uses this element for storage
        Reserved = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Cursor data. This field contains a 16-by-16 pixel 1-bit image to be
        // displayed when the cursor is on 1-bit or 2-bit screens. 
        CursorData = data.Slice(offset, 32).ToArray();
        offset += 32;

        // Cursor mask. A bitmap for the cursor’s mask. QuickDraw uses the
        // mask to crop the cursor’s outline into a background color or
        // pattern. QuickDraw then draws the cursor into this shape.
        CursorMask = data.Slice(offset, 32).ToArray();
        offset += 32;

        // Hot spot. The cursor’s hot spot.
        HotSpotY = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        HotSpotX = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));;
        offset += 2;

        //  Table ID. This contains an offset to the color table data from
        // the beginning of the resource data.
        ColorTableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Pixel map. This pixel map describes the image when drawing the
        // color cursor. The pixel map contains an offset to the color table
        // data from the beginning of the resource.
        offset = (int)PixelMapOffset;
        PixelMap = new PixelMap(data.Slice(offset, PixelMap.Size));
        offset += PixelMap.Size;

        // Bounds. The boundary rectangle of the cursor.
        Bounds = new RECT(data.Slice(offset, RECT.Size));
        offset += RECT.Size;
        
        // Pixel size. The number of pixels per bit in the cursor
        PixelSize = data.Slice(offset, 18).ToArray();
        offset += 18;

        // Pixel data. The data for the cursor.
        offset = (int)PixelDataOffset;
        int pixelDataSize = PixelMap.DataSize;
        CursorData = data.Slice(offset, pixelDataSize).ToArray();
        offset += pixelDataSize;

        // Color table. A color table containing the color information for the
        // cursor’s pixel map. 
        offset = (int)ColorTableOffset;
        ColorTable = new ColorTable(data[offset..], out var bytesRead);
        offset += bytesRead;

        Debug.Assert(offset <= data.Length, "Did not consume all data for Color Cursor Record.");
    }
}
