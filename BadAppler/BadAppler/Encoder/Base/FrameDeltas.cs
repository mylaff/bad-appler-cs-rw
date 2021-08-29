using System;
using System.Collections.Generic;

namespace BadAppler.Encoder.Base
{
    [Serializable]
    struct FrameDeltas<T>
    {
        public List<PixelDelta<T>> Deltas { get; private set; }

        public int Count
        {
            get => Deltas.Count;
        }

        public void AddDelta(PixelDelta<T> delta) => Deltas.Add(delta);

        public FrameDeltas(List<PixelDelta<T>> deltas) => Deltas = deltas;
    }
}
