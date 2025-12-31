using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Script Resource in an Installer Script Record ('insc') in a resource fork.
/// </summary>
public readonly struct InstallerScriptResource
{
    /// <summary>
    /// The minimum size of an Installer Script Resource in bytes.
    /// </summary>
    public const int MinSize = 27;

    /// <summary>
    /// Gets the resource spec resource ID.
    /// </summary>
    public short ResourceSpecResourceID { get; }
    
    /// <summary>
    /// Gets the type.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the source resource ID.
    /// </summary>
    public short SourceResourceID { get; }

    /// <summary>
    /// Gets the target resource ID.
    /// </summary>
    public short TargetResourceID { get; }

    /// <summary>
    /// Gets the CRC version.
    /// </summary>
    public ushort CRCVersion { get; }

    /// <summary>
    /// Gets the reserved bytes.
    /// </summary>
    public byte[] Reserved { get; }

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
    /// Gets the previous CRCs.
    /// </summary>
    public ushort PreviousCRCs { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerScriptResource"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Installer Script Resource data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to be a valid Installer Script Resource.</exception>
    public InstallerScriptResource(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Installer Script Resource. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L654C22-L703
        int offset = 0;

        ResourceSpecResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        SourceResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TargetResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        CRCVersion = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved = data.Slice(offset, 6).ToArray();
        offset += Reserved.Length;

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
        
        PreviousCRCs = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        bytesRead = offset;
        Debug.Assert(bytesRead <= data.Length, "Bytes read exceeds data length.");
    }
}
