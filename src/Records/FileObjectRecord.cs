using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a file object ('FOBJ') in a resource fork.
/// </summary>
public struct FileObjectRecord
{
    /// <summary>
    /// The minimum size of a valid FileObjectRecord in bytes.
    /// </summary>
    public const int MinSize = 40;

    /// <summary>
    /// Gets the type of the file object (8 = folder, 4 = disk).
    /// </summary>
    public short Type { get; }

    /// <summary>
    /// Gets the vertical position of the icon in the parent window.
    /// </summary>
    public short IconPositionV { get; }

    /// <summary>
    /// Gets the horizontal position of the icon in the parent window.
    /// </summary>
    public short IconPositionH { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 6 (6 bytes).
    /// </summary>
    public byte[] Reserved1 { get; }

    /// <summary>
    /// Gets the parent folder FOBJ resource ID.
    /// </summary>
    public short Parent { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 14 (12 bytes).
    /// </summary>
    public byte[] Reserved2 { get; }

    /// <summary>
    /// Gets the creation date (Mac epoch).
    /// </summary>
    public uint CreationDate { get; }

    /// <summary>
    /// Gets the modification date (Mac epoch).
    /// </summary>
    public uint ModificationDate { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 34 (4 bytes, possibly backup date).
    /// </summary>
    public byte[] Reserved3 { get; }

    /// <summary>
    /// Gets the Finder flags.
    /// </summary>
    public ushort Flags { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileObjectRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the file object data.</param>
    public FileObjectRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException("Data is too short to be a valid FileObjectRecord.", nameof(data));
        }

        int offset = 0;

        // +0: Type (2 bytes)
        Type = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +2: Icon position - vertical (2 bytes)
        IconPositionV = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +4: Icon position - horizontal (2 bytes)
        IconPositionH = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +6: Reserved1 (6 bytes)
        Reserved1 = data.Slice(offset, 6).ToArray();
        offset += 6;

        // +12: Parent folder FOBJ resource ID (2 bytes)
        Parent = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +14: Reserved2 (12 bytes)
        Reserved2 = data.Slice(offset, 12).ToArray();
        offset += 12;

        // +26: Creation date (4 bytes)
        CreationDate = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // +30: Modification date (4 bytes)
        ModificationDate = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // +34: Reserved3 (4 bytes, possibly backup date)
        Reserved3 = data.Slice(offset, 4).ToArray();
        offset += 4;

        // +38: Finder flags (2 bytes)
        Flags = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +40: Variable tail - ignored

        Debug.Assert(offset == MinSize, "Expected to parse exactly 40 bytes of fixed data.");
    }
}

