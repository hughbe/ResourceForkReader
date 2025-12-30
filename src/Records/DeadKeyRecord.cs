using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Dead Key Record in a resource fork.
/// </summary>
public readonly struct DeadKeyRecord
{
    /// <summary>
    /// The minimum size of a Dead Key Record in bytes.
    /// </summary>
    public const int MinSize = 6;

    /// <summary>
    /// Gets the table number.
    /// </summary>
    public byte TableNumber { get; }

    /// <summary>
    /// Gets the virtual key code.
    /// </summary>
    public byte VirtualKeyCode { get; }

    /// <summary>
    /// Gets the number of completion records.
    /// </summary>
    public ushort NumberOfCompletionRecords { get; }

    /// <summary>
    /// Gets the list of completion records.
    /// </summary>
    public List<DeadKeyCompletionRecord> CompletionRecords { get; }

    /// <summary>
    /// Gets the no-match character completion record.
    /// </summary>
    public DeadKeyCompletionRecord NoMatchCharacter { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeadKeyRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Dead Key Record data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 6 bytes long.</exception>
    public DeadKeyRecord(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Dead Key Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // C-18 to C-24
        int offset = 0;

        TableNumber = data[offset];
        offset += 1;

        VirtualKeyCode = data[offset];
        offset += 1;

        NumberOfCompletionRecords = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var completionRecords = new List<DeadKeyCompletionRecord>(NumberOfCompletionRecords);
        for (int i = 0; i < NumberOfCompletionRecords; i++)
        {
            completionRecords.Add(new DeadKeyCompletionRecord(data.Slice(offset, DeadKeyCompletionRecord.Size)));
            offset += DeadKeyCompletionRecord.Size;
        }

        CompletionRecords = completionRecords;

        NoMatchCharacter = new DeadKeyCompletionRecord(data.Slice(offset, DeadKeyCompletionRecord.Size));
        offset += DeadKeyCompletionRecord.Size;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all data for Dead Key Record.");
    }
}
