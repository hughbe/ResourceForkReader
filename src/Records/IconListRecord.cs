using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a large icon list resource ('ICN#').
/// </summary>
public struct IconListRecord
{
    /// <summary>
    /// The minimum size of an icon list record in bytes.
    /// </summary>
    public const int MinSize = 128;

    /// <summary>
    /// Gets the icon list data.
    /// </summary>
    public List<byte[]> Icons { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IconListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 128 bytes of icon list data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not at least 128 bytes long.</exception>
    public IconListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        int offset = 0;

        Icons = SpanUtilities.ReadMonochromeImageList(data, 32, 32, out var bytesRead);
        offset += bytesRead;

        // Seen some icon lists with extra data at the end; ignore it.
        Debug.Assert(offset <= data.Length, "Did not consume all bytes for IconListRecord.");
    }
}
