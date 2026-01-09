namespace ResourceForkReader.Records;

/// <summary>
/// Opcodes for QuickDraw picture operations (version 1).
/// </summary>
public enum PictureOpcodeV1 : byte
{
    /// <summary>
    /// No operation.
    /// Additional data: 0 bytes.
    /// </summary>
    NOP = 0x00,

    /// <summary>
    /// Clipping region.
    /// Additional data: region size.
    /// </summary>
    ClipRgn = 0x01,

    /// <summary>
    /// Background pattern.
    /// Additional data: 8 bytes.
    /// </summary>
    BkPat = 0x02,

    /// <summary>
    /// Font number for text.
    /// Additional data: 2 bytes (Integer).
    /// </summary>
    TxFont = 0x03,

    /// <summary>
    /// Text's font style.
    /// Additional data: 1 byte (0..255).
    /// </summary>
    TxFace = 0x04,

    /// <summary>
    /// Source mode.
    /// Additional data: 2 bytes (Integer).
    /// </summary>
    TxMode = 0x05,

    /// <summary>
    /// Extra space.
    /// Additional data: 4 bytes (Fixed).
    /// </summary>
    SpExtra = 0x06,

    /// <summary>
    /// Pen size.
    /// Additional data: 4 bytes (Point).
    /// </summary>
    PnSize = 0x07,

    /// <summary>
    /// Pen mode.
    /// Additional data: 2 bytes (Integer).
    /// </summary>
    PnMode = 0x08,

    /// <summary>
    /// Pen pattern.
    /// Additional data: 8 bytes.
    /// </summary>
    PnPat = 0x09,

    /// <summary>
    /// Fill pattern.
    /// Additional data: 8 bytes.
    /// </summary>
    FillPat = 0x0A,

    /// <summary>
    /// Oval size.
    /// Additional data: 4 bytes (Point).
    /// </summary>
    OvSize = 0x0B,

    /// <summary>
    /// Origin dh (Integer), dv (Integer).
    /// Additional data: 4 bytes.
    /// </summary>
    Origin = 0x0C,

    /// <summary>
    /// Text size.
    /// Additional data: 2 bytes (Integer).
    /// </summary>
    TxSize = 0x0D,

    /// <summary>
    /// Foreground color.
    /// Additional data: 4 bytes (Long).
    /// </summary>
    FgColor = 0x0E,

    /// <summary>
    /// Background color.
    /// Additional data: 4 bytes (Long).
    /// </summary>
    BkColor = 0x0F,

    /// <summary>
    /// Text ratio (numerator and denominator).
    /// Additional data: 8 bytes (Points).
    /// </summary>
    TxRatio = 0x10,

    /// <summary>
    /// Picture version.
    /// Additional data: 1 byte (0..255).
    /// </summary>
    PicVersion = 0x11,

    /// <summary>
    /// Line from pnLoc to newPt.
    /// Additional data: 8 bytes (Points).
    /// </summary>
    Line = 0x20,

    /// <summary>
    /// Line from current point to newPt.
    /// Additional data: 4 bytes (Point).
    /// </summary>
    LineFrom = 0x21,

    /// <summary>
    /// Short line from pnLoc with delta offsets.
    /// Additional data: 6 bytes (Point, dh, dv).
    /// </summary>
    ShortLine = 0x22,

    /// <summary>
    /// Short line from current point with delta offsets.
    /// Additional data: 2 bytes (dh, dv).
    /// </summary>
    ShortLineFrom = 0x23,

    /// <summary>
    /// Long text at location.
    /// Additional data: 5+ bytes (Point, count, text).
    /// </summary>
    LongText = 0x28,

    /// <summary>
    /// Text with horizontal delta.
    /// Additional data: 2+ bytes (dh, count, text).
    /// </summary>
    DHText = 0x29,

    /// <summary>
    /// Text with vertical delta.
    /// Additional data: 2+ bytes (dv, count, text).
    /// </summary>
    DVText = 0x2A,

    /// <summary>
    /// Text with horizontal and vertical delta.
    /// Additional data: 3+ bytes (dh, dv, count, text).
    /// </summary>
    DHDVText = 0x2B,

    /// <summary>
    /// Frame rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FrameRect = 0x30,

