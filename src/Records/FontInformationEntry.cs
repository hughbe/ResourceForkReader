using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents am entry in a Font Information Record ('finf') in a classic Macintosh resource fork.
/// </summary>
public readonly struct FontInformationEntry
{
    /// <summary>
    /// Gets the size of the FontInformationEntry in bytes.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Gets the font ID.
    /// </summary>
    public ushort FontID { get; }

    /// <summary>
    /// Gets the font style.
    /// </summary>
    public ushort FontStyle { get; }

    /// <summary>
    /// Gets the font size in points.
    /// </summary>
    public ushort FontSize { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontInformationEntry"/> struct.
    /// </summary>
    /// <param name="data"></param>
    /// <exception cref="ArgumentException"></exception>
    public FontInformationEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 8-86 to 8-87
        int offset = 0;
        
        // Font ID number. The Finder sets the graphics port’s txFont field to this value.
        FontID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Font style. The Finder sets the graphics port’s txFace field to this style. 
        FontStyle = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;
        
        // Font size. The Finder sets the graphics port’s txSize field to this size. 
        FontSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == Size, "Did not consume all bytes for FontInformationRecord");
    }
}
