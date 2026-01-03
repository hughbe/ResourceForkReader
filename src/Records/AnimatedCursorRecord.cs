using System.Buffers.Binary;
using System.Diagnostics;

namespace ResourceForkReader.Records;

/// <summary>
/// Represents an animated cursor resource ('acur').
/// </summary>
public readonly struct AnimatedCursorRecord
{
    /// <summary>
    /// Gets the animated cursor data.
    /// </summary>
    public const int MinSize = 4;

    /// <summary>
    /// Gets the number of frames in the animated cursor.
    /// </summary>
    public ushort NumberOfFrames { get; }

    /// <summary>
    /// Gets the current frame index.
    /// </summary>
    public ushort Counter { get; }

    /// <summary>
    /// Gets the list of frames in the animated cursor.
    /// </summary>
    public List<AnimatedCursorFrame> Frames { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimatedCursorRecord"/> struct by parsing binary data.
    /// </summary>
    /// <param name="data">A span containing the animated cursor data.</param>
    public AnimatedCursorRecord(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinSize)
        {
            throw new ArgumentException($"Data must be at least {MinSize} bytes long.", nameof(data));
        }

        int offset = 0;

        NumberOfFrames = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        Counter = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
        offset += 2;

        if (MinSize + NumberOfFrames * AnimatedCursorFrame.Size > data.Length)
        {
            throw new ArgumentException($"Data must be at least {MinSize + NumberOfFrames * AnimatedCursorFrame.Size} bytes long to contain all frames.", nameof(data));
        }

        var frames = new List<AnimatedCursorFrame>(NumberOfFrames);
        for (int i = 0; i < NumberOfFrames; i++)
        {
            frames.Add(new AnimatedCursorFrame(data.Slice(offset, AnimatedCursorFrame.Size)));
            offset += AnimatedCursorFrame.Size;
        }

        Frames = frames;

        Debug.Assert(offset == data.Length, "Did not consume all bytes for AnimatedCursorRecord.");
    }

    /// <summary>
    /// Represents a single frame in an animated cursor.
    /// </summary>
    public readonly struct AnimatedCursorFrame
    {
        /// <summary>
        /// The size of an animated cursor frame in bytes.
        /// </summary>
        public const int Size = 4;

        /// <summary>
        /// Gets the cursor resource ID for this frame.
        /// </summary>
        public short CursorResourceId { get; }

        /// <summary>
        /// Gets the padding bytes.
        /// </summary>
        public ushort Padding { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedCursorFrame"/> struct by parsing binary data.
        /// </summary>
        /// <param name="data">A span containing exactly 4 bytes of animated cursor frame data.</param>
        /// <exception cref="ArgumentException">Thrown when data is not exactly 4 bytes long.</exception>
        public AnimatedCursorFrame(ReadOnlySpan<byte> data)
        {
            if (data.Length != Size)
            {
                throw new ArgumentException($"Data must be exactly {Size} bytes long.", nameof(data));
            }

            int offset = 0;

            CursorResourceId = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            Padding = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;

            Debug.Assert(offset == data.Length, "Did not consume all bytes for AnimatedCursorFrame.");
        }
    }
}
