using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Script Record ('insc') in a resource fork.
/// </summary>
public readonly struct InstallerScriptRecord
{
    /// <summary>
    /// Gets the version.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// Gets the flags.
    /// </summary>
    public ushort Flags { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the help string.
    /// </summary>
    public string HelpString { get; }

    /// <summary>
    /// Gets the number of files.
    /// </summary>
    public ushort NumberOfFiles { get; }

    /// <summary>
    /// Gets the list of installer script files.
    /// </summary>
    public List<InstallerScriptFile> Files { get; }

    /// <summary>
    /// Gets the list of installer script resource files.
    /// </summary>
    public ushort NumberOfResourceFiles { get; }

    /// <summary>
    /// Gets the list of installer script resource files.
    /// </summary>
    public List<InstallerScriptResourceFile> ResourceFiles { get; }

    /// <summary>
    /// Gets the remaining data in the Installer Script Record.
    /// </summary>
    public byte[] RemainingData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerScriptRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Installer Script Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to be a valid Installer Script Record.</exception>
    public InstallerScriptRecord(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L654C22-L703
        int offset = 0;

        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Flags = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Name = SpanUtilities.ReadPascalString(data[offset..], out var nameBytesRead);
        offset += nameBytesRead;

        if (offset % 2 != 0)
        {
            // Align to even byte boundary.
            offset++;
        }

        HelpString = SpanUtilities.ReadPascalStringWordCount(data[offset..], out var bytesRead);
        offset += bytesRead;

        if (offset % 2 != 0)
        {
            // Align to even byte boundary.
            offset++;
        }

        NumberOfFiles = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var files = new List<InstallerScriptFile>(NumberOfFiles);
        for (int i = 0; i < NumberOfFiles; i++)
        {
            files.Add(new InstallerScriptFile(data[offset..], out var fileBytesRead));
            offset += fileBytesRead;

            if (offset % 2 != 0)
            {
                // Align to even byte boundary.
                offset++;
            }
        }

        Files = files;

        NumberOfResourceFiles = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var resourceFiles = new List<InstallerScriptResourceFile>(NumberOfResourceFiles);
        for (int i = 0; i < NumberOfResourceFiles; i++)
        {
            resourceFiles.Add(new InstallerScriptResourceFile(data[offset..], out var resourceFileBytesRead));
            offset += resourceFileBytesRead;
            
            if (offset % 2 != 0)
            {
                // Align to even byte boundary.
                offset++;
            }
        }

        ResourceFiles = resourceFiles;

        RemainingData = data[offset..].ToArray();
        offset += RemainingData.Length;

        Debug.Assert(offset == data.Length, "Did not consume all data for Installer Script Record.");
    }
}
