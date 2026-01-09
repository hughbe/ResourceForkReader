using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a type list resource ('TYP#') containing a list of resource types.
/// </summary>
public readonly struct TypeList
{
    /// <summary>
    /// The minimum size of a type list record in bytes.
    /// </summary>
    public const int MinSize = 2;

    /// <summary>
    /// Gets the number of types in the list.
    /// </summary>
    public ushort NumberOfTypes { get; }

    /// <summary>
    /// Gets the list of resource types.
    /// </summary>
    public List<string> Types { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeList"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing at least 2 bytes of type list data.</param>
    /// <exception cref="ArgumentException">Thrown when data is too short.</exception>
    public TypeList(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        // Structure not documented.
        int offset = 0;

        NumberOfTypes = BinaryPrimitives.ReadUInt16BigEndian(data[..2]);
        offset += 2;

        if (offset + NumberOfTypes * 4 > data.Length)
        {
            throw new ArgumentException($"Data must be at least {offset + NumberOfTypes * 4} bytes long to contain all types.", nameof(data));
        }

        var types = new List<string>(NumberOfTypes);
        for (int i = 0; i < NumberOfTypes; i++)
        {
            types.Add(Encoding.ASCII.GetString(data.Slice(offset, 4)));
            offset += 4;
        }

        Types = types;

        Debug.Assert(offset == data.Length, "Did not consume all data.");
    }
}
