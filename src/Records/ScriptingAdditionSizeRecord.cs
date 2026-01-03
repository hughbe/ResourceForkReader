using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Scripting Addition Size Record ('osiz') in a resource fork.
/// </summary>
public readonly struct ScriptingAdditionSizeRecord
{
    /// <summary>
    /// The size of a Scripting Size Record in bytes.
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Gets the flags for the Scripting Size Record.
    /// </summary>
    public ScriptingSizeRecordFlags Flags { get; }

    /// <summary>
    /// Parses a Scripting Addition Size Record from the given data.
    /// </summary>
    /// <param name="data">The data to parse.</param>
    /// <exception cref="ArgumentException">>Thrown if the data is not the correct length.</exception>
    public ScriptingAdditionSizeRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be {Size} bytes in length.", nameof(data));
        }

        // Structure documented in https://applescriptlibrary.wordpress.com/wp-content/uploads/2013/11/applescript-scripting-additions-guide.pdf
        // page 89
        int offset = 0;

        Flags = (ScriptingSizeRecordFlags)BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == data.Length, "Did not consume all data.");
    }
}
