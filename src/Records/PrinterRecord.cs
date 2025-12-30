namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Printer Record ('PREC') in a resource fork.
/// </summary>
public readonly struct PrinterRecord
{
    /// <summary>
    /// Gets the private data of the Printer Record.
    /// </summary>
    public byte[] PrivateData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PrinterRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Printer Record data.</param>
    public PrinterRecord(ReadOnlySpan<byte> data)
    {
        PrivateData = data.ToArray();
    }
}
