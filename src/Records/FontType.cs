namespace ResourceForkReader.Records;

/// <summary>
/// Specifies the font type flags for a font resource.
/// </summary>
[Flags]
public enum FontType : ushort
{
    /// <summary>
    /// The font contains an image height table.
    /// </summary>
    ContainsImageHeightTable = 0x0001,

    /// <summary>
    /// The font contains a glyph width table.
    /// </summary>
    ContainsGlyphWidthTable = 0x0002,

    /// <summary>
    /// Mask for extracting the bit depth.
    /// </summary>
    BitDepthMask = 0x000C,

    /// <summary>
    /// The font is monochrome (1-bit).
    /// </summary>
    Monochrome = 0x0000,

    /// <summary>
    /// The font has 2-bit depth.
    /// </summary>
    BitDepth2 = 0x0004,

    /// <summary>
    /// The font has 4-bit depth.
    /// </summary>
    BitDepth4 = 0x0008,

    /// <summary>
    /// The font has 8-bit depth.
    /// </summary>
    BitDepth8 = 0x000C,

    /// <summary>
    /// The font has a color table.
    /// </summary>
    HasColorTable = 0x0080,

    /// <summary>
    /// The font is dynamic.
    /// </summary>
    IsDynamic = 0x0010,

    /// <summary>
    /// The font has non-black colors.
    /// </summary>
    HasNonBlackColors = 0x0020,

    /// <summary>
    /// The font has fixed width characters.
    /// </summary>
    FixedWidth = 0x2000,

    /// <summary>
    /// The font cannot be expanded.
    /// </summary>
    CannotExpand = 0x4000,
}
