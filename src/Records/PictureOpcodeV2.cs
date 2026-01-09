namespace ResourceForkReader.Records;

/// <summary>
/// Opcodes for QuickDraw picture operations.
/// </summary>
public enum PictureOpcodeV2 : ushort
{
    /// <summary>
    /// No operation.
    /// Additional data: 0 bytes.
    /// </summary>
    NOP = 0x0000,

    /// <summary>
    /// Clipping region.
    /// Additional data: region size.
    /// </summary>
    Clip = 0x0001,

    /// <summary>
    /// Background pattern.
    /// Additional data: 8 bytes.
    /// </summary>
    BkPat = 0x0002,

    /// <summary>
    /// Font number for text.
    /// Additional data: 2 bytes (Integer).
    /// </summary>
    TxFont = 0x0003,

    /// <summary>
    /// Text's font style.
    /// Additional data: 1 byte.
    /// </summary>
    TxFace = 0x0004,

    /// <summary>
    /// Source mode.
    /// Additional data: 2 bytes.
    /// </summary>
    TxMode = 0x0005,

    /// <summary>
    /// Extra space.
    /// Additional data: 4 bytes.
    /// </summary>
    SpExtra = 0x0006,

    /// <summary>
    /// Pen size.
    /// Additional data: 4 bytes (Point).
    /// </summary>
    PnSize = 0x0007,

    /// <summary>
    /// Pen mode.
    /// Additional data: 2 bytes.
    /// </summary>
    PnMode = 0x0008,

    /// <summary>
    /// Pen pattern.
    /// Additional data: 8 bytes.
    /// </summary>
    PnPat = 0x0009,

    /// <summary>
    /// Fill pattern.
    /// Additional data: 8 bytes.
    /// </summary>
    FillPat = 0x000A,

    /// <summary>
    /// Oval size.
    /// Additional data: 4 bytes (Point).
    /// </summary>
    OvSize = 0x000B,

    /// <summary>
    /// Origin dh, dv.
    /// Additional data: 4 bytes.
    /// </summary>
    Origin = 0x000C,

    /// <summary>
    /// Text size.
    /// Additional data: 2 bytes.
    /// </summary>
    TxSize = 0x000D,

    /// <summary>
    /// Foreground color.
    /// Additional data: 4 bytes (Long).
    /// </summary>
    FgColor = 0x000E,

    /// <summary>
    /// Background color.
    /// Additional data: 4 bytes (Long).
    /// </summary>
    BkColor = 0x000F,

    /// <summary>
    /// Text ratio (numerator and denominator).
    /// Additional data: 8 bytes (Points).
    /// </summary>
    TxRatio = 0x0010,

    /// <summary>
    /// Version.
    /// Additional data: 1 byte.
    /// </summary>
    VersionOp = 0x0011,

    /// <summary>
    /// Background pixel pattern.
    /// Additional data: variable.
    /// </summary>
    BkPixPat = 0x0012,

    /// <summary>
    /// Pen pixel pattern.
    /// Additional data: variable.
    /// </summary>
    PnPixPat = 0x0013,

    /// <summary>
    /// Fill pixel pattern.
    /// Additional data: variable.
    /// </summary>
    FillPixPat = 0x0014,

    /// <summary>
    /// Fractional pen position.
    /// Additional data: 2 bytes.
    /// </summary>
    PnLocHFrac = 0x0015,

    /// <summary>
    /// Added width for nonspace characters.
    /// Additional data: 2 bytes.
    /// </summary>
    ChExtra = 0x0016,

    /// <summary>
    /// Foreground color.
    /// Additional data: 6 bytes (RGBColor).
    /// </summary>
    RGBFgCol = 0x001A,

    /// <summary>
    /// Background color.
    /// Additional data: 6 bytes (RGBColor).
    /// </summary>
    RGBBkCol = 0x001B,

    /// <summary>
    /// Highlight mode flag.
    /// Additional data: 0 bytes.
    /// </summary>
    HiliteMode = 0x001C,

    /// <summary>
    /// Highlight color.
    /// Additional data: 6 bytes (RGBColor).
    /// </summary>
    HiliteColor = 0x001D,

    /// <summary>
    /// Use default highlight color.
    /// Additional data: 0 bytes.
    /// </summary>
    DefHilite = 0x001E,

    /// <summary>
    /// Opcolor.
    /// Additional data: 6 bytes (RGBColor).
    /// </summary>
    OpColor = 0x001F,

    /// <summary>
    /// Line from pnLoc to newPt.
    /// Additional data: 8 bytes (Points).
    /// </summary>
    Line = 0x0020,

    /// <summary>
    /// Line from current point to newPt.
    /// Additional data: 4 bytes (Point).
    /// </summary>
    LineFrom = 0x0021,

    /// <summary>
    /// Short line from pnLoc with delta offsets.
    /// Additional data: 6 bytes (Point, dh, dv).
    /// </summary>
    ShortLine = 0x0022,

