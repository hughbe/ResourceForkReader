using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a component resource extension structure in a resource fork.
/// </summary>
public readonly struct ComponentResourceExtension
{
    /// <summary>
    /// Gets the size of a Component Resource Extension structure in bytes.
    /// </summary>
    public const int Size = 10;

    /// <summary>
    /// Gets the version number of the component.
    /// </summary>
    public uint Version { get; }

    /// <summary>
    /// Gets the registration flags for the component.
    /// </summary>
    public uint RegisterFlags { get; }

    /// <summary>
    /// Gets the resource ID of the icon family resource associated with the component.
    /// </summary>
    public short IconFamilyResourceID { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentResourceExtension"/> struct.
    /// </summary>
    /// <param name="data">The data for the Component Resource Extension.</param>
    /// <exception cref="ArgumentException">Thrown if the data length is not equal to the Component Resource Extension size.</exception>
    public ComponentResourceExtension(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length must be {Size}.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 6-82 to 6-83
        int offset = 0;

        // The version number of the component. If you specify the
        // componentDoAutoVersion flag in componentRegisterFlags,
        // the Component Manager must obtain the version number of your
        // component when your component is registered. Either you can
        // provide a version number in your component’s resource, or you can
        // specify a value of 0 for its version number. If you specify 0, the
        // Component Manager sends your component a version request to get
        // the version number of your component.
        Version = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // A set of flags containing additional registration information. You can
        // use these constants as flags:
        // CONST
        // componentDoAutoVersion = 1;
        // componentWantsUnregister = 2;
        // componentAutoVersionIncludeFlags = 4;
        // Specify the componentDoAutoVersion flag if you want the
        // Component Manager to resolve conflicts between different versions
        // of the same component. If you specify this flag, the Component
        // Manager registers your component only if there is no later version
        // available. If an older version is already registered, the Component
        // Manager unregisters it. If a newer version of the same component is
        // registered after yours, the Component Manager automatically
        // unregisters your component. You can use this automatic version
        // control feature to make sure that the most recent version of your
        // component is registered, regardless of the number of versions that
        // are installed.
        // Specify the componentWantsUnregister flag if you want your
        // component to receive an unregister request when it is unregistered.
        // Specify the flag componentAutoVersionIncludeFlags if you
        // want the Component Manager to include the componentFlags
        // field of the component description record when it searches for
        // identical components in the process of performing automatic
        // version control for your component. If you do not specify this flag,
        // the Component Manager searches only the componentType,
        // componentSubType, and componentManufacturer fields.
        // When the Component Manager performs automatic version control
        // for your component, it searches for components with identical
        // values in the componentType, componentSubType, and
        // componentManufacturer fields (and optionally, in
        // the componentFlags field). If it finds a matching component, it
        // compares version numbers and registers the most recent version of
        // the component. Note that the setting of the
        // componentAutoVersionIncludeFlags flag affects automatic
        // version control only and does not affect the search operations
        // performed by FindNextComponent and CountComponents.
        RegisterFlags = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // The resource ID of an icon family. You can provide an icon family in
        // addition to the icon provided in the componentIcon field. Note
        // that members of this icon family are not used by the Finder; you
        // supply an icon family only so that other components or applications
        // can display your component’s icon in a dialog box if needed.
        IconFamilyResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for Component Resource Extension.");
    }
}
