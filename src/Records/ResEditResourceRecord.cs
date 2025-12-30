using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Res Edit Resource Record in a resource fork.
/// </summary>
public readonly struct ResEditResourceRecord
{
    /// <summary>
    /// The minimum size of a Res Edit Resource Record in bytes.
    /// </summary>
    public const int MinSize = 22;

    /// <summary>
    /// Gets the resource type.
    /// </summary>
    public string ResourceType { get; }

    /// <summary>
    /// Gets the list of function codes.
    /// </summary>
    public ushort[] Functions { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResEditResourceRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Res Edit Resource Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 24 bytes long.</exception>
    public ResEditResourceRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Res Edit Resource Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure partially documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/ResourceFile.cc#L1411-L1431
        int offset = 0;

        ResourceType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        var functions = new ushort[9];
        for (int i = 0; i < 9; i++)
        {
            functions[i] = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        Functions = functions;

        Debug.Assert(offset <= data.Length, "Did not consume all data for ResEditResourceRecord.");
    }
}

