using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Component List Record ('thn#') in a resource fork.
/// </summary>
public readonly struct ComponentListRecord
{
    /// <summary>
    /// Gets the list of Component List Entries in the record.
    /// </summary>
    public List<ComponentListEntry> Components { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentListRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Component List Record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not a multiple of the Component List Entry size.</exception>
    public ComponentListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length % ComponentListEntry.Size != 0)
        {
            throw new ArgumentException($"Data length must be a multiple of {ComponentListEntry.Size}.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1171-L1176
        int offset = 0;

        var numberOfEntries = data.Length / ComponentListEntry.Size;
        var components = new List<ComponentListEntry>(numberOfEntries);
        for (int i = 0; i < numberOfEntries; i++)
        {
            components.Add(new ComponentListEntry(data.Slice(offset, ComponentListEntry.Size)));
            offset += ComponentListEntry.Size;
        }

        Components = components;

        Debug.Assert(offset == data.Length, "Did not consume all data for Component List Record.");
    }
}