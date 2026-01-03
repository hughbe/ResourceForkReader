using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Component List Entry in a Component List Record.
/// </summary>
public readonly struct ComponentListEntry
{
    /// <summary>
    /// Size of a Component List Entry in bytes.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Type of the component (4-character code).
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Resource ID of the component.
    /// </summary>
    public short ResourceID { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentListEntry"/> struct.
    /// </summary>
    /// <param name="data">The data for the Component List Entry.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not exactly 6 bytes.</exception>
    public ComponentListEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length must be exactly {Size} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1171-L1176
        int offset = 0;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for Component List Entry.");
    }
}
