using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an Error Strings Record ('errs') in a resource fork.
/// </summary>
public readonly struct ErrorStringsRecord
{
    /// <summary>
    /// Gets the list of Error Strings in the record.
    /// </summary>
    public List<ErrorString> ErrorStrings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorStringsRecord"/> struct.
    /// </summary>
    /// <param name="data">The data for the Error Strings Record.</param>
    public ErrorStringsRecord(ReadOnlySpan<byte> data)
    {
        // Structure documented in https://github.com/fuzziqersoftware/resource_dasm/blob/master/src/SystemTemplates.cc#L407
        int offset = 0;

        int count = data.Length / ErrorString.Size;
        var errorStrings = new List<ErrorString>(count);
        for (int i = 0; i < count; i++)
        {
            errorStrings.Add(new ErrorString(data.Slice(offset, ErrorString.Size)));
            offset += ErrorString.Size;
        }

        ErrorStrings = errorStrings;

        // Seen extra padding bytes in some 'errs' resources.
        Debug.Assert(offset <= data.Length, "Did not consume all data for Error Strings Record.");
    }
}
