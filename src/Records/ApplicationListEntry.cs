using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a single entry in an application list resource ('APPL').
/// </summary>
public readonly struct ApplicationListEntry
{
    /// <summary>
    /// The minimum size of an application list entry in bytes.
    /// </summary>
    public const int MinSize = 9;

    /// <summary>
    /// Gets the creator code.
    /// </summary>
    public string Creator { get; }

    /// <summary>
    /// Gets the directory ID.
    /// </summary>
    public uint DirectoryID { get; }

    /// <summary>
    /// Gets the application name.
    /// </summary>
    public string Application { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationListEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the application list entry data.</param>
    /// <param name="bytesRead">Outputs the total number of bytes read from the span.</param>
    /// <exception cref="ArgumentException">>Thrown when data is too short.</exception>
    public ApplicationListEntry(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L289-L295
        int offset = 0;

        Creator = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        DirectoryID = BitConverter.ToUInt32(data.Slice(offset, 4).ToArray().Reverse().ToArray(), 0);
        offset += 4;

        byte appNameLength = data[offset];
        offset += 1;

        Application = Encoding.ASCII.GetString(data.Slice(offset, appNameLength));
        offset += appNameLength;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all bytes for ApplicationListEntry.");
    }
}
