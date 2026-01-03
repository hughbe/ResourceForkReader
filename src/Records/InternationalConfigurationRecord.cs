using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an International Configuration Record ('itlc') in a resource fork.
/// </summary>
public readonly struct InternationalConfigurationRecord
{
    /// <summary>
    /// Gets the size of a small International Configuration Record in bytes.
    /// </summary>
    public const int SizeSmall = 6;

    /// <summary>
    /// Gets the size of an International Configuration Record in bytes.
    /// </summary>
    public const int Size = 48;

    /// <summary>
    /// Gets the system script code.
    /// </summary>
    public ushort SystemScriptCode { get; }

    /// <summary>
    /// Gets the keyboard cache size.
    /// </summary>
    public ushort KeyboardCacheSize { get; }

    /// <summary>
    /// Gets a value indicating whether font forcing is enabled.
    /// </summary>
    public bool FontForce { get; }

    /// <summary>
    /// Gets a value indicating whether international resources are forced.
    /// </summary>
    public bool InternationalResourcesForce { get; }

    /// <summary>
    /// Gets a value indicating whether an old keyboard is used.
    /// </summary>
    public bool OldKeyboard { get; }

    /// <summary>
    /// Gets the international configuration flags.
    /// </summary>
    public InternationalConfigurationFlags Flags { get; }

    /// <summary>
    /// Gets the script icon offset.
    /// </summary>
    public ushort ScriptIconOffset { get; }

    /// <summary>
    /// Gets a value indicating whether the script icon is on the side.
    /// </summary>
    public bool ScriptIconSide { get; }

    /// <summary>
    /// Gets the script icon reserved byte.
    /// </summary>
    public byte ScriptIconReserved { get; }

    /// <summary>
    /// Gets the region code.
    /// </summary>
    public ushort RegionCode { get; }

    /// <summary>
    /// Gets the global system flags.
    /// </summary>
    public InternationalConfigurationSystemFlags SystemFlags { get; }

    /// <summary>
    /// Gets the reserved bytes.
    /// </summary>
    public byte[]? Reserved { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InternationalConfigurationRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing International Configuration Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 48 bytes long.</exception>
    public InternationalConfigurationRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size && data.Length != SizeSmall)
        {
            throw new ArgumentException($"Data is too short to be a valid International Configuration Record. Minimum size is {Size} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-9 to B-11
        // and https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L788-L809
        int offset = 0;

        // The script code defining the system script. The system script affects
        // system default settings, such as the default font and the text that
        // appears in dialog boxes and menu bars, and so forth. Script codes
        // and their constants are listed in the chapter “Script Manager” in
        // this book. At startup, this value is copied into the Script Manager
        // variable accessed through the selector smSysScript.
        SystemScriptCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Reserved.
        KeyboardCacheSize = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The initial setting for the font force flag. A value of TRUE ($FF)
        // forces Roman fonts to be interpreted as belonging to the system
        // script. The font force flag is described in the chapter “Script
        // Manager” in this book. At startup, this value is copied into the
        // Script Manager variable accessed through the selector smFontForce.
        FontForce = data[offset] != 0x00;
        offset += 1;

        // The initial setting for the international resources selection flag.
        // A value of TRUE ($FF) forces Text Utilities routines to use the
        // international resources for the system script, rather than the
        // font script. The international resources selection flag is
        // described in the chapter “Script Manager” in this book.
        // At startup, this value is copied into the Script Manager variable
        // accessed through the selector smIntlForce..
        InternationalResourcesForce = data[offset] != 0x00;
        offset += 1;

        if (data.Length == SizeSmall)
        {
            // Seen examples in System 4.1 where only the first 6 bytes are
            // present.
            Reserved = null;
            return;
        }

        // The initial setting for the international keyboard flag for use
        // by the Macintosh Plus computer. In addition to the standard
        // Macintosh Plus keyboard (keyboard type 11), two types of smaller
        // keyboard without numeric keypad are available: a U.S. version and
        // an international version. Both have a keyboard type of 3, and the
        // user uses the Keyboard control panel to indicate which is being
        // used; the user’s selection is saved in this field. When TRUE ($FF),
        // this flag indicates that the international keyboard is being used.
        // When FALSE, this flag indicates that the U.S. keyboard is being used.
        OldKeyboard = data[offset] != 0x00;
        offset += 1;

        // The initial settings for the Script Manager general flags.
        // At startup, this value is copied into the first (high-order)
        // byte of the Script Manager variable accessed through the
        // selector smGenFlags.
        Flags = (InternationalConfigurationFlags)data[offset];
        offset += 1;

        // (reserved).
        ScriptIconOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // (reserved).
        ScriptIconSide = data[offset] != 0x00;
        offset += 1;

        // (reserved).
        ScriptIconReserved = data[offset];
        offset += 1;

        // The region code for this version of system software. It specifies
        // the region for which the system and system-script resources were
        // localized. The constants that define region codes are also
        // described in the chapter “Script Manager” in this book. At
        // startup, this value is copied into the Script Manager
        // variable accessed through the selector smRegionCode.
        RegionCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Flags for setting system global variables. Currently only one bit
        // is defined; it allows the configuration resource to set the system
        // direction (left-to-right or right-to-left) at startup. It is bit 15,
        // defined by the following constant:
        // CONST
        //  itlcSysDirection = 15;
        // The system global SysDirection is initialized from this value. A
        // value of 0 for bit 15 sets a system direction of left-to-right
        // (SysDirection = 0) at startup, whereas a value of 1 for the bit
        // sets a system direction of right-to-left (SysDirection = $FFFF).
        // You can access SysDirection through the Script Manager routines
        // GetSysDirection and SetSysDirection. System direction may
        // be initially localized to a value appropriate for the system
        // script, but the user can reset its value at any time if a
        // bidirectional script system is present.
        SystemFlags = (InternationalConfigurationSystemFlags)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // An array of 32 bytes that is reserved for future use.
        Reserved = data.Slice(offset, 32).ToArray();
        offset += Reserved.Length;

        Debug.Assert(offset == data.Length, "Did not parse all bytes of International Configuration Record.");
    }
}
