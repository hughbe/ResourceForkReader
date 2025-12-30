using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Software Keyboard Layout Record ('KCHR') in a resource fork.
/// </summary>
public readonly struct KeyboardLayoutRecord
{
    /// <summary>
    /// The minimum size of a Software Keyboard Layout Record in bytes.
    /// </summary>
    public const int MinSize = 262;

    /// <summary>
    /// Gets the version of the keyboard layout.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// Gets the table selection index.
    /// </summary>
    public byte[] TableSelectionIndex { get; }

    /// <summary>
    /// Gets the number of character mapping tables.
    /// </summary>
    public ushort NumberOfTables { get; }

    /// <summary>
    /// Gets the character mapping tables.
    /// </summary>
    public List<byte[]> CharacterMappingTables { get; }

    /// <summary>
    /// Gets the number of dead key records.
    /// </summary>
    public ushort NumberOfDeadKeyRecords { get; }

    /// <summary>
    /// Gets the dead key records.
    /// </summary>
    public List<DeadKeyRecord> DeadKeyRecords { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardLayoutRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Software Keyboard Layout Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 262 bytes long.</exception>
    public KeyboardLayoutRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Software Keyboard Layout Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Keyboard_Layouts.pdf
        int offset = 0;

        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TableSelectionIndex = data.Slice(offset, 256).ToArray();
        offset += 256;

        NumberOfTables = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var characterMappingTables = new List<byte[]>();
        for (int i = 0; i < NumberOfTables; i++)
        {
            byte[] table = data.Slice(offset, 128).ToArray();
            characterMappingTables.Add(table);
            offset += 128;
        }

        CharacterMappingTables = characterMappingTables;

        NumberOfDeadKeyRecords = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var deadKeyRecords = new List<DeadKeyRecord>(NumberOfDeadKeyRecords);
        for (int i = 0; i < NumberOfDeadKeyRecords; i++)
        {
            deadKeyRecords.Add(new DeadKeyRecord(data.Slice(offset), out int bytesRead));
            offset += bytesRead;
        }

        DeadKeyRecords = deadKeyRecords;

        Debug.Assert(offset == data.Length, "Did not consume all data for Software Keyboard Layout Record.");
    }
}
