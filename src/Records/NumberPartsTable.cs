using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// The Number Parts Table in a Tokens Record ('itl4') in a resource fork.
/// </summary>
public readonly struct NumberPartsTable
{
    /// <summary>
    /// The minimum size of a Number Parts Table in bytes.
    /// </summary>
    public const int Size = 172;

    /// <summary>
    /// Gets the version.
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// Gets the character array data.
    /// </summary>
    public char[] Data { get; }

    /// <summary>
    /// Gets the positive exponents.
    /// </summary>
    public WideCharArr PositiveExponents { get; }

    /// <summary>
    /// Gets the negative exponents.
    /// </summary>
    public WideCharArr NegativeExponents { get; }

    /// <summary>
    /// Gets the positive exponents for negative format.
    /// </summary>
    public WideCharArr PositiveExponentsForNegativeFormat { get; }
    
    /// <summary>
    /// Gets the alternate numerals.
    /// </summary>
    public WideCharArr AlternateNumerals { get; }

    /// <summary>
    /// Gets the reserved data.
    /// </summary>
    public byte[] Reserved { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NumberPartsTable"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Number Parts Table data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not the correct size for a Number Parts Table.</exception>
    public NumberPartsTable(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data is not the correct size for a Number Parts Table. Size is {Size} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // B-55 to B-56
        int offset = 0;

        // An integer that specifies which version of the number parts table
        // is being used. A value of 1 specifies the first version.
        Version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // An array of 31 wide characters (2 bytes each), indexed by a set
        // of constants. Each element of the array, accessed by one of the
        // constants, contains 1 or 2 bytes that make up that number part.
        // (If the element contains only one 1-byte character, it is in the
        // low-order byte and the high-order byte contains 0.) Each number
        // part, then, may consist of one or two 1-byte characters, or a
        // single 2-byte character.
        // Of the 31 allotted spaces, 15 through 31 are reserved for up to
        // 17 unquoted characters—special literals that do not need to be
        // enclosed in quotes in a numeric string. See the discussion of
        // number formatting in the chapter “Text Utilities” in this book
        // for more information. 
        // These are the defined constants for accessing number parts in the 
        // data array:
        // Constant Value Explanation
        // tokLeftQuote 1 Left quote
        // tokRightQuote 2 Right quote
        // tokLeadPlacer 3 Spacing leader format marker
        // tokLeader 4 Spacing leader character
        // tokNonLeader 5 No leader format marker
        // tokZeroLead 6 Zero leader format marker
        // tokPercent 7 Percent
        // tokPlusSign 8 Plus
        // tokMinusSign 9 Minus
        // tokThousands 10 Thousands separator
        // 11 (reserved)
        // tokSeparator 12 List separator
        // tokEscape 13 Escape character
        // tokDecPoint 14 Decimal separator
        // tokUnquoteds 15 (first unquoted character)
        // (15 through 31 reserved)
        // tokMaxSymbols 31 Maximum symbol (for range check)
        var dataArray = new char[31];
        for (int i = 0; i < 31; i++)
        {
            dataArray[i] = (char)BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        Data = dataArray;

        // An array that specifies how to represent positive exponents for
        // scientific notation. It is a wide character array, an 11-word
        // data structure defined by the WideCharArr data type. It contains
        // up to ten 1-byte or 2-byte number parts for representing positive
        // exponents.
        PositiveExponents = new WideCharArr(data.Slice(offset, WideCharArr.Size));
        offset += WideCharArr.Size;

        // An array that specifies how to represent negative exponents for
        // scientific notation. It is a wide character array, an 11-word
        // array defined by the WideCharArr data type. It contains up to
        // ten 1-byte or 2-byte number parts for representing negative exponents.
        NegativeExponents = new WideCharArr(data.Slice(offset, WideCharArr.Size));
        offset += WideCharArr.Size;

        // An array that specifies how to represent positive exponents for
        // scientific notation when the format string exponent is negative.
        // Symbols from this array can be used with the input number string
        // to the StringToExtended function; they are not for use with the
        // StringToFormatRec function. The array is a wide character array,
        // an 11-word array defined by the WideCharArr data type. It
        // contains up to ten 1-byte or 2-byte number parts for representing
        // positive exponents.
        PositiveExponentsForNegativeFormat = new WideCharArr(data.Slice(offset, WideCharArr.Size));
        offset += WideCharArr.Size;

        // A wide character array that specifies the alternate representation
        // of numerals. The array contains ten character codes, each of which
        // represents an alternate numeral. If the smsfB0Digits bit of the
        // script-flags word is set, you should substitute the characters in
        // this array for the character codes $30–$39 (regular ASCII numerals)
        // in a string whose token code is tokenAltNum or tokenAltReal.
        // Alternate numerals and the script flags word are described with
        // the list of selectors for script variables in the chapter “Script
        // Manager” in this book.
        AlternateNumerals = new WideCharArr(data.Slice(offset, WideCharArr.Size));
        offset += WideCharArr.Size;

        // (reserved for future expansion)
        Reserved = data.Slice(offset, 19).ToArray();
        offset += 19;

        Debug.Assert(offset <= data.Length, "Did not consume all data for NumberPartsTable.");
    }
}