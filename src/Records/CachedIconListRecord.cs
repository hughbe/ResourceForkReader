using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;


/// <summary>
/// Represents a Cached Icon List Record ('clst') in a resource fork.
/// </summary>
public readonly struct CachedIconListRecord
{
    /// <summary>
    /// The minimum size of a Cached Icon List Record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of entries in the cached icon list.
    /// </summary>
    public ushort NumberOfEntries { get; }

    /// <summary>
    /// Gets the list of cached icon list entries.
    /// </summary>
    public List<CachedIconListEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedIconListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 2 bytes of Cached Icon List Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 2 bytes long.</exception>
    public CachedIconListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Cached Icon List Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure unknown but mentioned in https://vintageapple.org/macprogramming/pdf/Programmers_Guide_to_MPW_1990.pdf
        // page 368.
        int offset = 0;

        NumberOfEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var entries = new List<CachedIconListEntry>(NumberOfEntries);
        for (int i = 0; i < NumberOfEntries; i++)
        {
            entries.Add(new CachedIconListEntry(data.Slice(offset, CachedIconListEntry.Size)));
            offset += CachedIconListEntry.Size;
        }

        Entries = entries;

        Debug.Assert(offset <= data.Length, "Did not consume all bytes for CachedIconListRecord.");
    }
}
