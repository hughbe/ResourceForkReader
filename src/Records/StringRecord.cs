using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Pascal-style string resource with a length prefix.
/// </summary>
public readonly struct StringRecord
{
    /// <summary>
    /// Gets the length of the string in bytes.
    /// </summary>
    public byte Length { get; }

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
        if (data.Length == 0)
        {
            throw new ArgumentException("Data must be at least 1 byte long.", nameof(data));
        }

        Length = data[0];

        if (data.Length < 1 + Length)
        {
            throw new ArgumentException("Data is too short for the specified string length.", nameof(data));
        }

        Value = Encoding.ASCII.GetString(data.Slice(1, Length));
    }
}
