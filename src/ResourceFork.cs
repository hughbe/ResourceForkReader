using System.Buffers.Binary;

namespace ResourceForkReader;

/// <summary>
/// Represents a classic Macintosh resource fork and provides methods for reading resource data.
/// </summary>
public class ResourceFork
{
    internal readonly Stream _stream;
    internal readonly long _baseOffset;

    /// <summary>
    /// Gets the resource fork header containing offsets and lengths for data and map sections.
    /// </summary>
    public ResourceForkHeader Header { get; }

    /// <summary>
    /// Gets the resource fork map containing resource types and entries.
    /// </summary>
    public ResourceForkMap Map { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceFork"/> class by reading from a stream.
    /// </summary>
    /// <param name="stream">A seekable and readable stream containing resource fork data.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="stream"/> is not seekable or readable.</exception>
    public ResourceFork(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (!stream.CanSeek || !stream.CanRead)
        {
            throw new ArgumentException("Stream must be seekable and readable.", nameof(stream));
        }

        _stream = stream;
        _baseOffset = stream.Position;

        // Read the header
        Span<byte> headerData = stackalloc byte[ResourceForkHeader.Size];
        if (_stream.Read(headerData) != ResourceForkHeader.Size)
        {
            throw new ArgumentException("Unable to read complete resource fork header from stream.", nameof(stream));
        }

        Header = new ResourceForkHeader(headerData);

        // Read the resource map
        var mapOffset = _baseOffset + Header.MapOffset;
        if (mapOffset >= _stream.Length)
        {
            throw new ArgumentException("Resource fork map offset is out of bounds.", nameof(stream));
        }

        _stream.Seek(mapOffset, SeekOrigin.Begin);
        Span<byte> mapData = Header.MapLength <= 1024
            ? stackalloc byte[(int)Header.MapLength]
            : new byte[Header.MapLength];
        if (_stream.Read(mapData) != Header.MapLength)
        {
            throw new ArgumentException("Unable to read complete resource fork map from stream.", nameof(stream));
        }

        Map = new ResourceForkMap(mapData);
    }

    /// <summary>
    /// Reads the resource data for a given resource entry and returns it as a byte array.
    /// </summary>
    /// <param name="entry">The resource list entry to read data for.</param>
    /// <returns>A byte array containing the resource data.</returns>
    public byte[] GetResourceData(ResourceListEntry entry)
    {
        using var ms = new MemoryStream();
        GetResourceData(entry, ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Reads the resource data for a given resource entry and writes it to the specified outputStream stream.
    /// </summary>
    /// <param name="entry">The resource list entry to read data for.</param>
    /// <param name="outputStream">The stream to write the resource data to.</param>
    /// <returns>The number of bytes written to the output stream.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="outputStream"/> is null.</exception>
    public int GetResourceData(ResourceListEntry entry, Stream outputStream)
    {
        ArgumentNullException.ThrowIfNull(outputStream);

        // Calculate the absolute offset of the resource data
        long dataOffset = _baseOffset + Header.DataOffset + entry.DataOffset;
        if (dataOffset >= _stream.Length)
        {
            throw new ArgumentException("Resource data offset is out of bounds.", nameof(entry));
        }

        // Length of following resource data
        _stream.Seek(dataOffset, SeekOrigin.Begin);
        Span<byte> sizeData = stackalloc byte[4];
        if (_stream.Read(sizeData) != 4)
        {
            throw new ArgumentException("Unable to read resource data size.", nameof(entry));
        }
        
        int dataSize = BinaryPrimitives.ReadInt32BigEndian(sizeData);

        // Read the resource data
        // Copy in chunks of 512 bytes.
        const int BufferSize = 512;
        Span<byte> buffer = stackalloc byte[BufferSize];
        int remaining = dataSize;
        while (remaining > 0)
        {
            var toRead = Math.Min(remaining, BufferSize);
            if (_stream.Read(buffer[..toRead]) != toRead)
            {
                throw new ArgumentException("Unable to read complete resource data.", nameof(entry));
            }

            outputStream.Write(buffer[..toRead]);
            remaining -= toRead;
        }

        return dataSize;
    }
}
