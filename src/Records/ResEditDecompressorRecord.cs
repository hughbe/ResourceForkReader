namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Res Edit Decompressor Record ('dcmp') in a resource fork.
/// </summary>
public readonly struct ResEditDecompressorRecord
{
    /// <summary>
    /// Gets the decompressor data.
    /// </summary>
    public byte[] DecompressorData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResEditDecompressorRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Res Edit Decompressor Record data.</param>
    public ResEditDecompressorRecord(ReadOnlySpan<byte> data)
    {
        DecompressorData = data.ToArray();
    }
}
