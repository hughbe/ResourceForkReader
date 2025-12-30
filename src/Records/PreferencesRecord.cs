using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Preferences Record ('PREF') in a resource fork.
/// </summary>
public readonly struct PreferencesRecord
{
    /// <summary>
    /// Gets the data of the Preferences Record.
    /// </summary>
    public string Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PreferencesRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Preferences Record data.</param>
    public PreferencesRecord(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/ResEditReference.pdf
        Data = Encoding.ASCII.GetString(data);
    }
}
