using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font List Family Font in a resource fork.
/// </summary>
public readonly struct FontListFamilyFont
{
    /// <summary>
    /// Size of a Font List Family Font in bytes.
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Gets the point size.
    /// </summary>
    public ushort PointSize { get; }

    /// <summary>
    /// Gets the style flags.
    /// </summary>
    public ushort StyleFlags { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontListFamilyFont"/> struct.
    /// </summary>
    /// <param name="data">The data for the Font List Family Font.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not equal to the size.</exception>
    public FontListFamilyFont(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1078-L1087
        int offset = 0;

        PointSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        StyleFlags = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for FontListFamilyFont.");
    }
}
