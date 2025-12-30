using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an system resource entry in an Installer software requirement.
/// </summary>
public readonly struct InstallerSystemResource
{
    /// <summary>
    /// The size of an Installer System Resource in bytes.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Gets the resource type.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the resource ID.
    /// </summary>
    public short ResourceID { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerSystemResource"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 6 bytes of Installer System Resource data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 6 bytes long.</exception>
    public InstallerSystemResource(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length {data.Length} is not equal to required size {Size} for Installer System Resource.", nameof(data));
        }

        int offset = 0;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for Installer System Resource.");
    }
}
