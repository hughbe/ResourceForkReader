using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Resource Spec structure in a resource fork.
/// </summary>
public readonly struct ResourceSpec
{
    /// <summary>
    /// Gets the size of a Resource Spec structure in bytes.
    /// </summary>
    public const int Size = 6;

    /// <summary>
    /// Gets the resource type of the resource.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the resource ID of the resource.
    /// </summary>
    public short ResourceID { get;  }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceSpec"/> struct.
    /// </summary>
    /// <param name="data">The data for the Resource Spec.</param>
    /// <exception cref="ArgumentException">Thrown if the data length is not equal to the Resource Spec size.</exception>
    public ResourceSpec(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://developer.apple.com/library/archive/documentation/mac/pdf/MoreMacintoshToolbox.pdf
        // 6-81 
        int offset = 0;

        // A four-character code that specifies the resource type.
        Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        // A signed integer that specifies the resource ID.
        ResourceID = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Debug.Assert(offset == data.Length, "Did not consume all data for Resource Spec.");
    }
}
