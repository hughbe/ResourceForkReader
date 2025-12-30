using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Key Caps Shape Entry in a Key Caps Record ('KCAP') in a resource fork.
/// </summary>
public readonly struct KeyCapsShapeEntry
{
    /// <summary>
    /// The minimum size of a Key Caps Shape Entry in bytes.
    /// </summary>
    public const int MinSize = 4;

    /// <summary>
    /// Gets the number of point entries minus one.
    /// </summary>
    public ushort NumberOfPointEntries { get; }

    /// <summary>
    /// Gets the list of point entries.
    /// </summary>
    public List<KeyCapsShapePointEntry> PointEntries { get; }

    /// <summary>
    /// Gets the number of key entries.
    /// </summary>
    public ushort NumberOfKeyEntries { get; }

    /// <summary>
    /// Gets the list of key entries.
    /// </summary>
    public List<KeyCapsShapeKeyEntry> KeyEntries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyCapsShapeEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Key Caps Shape Entry data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than the minimum size.</exception>
    public KeyCapsShapeEntry(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Key Caps Shape Entry. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // C-28 to C-34
        int offset = 0;

        // Number of point entries minus one.
        NumberOfPointEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Point entries.
        var pointEntries = new List<KeyCapsShapePointEntry>(NumberOfPointEntries + 1);
        for (int i = 0; i < NumberOfPointEntries + 1; i++)
        {
            pointEntries.Add(new KeyCapsShapePointEntry(data.Slice(offset, KeyCapsShapePointEntry.Size)));
            offset += KeyCapsShapePointEntry.Size;
        }

        PointEntries = pointEntries;

        // Number of key entries.
        NumberOfKeyEntries = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Key entries.
        var keyEntries = new List<KeyCapsShapeKeyEntry>(NumberOfKeyEntries + 1);
        for (int i = 0; i < NumberOfKeyEntries + 1; i++)
        {
            keyEntries.Add(new KeyCapsShapeKeyEntry(data.Slice(offset, KeyCapsShapeKeyEntry.Size)));
            offset += KeyCapsShapeKeyEntry.Size;
        }

        KeyEntries = keyEntries;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not parse all data for KeyCapsShapeEntry.");
    }
}
