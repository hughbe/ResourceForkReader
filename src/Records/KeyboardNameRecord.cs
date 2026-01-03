using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Keyboard Name Record ('KBDN') in a resource fork.
/// </summary>
public readonly struct KeyboardNameRecord
{
    /// <summary>
    /// The minimum size of a Keyboard Name Record in bytes.
    /// </summary>
    public const int MinSize = 1;

    /// <summary>
    /// Gets the keyboard name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardNameRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 1 byte of Keyboard Name Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 1 byte long.</exception>
    public KeyboardNameRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        int offset = 0;

        Name = SpanUtilities.ReadPascalString(data[offset..], out var nameBytesRead);
        offset += nameBytesRead;

        Debug.Assert(offset == data.Length, "Did not consume all data for Keyboard Name Record.");
    }
}
