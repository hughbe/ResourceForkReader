using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a code segment resource ('CODE') containing executable 68k code.
/// </summary>
public readonly ref struct CodeSegment
{
    /// <summary>
    /// Gets the offset to the executable code within the segment.
    /// </summary>
    public int CodeOffset { get; }
    
    /// <summary>
    /// Gets a span containing the executable code bytes.
    /// </summary>
    public Span<byte> ExecutableCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeSegment"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the code segment data.</param>
    public CodeSegment(Span<byte> data)
    {
        CodeOffset = SpanUtilities.ReadInt32BE(data, 0);
        ExecutableCode = data[CodeOffset..];
    }
}
