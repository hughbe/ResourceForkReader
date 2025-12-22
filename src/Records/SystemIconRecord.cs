using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a system icon resource ('SICN').
/// </summary>
public readonly struct SystemIconRecord
{
    /// <summary>
    /// Gets the icon data.
    /// </summary>
    public byte[] IconData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SystemIconRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the system icon data.</param>
    public SystemIconRecord(ReadOnlySpan<byte> data)
    {
        IconData = data.ToArray();
    }
}