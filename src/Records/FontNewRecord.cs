namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font New Record ('NFNT') in a resource fork.
/// </summary>
public readonly struct FontNewRecord
{
    /// <summary>
    /// Gets the font data.
    /// </summary>
    public byte[] FontData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontNewRecord"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the font record data.</param>
    public FontNewRecord(ReadOnlySpan<byte> data)
    {
        // TODO - not yet implemented
        FontData = data.ToArray();
    }
}
