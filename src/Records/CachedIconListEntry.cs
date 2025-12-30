using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Cached Icon List Entry in a Cached Icon List Record ('clst') in a resource fork.
/// </summary>
public readonly struct CachedIconListEntry
{
    /// <summary>
    /// The size of a Cached Icon List Entry in bytes.
    /// </summary>
    public const int Size = 270;

    /// <summary>
    /// Gets the unknown field.
    /// </summary>
    public ushort Unknown1 { get; }

    /// <summary>
    /// Gets the type of the cached icon list entry.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the unknown field.
    /// </summary>
    public uint Unknown2 { get; }

    /// <summary>
    /// Gets the unknown field.
    /// </summary>
    public ushort Unknown3 { get; }

    /// <summary>
    /// Gets the icon list data.
    /// </summary>
    public byte[] IconData { get; }

    /// <summary>
    /// Gets the mask data for the icon.
    /// </summary>
    public byte[] MaskData { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CachedIconListEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Cached Icon List Entry data.</param>
    public CachedIconListEntry(ReadOnlySpan<byte> data)
    {
        // Structure unknown but mentioned in https://vintageapple.org/macprogramming/pdf/Programmers_Guide_to_MPW_1990.pdf
        // page 368.
        int offset = 0;

        Unknown1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        Unknown2 = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Unknown3 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        IconData = data.Slice(offset, 128).ToArray();
        offset += IconData.Length;

        MaskData = data.Slice(offset, 128).ToArray();
        offset += MaskData.Length;

        Debug.Assert(offset <= data.Length, "Did not consume all bytes for CachedIconListEntry.");
    }
}
