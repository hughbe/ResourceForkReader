using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Script Resource File in an Installer Script Record ('insc') in a resource fork.
/// </summary>
public readonly struct InstallerScriptResourceFile
{
    /// <summary>
    /// The minimum size of an Installer Script Resource File in bytes.
    /// </summary>
    public const int MinSize = 29;

    /// <summary>
    /// Gets the file spec resource ID.
    /// </summary>
    public short FileSpecResourceID { get; }

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
    /// Gets the number of from files.
    /// </summary>
    public ushort NumberOfFromFiles { get; }

    /// <summary>
    /// Gets the list of installer script from files.
    /// </summary>
    public List<InstallerScriptFromFile> FromFiles { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerScriptResourceFile"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 29 bytes of Installer Script File data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 29 bytes long.</exception>
    public InstallerScriptResourceFile(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Installer Script File. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L654C22-L703
        int offset = 0;

        FileSpecResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
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

        Name = SpanUtilities.ReadPascalString(data.Slice(offset), out var nameBytesRead);
        offset += nameBytesRead;

        if (offset % 2 != 0)
        {
            // Align to even byte boundary.
            offset++;
        }

        NumberOfFromFiles = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var fromFiles = new List<InstallerScriptFromFile>();
        for (int i = 0; i < NumberOfFromFiles; i++)
        {
            fromFiles.Add(new InstallerScriptFromFile(data.Slice(offset), out var fromFileBytesRead));
            offset += fromFileBytesRead;

            if (offset % 2 != 0)
            {
                // Align to even byte boundary.
                offset++;
            }
        }

        FromFiles = fromFiles;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Parsed beyond the end of the data span.");
    }
}
