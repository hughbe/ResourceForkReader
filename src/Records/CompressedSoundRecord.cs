using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a compressed sound ('csnd') record in a resource fork.
/// </summary>
public readonly struct CompressedSoundRecord
{
    /// <summary>
    /// Minimum size of a CompressedSound record in bytes.
    /// </summary>
    public const int MinSize = 4;

    /// <summary>
    /// Gets the sample type.
    /// </summary>
    public byte SampleType { get; }

    /// <summary>
    /// Gets the decompressed size.
    /// </summary>
    public uint DecompressedSize { get;  }

    /// <summary>
    /// Gets the decompressed sound data.
    /// </summary>
    public byte[] DecompressedData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompressedSoundRecord"/> struct.
    /// </summary>
    /// <param name="data">The record data.</param>
    /// <exception cref="FormatException">Thrown if the data is invalid.</exception>
    public CompressedSoundRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new FormatException($"CompressedSound record must be at least {MinSize} bytes.");
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/ResourceFile.cc#L3570
        int offset = 0;

        var typeAndSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        SampleType = (byte)(typeAndSize >> 24);
        if (SampleType > 3 && SampleType != 0xFF)
        {
            throw new FormatException($"Invalid CompressedSound Type: {SampleType}");
        }

        DecompressedSize = typeAndSize & 0x00FFFFFF;
        if (SampleType != 0xFF)
        {
            // For types 1 and 2, sample size must be a multiple of 2.
            // For type 3, sample size must be a multiple of 4.
            var sampleBytes = SampleType == 2 ? SampleType : SampleType + 1;
            if (DecompressedSize % sampleBytes != 0)
            {
                throw new FormatException("Decompressed size is not aligned to sample size.");
            }
        }

        DecompressedData = new byte[DecompressedSize];
        LZSSUtilities.Decompress(data[offset..], DecompressedData);

        Debug.Assert(offset <= data.Length, "Offset should not exceed data length.");
    }
}
