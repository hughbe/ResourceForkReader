using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Information Record in a classic Macintosh resource fork.
/// </summary>
public readonly struct FontInformationRecord
{
    /// <summary>
    /// Gets the minimum size of the FontInformationRecord in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of entries in the FontInformationRecord.
    /// </summary>
    public ushort NumberOfEntries { get; }

    /// <summary>
    /// Gets the entries in the FontInformationRecord.
    /// </summary>
    public List<FontInformationEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontInformationRecord"/> struct.
    /// </summary>
    /// <param name="data"></param>
    /// <exception cref="ArgumentException"></exception>
    public FontInformationRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 8-86 to 8-87
        // But it doesn't include just a single entry, it includes a count and multiple entries.
        // See https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/ResourceFile.cc#L4709-L4725
        int offset = 0;
        
        NumberOfEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var entries = new List<FontInformationEntry>(NumberOfEntries);
        for (int i = 0; i < NumberOfEntries; i++)
        {
            entries.Add(new FontInformationEntry(data.Slice(offset, FontInformationEntry.Size)));
            offset += FontInformationEntry.Size;
        }

        Entries = entries;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for FontInformationRecord");
    }
}
