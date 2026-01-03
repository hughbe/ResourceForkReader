namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Glyph Data Table in a resource fork.
/// </summary>
public readonly struct GlyphDataTable
{
    /// <summary>
    /// Gets the list of Glyph Data Table Entries.
    /// </summary>
    public List<GlpyhDataTableEntry> Entries { get; }

    /// <summary>
    /// Gets the raw data of the Glyph Data Table.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GlyphDataTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Glyph Data Table data.</param>
    public GlyphDataTable(ReadOnlySpan<byte> data)
    {
        // Not sure how to parse each glyph entry data yet.
        Entries = [];
        RawData = data.ToArray();
    }   
}
