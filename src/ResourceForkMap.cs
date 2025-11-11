using System.Diagnostics;
using ResourceForkReader.Utilities;

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

        // Reserved for copy of resource header
        Reserved1 = new ResourceForkHeader(data);
        offset += ResourceForkHeader.Size;

        // Reserved for handle to next resource map
        Reserved2 = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        // Reserved for file reference number
        Reserved3 = SpanUtilities.ReadUInt16BE(data, offset);
        offset += 2;

        // Reserved for attributes
        Attributes = SpanUtilities.ReadUInt16BE(data, offset);
        offset += 2;

        // Offset from beginning of map to resource type list
        ResourceTypeListOffset = SpanUtilities.ReadUInt16BE(data, offset);
        offset += 2;

        if (ResourceTypeListOffset >= data.Length)
        {
            throw new ArgumentException("Resource type list offset is out of bounds.", nameof(data));
        }

        // Offset from beginning of map to resource name list
        ResourceNameListOffset = SpanUtilities.ReadUInt16BE(data, offset);
        offset += 2;

        Debug.Assert(offset == 28);

        // Move to the resource type list.
        offset = ResourceTypeListOffset;

        // The documentation incorrectly states that this is part of the 
        // header when it is actually part of the list.
        // Number of types in the map minus 1
        ResourceTypeCount = SpanUtilities.ReadUInt16BE(data, offset);
        offset += 2;

        // Read the resource types and the entries.
        var types = new Dictionary<string, List<ResourceListEntry>>(ResourceTypeCount + 1);
        for (int i = 0; i < ResourceTypeCount + 1; i++)
        {
            var resourceType = new ResourceTypeListItem(data.Slice(offset, ResourceTypeListItem.Size));
            offset += ResourceTypeListItem.Size;
            
            var resourceListOffset = ResourceTypeListOffset + resourceType.ResourceListOffset;
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
}