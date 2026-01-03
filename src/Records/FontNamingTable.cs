using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Font Naming Table in a resource fork.
/// </summary>
public readonly struct FontNamingTable
{
    /// <summary>
    /// Minimum size of a Font Naming Table in bytes.
    /// </summary>
    public const int MinSize = 6;

    /// <summary>
    /// Gets the format selector.
    /// </summary>
    public ushort FormatSelector { get; }

    /// <summary>
    /// Gets the number of names.
    /// </summary>
    public ushort NumberOfNames { get; }

    /// <summary>
    /// Gets the string area offset.
    /// </summary>
    public ushort StringAreaOffset { get; }

    /// <summary>
    /// Gets the list of font names.
    /// </summary>
    public List<FontName> Names { get; }

    /// <summary>
    /// Gets the array of name strings.
    /// </summary>
    public string[] NameStrings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontNamingTable"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Font Naming Table data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public FontNamingTable(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than the minimum required size of {MinSize} bytes for a Font Naming Table.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-84 to 4-88
        int offset = 0;

        // Format selector. The format selector (set to 0). This is an unsigned integer.
        FormatSelector = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        //  Number of names. The number of name records that follow. This is an
        // unsigned integer.
        NumberOfNames = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // String area offset. The offset from the start of the table to the start of string storage, in
        // bytes. This is an unsigned integer.
        StringAreaOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var names = new List<FontName>();
        for (int i = 0; i < NumberOfNames; i++)
        {
            var name = new FontName(data.Slice(offset, FontName.Size));
            names.Add(name);
            offset += FontName.Size;
        }

        Names = names;

        if (StringAreaOffset > data.Length)
        {
            throw new ArgumentException("String Area Offset exceeds the bounds of the provided data.", nameof(data));
        }

        var nameStrings = new string[NumberOfNames];
        for (int i = 0; i < NumberOfNames; i++)
        {
            var name = Names[i];
            var stringStart = StringAreaOffset + name.Offset;
            if (stringStart + name.Length > data.Length)
            {
                throw new ArgumentException($"Name string for entry {i} exceeds the bounds of the provided data.", nameof(data));
            }

            var stringData = data.Slice(stringStart, name.Length);
            nameStrings[i] = Encoding.ASCII.GetString(stringData);
        }

        NameStrings = nameStrings;

        Debug.Assert(offset <= data.Length, "Offset should not exceed data length after reading Font Naming Table.");
    }
}
