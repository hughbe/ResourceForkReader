using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Key Caps Record ('KCAP') in a resource fork.
/// </summary>
public readonly struct KeyCapsRecord
{
    /// <summary>
    /// The minimum size of a Key Caps Record in bytes.
    /// </summary>
    public const int MinSize = 18;

    /// <summary>
    /// Gets the bounds rectangle.
    /// </summary>
    public RECT Bounds { get; }

    /// <summary>
    /// Gets the text rectangle.
    /// </summary>
    public RECT TextRectangle { get; }

    /// <summary>
    /// Gets the number of shapes.
    /// </summary>
    public ushort NumberOfShapes { get; }

    /// <summary>
    /// Gets the list of shapes.
    /// </summary>
    public List<KeyCapsShapeEntry> Shapes { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyCapsRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Key Caps Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 18 bytes long.</exception>
    public KeyCapsRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Key Caps Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // C-28 to C-34
        int offset = 0;

        // Boundary rectangle. The position of the content region of the
        // Key Caps window.
        Bounds = new RECT(data.Slice(offset, RECT.Size));
        offset += RECT.Size;

        // Text rectangle. The position of the text box within the Key Caps window.
        TextRectangle = new RECT(data.Slice(offset, RECT.Size));
        offset += RECT.Size;

        // Main array. The remainder of the resource. It consists of an
        // array of one shape entry for each key shape.
        NumberOfShapes = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var shapes = new List<KeyCapsShapeEntry>();
        for (int i = 0; i < NumberOfShapes; i++)
        {
            shapes.Add(new KeyCapsShapeEntry(data.Slice(offset), out int bytesRead));
            offset += bytesRead;
        }

        Shapes = shapes;

        Debug.Assert(offset == data.Length, "Did not consume all data for Key Caps Record.");
    }
}
