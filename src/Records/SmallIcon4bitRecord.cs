using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a small 4 bit icon resource ('ics4').
/// </summary>
public readonly struct SmallIcon4bitRecord
{
    /// <summary>
    /// The size of an icon record in bytes.
    /// </summary>
    public const int Size = 128;

    /// <summary>
    /// Gets the icon data.
    /// </summary>
    public byte[] IconData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmallIcon4bitRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 128 bytes of icon data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 128 bytes long.</exception>
    public SmallIcon4bitRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;
        IconData = data[..128].ToArray();
        offset += 128;

        Debug.Assert(offset == Size, "Did not consume all bytes for IconRecord.");
    }
}