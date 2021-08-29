using BadAppler.Base;

namespace BadAppler.Translator
{
    interface ITranslator<T, Z> where T : struct where Z : struct
    {
        public FrameSequence<T> Translate(FrameSequence<Z> frames, object settings);
    }
}
