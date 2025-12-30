using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents user item data in an item list.
/// </summary>
public readonly struct UserItemData
{
    /// <summary>
    /// The size of the UserItemData structure in bytes.
    /// </summary>
    public const int Size = 1;

    /// <summary>
    /// Gets length of reserved data.
    /// </summary>
    public byte ReservedLength { get; }

    /// <summary>
    /// Gets the reserved data.
    /// </summary>
    public byte[] ReservedData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserItemData"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the user item data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
    public UserItemData(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < Size)
        {
            throw new ArgumentException($"Data must be at least {Size} bytes long.", nameof(data));
        }

        int offset = 0;

        ReservedLength = data[offset];
        offset += 1;

        if (data.Length - offset < ReservedLength)
        {
            throw new ArgumentException("Data is too small to contain the specified reserved data.", nameof(data));
        }

        // Advance by the reserved size.
        ReservedData = data.Slice(offset, ReservedLength).ToArray();
        offset += ReservedLength;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all data for UserItemData.");
    }
}
