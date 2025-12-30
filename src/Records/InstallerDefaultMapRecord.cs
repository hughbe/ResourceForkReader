using System.Buffers.Binary;
using System.Diagnostics;
using ResourceForkReader.Utilities;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Installer Default Map Record ('indm') in a resource fork.
/// </summary>
public readonly struct InstallerDefaultMapRecord
{
    /// <summary>
    /// The minimum size of an Installer Default Map Record in bytes.
    /// </summary>
    public const int MinSize = 39;

    /// <summary>
    /// Gets the default map format version.
    /// </summary>
    public ushort FormatVersion { get; }

    /// <summary>
    /// Gets the default map flags.
    /// </summary>
    public ushort Flags { get; }

    /// <summary>
    /// Gets the hardware requirements.
    /// </summary>
    public InstallerDefaultMapHardwareRequirements HardwareRequirements { get; }

    /// <summary>
    /// Gets the software requirements.
    /// </summary>
    public InstallerDefaultMapSoftwareRequirements SoftwareRequirements { get; }

    /// <summary>
    /// Gets the target volume requirements.
    /// </summary>
    public InstallerDefaultMapTargetVolumeRequirements TargetVolumeRequirements { get; }

    /// <summary>
    /// Gets the user function resource ID.
    /// </summary>
    public short UserFunctionResourceID { get; }

    /// <summary>
    /// Gets the user description.
    /// </summary>
    public string UserDescription { get; }

    /// <summary>
    /// Gets the number of packages.
    /// </summary>
    public ushort NumberOfPackages { get; }

    /// <summary>
    /// Gets the package resource IDs.
    /// </summary>
    public List<ushort> PackageResourceIDs { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerDefaultMapRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Installer Default Map Record data.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than minimum size.</exception>
    public InstallerDefaultMapRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure documented in http://www.bitsavers.org/pdf/apple/mac/blue/Apple_Blue_Book_vol2_1989.pdf
        // page 11 to 15
        int offset = 0;

        // The Default Map Format Version field (2 bytes) is an unsigned
        // integer indicating the version of the Default Map script format.
        // It is currently O. If the Installer encounters a default map
        // with a version higher than it knows how to handle, the map
        // will be ignored.
        FormatVersion = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The Default Map Flags field (2 bytes) consists of 16 bit-flags.
        // They are currently unused and should be set to O.
        Flags = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // Infonnation about the minimal hardware needed to install a specific
        // package is contained in the HW Requirements field.
        HardwareRequirements = new InstallerDefaultMapHardwareRequirements(data.Slice(offset), out int hardwareRequirementsBytesRead);;
        offset += hardwareRequirementsBytesRead;

        // Some installations may be dependent on the existence of certain
        // resources in the System file. For example, when updating a system,
        // it is necessary to know the patches which have been previously
        // installed on the target system. The Installer looks at the System
        // file in the blessed folder on the chosen target volume and compares
        // the following fields to the values found in the System's resource fork.
        SoftwareRequirements = new InstallerDefaultMapSoftwareRequirements(data.Slice(offset), out int softwareRequirementsBytesRead);
        offset += softwareRequirementsBytesRead;

        // The Target Volume Requirements field (8 bytes) contains information
        // about the requirements for a target disk drive
        TargetVolumeRequirements = new InstallerDefaultMapTargetVolumeRequirements(data.Slice(offset, InstallerDefaultMapTargetVolumeRequirements.Size));
        offset += InstallerDefaultMapTargetVolumeRequirements.Size;

        // The User Function field contains the ID of an "infn" resource.
        // This type ofresource is an executable Macintosh function which
        // returns a Boolean value.
        UserFunctionResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The User Description is an even-padded Pascal string that the
        // Installer displays on the "Easy Install screen".
        UserDescription = SpanUtilities.ReadPascalString(data.Slice(offset));
        offset += 1 + UserDescription.Length;

        if (offset % 2 != 0)
        {
            offset += 1; // Padding byte to align to even size
        }
        
        // The Package List field is a list of all the packages associated
        // with this default map. It contains the number of packages in the
        // list, and the resource IDs of the packages.
        NumberOfPackages = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var packageResourceIDs = new List<ushort>(NumberOfPackages);
        for (int i = 0; i < NumberOfPackages; i++)
        {
            packageResourceIDs.Add(BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2)));
            offset += 2;
        }

        PackageResourceIDs = packageResourceIDs;

        Debug.Assert(offset == data.Length, "Did not consume all data for Installer Default Map Record.");
    }
}
