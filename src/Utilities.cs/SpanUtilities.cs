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
    /// <param name="bytesRead">Outputs the total number of bytes read from the span.</param>
    /// <returns>The extracted string.</returns>
    /// <exception cref="ArgumentException">Thrown when the data is too short to contain the specified Pascal string.</exception>
    public static string ReadPascalString(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < 1)
        {
            throw new ArgumentException("Data is too short to contain a Pascal string length.", nameof(data));
        }

        var strLength = data[0];
        if (1 + strLength > data.Length)
        {
            throw new ArgumentException("Data is too short to contain the specified Pascal string.", nameof(data));
        }

        bytesRead = 1 + strLength;
        return Encoding.ASCII.GetString(data.Slice(1, strLength));
    }

    /// <summary>
    /// Reads a Pascal-style string with a 2-byte length prefix from the given span.
    /// </summary>
    /// <param name="data">The span containing the Pascal string.</param>
    /// <param name="bytesRead">Outputs the total number of bytes read from the span.</param>
    /// <returns>The extracted string.</returns>
    /// <exception cref="ArgumentException">Thrown when the data is too short to contain the specified Pascal string.</exception>
    public static string ReadPascalStringWordCount(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < 2)
        {
            throw new ArgumentException("Data is too short to contain a Pascal string length.", nameof(data));
        }

        var strLength = BinaryPrimitives.ReadUInt16BigEndian(data[..2]);
        if (2 + strLength > data.Length)
        {
            throw new ArgumentException("Data is too short to contain the specified Pascal string.", nameof(data));
        }

        bytesRead = 2 + strLength;
        return Encoding.ASCII.GetString(data.Slice(2, strLength));
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

    /// <summary>
    /// Reads an HFS timestamp from the specified span and converts it to a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="data">The span containing the data.</param>
    /// <returns>The corresponding <see cref="DateTime"/> value.</returns>
    public static DateTime ReadMacOSTimestampLong(ReadOnlySpan<byte> data)
    {
        Debug.Assert(data.Length >= 8, "Data span must contain at least 8 bytes for the timestamp.");

        // 8 bytes MacOS timestamp
        var timestamp = BinaryPrimitives.ReadUInt64BigEndian(data);

        // MacOS timestamps are seconds since 00:00:00 on January 1, 1904
        var epoch = new DateTime(1904, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddSeconds(timestamp);
    }

    /// <summary>
    /// Reads a list of monochrome images from the given span.
    /// </summary>
    /// <param name="data">The span containing the image data.</param>
    /// <param name="width">The width of each image in pixels.</param>
    /// <param name="height">The height of each image in pixels.</param>
    /// <param name="bytesRead">Outputs the total number of bytes read from the span.</param>
    /// <returns>>A list of byte arrays, each representing a monochrome image.</returns>
    /// <exception cref="ArgumentException">Thrown when the width is not a multiple of 8.</exception>
    public static List<byte[]> ReadMonochromeImageList(ReadOnlySpan<byte> data, int width, int height, out int bytesRead)
    {
        if (width % 8 != 0)
        {
            throw new ArgumentException("Width must be a multiple of 8 for monochrome images.", nameof(width));
        }

        int imageSize = (width + 7) / 8 * height;
        var numberOfImages = data.Length / imageSize;

        var images = new List<byte[]>(numberOfImages);
        int offset = 0;

        for (int i = 0; i < numberOfImages; i++)
        {
            var imageData = data.Slice(offset, imageSize).ToArray();
            images.Add(imageData);
            offset += imageSize;
        }

        bytesRead = offset;
        return images;
    }
}
