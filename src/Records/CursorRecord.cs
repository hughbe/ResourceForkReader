using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;
 
 /// <summary>
 /// Represents a cursor resource ('CURS').
 /// </summary>
public struct CursorRecord
{
    /// <summary>
    /// The size of a cursor record in bytes.
    /// </summary>
    public const int Size = 68;

    /// <summary>
    /// Gets the image data for the cursor.
    /// </summary>
    public byte[] ImageData { get; }

    /// <summary>
    /// Gets the mask data for the cursor.
    /// </summary>
    public byte[] MaskData { get; }

    /// <summary>
    /// Gets the hotspot Y coordinate.
    /// </summary>
    public ushort HotspotY { get; }

    /// <summary>
    /// Gets the hotspot Y coordinate.
    /// </summary>
    public ushort HotspotX { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CursorRecord"/> class by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 68 bytes of cursor data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 68 bytes long.</exception>
    public CursorRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        ImageData = data.Slice(offset, 32).ToArray();
        offset += 32;

        MaskData = data.Slice(offset, 32).ToArray();
        offset += 32;

        HotspotX = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        HotspotY = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == Size, "Did not consume all bytes for CursorRecord.");
    }
}