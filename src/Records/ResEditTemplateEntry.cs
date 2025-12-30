using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Res Edit Template Entry.
/// </summary>
public readonly struct ResEditTemplateEntry
{
    /// <summary>
    /// The minimum size of a Res Edit Template Entry in bytes.
    /// </summary>
    public const int MinSize = 5;

    /// <summary>
    /// A label character for the template entry.
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// A type code indicating the resource type for the template entry.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResEditTemplateEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 5 bytes of Res Edit Template Entry data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 5 bytes long.</exception>
    public ResEditTemplateEntry(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Res Edit Template Entry. Minimum size is {MinSize} bytes.", nameof(data));
        }

        int offset = 0;

        Label = SpanUtilities.ReadPascalString(data[offset..]);
        offset += 1 + Label.Length;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all data for ResEditTemplateEntry.");
    }
}
