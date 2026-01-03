using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Database Result Handler in a resource fork.
/// </summary>
public readonly struct DatabaseResultHandler
{
    /// <summary>
    /// Minimum size of a Database Result Handler in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the procedure resource ID.
    /// </summary>
    public short ProcedureResourceID { get; }

    /// <summary>
    /// Gets the number of types handled.
    /// </summary>
    public ushort NumberOfTypes { get; }

    /// <summary>
    /// Gets the list of types handled.
    /// </summary>
    public List<string> Types { get;  }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseResultHandler"/> struct.
    /// </summary>
    /// <param name="data">The data for the Database Result Handler.</param>
    /// <param name="byteRead">Outputs the total number of bytes read from the data.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public DatabaseResultHandler(ReadOnlySpan<byte> data, out int byteRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1184-L1190
        int offset = 0;

        ProcedureResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        NumberOfTypes = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var types = new List<string>(NumberOfTypes);
        for (int i = 0; i < NumberOfTypes; i++)
        {
            types.Add(Encoding.ASCII.GetString(data.Slice(offset, 4)));
            offset += 4;
        }

        Types = types;

        byteRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all bytes for DatabaseResultHandler.");
    }
}
