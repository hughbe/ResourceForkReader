using ResourceForkReader.Utilities;

namespace ResourceForkReader;

public class ResourceFork
{
    private readonly Stream _stream;
    public ResourceForkHeader Header { get; }
    public ResourceForkMap Map { get; }

    public ResourceFork(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (!stream.CanSeek || !stream.CanRead)
        {
            throw new ArgumentException("Stream must be seekable and readable.", nameof(stream));
        }

        _stream = stream;

        // Read the header
        Span<byte> headerData = stackalloc byte[ResourceForkHeader.Size];
        _stream.ReadExactly(headerData);
        Header = new ResourceForkHeader(headerData);

        // Read the resource map
        _stream.Seek(Header.MapOffset, SeekOrigin.Begin);
        Span<byte> mapData = Header.MapLength <= 1024
            ? stackalloc byte[(int)Header.MapLength]
            : new byte[Header.MapLength];
        _stream.ReadExactly(mapData);
        Map = new ResourceForkMap(mapData);
    }

    public byte[] GetResourceData(ResourceListEntry entry)
    {
        using var ms = new MemoryStream();
        GetResourceData(entry, ms);
        return ms.ToArray();
    }

    public int GetResourceData(ResourceListEntry entry, Stream output)
    {
        ArgumentNullException.ThrowIfNull(output);

        // Calculate the absolute offset of the resource data
        long dataOffset = Header.DataOffset + entry.DataOffset;

        // Length of following resource data
        _stream.Seek(dataOffset, SeekOrigin.Begin);
        Span<byte> sizeData = stackalloc byte[4];
        _stream.ReadExactly(sizeData);
        int dataSize = SpanUtilities.ReadInt32BE(sizeData, 0);

        // Read the resource data
        // Copy in chunks of 512 bytes.
        const int BufferSize = 512;
        Span<byte> buffer = stackalloc byte[BufferSize];
        int remaining = dataSize;
        while (remaining > 0)
        {
            int toRead = Math.Min(remaining, BufferSize);
            _stream.ReadExactly(buffer[..toRead]);
            output.Write(buffer[..toRead]);
            remaining -= toRead;
        }

        return dataSize;
    }
}
