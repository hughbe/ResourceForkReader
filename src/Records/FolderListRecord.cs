using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Folder List Record ('fld#') in a resource fork.
/// </summary>
public readonly struct FolderListRecord
{
    /// <summary>
    /// The folders in the Folder List.
    /// </summary>
    public List<FolderListEntry> Folders { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FolderListRecord"/> struct from the given data.
    /// </summary>
    /// <param name="data">The byte span containing the Folder List data.</param>
    public FolderListRecord(ReadOnlySpan<byte> data)
    {
        // Structured documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L430-L439
        int offset = 0;

        var folders = new List<FolderListEntry>();
        while (offset < data.Length)
        {
            folders.Add(new FolderListEntry(data[offset..], out var bytesRead));
            offset += bytesRead;

            if (offset % 2 != 0)
            {
                // Align to even boundary.
                offset++;
            }
        }

        Folders = folders;

        Debug.Assert(offset == data.Length, "Did not consume all data for Folder List Record.");
    }
}