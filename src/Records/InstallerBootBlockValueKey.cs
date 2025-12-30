namespace ResourceForkReader.Records;

/// <summary>
/// Keys for the Installer Boot Block Record ('inbb').
/// </summary>
public enum InstallerBootBlockValueKey : short
{
    /// <summary>
    /// Copy over boot blocks from a 'boot' resource found in the file whose file spec ID is given in
    /// the value field (word).
    /// </summary>
    CopyBootBlocks = -1,

    /// <summary>
    /// The value in the Boot Block Value field updates the boot blocks ID (word).
    /// </summary>
    BootBlocksId = 1,

    /// <summary>
    /// The value updates the boot block entrypoint (long).
    /// </summary>
    BootBlockEntrypoint = 2,

    /// <summary>
    /// The value updates the boot block version (word).
    /// </summary>
    BootBlockVersion = 3,

    /// <summary>
    /// The value updates the page 2 usage flags (word).
    /// </summary>
    Page2UsageFlags = 4,

    /// <summary>
    /// The value updates the name of the system resource file (string).
    /// </summary>
    SystemResourceFileName = 5,

    /// <summary>
    /// The value updates the name of the system shell (string).
    /// </summary>
    SystemShellName = 6,

    /// <summary>
    /// The value updates the first loaded debugger's name (string).
    /// </summary>
    FirstDebuggerName = 7,

    /// <summary>
    /// The value updates the second loaded debugger's name (string).
    /// </summary>
    SecondDebuggerName = 8,

    /// <summary>
    /// The value updates the file name of the startup screen (string).
    /// </summary>
    StartupScreenFileName = 9,

    /// <summary>
    /// The value updates the file name of the startup program (string).
    /// </summary>
    StartupProgramFileName = 10,

    /// <summary>
    /// The value updates the file name of the system scrap file (string).
    /// </summary>
    SystemScrapFileName = 11,

    /// <summary>
    /// The value updates the number of FCBs to open (word).
    /// </summary>
    NumberOfFCBs = 12,

    /// <summary>
    /// The value updates the size of the event queue (word).
    /// </summary>
    EventQueueSize = 13,

    /// <summary>
    /// This boot block field is no longer used.
    /// </summary>
    [Obsolete("This boot block field is no longer used.")]
    Unused14 = 14,

    /// <summary>
    /// This boot block field is no longer used.
    /// </summary>
    [Obsolete("This boot block field is no longer used.")]
    Unused15 = 15,

    /// <summary>
    /// The value updates the size of the system heap on a 512K Mac (long).
    /// </summary>
    SystemHeapSize512K = 16,

    /// <summary>
    /// The value updates the absolute size of the system heap (long).
    /// </summary>
    SystemHeapAbsoluteSize = 17,

    /// <summary>
    /// This boot block field is no longer used.
    /// </summary>
    [Obsolete("This boot block field is no longer used.")]
    Unused18 = 18,

    /// <summary>
    /// The value updates the minimal additional system heap space required (long).
    /// </summary>
    MinimalAdditionalSystemHeapSpace = 19,

    /// <summary>
    /// The value updates the fraction of memory available to be used for the system heap (long).
    /// </summary>
    SystemHeapMemoryFraction = 20,
}
