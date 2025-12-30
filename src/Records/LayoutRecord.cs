using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// 
/// </summary>
public readonly struct LayoutRecord
{
    /// <summary>
    /// The size in bytes of a LayoutRecord.
    /// </summary>
    public const int Size = 66;

    /// <summary>
    /// Gets the font resource ID used in the layout.
    /// </summary>
    public short FontResourceID { get; }

    /// <summary>
    /// Gets the font size used in the layout.
    /// </summary>
    public ushort FontSize { get; }

    /// <summary>
    /// Gets the height of the screen header.
    /// </summary>
    public ushort ScreenHeaderHeight { get; }

    /// <summary>
    /// Gets the top line break position.
    /// </summary>
    public short TopLineBreak { get; }

    /// <summary>
    /// Gets the bottom line break position.
    /// </summary>
    public short BottomLineBreak { get; }

    /// <summary>
    /// Gets the printing footer height.
    /// </summary>
    public short PrintingHeaderHeight { get; }

    /// <summary>
    /// Gets the printing footer height.
    /// </summary>
    public short PrintingFooterHeight { get; }

    /// <summary>
    /// Gets the window rectangle.
    /// </summary>
    public RECT WindowRectangle { get; init; }

    /// <summary>
    /// Gets the line spacing.
    /// </summary>
    public ushort LineSpacing { get; }

    /// <summary>
    /// Gets the first tab stop position.
    /// </summary>
    public ushort TabStop1 { get; }

    /// <summary>
    /// Gets the second tab stop position.
    /// </summary>
    public ushort TabStop2 { get; }

    /// <summary>
    /// Gets the third tab stop position.
    /// </summary>
    public ushort TabStop3 { get; }

    /// <summary>
    /// Gets the fourth tab stop position.
    /// </summary>
    public ushort TabStop4 { get; }

    /// <summary>
    /// Gets the fifth tab stop position.
    /// </summary>
    public ushort TabStop5 { get; }

    /// <summary>
    /// Gets the sixth tab stop position.
    /// </summary>
    public ushort TabStop6 { get; }

    /// <summary>
    /// Gets the seventh tab stop position.
    /// </summary>
    public ushort TabStop7 { get; }

    /// <summary>
    /// Gets the column justification.
    /// </summary>
    public byte ColumnJustification { get; }

    /// <summary>
    /// Gets the reserved byte.
    /// </summary>
    public byte Reserved1 { get; }

    /// <summary>
    /// Gets the icon horizontal spacing.
    /// </summary>
    public ushort IconHorizontalSpacing { get; }

    /// <summary>
    /// Gets the icon vertical spacing.
    /// </summary>
    public ushort IconVerticalSpacing { get; }

    /// <summary>
    /// Gets the icon vertical phase.
    /// </summary>
    public ushort IconVerticalPhase { get; }

    /// <summary>
    /// Gets the small icon horizontal spacing.
    /// </summary>
    public ushort SmallIconHorizontalSpacing { get; }

    /// <summary>
    /// Gets the small icon vertical spacing.
    /// </summary>
    public ushort SmallIconVerticalSpacing { get; }

    /// <summary>
    /// Gets the default view.
    /// </summary>
    public byte DefaultView { get; }

    /// <summary>
    /// Gets the reserved byte.
    /// </summary>
    public byte Reserved2 { get; }

    /// <summary>
    /// Gets the text view date.
    /// </summary>
    public ushort TextViewDate { get; }

    /// <summary>
    /// Gets the layout record flags.
    /// </summary>
    public LayoutRecordFlags1 Flags1 { get; }

    /// <summary>
    /// Gets the icon text gap.
    /// </summary>
    public byte IconTextGap { get; }

    /// <summary>
    /// Gets the sort style.
    /// </summary>
    public ushort SortStyle { get; }

    /// <summary>
    /// Gets the watch threshold.
    /// </summary>
    public uint WatchThreshold { get; }

    /// <summary>
    /// Gets the layout record flags 2.
    /// </summary>
    public LayoutRecordFlags2 Flags2 { get; }

    /// <summary>
    /// Gets the color style.
    /// </summary>
    public byte ColorStyle { get; }

    /// <summary>
    /// Gets the maximum number of windows.
    /// </summary>
    public ushort MaximumNumberOfWindows { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LayoutRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the layout record data.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="data"/> is not the correct size.</exception>
    public LayoutRecord(Span<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be at least {Size} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L824-L876
        int offset = 0;
        
        FontResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        FontSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ScreenHeaderHeight = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TopLineBreak = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        BottomLineBreak = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        PrintingHeaderHeight = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        PrintingFooterHeight = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        WindowRectangle = new RECT(data.Slice(offset, RECT.Size));
        offset += RECT.Size;

        LineSpacing = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TabStop1 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TabStop2 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TabStop3 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TabStop4 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TabStop5 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TabStop6 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TabStop7 = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ColumnJustification = data[offset];
        offset += 1;

        Reserved1 = data[offset];
        offset += 1;

        IconHorizontalSpacing = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        IconVerticalSpacing = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        IconVerticalPhase = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        SmallIconHorizontalSpacing = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        SmallIconVerticalSpacing = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        DefaultView = data[offset];
        offset += 1;

        Reserved2 = data[offset];
        offset += 1;

        TextViewDate = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Flags1 = (LayoutRecordFlags1)data[offset];
        offset += 1;

        IconTextGap = data[offset];
        offset += 1;

        SortStyle = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        WatchThreshold = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Flags2 = (LayoutRecordFlags2)data[offset];
        offset += 1;

        ColorStyle = data[offset];
        offset += 1;

        MaximumNumberOfWindows = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == Size, "Did not read all data for LayoutRecord.");
    }

    /// <summary>
    /// Flags for the LayoutRecord.
    /// </summary>
    [Flags]
    public enum LayoutRecordFlags1 : byte
    {
        /// <summary>
        /// Indicates whether zoom rectangles are used.
        /// </summary>
        UseZoomRectangles = 1 << 7,

        /// <summary>
        /// Indicates whether to skip trash warning.
        /// </summary>
        SkipTrashWarning = 1 << 6,

        /// <summary>
        /// Indicates whether grid drags are always enabled.
        /// </summary>
        AlwaysGridDrags = 1 << 5,

        /// <summary>
        /// Unused flag 4.
        /// </summary>
        Unused4 = 1 << 4,

        /// <summary>
        /// Unused flag 3.
        /// </summary>
        Unused3 = 1 << 3,

        /// <summary>
        /// Unused flag 2.
        /// </summary>
        Unused2 = 1 << 2,

        /// <summary>
        /// Unused flag 1.
        /// </summary>
        Unused1 = 1 << 1,

        /// <summary>
        /// Unused flag 0.
        /// </summary>
        Unused0 = 1 << 0,
    }
    
    /// <summary>
    /// Flags2 for the LayoutRecord.
    /// </summary>
    [Flags]
    public enum LayoutRecordFlags2 : byte
    {
        /// <summary>
        /// Unused flag 7.
        /// </summary>
        Unused7 = 1 << 7,

        /// <summary>
        /// Unused flag 6.
        /// </summary>
        Unused6 = 1 << 6,

        /// <summary>
        /// Unused flag 5.
        /// </summary>
        Unused5 = 1 << 5,

        /// <summary>
        /// Unused flag 4.
        /// </summary>
        Unused4 = 1 << 4,

        /// <summary>
        /// Indicates whether to use physical icons.
        /// </summary>
        UsePhysicalIcon = 1 << 3,

        /// <summary>
        /// Indicates whether the title is clickable.
        /// </summary>
        TitleClickable = 1 << 2,

        /// <summary>
        /// Indicates whether to copy on inherit.
        /// </summary>
        CopyInherit = 1 << 1,

        /// <summary>
        /// Indicates whether to new fold inherit.
        /// </summary>
        NewFoldInherit = 1 << 0,
    }
}