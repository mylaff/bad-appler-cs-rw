using BadAppler.Render;
using System.Collections.Generic;

namespace BadAppler.Translator
{
    interface ITranslator<T, Z> where T : struct where Z : struct
    {
        public List<Frame<T>> Translate(List<Frame<Z>> frames, object settings);
    }
}
