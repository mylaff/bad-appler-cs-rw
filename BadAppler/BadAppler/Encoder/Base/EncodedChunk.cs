using BadAppler.Base;
using System;
using System.Collections.Generic;


namespace BadAppler.Encoder.Base
{
    [Serializable]
    class EncodedChunk<T> where T : struct
    {
        public Frame<T> KeyFrame { get; init; }

        public int Size { get => Deltas.Count; }

        public List<FrameDeltas<T>> Deltas { get; init; }

        public EncodedChunk(Frame<T> keyFrame, List<FrameDeltas<T>> deltas)
        {
            Deltas = deltas;
            KeyFrame = keyFrame;
        }
    }
}
