using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Pattern Record ('PAT ') in a resource fork.
/// </summary>
public readonly struct PatternRecord
{
    /// <summary>
    /// The size of a Pattern Record in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the pattern data.
    /// </summary>
    public byte[] PatternData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PatternRecord"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the pattern record data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid or insufficient.</exception>
    public PatternRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be at least {Size} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/ResourceFile.cc#L1707-L1712
        int offset = 0;

        PatternData = data.Slice(offset, Size).ToArray();
        offset += Size;

        Debug.Assert(offset == data.Length, "Did not consume all data.");
    }
}
