using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Creation Date Record ('incd') in a resource fork.
/// </summary>
public readonly struct InstallerCreationDateRecord
{
    /// <summary>
    /// The size of an Installer Creation Date Record in bytes.
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Gets the creation date.
    /// </summary>
    public DateTime CreationDate { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerCreationDateRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 4 bytes of Installer Creation Date Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 4 bytes long.</exception>
    public InstallerCreationDateRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://www.docjava.com/posterous/file/2012/07/9621873-out3.pdf
        // page 219
        int offset = 0;

        CreationDate = SpanUtilities.ReadMacOSTimestamp(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == data.Length, "Did not consume all data for Installer Creation Date Record.");
    }
}