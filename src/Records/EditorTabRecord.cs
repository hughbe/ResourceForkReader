using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an editor tab record.
/// </summary>
public readonly struct EditorTabRecord
{
    /// <summary>
    /// The size of an editor tab record in bytes.
    /// </summary>
    public const int Size = 4;
    
    /// <summary>
    /// The number of pixels for each space.
    /// </summary>
    public ushort NumberOfPixelsForEachSpace { get; }

    /// <summary>
    /// The number of spaces per tab.
    /// </summary>
    public ushort NumberOfSpacesPerTab { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditorTabRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the editor tab record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data is invalid.</exception>
    public EditorTabRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length {data.Length} does not match required size {Size} for EditorTabRecord.");
        }

        // Structure documented in https://www.docjava.com/posterous/file/2012/07/9621873-out3.pdf
        // p247
        int offset = 0;

        NumberOfPixelsForEachSpace = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        NumberOfSpacesPerTab = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for EditorTabRecord.");
    }
}
