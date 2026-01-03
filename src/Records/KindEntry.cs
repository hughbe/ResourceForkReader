using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Kind Entry in a Kind Record.
/// </summary>
public readonly struct KindEntry
{
    /// <summary>
    /// Minimum size of a Kind Entry in bytes.
    /// </summary>
    public const int MinSize = 5;

    /// <summary>
    /// Gets the file type.
    /// </summary>
    public string FileType { get; }

    /// <summary>
    /// Gets the kind string.
    /// </summary>
    public string KindString { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KindEntry"/> struct.
    /// </summary>
    /// <param name="data">The data for the Kind Entry.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public KindEntry(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length must be at least {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 7-75
        int offset = 0;

        FileType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        KindString = SpanUtilities.ReadPascalString(data[offset..], out int kindStringBytesRead);
        offset += kindStringBytesRead;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Read more data than available for Kind Entry.");
    }
}
