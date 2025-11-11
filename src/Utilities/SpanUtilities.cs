namespace ResourceForkReader.Utilities;

/// <summary>
/// Provides utility methods for reading big-endian values from spans.
/// </summary>
internal static class SpanUtilities
{
    public static ushort ReadUInt16BE(ReadOnlySpan<byte> data, int offset)
    {
        return (ushort)((data[offset] << 8) | data[offset + 1]);
    }

    public static uint ReadUInt32BE(ReadOnlySpan<byte> data, int offset)
    {
        return (uint)((data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3]);
    }

    public static int ReadInt32BE(ReadOnlySpan<byte> data, int offset)
    {
        return (data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3];
    }

    public static ulong ReadUInt64BE(ReadOnlySpan<byte> data, int offset)
    {
        return ((ulong)data[offset] << 56) |
               ((ulong)data[offset + 1] << 48) |
               ((ulong)data[offset + 2] << 40) |
               ((ulong)data[offset + 3] << 32) |
               ((ulong)data[offset + 4] << 24) |
               ((ulong)data[offset + 5] << 16) |
               ((ulong)data[offset + 6] << 8) |
               data[offset + 7];
    }

    public static string ReadString(ReadOnlySpan<byte> data, int offset, int length)
    {
        return System.Text.Encoding.ASCII.GetString(data.Slice(offset, length));
    }
}