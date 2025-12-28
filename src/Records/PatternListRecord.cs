using System.Buffers.Binary;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Pattern List Record ('PTN#') in a resource fork.
/// </summary>
public readonly struct PatternListRecord
{
    /// <summary>
    /// The minimum size of a Pattern List Record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of patterns in the pattern list.
    /// </summary>
    public ushort NumberOfPatterns { get; }

    /// <summary>
    /// Gets the list of patterns in the pattern list.
    /// </summary>
    public List<PatternRecord> Patterns { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PatternListRecord"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the pattern list record data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid or insufficient.</exception>
    public PatternListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/ResourceFile.cc#L1722-L1731
        int offset = 0;

        NumberOfPatterns = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var patterns = new List<PatternRecord>(NumberOfPatterns);
        for (int i = 0; i < NumberOfPatterns; i++)
        {
            if (offset + PatternRecord.Size > data.Length)
            {
                throw new ArgumentException("Data is too short to contain all patterns.", nameof(data));
            }

            var patternData = new PatternRecord(data.Slice(offset, PatternRecord.Size));
            patterns.Add(patternData);
            offset += PatternRecord.Size;
        }

        Patterns = patterns;
    }
}
