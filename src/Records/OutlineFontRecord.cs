using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Outline Font Record in a resource fork.
/// </summary>
public readonly struct OutlineFontRecord
{
    /// <summary>
    /// Minimum size of an Outline Font Record in bytes.
    /// </summary>
    public const int MinSize = 12;
    
    /// <summary>
    /// Gets the Font Directory associated with this Outline Font Record.
    /// </summary>
    public FontDirectory FontDirectory { get; }

    /// <summary>
    /// Gets the Character Code Mapping Table ('cmap').
    /// </summary>
    public CharacterCodeMappingTable? CharacterCodeMappingTable { get; }

    /// <summary>
    /// Gets the Control Value Table ('cvt ').
    /// </summary>
    public ControlValueTable? ControlValueTable { get; }

    /// <summary>
    /// Gets the Font Program Table ('fpgm').
    /// </summary>
    public FontProgramTable? FontProgramTable { get; }

    /// <summary>
    /// Gets the Glyph Data Table ('glyf').
    /// </summary>
    public GlyphDataTable? GlyphDataTable { get; }

    /// <summary>
    /// Gets the Horizontal Device Metrics Table ('hdmx').
    /// </summary>
    public HorizontalDeviceMetricsTable? HorizontalDeviceMetricsTable { get; }

    /// <summary>
    /// Gets the Font Header Table ('head').
    /// </summary>
    public FontHeaderTable? FontHeaderTable { get; }

    /// <summary>
    /// Gets the Horizontal Header Table ('hhea').
    /// </summary>
    public HorizontalHeaderTable? HorizontalHeaderTable { get; }

    /// <summary>
    /// Gets the Horizontal Metrics Table ('hmtx').
    /// </summary>
    public HorizontalMetricsTable? HorizontalMetricsTable { get; }

    /// <summary>
    /// Gets the Kerning Table ('kern').
    /// </summary>
    public KerningTable? KerningTable { get; }

    /// <summary>
    /// Gets the Location Table ('loca').
    /// </summary>
    public LocationTable? LocationTable { get; }

    /// <summary>
    /// Gets the Maximum Profile Table ('maxp').
    /// </summary>
    public MaximumProfileTable? MaximumProfileTable { get; }

    /// <summary>
    /// Gets the Font Naming Table ('name').
    /// </summary>
    public FontNamingTable? NamingTable { get; }

    /// <summary>
    /// Gets the PostScript Table ('post').
    /// </summary>
    public PostScriptTable? PostScriptTable { get; }

    /// <summary>
    /// Gets the Preprogram Table ('prep').
    /// </summary>
    public PreprogramTable? PreprogramTable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OutlineFontRecord"/> struct from the provided data.
    /// </summary>
    /// <param name="data">The byte span containing the Outline Font Record data.</param>
    /// <exception cref="ArgumentException">Thrown when the provided data is less than the minimum required size.</exception>
    public OutlineFontRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than the minimum required size of {MinSize} bytes for an Outline Font Record.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/Text.pdf
        // 4-72 to 4-89
        int offset = 0;

        FontDirectory = new FontDirectory(data[offset..], out int bytesRead);
        offset += bytesRead;

        foreach (var table in FontDirectory.Tables)
        {
            if (table.Offset + table.Length > data.Length)
            {
                throw new ArgumentException($"Table '{table.TagName}' exceeds the bounds of the provided data.", nameof(data));
            }

            switch (table.TagName)
            {
                case "cmap":
                    CharacterCodeMappingTable = new CharacterCodeMappingTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "cvt ":
                    ControlValueTable = new ControlValueTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "fpgm":
                    FontProgramTable = new FontProgramTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "glyf":
                    GlyphDataTable = new GlyphDataTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "hdmx":
                    HorizontalDeviceMetricsTable = new HorizontalDeviceMetricsTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "head":
                    FontHeaderTable = new FontHeaderTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "hhea":
                    HorizontalHeaderTable = new HorizontalHeaderTable(data.Slice((int)table.Offset, (int)table.Length));    
                    break;
                case "hmtx":
                    HorizontalMetricsTable = new HorizontalMetricsTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "kern":
                    KerningTable = new KerningTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "loca":
                    LocationTable = new LocationTable(data.Slice((int)table.Offset, (int)table.Length), FontHeaderTable?.LocationTableFormat ?? 0);
                    break;
                case "maxp":
                    MaximumProfileTable = new MaximumProfileTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "name":
                    NamingTable = new FontNamingTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "post":
                    PostScriptTable = new PostScriptTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                case "prep":
                    PreprogramTable = new PreprogramTable(data.Slice((int)table.Offset, (int)table.Length));
                    break;
                default:
                    throw new NotImplementedException($"Parsing for table '{table.TagName}' is not implemented.");
            }
        }

        Debug.Assert(offset <= data.Length, "OutlineFontRecord: Parsed more bytes than available in data.");
    }
}
