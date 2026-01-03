using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Palette Entry in a Palette Record ('pltt').
/// </summary>
public readonly struct PaletteEntry
{
    /// <summary>
    /// The size of a Palette Entry in bytes.
    /// </summary>
    public const int Size = 16;

    /// <summary>
    /// Gets the red component of the palette entry.
    /// </summary>
    public ushort Red { get; }

    /// <summary>
    /// Gets the green component of the palette entry.
    /// </summary>
    public ushort Green { get; }

    /// <summary>
    /// Gets the blue component of the palette entry.
    /// </summary>
    public ushort Blue { get; }

    /// <summary>
    /// Gets the usage of the palette entry.
    /// </summary>
    public ushort Usage { get; }

    /// <summary>
    /// Gets the tolerance of the palette entry.
    /// </summary>
    public ushort Tolerance { get; }

    /// <summary>
    /// Gets the reserved field 1.
    /// </summary>
    public ushort Reserved1 { get; }

    /// <summary>
    /// Gets the reserved field 2.
    /// </summary>
    public ushort Reserved2 { get; }

    /// <summary>
    /// Gets the reserved field 3.
    /// </summary>
    public ushort Reserved3 { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaletteEntry"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the palette entry data.</param>
    /// <exception cref="ArgumentException">>Thrown when the data is invalid.</exception>
    public PaletteEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException("Data is too short to contain a valid Palette Entry.", nameof(data));
        }

        // Stucture documented in https://dev.os9.ca/techpubs/mac/ACI/ACI-28.html
        // and https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/ResourceFile.cc#L2736-L2751
        int offset = 0;

        Red = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Green = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Blue = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Usage = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Tolerance = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved2 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved3 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not read the expected number of bytes for Palette Entry.");
    }
}
