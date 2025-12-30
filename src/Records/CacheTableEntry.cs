using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an entry in a Cache Table Record ('CTAB') in a resource fork.
/// </summary>
public readonly struct CacheTableEntry
{
    /// <summary>
    /// The size of a Cache Table Entry in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the cache number.
    /// </summary>
    public ushort Key { get; }

    /// <summary>
    /// Gets the first value.
    /// </summary>
    public ushort Value1 { get; }

    /// <summary>
    /// Gets the second value.
    /// </summary>
    public ushort Value2 { get; }

    /// <summary>
    /// Gets the third value.
    /// </summary>
    public ushort Value3 { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheTableEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Cache Table Entry data.</param>
    /// <exception cref="ArgumentException">Thrown when data is less than 8 bytes long.</exception>
    public CacheTableEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data is not the correct size for a Cache Table Entry. Expected size is {Size} bytes.", nameof(data));
        }

        // Structure documented but not hard to reverse engineer.
        int offset = 0;

        Key = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Value1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Value2 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Value3 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == Size, "Did not consume all bytes for Cache Table Entry.");
    }
}