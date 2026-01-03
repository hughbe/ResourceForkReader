using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Printer Access Protocol Address Record ('PAPA') in a resource fork.
/// </summary>
public readonly struct PrinterAccessProtocolAddressRecord
{
    /// <summary>
    /// The minimum size of a Printer Access Protocol Address Record in bytes.
    /// </summary>
    public const int MinSize = 7;

    /// <summary>
    /// Gets the name of the printer.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the type of the printer.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the zone of the printer.
    /// </summary>
    public string Zone { get; }

    /// <summary>
    /// Gets the address block of the printer.
    /// </summary>
    public uint AddressBlock { get; }

    /// <summary>
    /// Gets the raw data of the printer address record.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PrinterAccessProtocolAddressRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Printer Access Protocol Address Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is less than the minimum size.</exception>
    public PrinterAccessProtocolAddressRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Printer Access Protocol Address Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in 
        int offset = 0;

        Name = SpanUtilities.ReadPascalString(data[offset..], out var nameBytesRead);
        offset += nameBytesRead;
        
        Type = SpanUtilities.ReadPascalString(data[offset..], out var typeBytesRead);
        offset += typeBytesRead;

        Zone = SpanUtilities.ReadPascalString(data[offset..], out var zoneBytesRead);
        offset += zoneBytesRead;

        AddressBlock = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Data = data[offset..].ToArray();
        offset += Data.Length;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for Printer Access Protocol Address Record.");
    }
}
