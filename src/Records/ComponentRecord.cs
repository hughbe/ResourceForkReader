using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Component Record ('thng') in a resource fork.
/// </summary>
public readonly struct ComponentRecord
{
    /// <summary>
    /// Gets the Component Description of the component.
    /// </summary>
    public ComponentDescription Description { get; }

    /// <summary>
    /// Gets the code resource specification of the component.
    /// </summary>
    public ResourceSpec CodeResource { get; }

    /// <summary>
    /// Gets the name resource specification of the component.
    /// </summary>
    public ResourceSpec NameResource { get; }

    /// <summary>
    /// Gets the information resource specification of the component.
    /// </summary>
    public ResourceSpec InformationResource { get; }

    /// <summary>
    /// Gets the icon resource specification of the component.
    /// </summary>
    public ResourceSpec IconResource { get; }

    /// <summary>
    /// Gets the component resource extension, if present.
    /// </summary>
    public ComponentResourceExtension? ResourceExtension { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentRecord"/> struct.
    /// </summary>
    /// <param name="data"></param>
    public ComponentRecord(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 6-80 to 6-85
        int offset = 0;

        // A component description record that specifies the characteristics
        // of the component. For a complete description of this record, see
        // page 6-52.
        Description = new ComponentDescription(data.Slice(offset, ComponentDescription.Size));
        offset += ComponentDescription.Size;

        // A resource specification record that specifies the type and ID of the
        // component code resource. The resType field of the resource
        // specification record may contain any value. The component’s main
        // entry point must be at offset 0 in the resource.
        CodeResource = new ResourceSpec(data.Slice(offset, ResourceSpec.Size));
        offset += ResourceSpec.Size;

        // A resource specification record that specifies the resource type and
        // ID for the name of the component. This is a Pascal string. Typically,
        // the component name is stored in a resource of type 'STR '. 
        NameResource = new ResourceSpec(data.Slice(offset, ResourceSpec.Size));
        offset += ResourceSpec.Size;

        // A resource specification record that specifies the resource type and
        // ID for the information string that describes the component. This is a
        // Pascal string. Typically, the information string is stored in a resource
        // of type 'STR '. You might use the information stored in this
        // resource in a Get Info dialog box. 
        InformationResource = new ResourceSpec(data.Slice(offset, ResourceSpec.Size));
        offset += ResourceSpec.Size;

        // A resource specification record that specifies the resource type and
        // ID for the icon for a component. Component icons are stored as
        // 32-by-32 bit maps. Typically, the icon is stored in a resource of type
        // 'ICON'. Note that this icon is not used by the Finder; you supply an
        // icon only so that other components or applications can display your
        // component’s icon in a dialog box if needed. 
        IconResource = new ResourceSpec(data.Slice(offset, ResourceSpec.Size));
        offset += ResourceSpec.Size;

        if (data.Length > offset)
        {
            ResourceExtension = new ComponentResourceExtension(data.Slice(offset, ComponentResourceExtension.Size));
            offset += ComponentResourceExtension.Size;
        }
        else
        {
            ResourceExtension = null;
        }

        Debug.Assert(offset <= data.Length, "Did not consume all data for Component Record.");
    }
}
