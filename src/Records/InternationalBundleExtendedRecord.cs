using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an International Bundle Extended Record.
/// </summary>
public readonly struct InternationalBundleExtendedRecord
{
    /// <summary>
    /// The size of the International Bundle Extended Record in bytes.
    /// </summary>
    public const int Size = 30;

    /// <summary>
    /// The size of the record of script variables for this script system.
    /// </summary>
    public uint ScriptRecordSize { get; }

    /// <summary>
    /// The font family ID for the preferred font for monospaced text in this script system.
    /// </summary>
    public short MonospacedFontFamilyResourceID { get; }

    /// <summary>
    /// The default size for monospaced text in this script system.
    /// </summary>
    public ushort MonospacedFontSize { get; }

    /// <summary>
    /// This field is currently unused.
    /// </summary>
    public short PreferredFontFamilyResourceID { get; }

    /// <summary>
    /// This field is currently unused.
    /// </summary>
    public ushort PreferredFontSize { get; }

    /// <summary>
    /// The font family ID for the default font to display small text in this script system.
    /// </summary>
    public short SmallFontFamilyResourceID { get; }

    /// <summary>
    /// The default size for small text in this script system.
    /// </summary>
    public ushort SmallFontSize { get; }

    /// <summary>
    /// The font family ID for the preferred system font in this script system.
    /// </summary>
    public short SystemFontFamilyResourceID { get; }

    /// <summary>
    /// The default size for the system font in this script system.
    /// </summary>
    public ushort SystemFontSize { get; }

    /// <summary>
    /// The font family ID for the preferred application font in this script system.
    /// </summary>
    public short ApplicationFontFamilyResourceID { get; }

    /// <summary>
    /// The default size for the application font in this script system.
    /// </summary>
    public ushort ApplicationFontSize { get; }

    /// <summary>
    /// The font family ID for the preferred font for Balloon Help in this script system.
    /// </summary>
    public short HelpManagerFontFamilyResourceID { get; }

    /// <summary>
    /// The default size for the Balloon Help font in this script system.
    /// </summary>
    public ushort HelpManagerFontSize { get; }

    /// <summary>
    /// A style code that defines all of the valid styles for this script system.
    /// </summary>
    public byte ValidStyles { get; }

    /// <summary>
    /// A style code that defines the styles to use for displaying alias names in this script system.
    /// </summary>
    public byte AliasStyles { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InternationalBundleExtendedRecord"/> struct.
    /// </summary>
    /// <param name="data">The byte span containing the International Bundle Extended Record data.</param>
    /// <exception cref="ArgumentException">Thrown when the length of the data span is not equal to the expected size.</exception>
    public InternationalBundleExtendedRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length must be exactly {Size} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-20 to B-21
        int offset = 0;

        // The size of the record of script variables for this script system.
        // (A script system whose smsfAutoInit bit in its itlbFlags field
        // is set needs to provide this information for the Script Manager.)
        ScriptRecordSize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The font family ID for the preferred font for monospaced text in
        // this script system. The Script Manager initializes the high-order
        // word of the script variable accessed through the selector
        // smScriptMonoFondSize from this field.
        MonospacedFontFamilyResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The default size for monospaced text in this script system. The
        // Script Manager initializes the low-order word of the script variable
        // accessed through the selector smScriptMonoFondSize from this field.
        MonospacedFontSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // This field is currently unused.
        PreferredFontFamilyResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // This field is currently unused.
        PreferredFontSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The font family ID for the default font to display small text in
        // this script system. The Script Manager initializes the high-order
        // word of the script variable accessed through the selector
        // smScriptSmallFondSize from this field.
        SmallFontFamilyResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The default size for small text in this script system. The Script
        // Manager initializes the low-order word of the script variable
        // accessed through the selector smScriptSmallFondSize from this field.
        SmallFontSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));;
        offset += 2;

        // The font family ID for the preferred system font in this script
        // system. The Script Manager initializes the high-order word of the
        // script variable accessed through the selector smSysFondSize from
        // this field.
        SystemFontFamilyResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The default size for the system font in this script system. The Script
        // Manager initializes the script variable accessed through the selector
        // smScriptSysFond, and the low-order word of the script variable
        // accessed through the selector smSysFondSize, from this field.
        SystemFontSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The font family ID for the preferred application font in this script
        // system. The Script Manager initializes the script variable accessed
        // through the selector smScriptAppFond, and the high-order word
        // of the script variable accessed through the selector
        // smScriptAppFondSize, from this field.
        ApplicationFontFamilyResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The default size for the application font in this script system.
        // The Script Manager initializes the low-order word of the script
        // variable accessed through the selector smScriptAppFondSize
        // from this field.
        ApplicationFontSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The font family ID for the preferred font for Balloon Help in this
        // script system. The Script Manager initializes the high-order word of
        // the script variable accessed through the selector
        // smScriptHelpFondSize from this field.
        HelpManagerFontFamilyResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The default size for the Balloon Help font in this script system.
        // The Script Manager initializes the low-order word of the script
        // variable accessed through the selector smScriptHelpFondSize
        // from this field.
        HelpManagerFontSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // A style code that defines all of the valid styles for this script system.
        // (In a style code, the bit corresponding to each QuickDraw style is set
        // if that style is valid for the specified script. For example, the extend
        // style is not valid in the Arabic script system.) The Script Manager
        // initializes the script variable accessed through the selector
        // smScriptValidStyles from this field.
        ValidStyles = data[offset];
        offset += 1;

        // A style code that defines the styles to use for displaying alias
        // names in this script system. For example, in the Roman script
        // system, alias names are displayed in italics. The Script Manager
        // initializes the script variable accessed through the selector
        // smScriptAliasStyle from this field.
        AliasStyles = data[offset];
        offset += 1;

        Debug.Assert(offset == data.Length, "Did not read exactly the expected number of bytes for InternationalBundleExtendedRecord.");
    }
}
