using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Sound Lookup Table Entry in a resource fork.
/// </summary>
public readonly struct SoundLookupTableEntry
{
    /// <summary>
    /// Gets the size of a Sound Lookup Table Entry structure in bytes.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Gets the resource type of the resource.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the resource ID of the resource.
    /// </summary>
    public short ResourceID { get;  }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundLookupTableEntry"/> struct.
    /// </summary>
    /// <param name="data">The data for the Resource Spec.</param>
    /// <exception cref="ArgumentException">Thrown if the data length is not equal to the Resource Spec size.</exception>
    public SoundLookupTableEntry(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1161-L1166
        int offset = 0;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for Sound Lookup Table Entry.");
    }
}
