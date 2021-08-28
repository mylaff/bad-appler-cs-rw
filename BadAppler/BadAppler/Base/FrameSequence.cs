namespace BadAppler.Base
{
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
    }
}
