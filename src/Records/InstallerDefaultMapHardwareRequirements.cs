using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents the hardware requirements in an Installer Default Map Record ('indm').
/// </summary>
public readonly struct InstallerDefaultMapHardwareRequirements
{
    /// <summary>
    /// The minimum size of an Installer Default Map Hardware Requirements in bytes.
    /// </summary>
    public const int MinSize = 12;

    /// <summary>
    /// Gets the number of supported machine types.
    /// </summary>
    public ushort NumberOfMachines { get; }

    /// <summary>
    /// Gets the list of supported machine types.
    /// </summary>
    public List<ushort> MachineTypes { get; }

    /// <summary>
    /// Gets the number of supported processor types.
    /// </summary>
    public ushort NumberOfProcessors { get; }

    /// <summary>
    /// Gets the list of supported processor types.
    /// </summary>
    public List<ushort> ProcessorTypes { get; }

    /// <summary>
    /// Gets the number of supported MMU types.
    /// </summary>
    public ushort NumberOfMMUs { get; }

    /// <summary>
    /// Gets the list of supported MMU types.
    /// </summary>
    public List<ushort> MMUTypes { get; }

    /// <summary>
    /// Gets the number of supported keyboard types.
    /// </summary>
    public ushort NumberOfKeyboards { get; }

    /// <summary>
    /// Gets the list of supported keyboard types.
    /// </summary>
    public List<ushort> KeyboardTypes { get; }

    /// <summary>
    /// Gets a value indicating whether a floating point unit (FPU) is required.
    /// </summary>
    public bool RequiresFPU { get; }

    /// <summary>
    /// Gets a value indicating whether Color QuickDraw is required.
    /// </summary>
    public bool RequiresColorQuickDraw { get; }

    /// <summary>
    /// Gets the minimal memory requirement in kilobytes.
    /// </summary>
    public ushort MinimalMemory { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerDefaultMapHardwareRequirements"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Installer Default Map Hardware Requirements data.</param>
    /// <param name="bytesRead">Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than minimum size.</exception>
    public InstallerDefaultMapHardwareRequirements(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than minimum size {MinSize} for Installer Default Map Hardware Requirements.", nameof(data));
        }

        // Structure documented in http://www.bitsavers.org/pdf/apple/mac/blue/Apple_Blue_Book_vol2_1989.pdf
        // page 13 to 14
        // and https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L550-L582
        int offset = 0;

        // The number of machines in the Machine Type list
        NumberOfMachines = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The Machine Type field is list of 2 byte values which specify
        // the machine for this default map. The values for this field
        // are the same as found in the machineType field returned from
        // SysEnvirons.
        var machineTypes = new List<ushort>(NumberOfMachines);
        for (int i = 0; i < NumberOfMachines; i++)
        {
            machineTypes.Add(BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2)));
            offset += 2;
        }

        MachineTypes = machineTypes;

        // The number of processors in the Processor Type list.
        NumberOfProcessors = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The Processor Type field is list of 2 byte values which specify
        // the processor for this default map. The values for this field
        // are the same as found in the processor field returned from
        // SysEnvirons.
        var processorTypes = new List<ushort>(NumberOfProcessors);
        for (int i = 0; i < NumberOfProcessors; i++)
        {
            processorTypes.Add(BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2)));
            offset += 2;
        }

        ProcessorTypes = processorTypes;

        // The number of MMUs in the MMU Type list.
        NumberOfMMUs = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The MMU .Type field is list of 2 byte values which specify the
        // MMU for this default map. The values for this field are as follows:
        // 0: GMMU
        // 1: 68851
        // 2: 68030
        var mmuTypes = new List<ushort>(NumberOfMMUs);
        for (int i = 0; i < NumberOfMMUs; i++)
        {
            mmuTypes.Add(BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2)));
            offset += 2;
        }

        MMUTypes = mmuTypes;

        // The number of keyboards in the Keyboard Type list.
        NumberOfKeyboards = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The Keyboard Type field is list of 2 byte values which specify
        // the keyboards for this default nlap. The values for this field
        // are the same as found in the keyBoardType field returned from
        // SysEnvirons.
        var keyboardTypes = new List<ushort>(NumberOfKeyboards);
        for (int i = 0; i < NumberOfKeyboards; i++)
        {
            keyboardTypes.Add(BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2)));
            offset += 2;
        }

        KeyboardTypes = keyboardTypes;

        // The FPU field is one byte, whose lowest ordered bit is a flag
        // that specifies whether an FPU is needed for this default map.
        RequiresFPU = (data[offset] & 0x01) != 0;
        offset += 1;

        // The Color QD field is one byte, whose lowest ordered bit is a flag
        // that specifies whether color QuickDraw is needed for this default
        // map.
        RequiresColorQuickDraw = (data[offset] & 0x01) != 0;
        offset += 1;

        // The Minimal MemoryType field is a 2 byte quantity specifying the
        // minimal amount of RAM (in MBytes) needed for this default map.
        MinimalMemory = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        bytesRead = offset;
        Debug.Assert(offset <= data.Length, "Did not consume all data for Installer Default Map Hardware Requirements.");
    }
}
