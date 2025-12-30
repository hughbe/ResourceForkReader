namespace ResourceForkReader.Records;

/// <summary>
/// Specifies the type of pixel pattern.
/// </summary>
public enum PixelPatternType : ushort
{
    /// <summary>
    /// QuickDraw pixel pattern.
    /// </summary>
    QuickDraw = 0,

    /// <summary>
    /// Full color pixel pattern.
    /// </summary>
    FullColor = 1,

    /// <summary>
    /// RGB pixel pattern.
    /// </summary>
    RGB = 2
}
