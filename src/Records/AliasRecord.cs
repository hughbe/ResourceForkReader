using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Alias Record.
/// </summary>
public readonly struct AliasRecord
{
    /// <summary>
    /// The minimum size of an Alias Record in bytes.
    /// </summary>
    public const int MinSize = 6;

    /// <summary>
    /// The type of the Alias Record.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// The length of the private data.
    /// </summary>
    public ushort DataLength { get; }

    /// <summary>
    /// The private data of the Alias Record.
    /// </summary>
    public byte[] PrivateData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AliasRecord"/> struct from the given data.
    /// </summary>
    /// <param name="data">The byte span containing the Alias Record data.</param>
    /// <exception cref="ArgumentException">Thrown if the data length is less than the minimum size.</exception>
    public AliasRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than minimum size {MinSize} for Alias Record.", nameof(data));
        }

        // Structure documented in https://www.dubeyko.com/development/FileSystems/HFS/inside_macintosh/AliasManager.pdf
        // 4-4 to 4-5
        int offset = 0;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;
        
        DataLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (DataLength > data.Length)
        {
            throw new ArgumentException($"Data length {data.Length} is less than expected size {MinSize + DataLength} for Alias Record with DataLength {DataLength}.", nameof(data));
        }

        PrivateData = data.Slice(offset, DataLength - MinSize).ToArray();
        offset += PrivateData.Length;

        Debug.Assert(offset <= data.Length, "Did not consume all data for Alias Record.");
    }
}
