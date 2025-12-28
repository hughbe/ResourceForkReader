namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Record ('FONT') in a resource fork.
/// </summary>
public readonly struct FontRecord
{
    /// <summary>
    /// Gets the font data.
    /// </summary>
    public byte[] FontData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontRecord"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the font record data.</param>
    public FontRecord(ReadOnlySpan<byte> data)
    {
        // TODO - not yet implemented
        FontData = data.ToArray();
    }
}
