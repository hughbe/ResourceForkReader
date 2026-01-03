using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Component Description in a Component Record.
/// </summary>
public readonly struct ComponentDescription
{
    /// <summary>
    /// Size of a Component Description in bytes.
    /// </summary>
    public const int Size = 20;

    /// <summary>
    /// Type of the component (4-character code).
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Subtype of the component (4-character code).
    /// </summary>
    public string SubType { get; }

    /// <summary>
    /// Manufacturer of the component (4-character code).
    /// </summary>
    public string Manufacturer { get; }

    /// <summary>
    /// Flags for the component.
    /// </summary>
    public ComponentFlags Flags { get; }

    /// <summary>
    /// Mask for the component flags.
    /// </summary>
    public uint FlagsMask { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentDescription"/> struct.
    /// </summary>
    /// <param name="data">The data for the Component Description.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not exactly 20 bytes.</exception>
    public ComponentDescription(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data length must be exactly {Size} bytes.", nameof(data));
        }

        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 6-52 to 6-54
        int offset = 0;

        // A four-character code that identifies the type of component. All
        // components of a particular type must support a common set of
        // interface routines. For example, drawing components all have a
        // component type of 'draw'.
        // Your component must support all of the standard routines for the
        // component type specified by this field. Type codes with all
        // lowercase characters are reserved for definition by Apple. See Inside
        // Macintosh: QuickTime Components for information about the
        // QuickTime components supplied by Apple. You can define your
        // own component type code as long as you register it with Apple’s
        // Component Registry Group.
        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        // A four-character code that identifies the subtype of the component.
        // Different subtypes of a component type may support additional
        // features or provide interfaces that extend beyond the standard
        // routines for a given component. For example, the subtype of a
        // drawing component indicates the type of object the component
        // draws. Drawing components that draw ovals have a subtype of
        // 'oval'.
        // Your component may use this field to indicate more specific
        // information about the capabilities of the component. There are no
        // restrictions on the content you assign to this field. If no additional
        // information is appropriate for your component type, you may set
        // the componentSubType field to 0.
        SubType = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        // A four-character code that identifies the manufacturer of the
        // component. This field allows for further differentiation between
        // individual components. For example, components made by a
        // specific manufacturer may support an extended feature set.
        // Components provided by Apple use a manufacturer value of
        // 'appl'.
        // Your component uses this field to indicate the manufacturer of the
        // component. You obtain your manufacturer code, which can be the
        // same as your application signature, from Apple’s Component
        // Registry Group.
        Manufacturer = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        // A 32-bit field that provides additional information about a particular
        // component.
        // The high-order 8 bits are reserved for definition by the Component
        // Manager and provide information about the component. The
        // following bits are currently defined:
        // CONST
        //  cmpWantsRegisterMessage = $80000000;
        //  cmpFastDispatch = $400000
        // The setting of the cmpWantsRegisterMessage bit determines
        // whether the Component Manager calls this component during
        // registration. Set this bit to 1 if your component should be called
        // when it is registered; otherwise, set this bit to 0. If you want to
        // automatically dispatch requests to your component to the
        // appropriate routine that handles the request (rather than your
        // component calling CallComponentFunction or
        // CallComponentFunctionWithStorage), set the
        // cmpFastDispatch bit. If you set this bit, you must write your
        // component’s entry point in assembly language. If you set this
        // bit, the Component Manager calls your component’s entry point
        // with the call’s parameters, the handle to that instance’s storage, and
        // the caller’s return address already on the stack. The Component
        // Manager passes the request code in register D0 and passes the stack
        // location of the instance’s storage in register A0. Your component can
        // then use the request code in register D0 to directly dispatch the
        // request itself (for example, by using this value as an index into a
        // table of function addresses). Be sure to note that the standard
        // request codes have negative values. Also note that the function
        // parameter that the caller uses to specify the component instance
        // instead contains a handle to the instance’s storage. When the
        // component function completes, control returns to the calling
        // application.
        // For more information about component registration and
        // initialization, see “Responding to the Register Request” on
        // page 6-23.
        // The low-order 24 bits are specific to each component type. You can
        // use these flags to indicate any special capabilities or features of your
        // component. Your component may use all 24 bits, as appropriate to
        // its component type. You must set all unused bits to 0.
        Flags = (ComponentFlags)BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        // Reserved. (However, note that applications can use this field when
        // performing search operations, as described on page 6-39.)
        // Your component must set the componentFlagsMask field in its
        // component description record to 0.
        FlagsMask = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
        offset += 4;

        Debug.Assert(offset == data.Length, "Did not consume all data for Component Description.");
    }
}
