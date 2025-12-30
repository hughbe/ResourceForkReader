using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Res Edit Tool Record ('TOOL') in a resource fork.
/// </summary>
public readonly struct ResEditToolRecord
{
    /// <summary>
    /// The minimum size of a Res Edit Tool Record in bytes.
    /// </summary>
    public const int MinSize = 4;

    /// <summary>
    /// Gets the number of tools per row.
    /// </summary>
    public ushort ToolsPerRow { get; }

    /// <summary>
    /// Gets the number of rows.
    /// </summary>
    public ushort NumberOfRows { get; }

    /// <summary>
    /// Gets the list of cursor IDs.
    /// </summary>
    public List<ushort> CursorIDs { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResEditToolRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Res Edit Tool Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 4 bytes long.</exception>
    public ResEditToolRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Res Edit Tool Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        int offset = 0;

        ToolsPerRow = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        NumberOfRows = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var cursorIDs = new List<ushort>();
        while (offset + 2 <= data.Length)
        {
            cursorIDs.Add(BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2)));
            offset += 2;
        }

        CursorIDs = cursorIDs;

        Debug.Assert(offset == data.Length, "Did not consume all data for ResEditToolRecord.");
    }
}
