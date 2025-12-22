namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Color Lookup Table Resource ('clut').
/// </summary>
public readonly struct ColorLookupTableRecord
{
    /// <summary>
    /// Gets the color lookup table data.
    /// </summary>
    public byte[] ColorData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorLookupTableRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the color lookup table data.</param>
    public ColorLookupTableRecord(ReadOnlySpan<byte> data)
    {
        ColorData = data.ToArray();
    }
}
