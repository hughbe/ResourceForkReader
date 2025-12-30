using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a NBP Retry record ('GNRL') in a resource fork.
/// </summary>
public readonly struct NBPRetryRecord
{
    /// <summary>
    /// The size of a NBP Retry record in bytes.
    /// </summary>
    public const int Size = 2;

    /// <summary>
    /// Gets the time between retries in seconds.
    /// </summary>
    public byte RetryInterval { get; }

    /// <summary>
    /// Gets the number of retries.
    /// </summary>
    public byte RetryCount { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NBPRetryRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A 2-byte span containing the NBP Retry record data.</param>
    /// <exception cref="ArgumentException">Thrown when data is not exactly 2 bytes.</exception>
    public NBPRetryRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length != Size)
        {
            throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
        }

        // Structure documented in https://vintageapple.org/inside_o/pdf/Inside_Macintosh_Volume_IV_1986.pdf
        // page IV-216 and IV-220
        int offset = 0;

        RetryInterval = data[offset];
        offset += 1;

        RetryCount = data[offset];
        offset += 1;

        Debug.Assert(offset == data.Length, "Did not consume all data for NBPRetryRecord.");
    }
}
