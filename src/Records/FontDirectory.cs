using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Directory in a resource fork.
/// </summary>
public readonly struct FontDirectory
{
    /// <summary>
    /// Minimum size of a Font Directory in bytes.
    /// </summary>
    public const int MinSize = 12;

    /// <summary>
    /// Gets the version of the font directory.
    /// </summary>
    public uint Version { get; }

    /// <summary>
    /// Gets the number of tables in the font directory.
    /// </summary>
    public ushort NumberOfTables { get; }

    /// <summary>
    /// Gets the search range of the font directory.
    /// </summary>
    public ushort SearchRange { get; }

    /// <summary>
    /// Gets the entry selector of the font directory.
    /// </summary>
    public ushort EntrySelector { get; }

    /// <summary>
    /// Gets the range shift of the font directory.
    /// </summary>
    public ushort RangeShift { get; }

    /// <summary>
    /// Gets the list of font directory entries (tables).
    /// </summary>
    public List<FontDirectoryEntry> Tables { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="FontDirectory"/> struct.
    /// </summary>
    /// <param name="data">The data for the Font Directory.</param>
    /// <param name="bytesRead"> The number of bytes read from the data.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public FontDirectory(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-74 to 4-76
        int offset = 0;

        //  Version. The version number of the font, given as a 32-bit fixed
        // point number. For version 1.0 of any font, this number is $00010000
        Version = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Number of tables. The number of tables in the outline font resource,
        // not counting the font directory or any subtables in the font. This is
        // an unsigned integer value.
        NumberOfTables = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Search range. An unsigned integer value that is used, along with the
        // entry selector and range shift values, to optimize a binary search
        // through the directory. 
        SearchRange = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Entry selector. An unsigned integer value that is used, along with
        // the search range and range shift values, to optimize a binary search
        // through the directory. 
        EntrySelector = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Range shift. An unsigned integer value that is used, along with the
        // search range and entry selector values, to optimize a binary search
        // through the directory.
        RangeShift = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var tables = new List<FontDirectoryEntry>(NumberOfTables);
        for (int i = 0; i < NumberOfTables; i++)
        {
            tables.Add(new FontDirectoryEntry(data.Slice(offset, FontDirectoryEntry.Size)));
            offset += FontDirectoryEntry.Size;
        }

        Tables = tables;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Not enough data for FontDirectory tables.");
    }
}
