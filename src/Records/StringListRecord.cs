using System.Buffers.Binary;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a string list resource ('STR#') containing multiple Pascal-style strings.
/// </summary>
public readonly struct StringListRecord
{
    /// <summary>
    /// Gets the list of strings.
    /// </summary>
    public List<string> Values { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringListRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 2 bytes for the string count, followed by length-prefixed strings.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short.</exception>
    public StringListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < 2)
        {
            throw new ArgumentException("Data must be at least 2 bytes long.", nameof(data));
        }

        int offset = 0;
        var stringCount = BinaryPrimitives.ReadUInt16BigEndian(data[offset..]);
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
