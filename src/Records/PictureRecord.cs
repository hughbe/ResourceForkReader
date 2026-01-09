using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a picture resource ('PICT').
/// </summary>
public readonly struct PictureRecord
{
    /// <summary>
    /// The minimum size of a PictureRecord in bytes.
    /// </summary>
    public const int MinSize = 10;

    /// <summary>
    /// Gets the picture header.
    /// </summary>
    public PictureHeader Header { get; }

    /// <summary>
    /// Gets the opcode data.
    /// </summary>
    public byte[] OpcodeData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PictureRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the picture data.</param>
    public PictureRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/ImagingWithQuickDraw.pdf
        // 7-68
        int offset = 0;

        Header = new PictureHeader(data.Slice(offset, PictureHeader.Size));
        offset += PictureHeader.Size;

        OpcodeData = data[offset..].ToArray();
        offset += OpcodeData.Length;

        Debug.Assert(offset == data.Length, "Did not consume all bytes in PictureRecord.");
    }
}