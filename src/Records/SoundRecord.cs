using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a sound resource ('snd ').
/// </summary>
public readonly struct SoundRecord
{
    /// <summary>
    /// The minimum size of a Sound Record in bytes.
    /// </summary>
    public const int MinSize = 6;

    /// <summary>
    /// Gets the sound format.
    /// </summary>
    public ushort Format { get; }

    /// <summary>
    /// Gets the number of data formats, if applicable.
    /// </summary>
    public ushort? NumberOfDataFormats { get; }

    /// <summary>
    /// Gets the list of sound data formats, if applicable.
    /// </summary>
    public List<SoundDataFormat>? DataFormats { get; }

    /// <summary>
    /// Gets the reference count, if applicable.
    /// </summary>
    public ushort? ReferenceCount { get; }

    /// <summary>
    /// Gets the number of sound commands.
    /// </summary>
    public ushort NumberOfCommands { get; }

    /// <summary>
    /// Gets the list of sound commands.
    /// </summary>
    public List<SoundCommand> Commands { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the sound data.</param>
    /// <exception cref="ArgumentException">Thrown when data is less than the minimum size.</exception>
    public SoundRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Sound Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://vintageapple.org/inside_r/pdf/Sound_1994.pdf
        // 2-74
        int offset = 0;

        Format = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (Format == 1)
        {
            NumberOfDataFormats = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            var dataFormats = new List<SoundDataFormat>();
            for (int i = 0; i < NumberOfDataFormats; i++)
            {
                var dataFormat = new SoundDataFormat(data.Slice(offset, SoundDataFormat.Size));
                dataFormats.Add(dataFormat);
                offset += SoundDataFormat.Size;
            }

            DataFormats = dataFormats;

            NumberOfCommands = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }
        else
        {
            ReferenceCount = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            NumberOfCommands = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        var commands = new List<SoundCommand>(NumberOfCommands);
        for (int i = 0; i < NumberOfCommands; i++)
        {
            commands.Add(new SoundCommand(data.Slice(offset, SoundCommand.Size)));
            offset += SoundCommand.Size;
        }

        Commands = commands;

        Debug.Assert(offset <= data.Length, "Parsed beyond the end of the SoundRecord data.");
    }
}