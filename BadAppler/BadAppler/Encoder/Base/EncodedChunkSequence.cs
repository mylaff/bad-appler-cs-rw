using System;
using System.Collections.Generic;
using BadAppler.Base;

namespace BadAppler.Encoder.Base
{
    [Serializable]
    class EncodedChunkSequence<T, Y> : Sequence<EncodedChunk<T>, Y> where T : struct
    {
        public override int Count { get; protected set; }

        public EncodedChunkSequence(int count, List<EncodedChunk<T>> content, Y meta) : base(content, meta) 
        {
            Count = count;
        }
    }
}
