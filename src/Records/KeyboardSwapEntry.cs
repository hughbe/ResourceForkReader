using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Keyboard Swap Entry in a resource fork.
/// </summary>
public readonly struct KeyboardSwapEntry
{
    /// <summary>
    /// The size of a Keyboard Swap Entry in bytes.
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Gets the script code or negative code.
    /// </summary>
    public short ScriptOrNegativeCode { get; }

    /// <summary>
    /// Gets the virtual key code.
    /// </summary>
    public byte VirtualKeyCode { get; }

    /// <summary>
    /// Gets the modifier state.
    /// </summary>
    public byte ModifierState { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardSwapEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 4 bytes of Keyboard Swap Entry data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 4 bytes long.</exception>
    public KeyboardSwapEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // C-26 to C-27
        int offset = 0;

        // Script code or negative code. The code number of a script system—such
        // as 0 (smRoman)—or a special negative code for switching. The special
        // negative codes are identical to the selectors for the Script Manager
        // KeyScript procedure. The selectors are listed and described along
        // with the KeyScript procedure in the chapter “Script Manager” in
        // this book.
        ScriptOrNegativeCode = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Virtual key code. The virtual key code (for example, $31 for Space
        // bar) required to generate the script code or special negative code
        // of this element.
        VirtualKeyCode = data[offset];
        offset += 1;

        // Modifier state. The modifier-key setting (for example, Command
        // key down) that must accompany the virtual key code.
        ModifierState = data[offset];
        offset += 1;

        Debug.Assert(offset == data.Length, "Did not consume all data for Keyboard Swap Entry.");
    }
}
