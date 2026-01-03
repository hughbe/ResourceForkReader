using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Database Result Handlers ('rtt#') Record in a resource fork.
/// </summary>
public readonly struct DatabaseResultHandlersRecord
{
    /// <summary>
    /// Minimum size of a Database Result Handlers Record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of handlers.
    /// </summary>
    public ushort NumberOfHandlers { get; }

    /// <summary>
    /// Gets the list of database result handlers.
    /// </summary>
    public List<DatabaseResultHandler> Handlers { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseResultHandlersRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Database Result Handlers Record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public DatabaseResultHandlersRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1184-L1190
        int offset = 0;

        NumberOfHandlers = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var handlers = new List<DatabaseResultHandler>(NumberOfHandlers);
        for (int i = 0; i < NumberOfHandlers; i++)
        {
            handlers.Add(new DatabaseResultHandler(data[offset..], out int bytesRead));
            offset += bytesRead;
        }

        Handlers = handlers;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for DatabaseResultHandlersRecord.");
    }
}
