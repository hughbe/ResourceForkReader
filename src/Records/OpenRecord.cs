using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Open Record ('open') in a resource fork.
/// </summary>
public readonly struct OpenRecord
{
    /// <summary>
    /// Minimum size of an Open Record in bytes.
    /// </summary>
    public const int MinSize = 4;

    /// <summary>
    /// Gets the application signature.
    /// </summary>
    public string ApplicationSignature { get; }

    /// <summary>
    /// Gets the list of file types.
    /// </summary>
    public List<string> FileTypes { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Open Record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public OpenRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length must be at least {MinSize} bytes.", nameof(data));
        }
        if (data.Length % 4 != 0)
        {
            throw new ArgumentException("Data length must be a multiple of 4 bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 7-44
        // But in practice appears to be:
        // Offset  Size    Description
        // 0       4       Application Signature (4-byte ASCII)
        // 4       ...     List of file types (each 4-byte ASCII), until end of data
        int offset = 0;

        ApplicationSignature = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        var count = (data.Length - offset) / 4;
        var fileTypes = new List<string>(count);
        for (int i = 0; i < count; i++)
        {
            fileTypes.Add(Encoding.ASCII.GetString(data.Slice(offset, 4)));
            offset += 4;
        }

        FileTypes = fileTypes;

        Debug.Assert(offset == data.Length, "Did not consume all data for Open Record.");
    }
}
