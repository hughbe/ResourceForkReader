using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader;

/// <summary>
/// Represents the header of a resource fork containing offsets and lengths for data and map sections.
/// </summary>
public struct ResourceForkHeader
{
    /// <summary>
    /// The size of the resource fork header in bytes.
    /// </summary>
    public const int Size = 16;

    /// <summary>
    /// Gets the offset from the beginning of the resource fork to the resource data section.
    /// </summary>
    public uint DataOffset { get; }

    /// <summary>
    /// Gets the offset from the beginning of the resource fork to the resource map section.
    /// </summary>
    public uint MapOffset { get; }

    /// <summary>
    /// Gets the length of the resource data section in bytes.
    /// </summary>
    public uint DataLength { get; }

    /// <summary>
    /// Gets the length of the resource map section in bytes.
    /// </summary>
    public uint MapLength { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceForkHeader"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A 16-byte span containing the resource fork header data.</param>
    public ResourceForkHeader(ReadOnlySpan<byte> data)
    {
        int offset = 0;

        // Offset from beginning of resource fork to resource data.
        DataOffset = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        // Offset from beginning of resource fork to resource map.
        MapOffset = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        // Length of resource data.
        DataLength = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        // Length of resource map.
        MapLength = SpanUtilities.ReadUInt32BE(data, offset);
        offset += 4;

        Debug.Assert(offset == Size);
    }
}
