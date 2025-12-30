namespace ResourceForkReader.Records;

/// <summary>
/// Defines the types of cursors.
/// </summary>
public enum CursorType : ushort
{
    /// <summary>
    /// Color cursor.
    /// </summary>
    ColorCursor = 0x8001,

    /// <summary>
    /// Black and white cursor.
    /// </summary>
    BlackAndWhiteCursor = 0x8000
}