    /// <summary>
    /// Short line from current point with delta offsets.
    /// Additional data: 2 bytes (dh, dv).
    /// </summary>
    ShortLineFrom = 0x0023,

    /// <summary>
    /// Long text at location.
    /// Additional data: 5+ bytes (Point, count, text).
    /// </summary>
    LongText = 0x0028,

    /// <summary>
    /// Text with horizontal delta.
    /// Additional data: 2+ bytes (dh, count, text).
    /// </summary>
    DHText = 0x0029,

    /// <summary>
    /// Text with vertical delta.
    /// Additional data: 2+ bytes (dv, count, text).
    /// </summary>
    DVText = 0x002A,

    /// <summary>
    /// Text with horizontal and vertical delta.
    /// Additional data: 3+ bytes (dh, dv, count, text).
    /// </summary>
    DHDVText = 0x002B,

    /// <summary>
    /// Font name definition.
    /// Additional data: 5+ bytes (length, font ID, name).
    /// </summary>
    FontName = 0x002C,

    /// <summary>
    /// Line justification.
    /// Additional data: 10 bytes (spacing and extra space).
    /// </summary>
    LineJustify = 0x002D,

    /// <summary>
    /// Glyph state.
    /// Additional data: 8 bytes (flags and settings).
    /// </summary>
    GlyphState = 0x002E,

    /// <summary>
    /// Frame rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FrameRect = 0x0030,

    /// <summary>
    /// Paint rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    PaintRect = 0x0031,

    /// <summary>
    /// Erase rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    EraseRect = 0x0032,

    /// <summary>
    /// Invert rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    InvertRect = 0x0033,

    /// <summary>
    /// Fill rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FillRect = 0x0034,

    /// <summary>
    /// Frame same rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FrameSameRect = 0x0038,

    /// <summary>
    /// Paint same rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    PaintSameRect = 0x0039,

    /// <summary>
    /// Erase same rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    EraseSameRect = 0x003A,

    /// <summary>
    /// Invert same rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    InvertSameRect = 0x003B,

    /// <summary>
    /// Fill same rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FillSameRect = 0x003C,

    /// <summary>
    /// Frame rounded rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FrameRRect = 0x0040,

    /// <summary>
    /// Paint rounded rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    PaintRRect = 0x0041,

    /// <summary>
    /// Erase rounded rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    EraseRRect = 0x0042,

    /// <summary>
    /// Invert rounded rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    InvertRRect = 0x0043,

    /// <summary>
    /// Fill rounded rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FillRRect = 0x0044,

    /// <summary>
    /// Frame same rounded rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FrameSameRRect = 0x0048,

    /// <summary>
    /// Paint same rounded rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    PaintSameRRect = 0x0049,

    /// <summary>
    /// Erase same rounded rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    EraseSameRRect = 0x004A,

    /// <summary>
    /// Invert same rounded rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    InvertSameRRect = 0x004B,

    /// <summary>
    /// Fill same rounded rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FillSameRRect = 0x004C,

    /// <summary>
    /// Frame oval.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FrameOval = 0x0050,

    /// <summary>
    /// Paint oval.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    PaintOval = 0x0051,

    /// <summary>
    /// Erase oval.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    EraseOval = 0x0052,

    /// <summary>
    /// Invert oval.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    InvertOval = 0x0053,

    /// <summary>
    /// Fill oval.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FillOval = 0x0054,

    /// <summary>
    /// Frame same oval (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FrameSameOval = 0x0058,

    /// <summary>
    /// Paint same oval (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    PaintSameOval = 0x0059,

    /// <summary>
    /// Erase same oval (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    EraseSameOval = 0x005A,

    /// <summary>
    /// Invert same oval (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    InvertSameOval = 0x005B,

    /// <summary>
    /// Fill same oval (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FillSameOval = 0x005C,

    /// <summary>
    /// Frame arc.
    /// Additional data: 12 bytes (Rect, startAngle, arcAngle).
    /// </summary>
    FrameArc = 0x0060,

    /// <summary>
    /// Paint arc.
    /// Additional data: 12 bytes (Rect, startAngle, arcAngle).
    /// </summary>
    PaintArc = 0x0061,

    /// <summary>
    /// Erase arc.
    /// Additional data: 12 bytes (Rect, startAngle, arcAngle).
    /// </summary>
    EraseArc = 0x0062,

    /// <summary>
    /// Invert arc.
    /// Additional data: 12 bytes (Rect, startAngle, arcAngle).
    /// </summary>
    InvertArc = 0x0063,

    /// <summary>
    /// Fill arc.
    /// Additional data: 12 bytes (Rect, startAngle, arcAngle).
    /// </summary>
    FillArc = 0x0064,

