using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Finder Version Record.
/// </summary>
public readonly struct FinderVersionRecord
{
    /// <summary>
    /// The minimum size of a Finder Version Record in bytes.
    /// </summary>
    public const int MinSize = 1;

    /// <summary>
    /// Gets the version string of the Finder Version Record.
    /// </summary>
    public string VersionString { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FinderVersionRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the Finder Version Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to contain the Finder Version Record.</exception>
    public FinderVersionRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure not documented.
        // But present in System 1.1 Finder resource fork.
        int offset = 0;

        VersionString = SpanUtilities.ReadPascalString(data.Slice(offset));
        offset += 1 + VersionString.Length;

        Debug.Assert(offset == data.Length, "Did not consume all data for FinderVersionRecord.");
    }
}
