using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Gets the number of ROM override entries in the list.
/// </summary>
public struct ROMOverrideListEntry
{
    /// <summary>
    /// The size of a ROM override list entry in bytes.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Gets the resource type of the ROM override.
    /// </summary>
    public string ResourceType { get; }

    /// <summary>
    /// Gets the resource ID of the ROM override.
    /// </summary>
    public short ResourceID { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ROMOverrideListEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A 6-byte span containing the ROM override list entry data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 6 bytes.</exception>
    public ROMOverrideListEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        ResourceType = Encoding.ASCII.GetString(data[offset..(offset + 4)]);
        offset += 4;

        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data[offset..]);
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for ROM override list entry.");
    }
}
