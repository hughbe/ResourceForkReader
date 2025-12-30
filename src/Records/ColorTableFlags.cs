namespace ResourceForkReader.Records;

/// <summary>
/// Defines flags for a Color Table.
/// </summary>
[Flags]
public enum ColorTableFlags : ushort
{
    /// <summary>
    /// Identifies this as a color table for a pixel map.
    /// </summary>
    PixelMap = 0x0000,

    /// <summary>
    /// Identifies this as a color table for an indexed device.
    /// </summary>
    IndexedDevice = 0x8000   
}
