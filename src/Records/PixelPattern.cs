using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Pixel Pattern (PixPat) structure.
/// </summary>
public readonly struct PixelPattern
{
    /// <summary>
    /// The size of a Pixel Pattern in bytes.
    /// </summary>
    public const int Size = 28;

    /// <summary>
    /// Gets the type of pixel pattern.
    /// </summary>
    public PixelPatternType Type { get; }

    /// <summary>
    /// Gets the offset to the PixMap record.
    /// </summary>
    public uint PixelMapOffset { get; }

    /// <summary>
    /// Gets the offset to the pixel data.
    /// </summary>
    public uint PixelDataOffset { get; } 

    /// <summary>
    /// Gets the offset to the expanded pixel data.
    /// </summary>
    public uint ExpandedPixelDataOffset { get; }

    /// <summary>
    /// Gets the expanded pixel data valid flag.
    /// </summary>
    public ushort ExpandedPixelDataValidFlag { get; }

    /// <summary>
    /// Gets the expanded pattern.
    /// </summary>
    public uint ExpandedPattern { get; }

    /// <summary>
    /// Gets the monochrome pattern.
    /// </summary>
    public ulong MonochromePattern { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PixelPatternRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 28 bytes of Pixel Pattern data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 28 bytes long.</exception>
    public PixelPattern(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/ImagingWithQuickDraw.pdf
        // 4-103
        int offset = 0;

        // The pattern’s type. The value 0 specifies a basic QuickDraw bit
        // pattern, the value 1 specifies a full-color pixel pattern, and
        // the value 2 specifies an RGB pattern. These pattern types are
        // described in greater detail in the rest of this section.
        Type = (PixelPatternType)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // A handle to a PixMap record (described on page 4-46) that describes
        // the pattern’s pixel image. The PixMap record can contain indexed
        // or direct pixels.
        PixelMapOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // A handle to the pattern’s pixel image. 
        PixelDataOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // A handle to an expanded pixel image used internally by Color QuickDraw.
        ExpandedPixelDataOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // A flag that, when set to –1, invalidates the expanded data. 
        ExpandedPixelDataValidFlag = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Reserved for use by Color QuickDraw. 
        ExpandedPattern = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // A bit pattern (described in the chapter “QuickDraw Drawing”) to
        // be used when this pattern is drawn into a GrafPort record
        // (described in the chapter “Basic QuickDraw”). The NewPixPat
        // function (described on page 4-88) sets this field to 50 percent
        // gray.
        MonochromePattern = BinaryPrimitives.ReadUInt64BigEndian(data.Slice(offset, 8));
        offset += 8;

        Debug.Assert(offset == Size, "Did not parse the entire Pixel Pattern data.");
    }
}
