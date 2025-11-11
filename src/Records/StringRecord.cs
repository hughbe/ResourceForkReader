using System.Text;

namespace ResourceForkReader.Records;

public struct StringRecord
{
    public byte Length { get; }

    public string Value { get; }

    public StringRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length == 0)
        {
            throw new ArgumentException("Data must be at least 1 byte long.", nameof(data));
        }

        Length = data[0];

        if (data.Length < 1 + Length)
        {
            throw new ArgumentException("Data is too short for the specified string length.", nameof(data));
        }

        Value = Encoding.ASCII.GetString(data.Slice(1, Length));
    }
}
