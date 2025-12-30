using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents text item data in an item list.
/// </summary>
public readonly struct TextItemData
{
    /// <summary>
    /// Gets the text of the item.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextItemData"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the text item data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">Thrown when data is smaller than the minimum size.</exception>
    public TextItemData(ReadOnlySpan<byte> data, out int bytesRead)
    {
        int offset = 0;

        // Text. This specifies the text that appears in the item. This element consists of a length
        // byte and as many as 255 additional bytes for the text. (“Titles for Buttons, Checkboxes,
        // and Radio Buttons” beginning on page 6-37 and “Text Strings for Static Text and
        // Editable Text Items” beginning on page 6-40 contain recommendations about appropriate text in items.)
        // ■ For a button, checkbox, or radio button, this is the title for that control.
        // ■ For a static text item, this is the text of the item.
        // ■ For an editable text item, this can be an empty string (in which case the editable text
        // item contains no text), or it can be a string that appears as the default string in the
        // editable text item.
        Text = SpanUtilities.ReadPascalString(data[offset..]);
        offset += 1 + Text.Length;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all data for TextItemData.");
    }
}
