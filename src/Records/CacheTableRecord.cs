using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Cache Table Record ('CTAB') in a resource fork.
/// </summary>
public readonly struct CacheTableRecord
{
    /// <summary>
    /// Gets the entries in the Cache Table Record.
    /// </summary>
    public List<CacheTableEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheTableRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Cache Table Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data size is not a multiple of Cache Table Entry size.</exception>
    public CacheTableRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length % CacheTableEntry.Size != 0)
        {
            throw new ArgumentException($"Data size is not a multiple of Cache Table Entry size ({CacheTableEntry.Size} bytes).", nameof(data));
        }

        // Structure not documented but easy to reverse engineer.
        // Series of Cache Table Entries.
        int offset = 0;

        int entryCount = data.Length / CacheTableEntry.Size;
        var entries = new List<CacheTableEntry>(entryCount);
        for (int i = 0; i < entryCount; i++)
        {
            entries.Add(new CacheTableEntry(data.Slice(offset, CacheTableEntry.Size)));
            offset += CacheTableEntry.Size;
        }

        Entries = entries;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for Cache Table Record.");
    }   
}
