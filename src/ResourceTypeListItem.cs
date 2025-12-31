using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader;

/// <summary>
/// Represents a resource type entry in the resource type list.
/// </summary>
public struct ResourceTypeListItem
{
    /// <summary>
    /// The size of a resource type list item in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the 4-character resource type code (e.g., "CODE", "STR#", "ICON").
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the number of resources of this type minus 1.
    /// </summary>
    public ushort ResourceCount { get; }

    /// <summary>
    /// Gets the offset from the beginning of the resource type list to the resource list for this type.
    /// </summary>
    public ushort ResourceListOffset { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceTypeListItem"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">An 8-byte span containing the resource type list item data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 8 bytes.</exception>
    public ResourceTypeListItem(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }


        int offset = 0;
        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        ResourceCount = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        ResourceListOffset = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        Debug.Assert(offset == Size, "Did not consume all data for ResourceTypeListItem.");
    }
}
