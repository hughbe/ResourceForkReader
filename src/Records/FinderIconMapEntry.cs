using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Finder Icon Map Entry.
/// </summary>
public readonly struct FinderIconMapEntry
{
    /// <summary>
    /// The size of a Finder Icon Map Entry in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// The type of the Finder Icon Map Entry.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// The standard file icon resource ID.
    /// </summary>
    public short StandardFileIconResourceID { get; }

    /// <summary>
    /// The Finder icon resource ID.
    /// </summary>
    public short FinderIconResourceID { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FinderIconMapEntry"/> struct from the given data.
    /// </summary>
    /// <param name="data">The byte span containing the Finder Icon Map Entry data.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not equal to the expected size.</exception>
    public FinderIconMapEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length {data.Length} is not equal to expected size {Size} for Finder Icon Map Entry.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L450-L456
        int offset = 0;

        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        StandardFileIconResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        FinderIconResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for Finder Icon Map Entry.");
    }
}
