using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a small icon list resource ('ics#').
/// </summary>
public struct SmallIconListRecord
{
    /// <summary>
    /// The size of an icon list record in bytes.
    /// </summary>
    public const int Size = 64;

    /// <summary>
    /// Gets the icon list data.
    /// </summary>
    public byte[] IconData { get; }

    /// <summary>
    /// Gets the mask data for the icon.
    /// </summary>
    public byte[] MaskData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmallIconListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 64 bytes of icon list data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 64 bytes long.</exception>
    public SmallIconListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        IconData = data.Slice(offset, 32).ToArray();
        offset += IconData.Length;

        MaskData = data.Slice(offset, 32).ToArray();
        offset += MaskData.Length;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for IconListRecord.");
    }
}
