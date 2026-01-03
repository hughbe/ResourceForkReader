using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Location Table in a resource fork.
/// </summary>
public readonly struct LocationTable
{
    /// <summary>
    /// Gets the word offsets (format 0).
    /// </summary>
    public ushort[]? WordOffsets { get; }

    /// <summary>
    /// Gets the long offsets (format 1).
    /// </summary>
    public uint[]? LongOffsets { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocationTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Location Table data.</param>
    /// <param name="format"> The format of the Location Table (0 for short offsets, 1 for long offsets).</param>
    public LocationTable(ReadOnlySpan<byte> data, int format)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-84
        int offset = 0;

        if (format == 0)
        {
            if (data.Length % 2 != 0)
            {
                throw new ArgumentException("Data length must be even to accommodate Word Offsets in a Location Table with format 0.", nameof(data));
            }
            
            int count = data.Length / 2;
            var wordOffsets = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                wordOffsets[i] = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
                offset += 2;
            }

            WordOffsets = wordOffsets;
            LongOffsets = null;
        }
        else
        {
            if (data.Length % 4 != 0)
            {
                throw new ArgumentException("Data length must be a multiple of 4 to accommodate Long Offsets in a Location Table with format 1.", nameof(data));
            }
 
            int count = data.Length / 4;
            var longOffsets = new uint[count];
            for (int i = 0; i < count; i++)
            {
                longOffsets[i] = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
                offset += 4;
            }

            LongOffsets = longOffsets;
            WordOffsets = null;
        }
        
        Debug.Assert(offset == data.Length, "Offset should equal data length after reading Location Table.");
    }
}
