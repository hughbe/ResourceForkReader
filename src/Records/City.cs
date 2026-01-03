using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a City record in a Cities List resource ('CTYN').
/// </summary>
public readonly struct City
{
    /// <summary>
    /// The minimum size of a City record in bytes.
    /// </summary>
    public const int MinSize = 19;

    /// <summary>
    /// Gets the number of characters in the city name.
    /// </summary>
    public ushort Numchars { get; }

    /// <summary>
    /// Gets the longitude of the city.
    /// </summary>
    public int Longitude { get; }

    /// <summary>
    /// Gets the latitude of the city.
    /// </summary>
    public int Latitude { get; }

    /// <summary>
    /// Gets the GMT difference of the city.
    /// </summary>
    public long GMTDifference { get; }
    
    /// <summary>
    /// Gets the reserved field.
    /// </summary>
    public long Reserved { get; }

    /// <summary>
    /// Gets the name of the city.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="City"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 19 bytes of City record data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 19 bytes long.</exception>
    public City(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }
        
        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L366-L376.
        int offset = 0;

        Numchars = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Longitude = BinaryPrimitives.ReadInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Latitude = BinaryPrimitives.ReadInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        GMTDifference = BinaryPrimitives.ReadInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Reserved = BinaryPrimitives.ReadInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Name = SpanUtilities.ReadPascalString(data[offset..], out var nameBytesRead);
        offset += nameBytesRead;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Offset exceeds data length when reading City record.");
    }
}