    /// <summary>
    /// Frame same arc (use previous).
    /// Additional data: 4 bytes (startAngle, arcAngle).
    /// </summary>
    FrameSameArc = 0x0068,

    /// <summary>
    /// Paint same arc (use previous).
    /// Additional data: 4 bytes (startAngle, arcAngle).
    /// </summary>
    PaintSameArc = 0x0069,

    /// <summary>
    /// Erase same arc (use previous).
    /// Additional data: 4 bytes (startAngle, arcAngle).
    /// </summary>
    EraseSameArc = 0x006A,

    /// <summary>
    /// Invert same arc (use previous).
    /// Additional data: 4 bytes (startAngle, arcAngle).
    /// </summary>
    InvertSameArc = 0x006B,

    /// <summary>
    /// Fill same arc (use previous).
    /// Additional data: 4 bytes (startAngle, arcAngle).
    /// </summary>
    FillSameArc = 0x006C,

    /// <summary>
    /// Frame polygon.
    /// Additional data: polygon size (Poly).
    /// </summary>
    FramePoly = 0x0070,

    /// <summary>
    /// Paint polygon.
    /// Additional data: polygon size (Poly).
    /// </summary>
    PaintPoly = 0x0071,

    /// <summary>
    /// Erase polygon.
    /// Additional data: polygon size (Poly).
    /// </summary>
    ErasePoly = 0x0072,

    /// <summary>
    /// Invert polygon.
    /// Additional data: polygon size (Poly).
    /// </summary>
    InvertPoly = 0x0073,

    /// <summary>
    /// Fill polygon.
    /// Additional data: polygon size (Poly).
    /// </summary>
    FillPoly = 0x0074,

    /// <summary>
    /// Frame region.
    /// Additional data: region size (Rgn).
    /// </summary>
    FrameRgn = 0x0080,

    /// <summary>
    /// Paint region.
    /// Additional data: region size (Rgn).
    /// </summary>
    PaintRgn = 0x0081,

    /// <summary>
    /// Erase region.
    /// Additional data: region size (Rgn).
    /// </summary>
    EraseRgn = 0x0082,

    /// <summary>
    /// Invert region.
    /// Additional data: region size (Rgn).
    /// </summary>
    InvertRgn = 0x0083,

    /// <summary>
    /// Fill region.
    /// Additional data: region size (Rgn).
    /// </summary>
    FillRgn = 0x0084,

    /// <summary>
    /// Frame same region (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FrameSameRgn = 0x0088,

    /// <summary>
    /// Paint same region (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    PaintSameRgn = 0x0089,

    /// <summary>
    /// Erase same region (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    EraseSameRgn = 0x008A,

    /// <summary>
    /// Invert same region (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    InvertSameRgn = 0x008B,

    /// <summary>
    /// Fill same region (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FillSameRgn = 0x008C,

    /// <summary>
    /// CopyBits with clipped rectangle.
    /// Additional data: variable.
    /// </summary>
    BitsRect = 0x0090,

    /// <summary>
    /// CopyBits with clipped region.
    /// Additional data: variable.
    /// </summary>
    BitsRgn = 0x0091,

    /// <summary>
    /// Packed CopyBits with clipped rectangle.
    /// Additional data: variable.
    /// </summary>
    PackBitsRect = 0x0098,

    /// <summary>
    /// Packed CopyBits with clipped region.
    /// Additional data: variable.
    /// </summary>
    PackBitsRgn = 0x0099,

    /// <summary>
    /// Direct PixMap with source and destination rectangles.
    /// Additional data: variable (PixMap, srcRect, dstRect, mode, PixData).
    /// </summary>
    DirectBitsRect = 0x009A,

    /// <summary>
    /// Direct PixMap with clipped region.
    /// Additional data: variable (PixMap, srcRect, dstRect, mode, maskRgn, PixData).
    /// </summary>
    DirectBitsRgn = 0x009B,

    /// <summary>
    /// Short comment.
    /// Additional data: 2 bytes (Kind).
    /// </summary>
    ShortComment = 0x00A0,

    /// <summary>
    /// Long comment.
    /// Additional data: 4+ bytes (Kind, size, data).
    /// </summary>
    LongComment = 0x00A1,

    /// <summary>
    /// End of picture.
    /// Additional data: 2 bytes.
    /// </summary>
    OpEndPic = 0x00FF,

    /// <summary>
    /// Header for extended version 2.
    /// Additional data: 24 bytes (version, reserved, hRes, vRes, srcRect, reserved).
    /// </summary>
    HeaderOp = 0x0C00,

    /// <summary>
    /// Compressed QuickTime data.
    /// Additional data: 4+ bytes (data length, compressed data).
    /// </summary>
    CompressedQuickTime = 0x8200,

    /// <summary>
    /// Uncompressed QuickTime data.
    /// Additional data: 4+ bytes (data length, uncompressed data).
    /// </summary>
    UncompressedQuickTime = 0x8201,
}
