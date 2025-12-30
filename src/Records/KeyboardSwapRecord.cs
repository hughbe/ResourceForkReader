using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Keyboard Swap Record ('KSWP') in a resource fork.
/// </summary>
public readonly struct KeyboardSwapRecord
{
    /// <summary>
    /// Gets the keyboard swap entries.
    /// </summary>
    public List<KeyboardSwapEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardSwapRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Keyboard Swap Record data.</param>
    /// <exception cref="ArgumentException">Thrown when data length is not a multiple of KeyboardSwapEntry.Size bytes.</exception>
    public KeyboardSwapRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length % KeyboardSwapEntry.Size != 0)
        {
            throw new ArgumentException($"Data length must be a multiple of {KeyboardSwapEntry.Size} bytes to be a valid Keyboard Swap Record.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // C-26 to C-27
        int offset = 0;

        int entryCount = data.Length / KeyboardSwapEntry.Size;
        var entries = new List<KeyboardSwapEntry>(entryCount);
        for (int i = 0; i < entryCount; i++)
        {
            entries.Add(new KeyboardSwapEntry(data.Slice(i * KeyboardSwapEntry.Size, KeyboardSwapEntry.Size)));
            offset += KeyboardSwapEntry.Size;
        }

        Entries = entries;

        Debug.Assert(offset == data.Length, "Did not consume all data for Keyboard Swap Record.");
    }
}
