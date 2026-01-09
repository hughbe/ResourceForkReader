using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader;

/// <summary>
/// Represents the resource map section of a resource fork, containing resource types and entries.
/// </summary>
public struct ResourceForkMap
{
    /// <summary>
    /// The minimum size of a resource fork map in bytes.
    /// </summary>
    public const int MinSize = 28;

    /// <summary>
    /// Gets the header of the resource fork map.
    /// </summary>
    public ResourceForkMapHeader Header { get; }

    /// <summary>
    /// Gets the number of resource types in the map.
    /// </summary>
    public ushort ResourceTypeCount { get; }

    /// <summary>
    /// Gets a dictionary mapping 4-character resource type codes to lists of resource entries.
    /// </summary>
    public Dictionary<string, List<ResourceListEntry>> Types { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceForkMap"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the resource map data (minimum 28 bytes).</param>
    /// <exception cref="ArgumentException">Thrown when data is too short or contains invalid offsets.</exception>
    public ResourceForkMap(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        int offset = 0;

        Header = new ResourceForkMapHeader(data.Slice(offset, ResourceForkMapHeader.Size));
        offset += ResourceForkMapHeader.Size;

        Debug.Assert(offset == ResourceForkMapHeader.Size, "Did not consume all data for ResourceForkMap header.");

        // Move to the resource type list.
        if (Header.ResourceTypeListOffset >= data.Length)
        {
            throw new ArgumentException("Resource type list offset is out of bounds.", nameof(data));
        }

        offset = Header.ResourceTypeListOffset;

        // The documentation incorrectly states that this is part of the 
        // header when it is actually part of the list.
        // Number of types in the map minus 1
        ResourceTypeCount = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        if (ResourceTypeCount == 0xFFFF)
        {
            // No resource types defined.
            Types = [];
        }
        else
        {
            // Read the resource types and the entries.
            var types = new Dictionary<string, List<ResourceListEntry>>(ResourceTypeCount + 1);
            for (int i = 0; i < ResourceTypeCount + 1; i++)
            {
                var resourceType = new ResourceTypeListItem(data.Slice(offset, ResourceTypeListItem.Size));
                offset += ResourceTypeListItem.Size;

                var resourceListOffset = Header.ResourceTypeListOffset + resourceType.ResourceListOffset;
                if (resourceListOffset >= data.Length)
                {
                    throw new ArgumentException("Resource list offset is out of bounds.", nameof(data));
                }

                for (int j = 0; j < resourceType.ResourceCount + 1; j++)
                {
                    var entry = new ResourceListEntry(data.Slice(resourceListOffset, ResourceListEntry.Size));
                    resourceListOffset += ResourceListEntry.Size;

                    if (!types.TryGetValue(resourceType.Type, out var entries))
                    {
                        entries = [];
                        types[resourceType.Type] = entries;
                    }

                    entries.Add(entry);
                }

            }

            Types = types;
        }

        Debug.Assert(offset <= data.Length, "Did not consume all data for ResourceForkMap.");
    }
}