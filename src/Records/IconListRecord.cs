using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a large icon list resource ('ICN#').
/// </summary>
public struct IconListRecord
{
    /// <summary>
    /// The size of an icon list record in bytes.
    /// </summary>
    public const int Size = 256;

    /// <summary>
    /// Gets the icon list data.
    /// </summary>
    public byte[] IconData { get; }

    /// <summary>
    /// Gets the mask data for the icon.
    /// </summary>
    public byte[] MaskData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IconListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 256 bytes of icon list data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 256 bytes long.</exception>
    public IconListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        IconData = data.Slice(offset, 128).ToArray();
        offset += IconData.Length;

        MaskData = data.Slice(offset, 128).ToArray();
        offset += MaskData.Length;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for IconListRecord.");
    }
}
