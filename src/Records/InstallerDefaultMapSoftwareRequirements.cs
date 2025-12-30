using System.Buffers.Binary;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents the software requirements section of an Installer Default Map Record ('dmap').
/// </summary>
public readonly struct InstallerDefaultMapSoftwareRequirements
{
    /// <summary>
    /// The minimum size of an Installer Default Map Software Requirements in bytes.
    /// </summary>
    public const int MinSize = 8;

    /// <summary>
    /// Gets the number of system resources.
    /// </summary>
    public ushort NumberOfSystemResources { get; }

    /// <summary>
    /// Gets the list of system resource IDs.
    /// </summary>
    public List<InstallerSystemResource> SystemResourceIDs { get; }

    /// <summary>
    /// Gets the system revision.
    /// </summary>
    public uint SystemRevision { get; }

    /// <summary>
    /// Gets the country code.
    /// </summary>
    public ushort CountryCode { get; }

    /// <summary>
    /// Gets the AppleTalk driver version.
    /// </summary>
    public ushort AppleTalkDriverVersion { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerDefaultMapSoftwareRequirements"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing Installer Default Map Software Requirements data.</param>
    /// <param name="bytesRead">>Outputs the number of bytes read from the data span.</param>
    /// <exception cref="ArgumentException">>Thrown when data is less than minimum size.</exception>
    public InstallerDefaultMapSoftwareRequirements(ReadOnlySpan<byte> data, out int bytesRead)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data length {data.Length} is less than minimum size {MinSize} for Installer Default Map Software Requirements.", nameof(data));
        }

        // Structure documented in http://www.bitsavers.org/pdf/apple/mac/blue/Apple_Blue_Book_vol2_1989.pdf
        // page 14
        // and https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L550-L582
        int offset = 0;

        // The number of resource specs in the System Resource Spec list.
        NumberOfSystemResources = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The System Resource Spec is a list. Each entry in the list contains
        // the type and ID of a resource to search for in the chosen target
        // volume's System file. All resources listed here must be found in
        // the target System file for the default map to be chosen.
        var systemResourceIDs = new List<InstallerSystemResource>(NumberOfSystemResources);
        for (int i = 0; i < NumberOfSystemResources; i++)
        {
            systemResourceIDs.Add(new InstallerSystemResource(data.Slice(offset, InstallerSystemResource.Size)));
            offset += InstallerSystemResource.Size;
        }

        SystemResourceIDs = systemResourceIDs;

        // This is a four byte field with the minimal system revision (as
        // defined in Tech Note 189) needed for this default map.
        SystemRevision = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The Country Code field is a 2 byte field indicating which version
        // of the Internationa Utilities Package must be found in the System
        // for this default map. This number has the same format as in the
        // intlOVers field of a IntlORec record. This information will come
        // fronl the 'vers' resource in the target volume's System File
        CountryCode = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        // The AppleTalk Version field is a 2 byte field which indicates the
        // lowest numbered version of the AppleTalk drivers needed for this
        // default map. It has the same format as the System Version field.
        AppleTalkDriverVersion = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        bytesRead = offset;
    }
}