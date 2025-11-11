using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

public struct VersionRecord
{
    public const int Size = 4;

    public string Major { get; }
    public string Minor { get; }

    public VersionRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < Size)
        {
            throw new ArgumentException($"Data must be at least {Size} bytes long.", nameof(data));
        }

        int offset = 0;
        Major = SpanUtilities.ReadString(data, offset, 2);
        offset += 2;

        Minor = SpanUtilities.ReadString(data, offset, 2);
        offset += 2;

        Debug.Assert(offset == Size);
    }
}
