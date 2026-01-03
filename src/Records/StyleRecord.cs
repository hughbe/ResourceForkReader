using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a 'styl' style resource.
/// </summary>
public readonly struct StyleRecord
{
    /// <summary>
    /// The minimum size of a StyleRecord structure in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// The number of style runs in the style record.
    /// </summary>
    public ushort NumberOfRuns { get; }

    /// <summary>
    /// The list of style runs in the style record.
    /// </summary>
    public List<StyleRun> StyleRuns { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Style Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to contain a valid StyleRecord.</exception>
    public StyleRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to contain a valid StyleRecord. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 2-74 to 2-76
        int offset = 0;

        // The number of style runs used in the text. This determines the size
        // of the style table. When character attribute information is written to
        // the null scrap, this field is set to 1; when the character attribute
        // information is removed, this field is set to 0.
        NumberOfRuns = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var styleRuns = new List<StyleRun>(NumberOfRuns);
        for (int i = 0; i < NumberOfRuns; i++)
        {
            styleRuns.Add(new StyleRun(data.Slice(offset, StyleRun.Size)));
            offset += StyleRun.Size;
        }

        StyleRuns = styleRuns;

        Debug.Assert(offset == data.Length, "There should be no more data after the NumberOfRuns field in a StyleRecord.");
    }
}
