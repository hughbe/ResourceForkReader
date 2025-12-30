using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Pixel Pattern List Record ('ppt#') in a resource fork.
/// </summary>
public readonly struct PixelPatternListRecord
{
    /// <summary>
    /// The minimum size of a Pixel Pattern List Record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of patterns.
    /// </summary>
    public ushort NumberOfPatterns { get; }

    /// <summary>
    /// Gets the pattern offsets.
    /// </summary>
    public List<uint> PatternOffsets { get; }

    /// <summary>
    /// Gets the pixel patterns.
    /// </summary>
    public List<PixelPattern> Patterns { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PixelPatternListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Pixel Pattern List Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than minimum size.</exception>
    public PixelPatternListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Pixel Pattern List Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/ResourceFile.cc#L1678-L1697
        int offset = 0;

        NumberOfPatterns = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var patternOffsets = new List<uint>(NumberOfPatterns);
        for (int i = 0; i < NumberOfPatterns; i++)
        {
            patternOffsets.Add(BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4)));
            offset += 4;
        }

        PatternOffsets = patternOffsets;

        var patterns = new List<PixelPattern>(NumberOfPatterns);
        for (int i = 0; i < NumberOfPatterns; i++)
        {
            var patternOffset = PatternOffsets[i];
            patterns.Add(new PixelPattern(data.Slice((int)patternOffset, PixelPattern.Size)));
        }

        Patterns = patterns;

        Debug.Assert(offset <= data.Length, "Read beyond the end of Pixel Pattern List Record data.");
    }
}
