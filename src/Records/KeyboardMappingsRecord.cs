using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Keyboard Mappings Record ('itlk') in a resource fork.
/// </summary>
public readonly struct KeyboardMappingsRecord
{
    /// <summary>
    /// Minimum size of a Keyboard Mapping Record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of entries.
    /// </summary>
    public ushort NumberOfMappings { get; }

    /// <summary>
    /// Gets the list of keyboard mapping entries.
    /// </summary>
    public List<KeyboardMapping> Mappings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardMappingsRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Keyboard Mapping Record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public KeyboardMappingsRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L810-L820
        int offset = 0;

        NumberOfMappings = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var mappings = new List<KeyboardMapping>(NumberOfMappings);
        for (int i = 0; i < NumberOfMappings; i++)
        {
            mappings.Add(new KeyboardMapping(data.Slice(offset, KeyboardMapping.Size)));
            offset += KeyboardMapping.Size;
        }

        Mappings = mappings;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for KeyboardMappingsRecord.");
    }
}
