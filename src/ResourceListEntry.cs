using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader;

/// <summary>
/// Represents a single resource entry in a resource fork.
/// </summary>
public struct ResourceListEntry
{
    /// <summary>
    /// The size of a resource list entry in bytes.
    /// </summary>
    public const int Size = 12;

    /// <summary>
    /// Gets the resource ID.
    /// </summary>
    public short ID { get; }

    /// <summary>
    /// Gets the offset from the beginning of the resource name list to the resource name.
    /// </summary>
    public ushort NameOffset { get; }

    /// <summary>
    /// Gets the resource attributes (flags such as Protected, Locked, Purgeable, etc.).
    /// </summary>
    public ResourceAttributeFlags Attributes { get; }

    /// <summary>
    /// Gets the offset from the beginning of the resource data section to this resource's data.
    /// </summary>
    public uint DataOffset { get; }

    /// <summary>
    /// Gets the reserved handle to the resource (used by Mac OS at runtime).
    /// </summary>
    public uint ReservedHandle { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceListEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A 12-byte span containing the resource list entry data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 12 bytes.</exception>
    public ResourceListEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        // Resource ID
        ID = BinaryPrimitives.ReadInt16BigEndian(data[offset..]);
        offset += 2;

        // Offset from beginning of resource name list to resource name
        NameOffset = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        // 1 byte: Resource attributes and
        // 3 bytes: Offset from beginning of resource data to data for this resource
        var attributesAndOffset = BinaryPrimitives.ReadUInt32BigEndian(data[offset..]);
        Attributes = (ResourceAttributeFlags)(attributesAndOffset >> 24);
        DataOffset = attributesAndOffset & 0x00FFFFFF;
        offset += 4;

        // Reserved for handle to resource
        ReservedHandle = BinaryPrimitives.ReadUInt32BigEndian(data[offset..]);
        offset += 4;

        Debug.Assert(offset == Size);
    }

    /// <summary>
    /// Gets the name of the resource by reading it from the resource fork.
    /// </summary>
    /// <param name="fork">The resource fork containing the resource.</param>
    /// <returns>The resource name as a string, or an empty string if the resource has no name.</returns>
    public readonly string GetName(ResourceFork fork)
    {
        if (NameOffset == 0xFFFF)
        {
            return string.Empty;
        }

        // Calculate the absolute offset of the resource name
        long nameListOffset = fork.Header.MapOffset + fork.Map.ResourceNameListOffset;
        long absoluteNameOffset = nameListOffset + NameOffset;

        // Read the name length (Pascal string format - first byte is length)
        fork._stream.Seek(fork._baseOffset + absoluteNameOffset, SeekOrigin.Begin);
        
        int nameLength = fork._stream.ReadByte();
        if (nameLength <= 0)
        {
            return string.Empty;
        }

        byte[] nameBytes = new byte[nameLength];
        if (fork._stream.Read(nameBytes, 0, nameLength) != nameLength)
        {
            throw new ArgumentException("Failed to read the full resource name from the stream.", nameof(fork));
        }
        
        return System.Text.Encoding.ASCII.GetString(nameBytes);
    }
}
