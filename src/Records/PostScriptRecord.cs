using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a PostScript Record ('POST') in a resource fork.
/// </summary>
public readonly struct PostScriptRecord
{
    /// <summary>
    /// Gets the PostScript data of the PostScript Record.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of PostScript commands.
    /// </summary>
    public ushort NumberOfCommands { get; }

    /// <summary>
    /// Gets the PostScript commands.
    /// </summary>
    public List<string> Commands { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostScriptRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing PostScript Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is less than 2 bytes long.</exception>
    public PostScriptRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid PostScript Record. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure not documented but has a template in ResEdit.
        int offset = 0;

        NumberOfCommands = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var commands = new List<string>(NumberOfCommands);
        for (int i = 0; i < NumberOfCommands; i++)
        {
            if (offset == data.Length)
            {
                // Some PostScript records may be malformed; stop parsing if we run out of data.
                break;
            }

            var length = data[offset];

            // Some PostScript records may be malformed; stop parsing if we run out of data.
            if (offset + 1 + length > data.Length)
            {
                break;
            }

            commands.Add(SpanUtilities.ReadPascalString(data.Slice(offset)));
            offset += 1 + commands[i].Length;

            if (offset % 2 != 0)
            {
                // Align to even byte boundary
                offset += 1;
            }
        }

        Commands = commands;

        Debug.Assert(offset <= data.Length, "Did not consume all data for PostScript Record.");
    }
}
