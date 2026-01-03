using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Key Map Exception in a resource fork.
/// </summary>
public readonly struct KeyMapException
{
    /// <summary>
    /// The minimum size of a Key Map Exception in bytes.
    /// </summary>
    public const int MinSize = 3;

    /// <summary>
    /// A raw key code.
    /// </summary>
    public byte RawKeyCode { get; }

    /// <summary>
    /// A Boolean (Xor or noXor) field that determines whether to
    /// instruct the driver to invert the state of the key instead of
    /// using the state provided by the hardware.
    /// </summary>
    public bool Xor { get; }

    /// <summary>
    /// The ADB opcode, an instruction to the keyboard to perform some
    /// </summary>
    public byte ADBOpCode { get; }

    /// <summary>
    /// The ADB argument.
    /// </summary>
    public string ADBArgument { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyMapException"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Key Map Exception data.</param>
    /// <param name="bytesRead">The number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 3 bytes long.</exception>
    public KeyMapException(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid Key Map Exception. Minimum size is {MinSize} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // C-12 to C-13
        int offset = 0;

        // A raw key code.
        RawKeyCode = data[offset];
        offset += 1;

        // One byte containing the following elements:
        // ■ A Boolean (Xor or noXor) field that determines whether to
        // instruct the driver to invert the state of the key instead of
        // using the state provided by the hardware.
        // ■ Filler (3 bits in length).
        // ■ The ADB opcode, an instruction to the keyboard to perform some
        // task. ADB opcodes are described in Inside Macintosh: Devices.
        var flags = data[offset];
        offset += 1;

        Xor = (flags & 0b1000_0000) != 0;
        ADBOpCode = (byte)(flags & 0b0000_1111);

        ADBArgument = SpanUtilities.ReadPascalString(data[offset..], out var adbArgumentBytesRead);
        offset += adbArgumentBytesRead;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Parsed more data than is available.");
    }
}
