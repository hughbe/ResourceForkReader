using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a sound resource ('snd ').
/// </summary>
public readonly struct SoundRecord
{
    /// <summary>
    /// Gets the sound data.
    /// </summary>
    public byte[] SoundData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the sound data.</param>
    public SoundRecord(ReadOnlySpan<byte> data)
    {
        // TODO - not implemented
        SoundData = data.ToArray();
    }
}