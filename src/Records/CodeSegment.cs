using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

public ref struct CodeSegment
{
    public int CodeOffset { get; }
    public Span<byte> ExecutableCode { get; }

    public CodeSegment(Span<byte> data)
    {
        CodeOffset = SpanUtilities.ReadInt32BE(data, 0);
        ExecutableCode = data[CodeOffset..];
    }
}
