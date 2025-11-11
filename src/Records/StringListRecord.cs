using System.Text;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

public struct StringListRecord
{
    public List<string> Values { get; }

    public StringListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < 2)
        {
            throw new ArgumentException("Data must be at least 2 bytes long.", nameof(data));
        }

        int offset = 0;
        var stringCount = SpanUtilities.ReadUInt16BE(data, offset);
        offset += 2;

        var strings = new List<string>(stringCount);
        for (int i = 0; i < stringCount; i++)
        {
            byte strLength = data[offset];
            offset += 1;

            string str = Encoding.ASCII.GetString(data.Slice(offset, strLength));
            offset += strLength;
            strings.Add(str);
        }

        Values = strings;
    }
}
