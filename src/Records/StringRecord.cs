using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Pascal-style string resource with a length prefix.
/// </summary>
public readonly struct StringRecord
{
    /// <summary>
    /// Gets the string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 1 byte for length plus the string data.</param>
    /// <exception cref="ArgumentException">Thrown when data is empty or too short for the specified string length.</exception>
    public StringRecord(ReadOnlySpan<byte> data)
    {
        int offset = 0;

        Value = SpanUtilities.ReadPascalString(data);
        offset += 1 + Value.Length;

        Debug.Assert(offset == data.Length, "Did not consume all data for StringRecord.");
    }
}
