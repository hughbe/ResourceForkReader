using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an International Bundle Record ('itlb') in a resource fork.
/// </summary>
public readonly struct InternationalBundleRecord
{
    /// <summary>
    /// Gets the size of the International Bundle Record in bytes.
    /// </summary>
    public const int MinSize = 20;

    /// <summary>
    /// Gets the resource ID of the numeric format resource ('intl0').
    /// </summary>
    public short NumericFormatResourceID { get; }

    /// <summary>
    /// Gets the resource ID of the date format resource ('intl1').
    /// </summary>
    public short DateFormatResourceID { get; }

    /// <summary>
    /// Gets the resource ID of the string manipulation resource ('intl2').
    /// </summary>
    public short StringManipulationResourceID { get; }

    /// <summary>
    /// Gets the flags for the international bundle.
    /// </summary>
    public ushort Flags { get; }

    /// <summary>
    /// Gets the resource ID of the token resource ('intl4').
    /// </summary>
    public short TokenResourceID { get; }

    /// <summary>
    /// Gets the resource ID of the encoding resource ('intl5').
    /// </summary>
    public short EncodingResourceID { get; }

    /// <summary>
    /// Gets the language code.
    /// </summary>
    public ushort LanguageCode { get; }

    /// <summary>
    /// Gets the numeral code.
    /// </summary>
    public sbyte NumeralCode { get; }

    /// <summary>
    /// Gets the calendar code.
    /// </summary>
    public sbyte CalendarCode { get; }

    /// <summary>
    /// Gets the resource ID of the keyboard layout resource ('KCHR').
    /// </summary>
    public short KeyboardLayoutResourceID { get; }

    /// <summary>
    /// Gets the resource ID of the keyboard icon resource ('kcs#', 'kcs4', 'kcs8').
    /// </summary>
    public short KeyboardIconResourceID { get; }

    /// <summary>
    /// Gets the extended record, if present.
    /// </summary>
    public InternationalBundleExtendedRecord? ExtendedRecord { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InternationalBundleRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the international bundle record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than 22 bytes long.</exception>
    public InternationalBundleRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data is too short to be a valid International Bundle Record. Minimum size is {MinSize} bytes.", nameof(data));   
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-17 to B-21
        // and https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L760-L787
        int offset = 0;

        // The resource ID of the numeric-format ('itl0') resource to be used
        // by this script. The Script Manager initializes the script variable
        // accessed through the selector smScriptNumber from this field.
        NumericFormatResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The resource ID of the long-date-format ('itl1') resource to be
        // used by this script. The Script Manager initializes the script variable
        // accessed through the selector smScriptDate from this field.
        DateFormatResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The resource ID of the string-manipulation ('itl2') resource to
        // be used by this script system. The Script Manager initializes
        // the script variable accessed through the selector smScriptSort
        // from this field.
        StringManipulationResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The bit flags that describe the features of this script system. The
        // Script Manager initializes the script flags variable, accessed through
        // the selector smScriptFlags, from this field. For example, you can
        // set the smsfAutoInit bit in the itlbFlags field to instruct the
        // Script Manager to initialize the script system automatically. For
        // definitions of the constants that specify the components of the script
        // flags word, see the list of selectors for script variables in the chapter
        // “Script Manager” in this book.
        Flags = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The resource ID of the tokens ('itl4') resource to be used by this
        // script. The Script Manager initializes the script variable accessed
        // through the selector smScriptToken from this field.
        TokenResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The resource ID of the encoding/rendering ('itl5') resource to be
        // used by this script system. The Script Manager initializes the script
        // variable accessed through the selector smScriptEncoding
        // from this field. If there is no encoding/rendering resource for this
        // script, this field is set to 0. This field was reserved in versions of
        // system software prior to 7.0.
        EncodingResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The language code specifying the default language for this script.
        // The Script Manager initializes the script variable accessed through
        // the selector smScriptLang from this field. See the chapter “Script
        // Manager” in this book for a list of defined language codes.
        LanguageCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The numeral code to be used by this script system. This byte
        // specifies which types of numerals the script supports. The Script
        // Manager initializes the high-order byte of the script variable
        // accessed through the selector smScriptNumDate from this field.
        // For definitions of the constants that specify numeral codes, see the
        // list of selectors for script variables in the chapter “Script Manager”
        // in this book.
        NumeralCode = (sbyte)data[offset];
        offset += 1;

        // The calendar code to be used by this script system. This byte
        // specifies which types of calendars the script supports. The Script
        // Manager initializes the low-order byte of the script variable accessed
        // through the selector smScriptNumDate from this field. For
        // definitions of the constants that specify calendar codes, see the list of
        // selectors for script variables in the chapter “Script Manager” in this
        // book.
        CalendarCode = (sbyte)data[offset];
        offset += 1;

        // The resource ID of the preferred keyboard-layout ('KCHR') resource
        // to be used by this script system. The Script Manager initializes the
        // script variable accessed through the selector smScriptKeys from
        // this field.
        KeyboardLayoutResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The resource ID of the keyboard icon family (resource types
        // 'kcs#', 'kcs4', and 'kcs8') for the default keyboard layout to
        // be used with this script. The Script Manager initializes the script
        // variable accessed through the selector smScriptIcon from this
        // field. (When loading a keyboard-layout resource, the Script Manager
        // in fact ignores that variable and looks only for a keyboard icon suite
        // whose ID matches that of the keyboard-layout resource being
        // loaded.)
        KeyboardIconResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (offset < data.Length)
        {
            ExtendedRecord = new InternationalBundleExtendedRecord(data.Slice(offset, InternationalBundleExtendedRecord.Size));
            offset += InternationalBundleExtendedRecord.Size;
        }

        Debug.Assert(offset == data.Length, "Parsed beyond the end of the data span.");
    }
}