    /// <summary>
    /// Paint rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    PaintRect = 0x31,

    /// <summary>
    /// Erase rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    EraseRect = 0x32,

    /// <summary>
    /// Invert rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    InvertRect = 0x33,

    /// <summary>
    /// Fill rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FillRect = 0x34,

    /// <summary>
    /// Frame same rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FrameSameRect = 0x38,

    /// <summary>
    /// Paint same rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    PaintSameRect = 0x39,

    /// <summary>
    /// Erase same rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    EraseSameRect = 0x3A,

    /// <summary>
    /// Invert same rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    InvertSameRect = 0x3B,

    /// <summary>
    /// Fill same rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FillSameRect = 0x3C,

    /// <summary>
    /// Frame rounded rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FrameRRect = 0x40,

    /// <summary>
    /// Paint rounded rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    PaintRRect = 0x41,

    /// <summary>
    /// Erase rounded rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    EraseRRect = 0x42,

    /// <summary>
    /// Invert rounded rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    InvertRRect = 0x43,

    /// <summary>
    /// Fill rounded rectangle.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FillRRect = 0x44,

    /// <summary>
    /// Frame same rounded rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FrameSameRRect = 0x48,

    /// <summary>
    /// Paint same rounded rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    PaintSameRRect = 0x49,

    /// <summary>
    /// Erase same rounded rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    EraseSameRRect = 0x4A,

    /// <summary>
    /// Invert same rounded rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    InvertSameRRect = 0x4B,

    /// <summary>
    /// Fill same rounded rectangle (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FillSameRRect = 0x4C,

    /// <summary>
    /// Frame oval.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FrameOval = 0x50,

    /// <summary>
    /// Paint oval.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    PaintOval = 0x51,

    /// <summary>
    /// Erase oval.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    EraseOval = 0x52,

    /// <summary>
    /// Invert oval.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    InvertOval = 0x53,

    /// <summary>
    /// Fill oval.
    /// Additional data: 8 bytes (Rect).
    /// </summary>
    FillOval = 0x54,

    /// <summary>
    /// Frame same oval (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FrameSameOval = 0x58,

    /// <summary>
    /// Paint same oval (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    PaintSameOval = 0x59,

    /// <summary>
    /// Erase same oval (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    EraseSameOval = 0x5A,

    /// <summary>
    /// Invert same oval (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    InvertSameOval = 0x5B,

    /// <summary>
    /// Fill same oval (use previous).
    /// Additional data: 0 bytes.
    /// </summary>
    FillSameOval = 0x5C,

    /// <summary>
    /// Frame arc.
    /// Additional data: 12 bytes (Rect, startAngle, arcAngle).
    /// </summary>
    FrameArc = 0x60,

    /// <summary>
    /// Paint arc.
    /// Additional data: 12 bytes (Rect, startAngle, arcAngle).
    /// </summary>
    PaintArc = 0x61,

    /// <summary>
    /// Erase arc.
    /// Additional data: 12 bytes (Rect, startAngle, arcAngle).
    /// </summary>
    EraseArc = 0x62,

    /// <summary>
    /// Invert arc.
    /// Additional data: 12 bytes (Rect, startAngle, arcAngle).
    /// </summary>
    InvertArc = 0x63,

    /// <summary>
    /// Fill arc.
    /// Additional data: 12 bytes (Rect, startAngle, arcAngle).
    /// </summary>
    FillArc = 0x64,

    /// <summary>
    /// Frame same arc (use previous).
    /// Additional data: 4 bytes (startAngle, arcAngle).
    /// </summary>
    FrameSameArc = 0x68,

    /// <summary>
    /// Paint same arc (use previous).
    /// Additional data: 4 bytes (startAngle, arcAngle).
    /// </summary>
    PaintSameArc = 0x69,

    /// <summary>
    /// Erase same arc (use previous).
    /// Additional data: 4 bytes (startAngle, arcAngle).
    /// </summary>
    EraseSameArc = 0x6A,

    /// <summary>
    /// Invert same arc (use previous).
    /// Additional data: 4 bytes (startAngle, arcAngle).
    /// </summary>
    InvertSameArc = 0x6B,

    /// <summary>
    /// Fill same arc (use previous).
    /// Additional data: 4 bytes (startAngle, arcAngle).
    /// </summary>
    FillSameArc = 0x6C,

