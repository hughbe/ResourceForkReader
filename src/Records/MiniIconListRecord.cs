using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a mini icon list resource ('ics#').
/// </summary>
public struct MiniIconListRecord
{
    /// <summary>
    /// The size of an icon list record in bytes.
    /// </summary>
    public const int Size = 48;

    /// <summary>
    /// Gets the icon list data.
    /// </summary>
    public List<byte[]> Icons { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MiniIconListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 64 bytes of icon list data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 64 bytes long.</exception>
    public MiniIconListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        Icons = SpanUtilities.ReadMonochromeImageList(data, 16, 12, out var bytesRead);
        offset += bytesRead;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for IconListRecord.");
    }
}
