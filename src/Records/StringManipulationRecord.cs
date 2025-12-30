using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a String Manipulation resource ('itl2') in a resource fork.
/// </summary>
public readonly struct StringManipulationRecord
{
    /// <summary>
    /// Gets the header for the String Manipulation resource.
    /// </summary>
    public StringManipulationResourceHeader Header { get; }

    /// <summary>
    /// Gets the initialization hook code.
    /// </summary>
    public byte[]? InitHook { get; }

    /// <summary>
    /// Gets the fetch hook code.
    /// </summary>
    public byte[]? FetchHook { get; }

    /// <summary>
    /// Gets the project hook code.
    /// </summary>
    public byte[]? ProjectHook { get; }

    /// <summary>
    /// Gets the vernier hook code.
    /// </summary>
    public byte[]? VernierHook { get; }

    /// <summary>
    /// Gets the exit hook code.
    /// </summary>
    public byte[]? ExitHook { get; }

    /// <summary>
    /// Gets the character-type table.
    /// </summary>
    public byte[]? TypeList { get; }

    /// <summary>
    /// Gets the class array table.
    /// </summary>
    public byte[]? ClassArray { get; }

    /// <summary>
    /// Gets the uppercase list table.
    /// </summary>
    public byte[]? UppercaseList { get; }

    /// <summary>
    /// Gets the lowercase list table.
    /// </summary>
    public byte[]? LowercaseList { get; }

    /// <summary>
    /// Gets the uppercase strip list table.
    /// </summary>
    public byte[]? UppercaseStripList { get; }

    /// <summary>
    /// Gets the lowercase strip list table.
    /// </summary>
    public byte[]? StripList { get; }

    /// <summary>
    /// Gets the word selection table.
    /// </summary>
    public byte[]? WordSelectionTable { get; }
    
    /// <summary>
    /// Gets the line break table.
    /// </summary>
    public byte[]? LineBreakTable { get; }

    /// <summary>
    /// Gets the script run table.
    /// </summary>
    public byte[]? ScriptRunTable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringManipulationRecord"/> struct.
    /// </summary>
    /// <param name="data">The binary data for the record.</param>
    public StringManipulationRecord(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-34 to B-50
        int offset = 0;

        // Header.
        Header = new StringManipulationResourceHeader(data.Slice(offset), out var bytesRead);
        offset += bytesRead;

        // String comparison routines (sorting hooks)
        if (Header.InitHookLength != 0)
        {
            if (Header.InitHookOffset + Header.InitHookLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the InitHook.", nameof(data));
            }

            InitHook = data.Slice(offset, Header.InitHookLength).ToArray();
        }
        else
        {
            InitHook = null;
        }

        if (Header.FetchHookLength != 0)
        {
            if (Header.FetchHookOffset + Header.FetchHookLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the FetchHook.", nameof(data));
            }

            FetchHook = data.Slice(offset, Header.FetchHookLength).ToArray();
        }
        else
        {
            FetchHook = null;
        }

        if (Header.ProjectHookLength != 0)
        {
            if (Header.ProjectHookOffset + Header.ProjectHookLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the ProjectHook.", nameof(data));
            }

            ProjectHook = data.Slice(offset, Header.ProjectHookLength).ToArray();
        }
        else
        {
            ProjectHook = null;
        }

        if (Header.VernierHookLength != 0)
        {
            if (Header.VernierHookOffset + Header.VernierHookLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the VernierHook.", nameof(data));
            }

            VernierHook = data.Slice(offset, Header.VernierHookLength).ToArray();
        }
        else
        {
            VernierHook = null;
        }

        if (Header.ExitHookLength != 0)
        {
            if (Header.ExitHookOffset + Header.ExitHookLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the ExitHook.", nameof(data));
            }

            ExitHook = data.Slice(offset, Header.ExitHookLength).ToArray();
        }
        else
        {
            ExitHook = null;
        }

        // Character-type tables (optional)
        // Type list. Contains character-type information for each class of
        // character (as specified by the class array) in the script system’s
        // character set. The Script Manager CharacterType function uses this
        // table. The type list is used by 1-byte script systems only;
        // character-type information for a 2-byte script system is in that
        // script’s encoding/ rendering ('itl5') resource.
        if (Header.TypeListLength != 0)
        {
            if (Header.TypeListOffset + Header.TypeListLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the TypeList.", nameof(data));
            }

            TypeList = data.Slice(Header.TypeListOffset, Header.TypeListLength).ToArray();
        }
        else
        {
            TypeList = null;
        }

        // Class array. Maps each character in the script system’s character
        // set to a class, which is used to index into the other character
        // tables in the string-manipulation resource. The Script Manager
        // CharacterType function uses this table. The class array is used
        // by 1-byte script systems only; character-class information for a
        // 2-byte script system is in that script’s encoding/rendering
        // ('itl5') resource.
        if (Header.ClassArrayLength != 0)
        {
            if (Header.ClassArrayOffset + Header.ClassArrayLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the ClassArray.", nameof(data));
            }

            ClassArray = data.Slice(Header.ClassArrayOffset, Header.ClassArrayLength).ToArray();
        }
        else
        {
            ClassArray = null;
        }

        // Case-conversion and stripping tables (optional)


        // Uppercase list. Used to generate uppercase equivalents for all
        // lowercase characters in the script system’s character set. For each
        // character class, contains a value to be added to the character code
        // to convert all characters to uppercase. The Text Utilities
        // UppercaseText procedure uses this table. The uppercase list is used
        // by 1-byte script systems only.
        if (Header.UppercaseListLength != 0)
        {
            if (Header.UppercaseListOffset + Header.UppercaseListLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the UppercaseList.", nameof(data));
            }

            UppercaseList = data.Slice(Header.UppercaseListOffset, Header.UppercaseListLength).ToArray();
        }
        else
        {
            UppercaseList = null;
        }

        // Lowercase list. Used to generate lowercase equivalents for all
        // uppercase characters in the script system’s character set. For
        // each character class, contains a value to be added to the
        // character code to convert all characters to lowercase. The
        // Text Utilities LowercaseText procedure uses this table. The
        // lowercase list is used by 1-byte script systems only.
        if (Header.LowercaseListLength != 0)
        {
            if (Header.LowercaseListOffset + Header.LowercaseListLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the LowercaseList.", nameof(data));
            }

            LowercaseList = data.Slice(Header.LowercaseListOffset, Header.LowercaseListLength).ToArray();
        }
        else
        {
            LowercaseList = null;
        }

        // Uppercase strip list. Used to generate uppercase equivalents—without
        // diacritical marks—for all characters in the script system’s character
        // set. For each character class, contains a value to be added to the
        // character code to convert all characters to uppercase versions without
        // diacritical marks. The Text Utilities UppercaseStripDiacritics
        // procedure uses this table. The uppercase strip list is used by
        // 1-byte script systems only.
        if (Header.UppercaseStripListLength != 0)
        {
            if (Header.UppercaseStripListOffset + Header.UppercaseStripListLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the UppercaseStripList.", nameof(data));
            }

            UppercaseStripList = data.Slice(Header.UppercaseStripListOffset, Header.UppercaseStripListLength).ToArray();
        }
        else
        {
            UppercaseStripList = null;
        }

        // Strip list. Used to generate equivalents—without diacritical
        // marks—for all characters in the script system’s character set.
        // For each character class, contains a value to be added to the
        // character code to strip diacritical marks. The Text Utilities
        // StripDiacritics procedure uses this table. The strip list is
        // used by 1-byte script systems only.
        if (Header.StripListLength != 0)
        {
            if (Header.StripListOffset + Header.StripListLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the StripList.", nameof(data));
            }

            StripList = data.Slice(Header.StripListOffset, Header.StripListLength).ToArray();
        }
        else
        {
            StripList = null;
        }

        // Word break tables
        // Word-selection table. A table of data type NBreakTable or
        // BreakTable, used by the Text Utilities FindWordBreaks procedure
        // to find word boundaries for the purposes of word selection.
        // See “Supplying Custom Word-Break Tables” on page B-44 for a
        // description of the break-table formats.
        if (Header.WordSelectionTableLength != 0)
        {
            if (Header.WordSelectionTableOffset + Header.WordSelectionTableLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the WordSelectionTable.", nameof(data));
            }

            WordSelectionTable = data.Slice(Header.WordSelectionTableOffset, Header.WordSelectionTableLength).ToArray();
        }
        else
        {
            WordSelectionTable = null;
        }
        
        // Line-break table. A table of data type NBreakTable or BreakTable, used by theText Utilities FindWordBreaks procedure to find word boundaries for breaking lines of text. The rules governing word boundaries for line breaking are generally somewhat different from those for word selection. ■ Script run table. A data structure used by the Text Utilities FindScriptRun function. It is used to find runs of a subscript, such as Roman, within text of a non-Roman script system. See the next section.
        if (Header.LineBreakTableLength != 0)
        {
            if (Header.LineBreakTableOffset + Header.LineBreakTableLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the LineBreakTable.", nameof(data));
            }

            LineBreakTable = data.Slice(Header.LineBreakTableOffset, Header.LineBreakTableLength).ToArray();
        }
        else
        {
            LineBreakTable = null;
        }

        // Subscript table (optional)
        if (Header.ScriptRunTableLength != 0)
        {
            if (Header.ScriptRunTableOffset + Header.ScriptRunTableLength > data.Length)
            {
                throw new ArgumentException("Data is too short to contain the ScriptRunTable.", nameof(data));
            }

            ScriptRunTable = data.Slice(Header.ScriptRunTableOffset, Header.ScriptRunTableLength).ToArray();
        }
        else
        {
            ScriptRunTable = null;
        }

        Debug.Assert(offset <= data.Length, "Did not consume all data for StringManipulationRecord.");
    }
}
