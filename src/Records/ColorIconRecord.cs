namespace ResourceForkReader.Records;

/// <summary>
/// Represents a color icon resource ('cicn').
/// </summary>
public readonly struct ColorIconRecord
{
    /// <summary>
    /// Gets the icon data.
    /// </summary>
    public byte[] IconData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorIconRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 128 bytes of icon data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 128 bytes long.</exception>
    public ColorIconRecord(ReadOnlySpan<byte> data)
    {
        // TODO - not implemented
        IconData = data.ToArray();
    }
}