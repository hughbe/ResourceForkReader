using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using ResourceForkReader.Utilities;

namespace ResourceForkReader;

public struct ResourceForkMap
{
    public const int MinSize = 28;

    public ResourceForkHeader Reserved1 { get; }

    public uint Reserved2 { get; }

    public ushort Reserved3 { get; }

    public ushort Attributes { get; }

    public ushort ResourceTypeListOffset { get; }

    public ushort ResourceNameListOffset { get; }

    public ushort ResourceTypeCount { get; }

    public Dictionary<string, List<ResourceListEntry>> Types { get; }
    
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
        var resourceTypes = new List<ResourceTypeListItem>(ResourceTypeCount + 1);
        var types = new Dictionary<string, List<ResourceListEntry>>(ResourceTypeCount + 1);
        for (int i = 0; i < ResourceTypeCount + 1; i++)
        {
            var resourceType = new ResourceTypeListItem(data.Slice(offset, ResourceTypeListItem.Size));
            offset += ResourceTypeListItem.Size;

            resourceTypes.Add(resourceType);
            
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