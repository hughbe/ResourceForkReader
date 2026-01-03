using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Key Map Record ('KMAP') in a resource fork.
/// </summary>
public readonly struct KeyMapRecord
{
    /// <summary>
    /// The minimum size of a Key Map Record in bytes.
    /// </summary>
    public const int MinSize = 134;

    /// <summary>
    /// The resource ID for this particular key-map resource.
    /// </summary>
    public short ResourceID { get; }

    /// <summary>
    /// The version number of this key-map resource format.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// A 128-byte table that contains virtual key codes.
    /// </summary>
    public byte[] KeyCodeMap { get; }

    /// <summary>
    /// The number of entries in the exception array.
    /// </summary>
    public ushort NumberOfExceptions { get; }

    /// <summary>
    /// An array of exception records, which map raw key
    /// codes to communication instructions.
    /// </summary>
    public List<KeyMapException> Exceptions { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyMapRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Key Map Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 134 bytes long.</exception>
    public KeyMapRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Key Map Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // C-12 to C-13
        int offset = 0;

        // ID. The resource ID for this particular key-map resource.
        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Version. The version number of this key-map resource format.
        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Key code map. A 128-byte table that contains virtual key codes.
        // At each byte offset into the table, the entry is the virtual
        // key code (plus possibly an exception entry flag) for the raw
        // key code whose value equals that offset.
        KeyCodeMap = data.Slice(offset, 128).ToArray();
        offset += KeyCodeMap.Length;

        // Count of exception records. The number of entries in the exception array.
        NumberOfExceptions = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Exception array. An array of exception records, which map raw key
        // codes to communication instructions.
        var exceptions = new List<KeyMapException>(NumberOfExceptions);
        for (int i = 0; i < NumberOfExceptions; i++)
        {
            exceptions.Add(new KeyMapException(data[offset..], out var bytesRead));
            offset += bytesRead;
        }

        Exceptions = exceptions;

        Debug.Assert(offset == data.Length, "Did not consume all data for Key Map Record.");
    }
}
