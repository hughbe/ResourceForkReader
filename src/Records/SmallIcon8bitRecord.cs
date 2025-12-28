using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a small 8 bit icon resource ('ics8').
/// </summary>
public readonly struct SmallIcon8bitRecord
{
    /// <summary>
    /// The size of an icon record in bytes.
    /// </summary>
    public const int Size = 256;

    /// <summary>
    /// Gets the icon data.
    /// </summary>
    public byte[] IconData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmallIcon8bitRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 256 bytes of icon data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 256 bytes long.</exception>
    public SmallIcon8bitRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;
        IconData = data[..256].ToArray();
        offset += 256;

        Debug.Assert(offset == Size, "Did not consume all bytes for IconRecord.");
    }
}