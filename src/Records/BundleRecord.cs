using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents a bundle ('BNDL') in a resource fork.
/// </summary>
public readonly struct BundleRecord
{
    /// <summary>
    /// Gets the owner of the bundle.
    /// </summary>
    public string Owner { get; }

    /// <summary>
    /// Gets the owner ID of the bundle.
    /// </summary>
    public ushort OwnerID { get; }

    /// <summary>
    /// Gets the number of types in the bundle.
    /// </summary>
    public ushort NumberOfTypes { get; }

    /// <summary>
    /// Gets the list of bundle types.
    /// </summary>
    public List<BundleType> Types { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BundleRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the bundle data.</param>
    public BundleRecord(ReadOnlySpan<byte> data)
    {
        int offset = 0;
        
        Owner = Encoding.ASCII.GetString(data.Slice(offset, 4));
        offset += 4;

        OwnerID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        NumberOfTypes = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        var types = new List<BundleType>(NumberOfTypes + 1);
        for (int i = 0; i <= NumberOfTypes; i++)
        {
            var type = new BundleType(data, ref offset);
            types.Add(type);
        }

        Types = types;

        Debug.Assert(offset <= data.Length, "Parsed beyond the end of the data span.");
    }

    /// <summary>
    /// Represents a bundle type within a bundle record.
    /// </summary>
    public readonly struct BundleType
    {
        /// <summary>
        /// Gets the type of the bundle.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the number of mappings for this bundle type.
        /// </summary>
        public ushort NumberOfMappings { get; }

        /// <summary>
        /// Gets the list of bundle mappings.
        /// </summary>
        public List<BundleMapping> Mappings { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BundleType"/> struct by parsing binary data.
        /// </summary>
        /// <param name="data">The span containing the bundle type data.</param>
        /// <param name="offset">A reference to the current offset within the data span.</param>
        public BundleType(ReadOnlySpan<byte> data, ref int offset)
        {
            if (data.Length - offset < 6)
            {
                throw new ArgumentException("Data is too short to be a valid BundleType.", nameof(data));
            }

            Type = Encoding.ASCII.GetString(data.Slice(offset, 4));
            offset += 4;

            NumberOfMappings = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            if (data.Length - offset < (NumberOfMappings + 1) * BundleMapping.Size)
            {
                throw new ArgumentException("Data is too short to contain all BundleMappings.", nameof(data));
            }

            var mappings = new List<BundleMapping>(NumberOfMappings + 1);
            for (int i = 0; i <= NumberOfMappings; i++)
            {
                var mapping = new BundleMapping(data.Slice(offset, 4));
                mappings.Add(mapping);
                offset += 4;
            }

            Mappings = mappings;

            Debug.Assert(offset <= data.Length, "Parsed beyond the end of the data span.");
        }
    }

    /// <summary>
    /// Represents a bundle mapping within a bundle type.
    /// </summary>
    public struct BundleMapping
    {
        /// <summary>
        /// Gets the size of a bundle mapping in bytes.
        /// </summary>
        public const int Size = 4;

        /// <summary>
        /// Gets the local ID of the resource.
        /// </summary>
        public ushort LocalID { get; }

        /// <summary>
        /// Gets the actual resource ID.
        /// </summary>
        public ushort ActualResourceID { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BundleMapping"/> struct by parsing binary data.
        /// </summary>
        /// <param name="data">A span containing the bundle mapping data.</param>
        /// <exception cref="ArgumentException">Thrown when the provided data is too short.</exception>
        public BundleMapping(ReadOnlySpan<byte> data)
        {
            if (data.Length < Size)
            {
                throw new ArgumentException("Data is too short to be a valid BundleMapping.", nameof(data));
            }

            int offset = 0;
            LocalID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            ActualResourceID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            Debug.Assert(offset == Size);
        }
    }
}

