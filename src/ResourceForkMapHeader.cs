using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader;

/// <summary>
/// Represents the header of a resource fork containing offsets and lengths for data and map sections.
/// </summary>
public struct ResourceForkMapHeader
{
    /// <summary>
    /// The size of a resource fork map header in bytes.
    /// </summary>
    public const int Size = 28;

    /// <summary>
    /// Gets the reserved copy of the resource header.
    /// </summary>
    public ResourceForkHeader Reserved1 { get; }

    /// <summary>
    /// Gets the reserved handle to the next resource map.
    /// </summary>
    public uint Reserved2 { get; }

    /// <summary>
    /// Gets the reserved file reference number.
    /// </summary>
    public ushort Reserved3 { get; }

    /// <summary>
    /// Gets the resource map attributes.
    /// </summary>
    public ushort Attributes { get; }

    /// <summary>
    /// Gets the offset from the beginning of the map to the resource type list.
    /// </summary>
    public ushort ResourceTypeListOffset { get; }

    /// <summary>
    /// Gets the offset from the beginning of the map to the resource name list.
    /// </summary>
    public ushort ResourceNameListOffset { get; }

    /// <summary>
    /// Gets the number of resource types in the map.
    /// </summary>
    public ushort ResourceTypeCount { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceForkMapHeader"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the resource map data (exactly 28 bytes).</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 28 bytes long or contains invalid offsets.</exception>
    public ResourceForkMapHeader(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        // Reserved for copy of resource header
        Reserved1 = new ResourceForkHeader(data.Slice(offset, ResourceForkHeader.Size));
        offset += ResourceForkHeader.Size;

        // Reserved for handle to next resource map
        Reserved2 = BinaryPrimitives.ReadUInt32BigEndian(data[offset..]);
        offset += 4;

        // Reserved for file reference number
        Reserved3 = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        // Reserved for attributes
        Attributes = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        // Offset from beginning of map to resource type list
        ResourceTypeListOffset = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        // Offset from beginning of map to resource name list
        ResourceNameListOffset = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        Debug.Assert(offset == Size, "Did not consume all data for ResourceForkMapHeader.");
    }
}
