using BadAppler.Base;
using BadAppler.Encoder.Base;

namespace BadAppler.Encoder
{ 
    /// <summary>
    /// Interface for Encoders :)
    /// </summary>
    /// <typeparam name="T">Type used for character encoding. Usually is byte or char.</typeparam>
    /// <typeparam name="Y">Type of the Sequence Meta's class.</typeparam>
    /// <typeparam name="Z">Type of the Encoder Option's class</typeparam>
    interface IEncoder<T, Y, Z> where T : struct
    {
        public EncodedChunkSequence<T, Y> EncodeFrames(FrameSequence<T> frames, Z options);
    }
}
