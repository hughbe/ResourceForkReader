using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a collection of Macintosh Models in a resource fork.
/// </summary>
public readonly struct MacintoshModels
{
    /// <summary>
    /// Gets the list of Macintosh models.
    /// </summary>
    public List<MacintoshModel> Models { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacintoshModels"/> struct.
    /// </summary>
    /// <param name="data">The data for the Macintosh Models.</param>
    /// <exception cref="ArgumentException">>Thrown if the data length is not a multiple of the MacintoshModel size.</exception>
    public MacintoshModels(ReadOnlySpan<byte> data)
    {
        if (data.Length % MacintoshModel.Size != 0)
        {
            throw new ArgumentException($"Data length must be a multiple of {MacintoshModel.Size} bytes.", nameof(data));
        }

        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L1078-L1087
        int offset = 0;

        int numberOfModels = data.Length / MacintoshModel.Size;
        var models = new List<MacintoshModel>(numberOfModels);
        for (int i = 0; i < numberOfModels; i++)
        {
            models.Add(new MacintoshModel(data.Slice(offset, MacintoshModel.Size)));
            offset += MacintoshModel.Size;
        }

        Models = models;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for MacintoshModels.");
    }
}
