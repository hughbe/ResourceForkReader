using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Structure of the header for a String Manipulation resource.
/// </summary>
public readonly struct StringManipulationResourceHeader
{
    /// <summary>
    /// The minimum size of the header in bytes.
    /// </summary>
    public const int MinSize = 10;

    /// <summary>
    /// Gets the offset to the init hook.
    /// </summary>
    public ushort InitHookOffset { get; }

    /// <summary>
    /// Gets the offset to the fetch hook.
    /// </summary>
    public ushort FetchHookOffset { get; }

    /// <summary>
    /// Gets the offset to the vernier hook.
    /// </summary>
    public ushort VernierHookOffset { get; }

    /// <summary>
    /// Gets the offset to the project hook.
    /// </summary>
    public ushort ProjectHookOffset { get; }

    /// <summary>
    /// Gets the version flag.
    /// </summary>
    public short VersionFlag { get; }

    /// <summary>
    /// Gets the offset to the exit hook.
    /// </summary>
    public ushort ExitHookOffset { get; }

    /// <summary>
    /// Gets the offset to the type list.
    /// </summary>
    public ushort TypeListOffset { get; }

    /// <summary>
    /// Gets the offset to the class array.
    /// </summary>
    public ushort ClassArrayOffset { get; }

    /// <summary>
    /// Gets the offset to the uppercase list.
    /// </summary>
    public ushort UppercaseListOffset { get; }

    /// <summary>
    /// Gets the offset to the lowercase list.
    /// </summary>
    public ushort LowercaseListOffset { get; }

    /// <summary>
    /// Gets the offset to the uppercase strip list.
    /// </summary>
    public ushort UppercaseStripListOffset { get; }

    /// <summary>
    /// Gets the offset to the word selection table.
    /// </summary>
    public ushort WordSelectionTableOffset { get; }

    /// <summary>
    /// Gets the offset to the line break table.
    /// </summary>
    public ushort LineBreakTableOffset { get; }

    /// <summary>
    /// Gets the offset to the strip list.
    /// </summary>
    public ushort StripListOffset { get; }

    /// <summary>
    /// Gets the version.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// Gets the format code.
    /// </summary>
    public ushort FormatCode { get; }

    /// <summary>
    /// Gets the length of the init hook.
    /// </summary>
    public ushort InitHookLength { get; }

    /// <summary>
    /// Gets the length of the fetch hook.
    /// </summary>
    public ushort FetchHookLength { get; }

    /// <summary>
    /// Gets the length of the vernier hook.
    /// </summary>
    public ushort VernierHookLength { get; }

    /// <summary>
    /// Gets the length of the project hook.
    /// </summary>
    public ushort ProjectHookLength { get; }

    /// <summary>
    /// Gets the reserved field.
    /// </summary>
    public ushort Reserved { get; }

    /// <summary>
    /// Gets the length of the exit hook.
    /// </summary>
    public ushort ExitHookLength { get; }

    /// <summary>
    /// Gets the length of the type list.
    /// </summary>
    public ushort TypeListLength { get; }

    /// <summary>
    /// Gets the length of the class array.
    /// </summary>
    public ushort ClassArrayLength { get; }

    /// <summary>
    /// Gets the length of the uppercase list.
    /// </summary>
    public ushort UppercaseListLength { get; }

    /// <summary>
    /// Gets the length of the lowercase list.
    /// </summary>
    public ushort LowercaseListLength { get; }

    /// <summary>
    /// Gets the length of the uppercase strip list.
    /// </summary>
    public ushort UppercaseStripListLength { get; }

    /// <summary>
    /// Gets the length of the word selection table.
    /// </summary>
    public ushort WordSelectionTableLength { get; }

    /// <summary>
    /// Gets the length of the line break table.
    /// </summary>
    public ushort LineBreakTableLength { get; }

    /// <summary>
    /// Gets the length of the strip list.
    /// </summary>
    public ushort StripListLength { get; }

    /// <summary>
    /// Gets the offset to the script run table.
    /// </summary>
    public ushort ScriptRunTableOffset { get; }

    /// <summary>
    /// Gets the length of the script run table.
    /// </summary>
    public ushort ScriptRunTableLength { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringManipulationResourceHeader"/> struct.
    /// </summary>
    /// <param name="data">The data for the header.</param>
    /// <param name="bytesRead">The number of bytes read from the data.</param>
    /// <exception cref="ArgumentException">Thrown if the data is not the correct length.</exception>
    public StringManipulationResourceHeader(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes for a StringManipulationResourceHeader.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-35 to B-36
        int offset = 0;

        // The first section of the header contains a version flag and five
        // offsets to the sorting hooks: init hook, fetch hook, vernier hook,
        // project hook, and exit hook. The sorting hooks are string-comparison
        // routines, code segments that control sorting behavior. The hooks
        // can replace or modify the built-in U.S. sorting behavior, on a 
        // character-by-character basis.
        // The Itl2 version flag is a long integer value that describes the
        // format of this string-manipulation resource. A value of –1
        // indicates that this string-manipulation resource is in the
        // system software version 6.0.4 (or newer) format. In versions
        // previous to 6.0.4, this element contains the offset to the
        // reserved hook, another sorting hook. In versions previous to
        // 6.0.4, the string-manipulation resource header stops at this point.
        InitHookOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        FetchHookOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        VernierHookOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ProjectHookOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        VersionFlag = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (VersionFlag != -1)
        {
            // Older format without the exit hook and further data.
            bytesRead = offset;
            return;
        }

        ExitHookOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The second section of the resource header contains the following
        // elements:
        // ■ Offsets to the character-type tables: type list, class array.
        // ■ Offsets to the case conversion and diacritical stripping lists:
        // uppercase list, lowercase list, uppercase strip list, strip list.
        // ■ Offsets to the word-break tables: word-selection table, line-break
        // table.
        // ■ Version number. The version number of this string-manipulation
        // resource.
        // ■ Format code. Contains 0 if the string-manipulation resource header
        // stops at this point (true for system software version 6.0.4 );
        // contains 1 if the string-manipulation resource header has the
        // format shown in Figure B-4 (true for system software version 7.0
        // and later).
        TypeListOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ClassArrayOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        UppercaseListOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        LowercaseListOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        UppercaseStripListOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        WordSelectionTableOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        LineBreakTableOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        StripListOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        FormatCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The third section of the header contains the lengths of all of the
        // code blocks and tables for which there are offsets in the first
        // two sections. The Script Manager requires valid length values in
        // this section only for those tables that can be accessed through
        // the GetIntlResourceTable procedure (the word-selection and
        // line-break tables).
        InitHookLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        FetchHookLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        VernierHookLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ProjectHookLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Reserved = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ExitHookLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        TypeListLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ClassArrayLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        UppercaseListLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        LowercaseListLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        UppercaseStripListLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        WordSelectionTableLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        LineBreakTableLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        StripListLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The fourth section contains offset and length pairs for additional
        // tables. The first pair in this section is used for an optional
        // table (findScriptTable) defining characters of a subscript within
        // a non-Roman script system. It is used by the Script Manager
        // FindScriptRun function. See “Script Run Table Format” beginning
        // on page B-40. If this table is not present, the offset and length
        // are 0.
        ScriptRunTableOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        ScriptRunTableLength = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));;
        offset += 2;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all data for StringManipulationResourceHeader.");
    }
}
