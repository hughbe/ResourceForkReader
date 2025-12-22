using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Menu Bar ('MBAR') in a resource fork.
/// </summary>
public readonly struct MenuBarRecord
{
    /// <summary>
    /// Gets the number of menus in the menu bar.
    /// </summary>
    public ushort NumberOfMenus { get; }

    /// <summary>
    /// Gets the list of resource IDs for the menus in the menu bar.
    /// </summary>
    public List<ushort> ResourceIDs { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuBarRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the menu bar data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short to contain a valid MenuBarRecord.</exception>
    public MenuBarRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < 2)
        {
            throw new ArgumentException("Data is too short to contain a MenuBarRecord.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MacintoshToolboxEssentials.pdf
        // 3-155
        int offset = 0;

        // Number of menus described by this menu bar.
        NumberOfMenus = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (data.Length < 2 + NumberOfMenus * 2)
        {
            throw new ArgumentException($"Data is too short for the specified number of menus ({NumberOfMenus}).", nameof(data));
        }

        // A variable number (the amount should match the number
        // declared in the first 2 bytes) of resource IDs; each
        // resource ID should identify a 'MENU' resource.
        var resourceIDs = new List<ushort>(NumberOfMenus);
        for (int i = 0; i < NumberOfMenus; i++)
        {
            ushort resourceID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
            resourceIDs.Add(resourceID);
        }

        ResourceIDs = resourceIDs;

        Debug.Assert(offset == data.Length, "Did not consume all data for MenuBarRecord.");
    }
}
