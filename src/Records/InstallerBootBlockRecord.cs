using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Boot Block Record ('inbb') in a resource fork.
/// </summary>
public readonly struct InstallerBootBlockRecord
{
    /// <summary>
    /// The minimum size of an Installer Boot Block Record in bytes.
    /// </summary>
    public const int MinSize = 6;

    /// <summary>
    /// Gets the format version.
    /// </summary>
    public ushort FormatVersion { get; }

    /// <summary>
    /// Gets the installer boot block flags.
    /// </summary>
    public InstallerBootBlockFlags Flags { get; }

    /// <summary>
    /// Gets the value key.
    /// </summary>
    public InstallerBootBlockValueKey ValueKey { get; }

    /// <summary>
    /// Gets the value data.
    /// </summary>
    public byte[] ValueData { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerBootBlockRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 6 bytes of Installer Boot Block Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 6 bytes long.</exception>
    public InstallerBootBlockRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in http://www.bitsavers.org/pdf/apple/mac/blue/Apple_Blue_Book_vol2_1989.pdf
        // page 25 to 27
        // and https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L534-L549
        int offset = 0;

        FormatVersion = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Flags = (InstallerBootBlockFlags)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ValueKey = (InstallerBootBlockValueKey)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ValueData = data[offset..].ToArray();
        offset += ValueData.Length;

        Debug.Assert(offset == data.Length, "Did not consume all data for InstallerBootBlockRecord.");
    }
}
