using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a string record ('STR ') in a resource fork.
/// </summary>
public readonly struct StringRecord
{
    /// <summary>
    /// Gets the string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets the remaining data after the string.
    /// </summary>
    public byte[] RemainingData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 1 byte for length plus the string data.</param>
    /// <exception cref="ArgumentException">Thrown when data is empty or too short for the specified string length.</exception>
    public StringRecord(ReadOnlySpan<byte> data)
    {
        int offset = 0;

        Value = SpanUtilities.ReadPascalString(data, out var valueBytesRead);
        offset += valueBytesRead;

        // Seen cases where string length is odd, followed by a padding byte.
        // Or there are additional zero bytes after the string.
        RemainingData = data[offset..].ToArray();
        offset += RemainingData.Length;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for StringRecord.");
    }
}
