using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Psap Resource ('psap').
/// </summary>
public readonly struct PsapRecord
{
    /// <summary>
    /// The minimum size of a Psap Record in bytes.
    /// </summary>
    public const int MinSize = 1;

    /// <summary>
    /// Gets the string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PsapRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the Psap record data.</param>
    /// <exception cref="ArgumentException">>Thrown when the data is too short to contain a valid Psap Record.</exception>
    public PsapRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to contain a valid Psap Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/ResEditReference.pdf
        // But meaning unknown.
        int offset = 0;
        Value = SpanUtilities.ReadPascalString(data, out var bytesRead);
        offset += bytesRead;

        Debug.Assert(offset == data.Length, "Did not consume all data for Psap Record.");
    }
}
