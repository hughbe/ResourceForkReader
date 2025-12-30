using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Cities List Record ('CTYN') in a resource fork.
/// </summary>
public readonly struct CitiesListRecord
{
    /// <summary>
    /// The minimum size of a Cities List Record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of cities.
    /// </summary>
    public ushort NumberOfCities { get; }

    /// <summary>
    /// Gets the list of cities.
    /// </summary>
    public List<City> Cities { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CitiesListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Cities List Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 2 bytes long.</exception>
    public CitiesListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Cities List Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        int offset = 0;

        NumberOfCities = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var cities = new List<City>(NumberOfCities + 1);
        for (int i = 0; i < NumberOfCities + 1; i++)
        {
            cities.Add(new City(data[offset..], out var bytesRead));
            offset += bytesRead;

            if (offset % 2 != 0)
            {
                // Align to even byte boundary.
                offset += 1;
            }
        }

        Cities = cities;

        Debug.Assert(offset == data.Length, "Did not consume all data for CitiesListRecord.");
    }
}
