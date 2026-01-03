using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an large 8 bit icon resource ('icl8').
/// </summary>
public readonly struct LargeIcon8BitRecord
{
    /// <summary>
    /// The size of an icon record in bytes.
    /// </summary>
    public const int Size = 1024;

    /// <summary>
    /// Gets the icon data.
    /// </summary>
    public byte[] IconData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LargeIcon8BitRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 1024 bytes of icon data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 1024 bytes long.</exception>
    public LargeIcon8BitRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;
        IconData = data[..1024].ToArray();
        offset += 1024;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for IconRecord.");
    }
}