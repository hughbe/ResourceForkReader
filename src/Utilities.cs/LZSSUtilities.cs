using System.Buffers.Binary;

namespace ResourceForkReader.Utilities;

internal static class LZSSUtilities
{
    /// <summary>
    /// Decompresses LZSS-compressed data used by Sound Music System.
    /// </summary>
    /// <param name="source">The compressed source data.</param>
    /// <param name="destination">The destination span to write decompressed data to.</param>
    /// <returns>The number of bytes written to the destination.</returns>
    /// <exception cref="ArgumentException">Thrown when the destination is too small.</exception>
    public static int Decompress(ReadOnlySpan<byte> source, Span<byte> destination)
    {
        if (source.IsEmpty)
        {
            return 0;
        }

        var sourceIndex = 0;
        var outputLength = 0;

        while (sourceIndex < source.Length)
        {
            var controlBits = source[sourceIndex++];

            for (byte controlMask = 0x01; controlMask != 0; controlMask <<= 1)
            {
                if ((controlBits & controlMask) != 0)
                {
                    // Literal byte
                    if (sourceIndex >= source.Length)
                    {
                        return outputLength;
                    }

                    if (outputLength >= destination.Length)
                    {
                        throw new ArgumentException("Destination buffer is too small.", nameof(destination));
                    }

                    destination[outputLength++] = source[sourceIndex++];
                }
                else
                {
                    // Back-reference
                    if (sourceIndex >= source.Length - 1)
                    {
                        return outputLength;
                    }

                    var parameters = BinaryPrimitives.ReadUInt16BigEndian(source.Slice(sourceIndex, 2));
                    sourceIndex += 2;

                    var offset = (1 << 12) - (parameters & 0x0FFF);
                    var copyOffset = outputLength - offset;
                    var count = ((parameters >> 12) & 0x0F) + 3;

                    // Validate the back-reference is within bounds
                    if (copyOffset < 0 || copyOffset >= outputLength)
                    {
                        return outputLength;
                    }

                    if (outputLength + count > destination.Length)
                    {
                        throw new ArgumentException("Destination buffer is too small.", nameof(destination));
                    }

                    // Copy bytes one at a time since source and destination may overlap
                    var copyEndOffset = copyOffset + count;
                    for (var i = copyOffset; i < copyEndOffset; i++)
                    {
                        destination[outputLength++] = destination[i];
                    }
                }
            }
        }

        return outputLength;
    }
}
