using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Res Edit Creator Signature Record ('CMDK') in a resource fork.
/// </summary>
public readonly struct ResEditCreatorSignatureRecord
{
    /// <summary>
    /// The minimum size of a Res Edit Creator Signature Record in bytes.
    /// </summary>
    public const int MinSize = 1;

    /// <summary>
    /// Gets the signature string.
    /// </summary>
    public string Signature { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResEditCreatorSignatureRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 1 byte of Res Edit Creator Signature Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 1 byte long.</exception>
    public ResEditCreatorSignatureRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Res Edit Creator Signature Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure not documented.
        // Mentioned in https://vintageapple.org/macprogramming/pdf/Power_Macintosh_Programming_Starter_kit_1994.pdf
        // p 187
        // It is a Pascal string.
        int offset = 0;

        Signature = SpanUtilities.ReadPascalString(data[offset..]);
        offset += 1 + Signature.Length;

        Debug.Assert(offset == data.Length, "Parsed beyond the end of the Res Edit Creator Signature Record data.");
    }
}
