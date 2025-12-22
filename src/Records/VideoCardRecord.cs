using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// The 'card' resource type for video card resources.
/// </summary>
public readonly struct VideoCardRecord
{
    /// <summary>
    /// The minimum size of a video card record in bytes.
    /// </summary>
    public const int MinSize = 1;

    /// <summary>
    /// Gets the name of the video card.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoCardRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the video card record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to contain the video card record.</exception>
    public VideoCardRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 8-87
        int offset = 0;

        Name = SpanUtilities.ReadPascalString(data);
        offset += 1 + Name.Length;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for VideoCardRecord.");
    }
}
