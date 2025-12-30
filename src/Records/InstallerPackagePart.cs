using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Package Part in a resource fork.
/// </summary>
public readonly struct InstallerPackagePart
{
    /// <summary>
    /// The size of an Installer Package Part in bytes.
    /// </summary>
    public const int Size = 6;
    
    /// <summary>
    /// Gets the package part type.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the package part ID.
    /// </summary>
    public ushort ID { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerPackagePart"/> struct.
    /// </summary>
    /// <param name="data">A span containing the Installer Package Part data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid.</exception>
    public InstallerPackagePart(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // // Structure documented in http://www.bitsavers.org/pdf/apple/mac/blue/Apple_Blue_Book_vol2_1989.pdf
        // page 15 to 17
        // and https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L602-L622
        int offset = 0;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        ID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for InstallerPackagePart.");
    }
}
