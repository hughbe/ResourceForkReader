using System.Buffers.Binary;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Rectangle Positions List Resource ('nrct').
/// </summary>
public readonly struct RectanglePositionsListRecord
{
    /// <summary>
    /// The minimum size of a RectanglePositionsListRecord in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the rectangle positions data.
    /// </summary>
    public ushort NumberOfRectangles { get; }

    /// <summary>
    /// Gets the list of rectangles.
    /// </summary>
    public List<RECT> Rectangles { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RectanglePositionsListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the rectangle positions list data.</param>
    public RectanglePositionsListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 8-85 to 8-86
        int offset = 0;

        NumberOfRectangles = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (MinSize + NumberOfRectangles * RECT.Size > data.Length)
        {
            throw new ArgumentException($"Data must be at least {MinSize + NumberOfRectangles * RECT.Size} bytes long to contain all rectangles.", nameof(data));
        }

        var rectangles = new List<RECT>(NumberOfRectangles);
        for (int i = 0; i < NumberOfRectangles; i++)
        {
            rectangles.Add(new RECT(data.Slice(offset, RECT.Size)));
            offset += RECT.Size;
        }

        Rectangles = rectangles;
    }
}
