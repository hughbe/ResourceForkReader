using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Keyboard Mapping in a resource fork.
/// </summary>
public readonly struct KeyboardMapping
{
    /// <summary>
    /// Size of a Keyboard Mapping in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// Gets the keyboard type.
    /// </summary>
    public ushort KeyboardType { get; }

    /// <summary>
    /// Gets the old modifiers.
    /// </summary>
    public byte OldModifiers { get; }

    /// <summary>
    /// Gets the old key code.
    /// </summary>
    public byte OldKeyCode { get; }

    /// <summary>
    /// Gets the mask modifiers.
    /// </summary>
    public byte MaskModifiers { get; }

    /// <summary>
    /// Gets the mask key code.
    /// </summary>
    public byte MaskKeyCode { get; }

    /// <summary>
    /// Gets the new modifiers.
    /// </summary>
    public byte NewModifiers { get; }

    /// <summary>
    /// Gets the new key code.
    /// </summary>
    public byte NewKeyCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardMapping"/> struct.
    /// </summary>
    /// <param name="data">The data for the Keyboard Mapping.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not equal to the size.</exception>
    public KeyboardMapping(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L810-L820
        int offset = 0;

        KeyboardType = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        OldModifiers = data[offset];
        offset += 1;

        OldKeyCode = data[offset];
        offset += 1;

        MaskModifiers = data[offset];
        offset += 1;

        MaskKeyCode = data[offset];
        offset += 1;

        NewModifiers = data[offset];
        offset += 1;

        NewKeyCode = data[offset];
        offset += 1;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for KeyboardMapping.");
    }
}
