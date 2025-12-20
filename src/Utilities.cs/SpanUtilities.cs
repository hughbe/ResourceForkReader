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
}
