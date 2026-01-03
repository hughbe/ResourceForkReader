namespace ResourceForkReader.Records;

/// <summary>
/// Represents a picture resource ('PICT').
/// </summary>
public readonly struct PictureRecord
{
    /// <summary>
    /// Gets the picture data.
    /// </summary>
    public byte[] PictureData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PictureRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the picture data.</param>
    public PictureRecord(ReadOnlySpan<byte> data)
    {
        PictureData = data.ToArray();
    }
}