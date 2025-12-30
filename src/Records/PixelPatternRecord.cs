using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Pixel Pattern Record ('ppat') in a resource fork.
/// </summary>
public readonly struct PixelPatternRecord
{
    /// <summary>
    /// The minimum size of a Pixel Pattern Record in bytes.
    /// </summary>
    public const int MinSize = 78;

    /// <summary>
    /// Gets the pixel pattern.
    /// </summary>
    public PixelPattern Pattern { get; }

    /// <summary>
    /// Gets the pixel map.
    /// </summary>
    public PixelMap PixelMap { get; }

    /// <summary>
    /// Gets the pixel map image data.
    /// </summary>
    public byte[] PixelMapData { get; }

    /// <summary>
    /// Gets the color table.
    /// </summary>
    public ColorTable ColorTable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PixelPatternRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 78 bytes of Pixel Pattern Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 78 bytes long.</exception>
    public PixelPatternRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Pixel Pattern Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        int offset = 0;

        //  A pattern record. This is similar to the PixPat record (described
        // on page 4-58), except that the resource contains an offset (rather
        // than a handle) to a PixMap record (which is included in the
        // resource), and it contains an offset (rather than a handle) to the
        // pattern image data (which is also included in the resource).
        Pattern = new PixelPattern(data.Slice(offset, PixelPattern.Size));
        offset += PixelPattern.Size;

        // A pixel map. This is similar to the PixMap record (described on
        // page 4-46), except that the resource contains an offset (rather
        // than a handle) to a color table (which is included in the resource).
        PixelMap = new PixelMap(data.Slice(offset, PixelMap.Size));
        offset += PixelMap.Size;

        // Pattern image data. The size of the image data is calculated by
        // subtracting the offset to the image data from the offset to the
        // color table data.
        int pixelMapDataSize = PixelMap.DataSize;
        PixelMapData = data.Slice(offset, pixelMapDataSize).ToArray();
        offset += pixelMapDataSize;

        // A color table. This follows the same format as the color table
        // ('clut') resource described next.
        ColorTable = new ColorTable(data[offset..], out var bytesRead);
        offset += bytesRead;

        Debug.Assert(offset == data.Length, "Did not consume all data for Pixel Pattern Record.");
    }
}
