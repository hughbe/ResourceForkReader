using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

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
    public FileObjectRecordType Type { get; }

    /// <summary>
    /// Gets the vertical position of the icon in the parent window.
    /// </summary>
    public ushort ParentLocationX { get; }

    /// <summary>
    /// Gets the horizontal position of the icon in the parent window.
    /// </summary>
    public ushort ParentLocationY { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 6.
    /// </summary>
    public ushort Reserved1 { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 8.
    /// </summary>
    public ushort Reserved2 { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 10.
    /// </summary>
    public ushort Reserved3 { get; }

    /// <summary>
    /// Gets the parent folder FOBJ resource ID.
    /// </summary>
    public ushort Parent { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 14.
    /// </summary>
    public ushort Reserved4 { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 16.
    /// </summary>
    public ushort Reserved5 { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 18.
    /// </summary>
    public ushort Reserved6 { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 20.
    /// </summary>
    public ushort Reserved7 { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 22.
    /// </summary>
    public ushort Reserved8 { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 24.
    /// </summary>
    public ushort Reserved9 { get; }

    /// <summary>
    /// Gets the creation date (Mac epoch).
    /// </summary>
    public DateTime CreationDate { get; }

    /// <summary>
    /// Gets the modification date (Mac epoch).
    /// </summary>
    public DateTime LastModificationDate { get; }

    /// <summary>
    /// Gets the reserved bytes at offset 34 (4 bytes, possibly backup date).
    /// </summary>
    public uint Reserved10 { get; }

    /// <summary>
    /// Gets the Finder flags.
    /// </summary>
    public FileObjectRecordFlags Flags { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileObjectRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the file object data.</param>
    public FileObjectRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException("Data is too ushort to be a valid FileObjectRecord.", nameof(data));
        }

        int offset = 0;

        // +0: Type (2 bytes)
        Type = (FileObjectRecordType)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +2: Icon position - vertical (2 bytes)
        ParentLocationX = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +4: Icon position - horizontal (2 bytes)
        ParentLocationY = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +6: Reserved1 (6 bytes)
        Reserved1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved2 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved3 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +12: Parent folder FOBJ resource ID (2 bytes)
        Parent = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +14: Reserved2 (12 bytes)
        Reserved4 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved5 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved6 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved7 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved8 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved9 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +26: Creation date (4 bytes)
        CreationDate = SpanUtilities.ReadMacOSTimestamp(data.Slice(offset, 4));
        offset += 4;

        // +30: Modification date (4 bytes)
        LastModificationDate = SpanUtilities.ReadMacOSTimestamp(data.Slice(offset, 4));
        offset += 4;

        // +34: Reserved10 (4 bytes, possibly backup date)
        Reserved10 = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // +38: Finder flags (2 bytes)
        Flags = (FileObjectRecordFlags)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // +40: Variable tail - ignored

        Debug.Assert(offset == MinSize, "Expected to parse exactly 40 bytes of fixed data.");
    }
}

