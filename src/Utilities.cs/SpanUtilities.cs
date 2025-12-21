using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Utilities;

internal static class SpanUtilities
{
    /// <summary>
    /// Reads a Pascal-style string from the given span.
    /// </summary>
    /// <param name="data">The span containing the Pascal string.</param>
    /// <returns>The extracted string.</returns>
    public static string ReadPascalString(ReadOnlySpan<byte> data)
    {
        if (data.Length == 0)
        {
            return string.Empty;
        }

        byte strLength = data[0];
        if (data.Length < 1 + strLength)
        {
            throw new ArgumentException("Data is too short to contain the specified Pascal string.", nameof(data));
        }

        return Encoding.ASCII.GetString(data.Slice(1, strLength));
    }


    /// <summary>
    /// Reads an HFS timestamp from the specified span and converts it to a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="data">The span containing the data.</param>
    /// <returns>The corresponding <see cref="DateTime"/> value.</returns>
    public static DateTime ReadMacOSTimestamp(ReadOnlySpan<byte> data)
    {
        Debug.Assert(data.Length >= 4, "Data span must contain at least 4 bytes for the timestamp.");

        // 4 bytes MacOS timestamp
        var timestamp = BinaryPrimitives.ReadUInt32BigEndian(data);

        // MacOS timestamps are seconds since 00:00:00 on January 1, 1904
        var epoch = new DateTime(1904, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddSeconds(timestamp);
    }
}
