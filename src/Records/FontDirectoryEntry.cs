using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Directory Entry in a resource fork.
/// </summary>
public readonly struct FontDirectoryEntry
{
    /// <summary>
    /// Size of a Font Directory Entry in bytes.
    /// </summary>
    public const int Size = 16;

    /// <summary>
    /// Gets the tag name of the font.
    /// </summary>
    public string TagName { get; }

    /// <summary>
    /// Gets the checksum of the font.
    /// </summary>
    public uint Checksum { get; }

    /// <summary>
    /// Gets the offset of the font data.
    /// </summary>
    public uint Offset { get; }

    /// <summary>
    /// Gets the length of the font data.
    /// </summary>
    public uint Length { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontDirectoryEntry"/> struct.
    /// </summary>
    /// <param name="data">The data for the Font Directory Entry.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not equal to the Font Directory Entry size.</exception>
    public FontDirectoryEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-74 to 4-76
        int offset = 0;

        // Tag name. The identifying name for this table, such as 'cmap'.
        TagName = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        // Checksum. The checksum for this table, which is the unsigned sum of the
        // long values in the table. This number can be used to verify the
        // integrity of the data in the table. 
        Checksum = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Offset. The offset from the beginning of the outline font resource
        // to the beginning of this table, in bytes.
        Offset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Length. The length of this table, in bytes.
        Length = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for FontDirectoryEntry.");
    }
}
