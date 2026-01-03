namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Bitmap Record ('BMAP') in a resource fork.
/// </summary>
public readonly struct BitmapRecord
{
    /// <summary>
    /// Gets the raw data of the Bitmap Record.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitmapRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Bitmap Record.</param>
    public BitmapRecord(ReadOnlySpan<byte> data)
    {
        Data = data.ToArray();
    }
}
