using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Error String in an Error Strings Record ('errs') in a resource fork.
/// </summary>
public readonly struct ErrorString
{
    /// <summary>
    /// Gets the size of an Error String in bytes.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Gets the minimum ID for which this error string applies.
    /// </summary>
    public ushort MinimumID { get; }

    /// <summary>
    /// Gets the maximum ID for which this error string applies.
    /// </summary>
    public ushort MaximumID { get; }

    /// <summary>
    /// Gets the string resource ID that contains the error string.
    /// </summary>
    public short StringResourceID { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorString"/> struct.
    /// </summary>
    /// <param name="data">The data for the Error String.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not equal to <see cref="Size"/>.</exception>
    public ErrorString(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length must be {Size}.", nameof(data));
        }

        int offset = 0;

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L407
        MinimumID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        MaximumID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        StringResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for Error String.");
    }

}
