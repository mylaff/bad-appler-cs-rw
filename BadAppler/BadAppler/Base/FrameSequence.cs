using System;
using System.Collections.Generic;

namespace BadAppler.Base
{
    [Serializable]
    class FrameSequence<T> : Sequence<Frame<T>, FrameSequenceMeta> where T : struct
    {
        public Frame<T> this[int param]
        {
            get => Content[param];
        }

        public override int Count
        {
            get => Content.Count;
        }

        public FrameSequence(IList<Frame<T>> frames, FrameSequenceMeta meta) : base(frames, meta) { }
    }
}
