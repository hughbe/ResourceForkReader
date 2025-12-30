using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Res Edit Map Exception in a resource fork.
/// </summary>
public readonly struct ResEditMapException
{
    /// <summary>
    /// The size of a Res Edit Map Exception in bytes.
    /// </summary>
    public const int Size = 7;

    /// <summary>
    /// Gets the resource ID.
    /// </summary>
    public short ResourceID { get; }

    /// <summary>
    /// Gets the type code indicating the resource type that this exception applies to.
    /// </summary>
    public string MapToType { get; }

    /// <summary>
    /// Gets a byte indicating whether this exception is for editor-only resources.
    /// </summary>
    public byte EditorOnly { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResEditMapException"/> struct.
    /// </summary>
    /// <param name="data">A span containing the Res Edit Map Exception data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid.</exception>
    public ResEditMapException(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes to be a valid Res Edit Map Exception.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1088-L1098
        int offset = 0;

        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        MapToType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        EditorOnly = data[offset];
        offset += 1;

        Debug.Assert(offset == data.Length, "Did not consume all data for ResEditMapException.");
    }
}