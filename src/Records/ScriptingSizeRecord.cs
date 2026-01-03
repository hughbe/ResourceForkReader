using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Scripting Size Record ('scsz') in a resource fork.
/// </summary>
public readonly struct ScriptingSizeRecord
{
    /// <summary>
    /// The size of a Scripting Size Record in bytes.
    /// </summary>
    public const int Size = 26;

    /// <summary>
    /// Gets the flags for the Scripting Size Record.
    /// </summary>
    public ScriptingSizeRecordFlags Flags { get; }

    /// <summary>
    /// Gets the minimum stack size in bytes.
    /// </summary>
    public uint MinimumStackSize { get; }

    /// <summary>
    /// Gets the preferred stack size in bytes.
    /// </summary>
    public uint PreferredStackSize { get; }

    /// <summary>
    /// Gets the maximum stack size in bytes.
    /// </summary>
    public uint MaximumStackSize { get; }

    /// <summary>
    /// Gets the minimum heap size in bytes.
    /// </summary>
    public uint MinimumHeapSize { get; }

    /// <summary>
    /// Gets the preferred heap size in bytes.
    /// </summary>
    public uint PreferredHeapSize { get; }

    /// <summary>
    /// Gets the maximum heap size in bytes.
    /// </summary>
    public uint MaximumHeapSize { get; }

    /// <summary>
    /// Parses a Scripting Size Record from the given data.
    /// </summary>
    /// <param name="data">The data to parse.</param>
    /// <exception cref="ArgumentException">>Thrown if the data is not the correct length.</exception>
    public ScriptingSizeRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be {Size} bytes in length.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Interapplication_Communication/AE_Term_Resources.pdf
        // 8-45 to 8-46.
        int offset = 0;

        Flags = (ScriptingSizeRecordFlags)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        MinimumStackSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        PreferredStackSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        MaximumStackSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        MinimumHeapSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        PreferredHeapSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        MaximumHeapSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == data.Length, "Did not consume all data.");
    }
}
