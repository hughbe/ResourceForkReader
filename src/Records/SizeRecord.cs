using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a size resource ('SIZE').
/// </summary>
public readonly struct SizeRecord
{
    /// <summary>
    /// The size of a size record in bytes.
    /// </summary>
    public const int Size = 10;

    /// <summary>
    /// Gets a value indicating whether the reserved flag is set.
    /// </summary>
    public bool Reserved1 { get; }

    /// <summary>
    /// Gets a value indicating whether the application accepts suspend/resume events.
    /// </summary>
    public bool AcceptSuspendResumeEvents { get; }

    /// <summary>
    /// Gets a value indicating whether the reserved2 flag is set.
    /// </summary>
    public bool Reserved2 { get; }

    /// <summary>
    /// Gets a value indicating whether the application can run in the background.
    /// </summary>
    public bool CanBackground { get; }

    /// <summary>
    /// Gets a value indicating whether the application activates on foreground switch.
    /// </summary>
    public bool DoesActivateOnFGSwitch { get; }

    /// <summary>
    /// Gets a value indicating whether the application supports background and foreground operation.
    /// </summary>
    public bool BackgroundAndForeground { get; }

    /// <summary>
    /// Gets a value indicating whether the application does not get front clicks.
    /// </summary>
    public bool DontGetFrontClicks { get; }

    /// <summary>
    /// Gets a value indicating whether the application ignores app died events.
    /// </summary>
    public bool IgnoreAppDiedEvents { get; }

    /// <summary>
    /// Gets a value indicating whether the application is 32-bit compatible.
    /// </summary>
    public bool Is32BitCompatible { get; }

    /// <summary>
    /// Gets a value indicating whether the application is high-level event aware.
    /// </summary>
    public bool IsHighLevelEventAware { get; }

    /// <summary>
    /// Gets a value indicating whether the application supports local and remote high-level events.
    /// </summary>
    public bool LocalAndRemoteHLEvents { get; }

    /// <summary>
    /// Gets a value indicating whether the application is stationery aware.
    /// </summary>
    public bool IsStationeryAware { get; }

    /// <summary>
    /// Gets a value indicating whether the application does not use text edit services.
    /// </summary>
    public bool DontUseTextEditServices { get; }

    /// <summary>
    /// Gets a value indicating whether the reserved3 flag is set.
    /// </summary>
    public bool Reserved3 { get; }

    /// <summary>
    /// Gets a value indicating whether the reserved4 flag is set.
    /// </summary>
    public bool Reserved4 { get; }

    /// <summary>
    /// Gets a value indicating whether the reserved5 flag is set.
    /// </summary>
    public bool Reserved5 { get; }

    /// <summary>
    /// Gets the preferred memory size.
    /// </summary>
    public uint PreferredMemorySize { get; }

    /// <summary>
    /// Gets the minimum memory size.
    /// </summary>
    public uint MinimumMemorySize { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SizeRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing exactly 12 bytes of size record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 12 bytes long.</exception>
    public SizeRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        int offset = 0;
        byte flags = data[offset];
        offset += 1;

        Reserved1 = (flags & 0x80) != 0;
        AcceptSuspendResumeEvents = (flags & 0x40) != 0;
        Reserved2 = (flags & 0x20) != 0;
        CanBackground = (flags & 0x10) != 0;
        DoesActivateOnFGSwitch = (flags & 0x08) != 0;
        BackgroundAndForeground = (flags & 0x04) != 0;
        DontGetFrontClicks = (flags & 0x02) != 0;
        IgnoreAppDiedEvents = (flags & 0x01) != 0;

        flags = data[offset];
        offset += 1;

        Is32BitCompatible = (flags & 0x80) != 0;
        IsHighLevelEventAware = (flags & 0x40) != 0;
        LocalAndRemoteHLEvents = (flags & 0x20) != 0;
        IsStationeryAware = (flags & 0x10) != 0;
        DontUseTextEditServices = (flags & 0x08) != 0;
        Reserved3 = (flags & 0x04) != 0;
        Reserved4 = (flags & 0x02) != 0;
        Reserved5 = (flags & 0x01) != 0;

        PreferredMemorySize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        MinimumMemorySize = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == Size, "Did not consume all bytes for SizeRecord.");
    }
}
