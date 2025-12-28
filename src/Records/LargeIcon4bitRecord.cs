using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an large 4 bit icon resource ('icl4').
/// </summary>
public readonly struct LargeIcon4bitRecord
{
    /// <summary>
    /// The size of an icon record in bytes.
    /// </summary>
    public const int Size = 512;

    /// <summary>
    /// Gets the icon data.
    /// </summary>
    public byte[] IconData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LargeIcon4bitRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 512 bytes of icon data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 512 bytes long.</exception>
    public LargeIcon4bitRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;
        IconData = data[..512].ToArray();
        offset += 512;

        Debug.Assert(offset == Size, "Did not consume all bytes for IconRecord.");
    }
}