    /// <summary>
    /// Frame polygon.
    /// Additional data: polygon size (Poly).
    /// </summary>
    FramePoly = 0x70,

    /// <summary>
    /// Paint polygon.
    /// Additional data: polygon size (Poly).
    /// </summary>
    PaintPoly = 0x71,

    /// <summary>
    /// Erase polygon.
    /// Additional data: polygon size (Poly).
    /// </summary>
    ErasePoly = 0x72,

    /// <summary>
    /// Invert polygon.
    /// Additional data: polygon size (Poly).
    /// </summary>
    InvertPoly = 0x73,

    /// <summary>
    /// Fill polygon.
    /// Additional data: polygon size (Poly).
    /// </summary>
    FillPoly = 0x74,

    /// <summary>
    /// Frame same polygon (not yet implemented).
    /// Additional data: 0 bytes.
    /// </summary>
    FrameSamePoly = 0x78,

    /// <summary>
    /// Paint same polygon (not yet implemented).
    /// Additional data: 0 bytes.
    /// </summary>
    PaintSamePoly = 0x79,

    /// <summary>
    /// Erase same polygon (not yet implemented).
    /// Additional data: 0 bytes.
    /// </summary>
    EraseSamePoly = 0x7A,

    /// <summary>
    /// Invert same polygon (not yet implemented).
    /// Additional data: 0 bytes.
    /// </summary>
    InvertSamePoly = 0x7B,

    /// <summary>
    /// Fill same polygon (not yet implemented).
    /// Additional data: 0 bytes.
    /// </summary>
    FillSamePoly = 0x7C,

    /// <summary>
    /// Frame region.
    /// Additional data: region size (Rgn).
    /// </summary>
    FrameRgn = 0x80,

    /// <summary>
    /// Paint region.
    /// Additional data: region size (Rgn).
    /// </summary>
    PaintRgn = 0x81,

    /// <summary>
    /// Erase region.
    /// Additional data: region size (Rgn).
    /// </summary>
    EraseRgn = 0x82,

    /// <summary>
    /// Invert region.
    /// Additional data: region size (Rgn).
    /// </summary>
    InvertRgn = 0x83,

    /// <summary>
    /// Fill region.
    /// Additional data: region size (Rgn).
    /// </summary>
    FillRgn = 0x84,

    /// <summary>
    /// Frame same region (not yet implemented).
    /// Additional data: 0 bytes.
    /// </summary>
    FrameSameRgn = 0x88,

    /// <summary>
    /// Paint same region (not yet implemented).
    /// Additional data: 0 bytes.
    /// </summary>
    PaintSameRgn = 0x89,

    /// <summary>
    /// Erase same region (not yet implemented).
    /// Additional data: 0 bytes.
    /// </summary>
    EraseSameRgn = 0x8A,

    /// <summary>
    /// Invert same region (not yet implemented).
    /// Additional data: 0 bytes.
    /// </summary>
    InvertSameRgn = 0x8B,

    /// <summary>
    /// Fill same region (not yet implemented).
    /// Additional data: 0 bytes.
    /// </summary>
    FillSameRgn = 0x8C,

    /// <summary>
    /// CopyBits with clipped rectangle.
    /// Additional data: variable.
    /// </summary>
    BitsRect = 0x90,

    /// <summary>
    /// CopyBits with clipped region.
    /// Additional data: variable.
    /// </summary>
    BitsRgn = 0x91,

    /// <summary>
    /// Packed CopyBits with clipped rectangle.
    /// Additional data: variable.
    /// </summary>
    PackBitsRect = 0x98,

    /// <summary>
    /// Packed CopyBits with clipped region.
    /// Additional data: variable.
    /// </summary>
    PackBitsRgn = 0x99,

    /// <summary>
    /// Short comment.
    /// Additional data: 2 bytes (Kind).
    /// </summary>
    ShortComment = 0xA0,

    /// <summary>
    /// Long comment.
    /// Additional data: 4+ bytes (Kind, size, data).
    /// </summary>
    LongComment = 0xA1,

    /// <summary>
    /// End of picture.
    /// Additional data: 0 bytes.
    /// </summary>
    EndOfPicture = 0xFF,
}
