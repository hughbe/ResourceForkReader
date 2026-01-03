using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a Title List Record ('Tlst') in a resource fork.
/// </summary>
public readonly struct TitleListRecord
{
    /// <summary>
    /// Gets the list of title types.
    /// </summary>
    public List<string> Types { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TitleListRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Title List Record.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not a multiple of 4 bytes.</exception>
    public TitleListRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length % 4 != 0)
        {
            throw new ArgumentException("Data length must be a multiple of 4 bytes.", nameof(data));
        }

        // Structure not documented but can be reverse engineered from existing resource forks.
        int offset = 0;

        var count = data.Length / 4;
        var types = new List<string>(count);
        for (int i = 0; i < count; i++)
        {
            types.Add(Encoding.ASCII.GetString(data.Slice(offset, 4)));
            offset += 4;
        }

        Types = types;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for TitleListRecord.");
    }
}
