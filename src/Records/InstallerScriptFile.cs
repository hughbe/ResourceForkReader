using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Script File in an Installer Script Record ('insc') in a resource fork.
/// </summary>
public readonly struct InstallerScriptFile
{
    /// <summary>
    /// The minimum size of an Installer Script File in bytes.
    /// </summary>
    public const int MinSize = 27;

    /// <summary>
    /// Gets the file spec resource ID.
    /// </summary>
    public ushort FileSpecResourceID { get; }

    /// <summary>
    /// Gets the type.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the creator.
    /// </summary>
    public string Creator { get; }

    /// <summary>
    /// Gets the creation date.
    /// </summary>
    public DateTime CreationDate { get; }

    /// <summary>
    /// Gets the handle.
    /// </summary>
    public uint Handle { get; }

    /// <summary>
    /// Gets the delete size.
    /// </summary>
    public uint DeleteSize { get; }

    /// <summary>
    /// Gets the add size.
    /// </summary>
    public uint AddSize { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerScriptFile"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 27 bytes of Installer Script File data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 27 bytes long.</exception>
    public InstallerScriptFile(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Installer Script File. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L654C22-L703
        int offset = 0;

        FileSpecResourceID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        Creator = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        CreationDate = SpanUtilities.ReadMacOSTimestamp(data.Slice(offset, 4));
        offset += 4;

        Handle = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        DeleteSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        AddSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Name = SpanUtilities.ReadPascalString(data.Slice(offset));
        offset += 1 + Name.Length;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Parsed beyond the end of the data span.");
    }
}
