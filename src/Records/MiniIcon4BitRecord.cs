using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an mini 4 bit icon resource ('icl4').
/// </summary>
public readonly struct MiniIcon4BitRecord
{
    /// <summary>
    /// The size of an icon record in bytes.
    /// </summary>
    public const int Size = 96;

    /// <summary>
    /// Gets the icon data.
    /// </summary>
    public byte[] IconData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MiniIcon4BitRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 96 bytes of icon data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 96 bytes long.</exception>
    public MiniIcon4BitRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        IconData = data.Slice(offset, 96).ToArray();
        offset += IconData.Length;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for IconRecord.");
    }
}