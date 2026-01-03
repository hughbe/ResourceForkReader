using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Name record in a resource fork.
/// </summary>
public readonly struct FontName
{
    /// <summary>
    /// Size of a Font Name record in bytes.
    /// </summary>
    public const int Size = 12;

    /// <summary>
    /// Gets the platform identifier.
    /// </summary>
    public ushort PlatformID { get; }

    /// <summary>
    /// Gets the platform-specific encoding identifier.
    /// </summary>
    public ushort PlatformSpecificID { get; }

    /// <summary>
    /// Gets the language identifier.
    /// </summary>
    public ushort LanguageID { get; }

    /// <summary>
    /// Gets the name identifier.
    /// </summary>
    public ushort NameID { get; }

    /// <summary>
    /// Gets the length of the string, in bytes.
    /// </summary>
    public ushort Length { get; }

    /// <summary>
    /// Gets the offset from the start of storage area, in bytes.
    /// </summary>
    public ushort Offset { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontName"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Font Name record data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public FontName(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length {data.Length} does not equal the required size of {Size} bytes for a Font Name record.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-84 to 4-88
        int offset = 0;

        // Platform ID. The platform identifier
        PlatformID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Platform-specific ID. The platform-specific encoding identifier.
        PlatformSpecificID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Language ID. The language identifier
        LanguageID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Name ID. The name identifier.
        NameID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        //  Length. The length of the string, in bytes.
        Length = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Offset. The offset from the start of storage area, in bytes.
        Offset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == Size, "Offset should equal Size after reading Font Name record.");
    }
}
