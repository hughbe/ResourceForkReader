using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a string word count record ('WSTR') in a resource fork.
/// </summary>
public readonly struct StringWordCountRecord
{
    /// <summary>
    /// The minimum size of a string word count record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringWordCountRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 2 byte for length plus the string data.</param>
    /// <exception cref="ArgumentException">Thrown when data is empty or too short for the specified string length.</exception>
    public StringWordCountRecord(ReadOnlySpan<byte> data)
    {
        int offset = 0;

        Value = SpanUtilities.ReadPascalStringWordCount(data, out var valueBytesRead);
        offset += valueBytesRead;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for StringRecord.");
    }
}
