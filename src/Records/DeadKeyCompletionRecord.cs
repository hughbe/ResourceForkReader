using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Dead Key Completion Record in a resource fork.
/// </summary>
public readonly struct DeadKeyCompletionRecord
{
    /// <summary>
    /// The size of a Dead Key Completion Record in bytes.
    /// </summary>
    public const int Size = 2;

    /// <summary>
    /// Gets the completion character.
    /// </summary>
    public byte CompletionCharacter { get; }

    /// <summary>
    /// Gets the substitution character.
    /// </summary>
    public byte SubstitutionCharacter { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeadKeyCompletionRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 2 bytes of Dead Key Completion Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is not exactly 4 bytes long.</exception>
    public DeadKeyCompletionRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // C-18 to C-24
        int offset = 0;

        CompletionCharacter = data[offset];
        offset += 1;

        SubstitutionCharacter = data[offset];
        offset += 1;

        Debug.Assert(offset == data.Length, "Did not consume all data for Dead Key Completion Record.");
    }
}