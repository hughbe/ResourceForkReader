namespace ResourceForkReader;

/// <summary>
/// Represents resource attributes (flags) that control resource behavior in classic Mac OS.
/// </summary>
[Flags]
public enum ResourceAttributes : byte
{
    /// <summary>
    /// No attributes set.
    /// </summary>
    None = 0x00,
    
    /// <summary>
    /// Reserved bit 1.
    /// </summary>
    Reserved1 = 0x01,
    
    /// <summary>
    /// Resource has been changed.
    /// </summary>
    Changed = 0x02,
    
    /// <summary>
    /// Resource should be preloaded into memory.
    /// </summary>
    Preload = 0x04,
    
    /// <summary>
    /// Resource is protected from modification.
    /// </summary>
    Protected = 0x08,
    
    /// <summary>
    /// Resource is locked in memory.
    /// </summary>
    Locked = 0x10,
    
    /// <summary>
    /// Resource is purgeable from memory when needed.
    /// </summary>
    Purgeable = 0x20,
    
    /// <summary>
    /// Resource should be loaded into system heap instead of application heap.
    /// </summary>
    SystemHeap = 0x40,
    
    /// <summary>
    /// Reserved bit 2.
    /// </summary>
    Reserved2 = 0x80
}
