using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Kind Record ('kind') in a resource fork.
/// </summary>
public readonly struct KindRecord
{
    /// <summary>
    /// Minimum size of a Kind Record in bytes.
    /// </summary>
    public const int MinSize = 10;

    /// <summary>
    /// Gets the application signature.
    /// </summary>    
    public string ApplicationSignature { get; }

    /// <summary>
    /// Gets the region code.
    /// </summary>
    public ushort RegionCode { get; }

    /// <summary>
    /// Gets the filler.
    /// </summary>
    public ushort Filler { get; }

    /// <summary>
    /// Gets the number of kinds.
    /// </summary>
    public ushort NumberOfKinds { get; }

    /// <summary>
    /// Gets the list of Kind Entries.
    /// </summary>
    public List<KindEntry> Kinds { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KindRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Kind Record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is less than the minimum size.</exception>
    public KindRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length must be at least {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 7-75
        int offset = 0;

        ApplicationSignature = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        RegionCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Filler = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        NumberOfKinds = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var kinds = new List<KindEntry>(NumberOfKinds);
        for (int i = 0; i < NumberOfKinds; i++)
        {
            kinds.Add(new KindEntry(data[offset..], out int bytesRead));
            offset += bytesRead;

            if (offset % 2 != 0)
            {
                // Align to even boundary
                offset += 1;
            }
        }

        Kinds = kinds;

        Debug.Assert(offset == data.Length, "Did not consume all data for Kind Record.");
    }
}
