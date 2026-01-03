using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an entry in a Folder List Entry Record ('fld#').
/// </summary>
public readonly struct FolderListEntry
{
    /// <summary>
    /// The minimum size of a Folder record in bytes.
    /// </summary>
    public const int MinSize = 8;

    /// <summary>
    /// The type of the Folder record.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// The version of the Folder record.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// The name of the folder.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FolderListEntry"/> struct from the given data.
    /// </summary>
    /// <param name="data">The byte span containing the Folder record data.</param>
    /// <param name="bytesRead">The number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size for a Folder record.</exception>
    public FolderListEntry(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than minimum size {MinSize} for Folder record.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L430-L439
        int offset = 0;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;
        
        Name = SpanUtilities.ReadPascalStringWordCount(data[offset..], out int nameBytesRead);
        offset += nameBytesRead;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all data for Folder record.");
    }
}
