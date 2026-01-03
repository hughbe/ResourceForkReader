using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Finder Icon Map ('fmap') Record.
/// </summary>
public readonly struct FinderIconMapRecord
{
    /// <summary>
    /// The entries in the Finder Icon Map.
    /// </summary>
    public List<FinderIconMapEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FinderIconMapRecord"/> struct from the given data.
    /// </summary>
    /// <param name="data">The byte span containing the Finder Icon Map data.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not a multiple of the Finder Icon Map Entry size.</exception>
    public FinderIconMapRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length % FinderIconMapEntry.Size != 0)
        {
            throw new ArgumentException($"Data length {data.Length} is not a multiple of Finder Icon Map Entry size {FinderIconMapEntry.Size}.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L450-L456
        int offset = 0;

        int entryCount = data.Length / FinderIconMapEntry.Size;
        var entries = new List<FinderIconMapEntry>(entryCount);
        for (int i = 0; i < entryCount; i++)
        {
            entries.Add(new FinderIconMapEntry(data.Slice(offset, FinderIconMapEntry.Size)));
            offset += FinderIconMapEntry.Size;
        }

        Entries = entries;

        Debug.Assert(offset == data.Length, "Did not consume all data for Finder Icon Map Record.");
    }
}
