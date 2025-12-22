using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a ROM fonts record resource.
/// </summary>
public readonly struct ROMFontsRecord
{
    /// <summary>
    /// The minimum size of a ROM fonts record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of fonts in the ROM fonts record.
    /// </summary>
    public ushort NumberOfFonts { get; }

    /// <summary>
    /// Gets the list of font resource IDs included in the ROM fonts record.
    /// </summary>
    public List<ushort> FontResourceIDs { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ROMFontsRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the ROM fonts record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to contain the ROM fonts record.</exception>
    public ROMFontsRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/2725d5d8011c08cc2d11476375541f739433f19f/src/SystemTemplates.cc#L462-L466
        int offset = 0;

        NumberOfFonts = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        var fontResourceIDs = new List<ushort>(NumberOfFonts);
        for (int i = 0; i < NumberOfFonts; i++)
        {
            if (offset + 2 > data.Length)
            {
                throw new ArgumentException("Data is too short to contain all font resource IDs.", nameof(data));
            }

            ushort fontResourceID = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
            fontResourceIDs.Add(fontResourceID);
            offset += 2;
        }

        FontResourceIDs = fontResourceIDs;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for ROM fonts record.");
    }
}