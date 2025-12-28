using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Text Record ('TEXT') in a resource fork.
/// </summary>
public readonly struct TextRecord
{
    /// <summary>
    /// Gets the text content of the Text Record.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextRecord"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the text record data.</param>
    /// <exception cref="ArgumentException">Thrown when the data is invalid.</exception>
    public TextRecord(ReadOnlySpan<byte> data)
    {
        Text = Encoding.ASCII.GetString(data);
    }
}
