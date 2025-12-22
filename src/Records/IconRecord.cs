using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an icon resource ('ICON').
/// </summary>
public readonly struct IconRecord
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
    /// Initializes a new instance of the <see cref="IconRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 128 bytes of icon data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 128 bytes long.</exception>
    public IconRecord(ReadOnlySpan<byte> data)
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