using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Command Keys Record ('CMDK') in a resource fork.
/// </summary>
public readonly struct CommandKeysRecord
{
    /// <summary>
    /// The minimum size of a Command Keys Record in bytes.
    /// </summary>
    public const int MinSize = 1;

    /// <summary>
    /// Gets the command keys string.
    /// </summary>
    public string CommandKeys { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandKeysRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 1 byte of Command Keys Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 1 byte long.</exception>
    public CommandKeysRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Command Keys Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L317-L319
        int offset = 0;

        CommandKeys = SpanUtilities.ReadPascalString(data[offset..]);
        offset += 1 + CommandKeys.Length;

        Debug.Assert(offset == data.Length, "Parsed beyond the end of the Command Keys Record data.");
    }
}
