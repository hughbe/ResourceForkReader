using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an editor font record.
/// </summary>
public readonly struct EditorFont
{
    /// <summary>
    /// The minimum size of an editor font record.
    /// </summary>
    public const int MinSize = 3;

    /// <summary>
    /// The point size of the editor font.
    /// </summary>
    public ushort PointSize { get; }

    /// <summary>
    /// The name of the editor font.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditorFont"/> struct.
    /// </summary>
    /// <param name="data">The data for the editor font record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data is invalid.</exception>
    public EditorFont(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than minimum size {MinSize} for EditorFont record.");
        }

        // Structure documented in https://www.docjava.com/posterous/file/2012/07/9621873-out3.pdf
        // p247
        int offset = 0;

        PointSize = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        Name = SpanUtilities.ReadPascalString(data[offset..], out var bytesRead);
        offset += bytesRead;

        if (offset % 2 != 0)
        {
            // Align to even byte boundary.
            offset++;
        }

        Debug.Assert(offset == data.Length, "Did not consume all data for EditorFont record.");
    }
}
