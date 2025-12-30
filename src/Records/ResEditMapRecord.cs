using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Res Edit Map Record ('RMAP') in a resource fork.
/// </summary>
public readonly struct ResEditMapRecord
{
    /// <summary>
    /// The minimum size of a Res Edit Map Record in bytes.
    /// </summary>
    public const int MinSize = 8;

    /// <summary>
    /// A type code indicating the resource type that this map entry applies to.
    /// </summary>
    public string MapToType { get; }

    /// <summary>
    /// A byte indicating whether this map entry is for editor-only resources.
    /// </summary>
    public byte EditorOnly { get; }

    /// <summary>
    /// The number of exceptions for this map entry.
    /// </summary>
    public ushort ExceptionsCount { get; }

    /// <summary>
    /// The list of exceptions for this map entry.
    /// </summary>
    public List<ResEditMapException> Exceptions { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResEditMapRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 8 bytes of Res Edit Map Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 8 bytes long.</exception>
    public ResEditMapRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Res Edit Map Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1088-L1098
        int offset = 0;

        MapToType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        EditorOnly = data[offset];
        offset += 1;

        // Padding byte
        offset += 1;

        ExceptionsCount = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var exceptions = new List<ResEditMapException>(ExceptionsCount);
        for (int i = 0; i < ExceptionsCount; i++)
        {
            var exception = new ResEditMapException(data.Slice(offset, ResEditMapException.Size));
            exceptions.Add(exception);
            offset += ResEditMapException.Size;

            if (offset % 2 != 0)
            {
                offset += 1; // Padding byte
            }
        }

        Exceptions = exceptions;

        Debug.Assert(offset == data.Length, "Did not consume all data for ResEditMapRecord.");
    }
}
