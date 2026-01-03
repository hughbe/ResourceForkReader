using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Sound Lookup Table Record ('slut') in a resource fork.
/// </summary>
public readonly struct SoundLookupTableRecord
{
    /// <summary>
    /// Gets the list of Sound Lookup Table Entries.
    /// </summary>
    public List<SoundLookupTableEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundLookupTableRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the sound lookup table data.</param>
    /// <exception cref="ArgumentException">Thrown when data length is not a multiple of the entry size.</exception>
    public SoundLookupTableRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length % SoundLookupTableEntry.Size != 0)
        {
            throw new ArgumentException($"Data length must be a multiple of {SoundLookupTableEntry.Size}.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1161-L1166
        int offset = 0;

        var count = data.Length / SoundLookupTableEntry.Size;
        var entries = new List<SoundLookupTableEntry>(count);
        for (int i = 0; i < count; i++)
        {
            entries.Add(new SoundLookupTableEntry(data.Slice(offset, SoundLookupTableEntry.Size)));
            offset += SoundLookupTableEntry.Size;
        }

        Entries = entries;

        Debug.Assert(offset == data.Length, "Did not consume all data for Sound Lookup Table Record.");
    }
}
