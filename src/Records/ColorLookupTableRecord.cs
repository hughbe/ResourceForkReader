using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Color Lookup Table Resource ('clut').
/// </summary>
public readonly struct ColorLookupTableRecord
{
    /// <summary>
    /// Gets the minimum size of a Color Lookup Table Record in bytes.
    /// </summary>
    public const int MinSize = 8;

    /// <summary>
    /// Gets the color table.
    /// </summary>
    public ColorTable ColorTable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorLookupTableRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the color lookup table data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to be a valid Color Lookup Table Record.</exception>
    public ColorLookupTableRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException("Data is too short to be a valid Color Lookup Table Record.", nameof(data));
        }

        int offset = 0;
        
        ColorTable = new ColorTable(data[offset..], out var bytesRead);
        offset += bytesRead;

        Debug.Assert(offset == data.Length, "Did not consume all data for Color Lookup Table Record.");
    }
}
