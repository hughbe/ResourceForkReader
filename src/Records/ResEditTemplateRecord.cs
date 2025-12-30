namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Res Edit Template Record ('TMPL') in a resource fork.
/// </summary>
public readonly struct ResEditTemplateRecord
{
    /// <summary>
    /// Gets the template entries.
    /// </summary>
    public List<ResEditTemplateEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResEditTemplateRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Res Edit Template Record data.</param>
    public ResEditTemplateRecord(ReadOnlySpan<byte> data)
    {
        var entries = new List<ResEditTemplateEntry>();

        int offset = 0;
        while (offset < data.Length)
        {
            entries.Add(new ResEditTemplateEntry(data[offset..], out var bytesRead));
            offset += bytesRead;
        }

        Entries = entries;
    }
}
