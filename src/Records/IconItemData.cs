using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents icon item data in an item list.
/// </summary>
public readonly struct IconItemData
{
    /// <summary>
    /// The size of the IconItemData structure in bytes.
    /// </summary>
    public const int Size = 3;

    /// <summary>
    /// Gets the reserved field.
    /// </summary>
    public byte Reserved { get; }

    /// <summary>
    /// Gets the icon resource ID.
    /// </summary>
    public ushort IconResourceID { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IconItemData"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the icon item data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
    public IconItemData(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < Size)
        {
            throw new ArgumentException($"Data must be at least {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        Reserved = data[offset];
        offset += 1;

        // Resource ID.
        // ■ For a control item, this is the resource ID of a 'CTRL' resource.
        // ■ For an icon item, this is the resource ID of an 'ICON' resource
        // and, optionally, a 'cicn' resource
        // ■ For a picture item, this is the resource ID of a 'PICT'
        // resource. 
        IconResourceID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all data for IconItemData.");
    }
}
