using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an application list resource ('APPL') in a resource fork.
/// </summary>
public readonly struct ApplicationListRecord
{
    /// <summary>
    /// Gets the list of application entries.
    /// </summary>
    public List<ApplicationListEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the application list data.</param>
    /// <exception cref="ArgumentException">Thrown when data is invalid.</exception>
    public ApplicationListRecord(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L289-L295
        int offset = 0;

        var entries = new List<ApplicationListEntry>();
        while (offset < data.Length)
        {
            entries.Add(new ApplicationListEntry(data.Slice(offset), out var bytesRead));
            offset += bytesRead;
            
            if (offset % 2 != 0)
            {
                // Align to even byte boundary
                offset += 1;
            }
        }

        Entries = entries;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for ApplicationListRecord.");
    }
}
