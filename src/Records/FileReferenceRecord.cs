using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a file reference ('FREF') in a resource fork.
/// </summary>
public readonly struct FileReferenceRecord
{
    /// <summary>
    /// Gets the file reference type.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the local icon ID.
    /// </summary>
    public ushort LocalIconID { get; }

    /// <summary>
    /// Gets the name of the file reference.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileReferenceRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the file reference data.</param>
    public FileReferenceRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < 7)
        {
            throw new ArgumentException("Data is too short to be a valid FileReferenceRecord.", nameof(data));
        }

        int offset = 0;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        LocalIconID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Name = SpanUtilities.ReadPascalString(data[offset..]);
        offset += 1 + Name.Length;

        Debug.Assert(offset <= data.Length, "Parsed beyond the end of the data span.");
    }
}
