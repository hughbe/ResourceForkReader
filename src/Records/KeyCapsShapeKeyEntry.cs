using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Key Caps Shape Key Entry in a Key Caps Record ('KCAP') in a resource fork.
/// </summary>
public readonly struct KeyCapsShapeKeyEntry
{
    /// <summary>
    /// The size of a Key Caps Shape Key Entry in bytes.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Gets the modifier mask.
    /// </summary>
    public byte ModifierMask { get; }

    /// <summary>
    /// Gets the virtual key code.
    /// </summary>
    public ushort VirtualKeyCode { get; }

    /// <summary>
    /// Gets the vertical delta.
    /// </summary>
    public short VerticalDelta { get; }

    /// <summary>
    /// Gets the horizontal delta.
    /// </summary>
    public short HorizontalDelta { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyCapsShapeKeyEntry"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 6 bytes of Key Caps Shape Key Entry data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 6 bytes long.</exception>
    public KeyCapsShapeKeyEntry(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // C-28 to C-34
        int offset = 0;

        // Modifier mask and Boolean. A modifier mask and a Boolean flag for
        // how to use it. When Key Caps draws the current key, it retrieves
        // the byte that represents the real modifier key state, combines it
        // with this mask performing an OR or AND operation as specified,
        // calls the KeyTranslate function with the resulting modifier byte
        // and the virtual key code from the key-caps resource, and draws
        // the resulting character or characters in the current key’s location.
        // The modifier mask is only required for non-ADB keyboards, which
        // use artificial modifier key states to overlap the key codes for
        // arrow keys and keypad operator keys. For other keyboards, the mask
        // is 0 and the flag is set to specify an OR operation.
        ModifierMask = data[offset];
        offset += 1;

        // Virtual key code. The virtual key code for the current key.
        // Because it uses virtual key codes, each key-caps resource is tied
        // directly to a particular key-map resource and hardware keyboard
        // but can work with any keyboard-layout resource.
        VirtualKeyCode = data[offset];
        offset += 1;

        // Vertical delta and horizontal delta. Vertical and horizontal values
        // to move the pen before drawing the current key. For each shape (that
        // is, for each shape entry in the main array), the pen starts out at
        // the upper-left corner of the content region of the Key Caps window,
        // so the vertical and horizontal delta values for the first key in
        // the key array for that shape are distances from the upper-left corner
        // to the origin of the first key. For subsequent keys in the key
        // array, the deltas are distances from the origin of the previous
        // key to the origin of the current key. Each key is drawn with the
        // shape defined by the shape array for that shape.
        VerticalDelta = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        HorizontalDelta = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for KeyCapsShapeKeyEntry.");
    }
}
